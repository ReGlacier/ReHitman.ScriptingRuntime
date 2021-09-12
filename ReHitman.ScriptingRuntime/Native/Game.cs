using System;
using System.Runtime.CompilerServices;

namespace ReHitman.ScriptingRuntime.Native
{
    public class Game
    {
        [MethodImpl(MethodImplOptions.InternalCall)] public static extern float GetTimeScale();
        [MethodImpl(MethodImplOptions.InternalCall)] public static extern void SetTimeScale(float timescale);
        [MethodImpl(MethodImplOptions.InternalCall)] public static extern string GetSceneName();
        [MethodImpl(MethodImplOptions.InternalCall)] public static extern string GetProfileName();
        [MethodImpl(MethodImplOptions.InternalCall)] public static extern Int32 GetProfileMoney();
        [MethodImpl(MethodImplOptions.InternalCall)] public static extern void SetProfileMoney(Int32 value);
    }
}