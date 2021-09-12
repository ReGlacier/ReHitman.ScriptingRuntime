using System;
using ReHitman.ScriptingRuntime;
using ReHitman.ScriptingRuntime.Native;
using Game = ReHitman.ScriptingRuntime.Game;

namespace SampleMod
{
    public class SampleModBase : GameModBase
    {
        public override void OnLoad()
        {
            DeveloperConsole.Info("SampleMod loaded!");
        }

        public override void OnUnLoad()
        {
            DeveloperConsole.Info("SampleMod unloaded!");
        }

        public override void OnUpdate(float dt)
        {
        }

        public override string GetGameModName()
        {
            return "SampleModBase";
        }
    }
}
