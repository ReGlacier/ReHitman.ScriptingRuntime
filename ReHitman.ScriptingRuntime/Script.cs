using System;
using System.IO;
using System.Reflection;

namespace ReHitman.ScriptingRuntime
{
    /// <summary>
    /// This class implements basic logic of load, locate, run, update and destroy managed game mod
    /// This class trying to locate first class inherited of IGameMod
    /// </summary>
    public class Script
    {
        private Assembly _assembly;
        private GameModBase _gameModBase;
        private string _path;

        public string Name => _path;
        public GameModBase GameMod => _gameModBase;

        public Script(string assemblyPath)
        {
            try
            {
                _assembly = Assembly.LoadFile(assemblyPath);
                _path = assemblyPath;
            }
            catch (FileLoadException)
            {
                Native.DeveloperConsole.Error($"Failed to load script assembly {assemblyPath}. File not found!");
            }
            catch (ArgumentException) 
            {
                Native.DeveloperConsole.Error($"Failed to load script assembly {assemblyPath}. Bad path to script! Absolute path is required!");
            }
            catch (BadImageFormatException)
            {
                Native.DeveloperConsole.Error($"Failed to load script assembly {assemblyPath}. Bad assembly format!");
            }
        }

        public bool TryLoad()
        {
            if (_assembly == null) {
                return false;
            }

            Type[] loadedTypes = _assembly.GetTypes();
            Native.DeveloperConsole.Info($"Assembly '{_assembly.FullName}' contains {loadedTypes.Length} types");

            foreach (var assemblyType in loadedTypes)
            {
                if (!assemblyType.IsClass || assemblyType.BaseType == null) 
                {
                    Native.DeveloperConsole.Warning($"Disallow type {assemblyType.FullName} : reason NO_CLASS_OR_NO_BASE");
                    continue; // Skip non-class & non-inherited types
                }

                if (assemblyType.BaseType != typeof(GameModBase))
                {
                    Native.DeveloperConsole.Warning($"Disallow type {assemblyType.FullName} : reason NO_GAME_MOD (BaseType is {assemblyType.BaseType.FullName})");
                    continue;
                }
                
                _gameModBase = Activator.CreateInstance(assemblyType) as GameModBase;
                if (_gameModBase != null)
                {
                    Native.DeveloperConsole.Info(
                        $"Script assembly '{assemblyType.FullName}' was loaded successfully! Game mod name is '{_gameModBase.GetGameModName()}'");
                    break;
                }
                else
                {
                    Native.DeveloperConsole.Warning(
                        $"Failed to instantiate game mod class {assemblyType.FullName}. Loader will try to locate another class");
                }
            }
            
            if (_gameModBase != null)
            {
                _gameModBase.OnLoad();
            }
            else
            {
                Native.DeveloperConsole.Error($"Failed to locate implementation of IGameMod class at {_assembly.FullName} assembly");
                return false;
            }
            
            return true;
        }

        ~Script()
        {
            _gameModBase?.OnUnLoad();
            _gameModBase = null;
            _assembly = null;
        }

        public void Update(float dt)
        {
            _gameModBase?.OnUpdate(dt);
        }

        public void Draw()
        {
            _gameModBase?.OnDraw();
        }
    }
}