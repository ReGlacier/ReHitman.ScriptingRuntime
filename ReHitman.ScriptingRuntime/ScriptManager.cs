using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReHitman.ScriptingRuntime
{
    /// <summary>
    /// Class implements basic logic of "load", "unload" & "update" game mods (aka scripts)
    /// </summary>
    public class ScriptManager
    {
        private string _scriptRootPath;
        private FileSystemWatcher _fsWatcher;
        private Dictionary<string, Script> _loadedScripts = new Dictionary<string, Script>();

        public void StartWatchFolder(string folder) {
            _scriptRootPath = folder;
            _fsWatcher = new FileSystemWatcher(_scriptRootPath);

            _fsWatcher.Changed += OnScriptFolderHasChanges;
            _fsWatcher.Created += OnScriptFolderHasChanges;
            _fsWatcher.Deleted += OnScriptFolderHasChanges;
            _fsWatcher.Renamed += OnScriptMoved;
            
            _fsWatcher.Filter = "*.dll";
            _fsWatcher.EnableRaisingEvents = true;
        }

        public void EnumerateAndLoadScripts() {
            UnLoadAllScripts();
            
            var scripts = Directory.GetFiles(_scriptRootPath);
            foreach (var scriptFile in scripts)
            {
                if (!scriptFile.EndsWith(".dll")) {
                    continue;
                }

                LoadScript(Path.Combine(Directory.GetCurrentDirectory(), scriptFile));
            }
        }

        public Script[] GetActiveScripts() {
            return _loadedScripts.Values.ToArray();
        }

        public void Update(float dt) {
            foreach (var keyValuePair in _loadedScripts)
            {
                keyValuePair.Value.Update(dt);
            }
        }

        public void Draw()
        {
            foreach (var keyValuePair in _loadedScripts)
            {
                keyValuePair.Value.Draw();
            }
        }

        private void OnScriptFolderHasChanges(object sender, FileSystemEventArgs e) {
            if (e.ChangeType == WatcherChangeTypes.Changed) {
                UnLoadScript(e.FullPath);
                LoadScript(e.FullPath);
            }
            else if (e.ChangeType == WatcherChangeTypes.Created) {
                LoadScript(e.FullPath);
            }
            else if (e.ChangeType == WatcherChangeTypes.Deleted) {
                UnLoadScript(e.FullPath);
            }
        }

        private void OnScriptMoved(object sender, RenamedEventArgs e)
        {
            UnLoadScript(e.OldFullPath);
            LoadScript(e.FullPath);
        }

        private bool LoadScript(string scriptPath) {
            Native.DeveloperConsole.Info($"Wants to load script {scriptPath}");
            
            if (_loadedScripts.ContainsKey(scriptPath)) {
                Native.DeveloperConsole.Warning($"Script assembly {scriptPath} already loaded!");
                return false;
            }

            var script = new Script(scriptPath);
            if (!script.TryLoad()) {
                Native.DeveloperConsole.Warning($"Failed to load script assembly {scriptPath}! See log for details");
                return false;
            }

            _loadedScripts.Add(scriptPath, script);
            return true;
        }

        private bool UnLoadScript(string scriptPath) {
            if (!_loadedScripts.ContainsKey(scriptPath)) {
                Native.DeveloperConsole.Warning($"Script assembly {scriptPath} not found in loaded scripts list!");
                return false;
            }
            
            _loadedScripts.Remove(scriptPath);
            return true;
        }

        private void UnLoadAllScripts() {
            _loadedScripts.Clear();
        }
    }
}