using System.Runtime.CompilerServices;

namespace ReHitman.ScriptingRuntime.Native
{
    public class DeveloperConsole {
        ///<summary>
        ///Print information message in developer console
        ///</summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void Info(string message);
            
        ///<summary>
        ///Print warning message in developer console
        ///</summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void Warning(string message);

        ///<summary>
        ///Print error message in developer console
        ///</summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern void Error(string message);
    }
}