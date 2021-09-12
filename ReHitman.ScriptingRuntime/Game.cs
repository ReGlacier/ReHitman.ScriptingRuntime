using System;

namespace ReHitman.ScriptingRuntime
{
    public class Game
    {
        #region Events
        #endregion
        #region Variables
        /// <summary>
        /// Current time multiplier value
        /// </summary>
        public static float TimeScale
        {
            get => Native.Game.GetTimeScale();
            set => Native.Game.SetTimeScale(value);
        }

        /// <summary>
        /// Name of current scene
        /// </summary>
        public static string SceneName
        {
            get => Native.Game.GetSceneName();
        }

        /// <summary>
        /// Name of current profile. "SingleSceneRun" if game starts not from HitmanBloodMoney.gms scene
        /// </summary>
        public static string ProfileName
        {
            get => Native.Game.GetProfileName();
        }

        /// <summary>
        /// Count of money on account balance
        /// </summary>
        public static Int32 ProfileMoney
        {
            get => Native.Game.GetProfileMoney();
            set => Native.Game.SetProfileMoney(value);
        }
        #endregion
        #region Functions
        #endregion
    }
}