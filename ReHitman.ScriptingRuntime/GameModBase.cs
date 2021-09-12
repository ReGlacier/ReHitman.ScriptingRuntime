namespace ReHitman.ScriptingRuntime
{
    /// <summary>
    /// Basic interface for every game mod
    /// </summary>
    public class GameModBase
    {
        /// <summary>
        /// Called once when mod loaded by ScriptManager
        /// </summary>
        public virtual void OnLoad()
        {
        }

        /// <summary>
        /// Called once when mod ready to be unloaded
        /// </summary>
        public virtual void OnUnLoad()
        {
        }

        /// <summary>
        /// Called on game update
        /// </summary>
        /// <param name="dt">time delta</param>
        public virtual void OnUpdate(float dt)
        {
        }

        /// <summary>
        /// Called on draw
        /// </summary>
        public virtual void OnDraw()
        {
        }

        /// <summary>
        /// </summary>
        /// <returns>Name of game mod</returns>
        public virtual string GetGameModName()
        {
            return "NoName";
        }
    }
}