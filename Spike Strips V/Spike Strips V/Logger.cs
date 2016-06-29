namespace Spike_Strips_V
{
    // System
    using System;

    // RPH
    using Rage;

    internal static class Logger
    {
        public static void LogTrivial(string text)
        {
            Game.LogTrivial(text);
        }

        public static void LogTrivial(string specific, string text)
        {
            Game.LogTrivial("[" + specific + "] " + text);
        }


        public static void LogDebug(string text)
        {
#if DEBUG
                Game.LogTrivial("<DEBUG> " + text);
#endif
        }

        public static void LogDebug(string specific, string text)
        {
#if DEBUG
                Game.LogTrivial("[" + specific + "]<DEBUG> " + text);
#endif
        }


        public static void LogException(Exception ex)
        {
            Game.LogTrivial("<EXCEPTION> " + ex.Message + " :: " + ex.StackTrace);
        }

        public static void LogException(string specific, Exception ex)
        {
            Game.LogTrivial("[" + specific + "]<EXCEPTION> " + ex.Message + " :: " + ex.StackTrace);
        }


        public static void LogExceptionDebug(Exception ex)
        {
#if DEBUG
            Game.LogTrivial("<DEBUG | EXCEPTION> " + ex.Message + " :: " + ex.StackTrace);
#endif
        }

        public static void LogExceptionDebug(string specific, Exception ex)
        {
#if DEBUG
            Game.LogTrivial("[" + specific + "]<DEBUG | EXCEPTION> " + ex.Message + " :: " + ex.StackTrace);
#endif
        }

        public static void LogWelcome()
        {
            Game.Console.Print("================================================= Spike Strips V =================================================");
            Game.Console.Print("Created by:  alexguirre");
            Game.Console.Print("Version:  " + Common.GetFileVersion(@"Plugins\Spike Strips V.dll"));
            Game.Console.Print("RPH Version:  " + Common.GetFileVersion("RAGEPluginHook.exe"));
            Game.Console.Print();
            Game.Console.Print("Report any issues you have in the comments section and include the RagePluginHook.log");
            Game.Console.Print("Enjoy!");
            Game.Console.Print("================================================= Spike Strips V =================================================");
        }
    }
}
