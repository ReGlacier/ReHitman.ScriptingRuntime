using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ReHitman.ScriptingRuntime
{
    public class RuntimeBroker
    {
        private static RuntimeBroker _runtimeBroker;
        private ScriptManager _scriptManager;

        #region Native cross calls & delegates
        [MethodImpl(MethodImplOptions.InternalCall)] public static extern void SetOnUpdateCallbackCxxPtr(IntPtr ptr);
        [MethodImpl(MethodImplOptions.InternalCall)] public static extern void SetOnDrawCallbackCxxPtr(IntPtr ptr);
        
        private delegate void UpdateDelegate(float dt);
        private delegate void DrawDelegate();
        
        private UpdateDelegate _updateDelegate;
        private DrawDelegate _drawDelegate;
        #endregion

        private RuntimeBroker()
        {
            _scriptManager = new ScriptManager();
            
            #region Provide raw pointers to C++
            _updateDelegate = new UpdateDelegate(OnUpdate);
            _drawDelegate = new DrawDelegate(OnDraw);
            
            SetOnUpdateCallbackCxxPtr(Marshal.GetFunctionPointerForDelegate(_updateDelegate));
            SetOnDrawCallbackCxxPtr(Marshal.GetFunctionPointerForDelegate(_drawDelegate));
            #endregion
        }

        public static RuntimeBroker Instance
        {
            get
            {
                if (_runtimeBroker == null) _runtimeBroker = new RuntimeBroker();
                return _runtimeBroker;
            }
        }
        
        public static void Init()
        {
            #region Prepare runtime and load scripts
            Native.DeveloperConsole.Info("RuntimeBroker ONLINE");
            Instance._scriptManager.StartWatchFolder("Mods");
            Instance._scriptManager.EnumerateAndLoadScripts();

            var scripts = Instance._scriptManager.GetActiveScripts();
            Native.DeveloperConsole.Info($"Loaded scripts ({scripts.Length}): ");
            if (scripts.Length == 0)
            { 
                Native.DeveloperConsole.Info("(No scripts loaded)");
            }

            foreach (var script in scripts)
            { 
                Native.DeveloperConsole.Info($" [+] {script.GameMod.GetGameModName()} (at {script.Name})");
            }

            Native.DeveloperConsole.Info("--------------------------------------------");
            #endregion
        }

        private static void OnDraw()
        {
            Instance._scriptManager.Draw();
        }

        private static void OnUpdate(float dt)
        {
            Instance._scriptManager.Update(dt);
        }
    }
}