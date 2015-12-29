using System;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Utils;

namespace Marksman_Buddy
{
    internal class Loader
    {
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
            AppDomain.CurrentDomain.UnhandledException +=
                delegate(object sender, UnhandledExceptionEventArgs exceptionArgs)
                {
                    Logger.Log(LogLevel.Error, exceptionArgs.ExceptionObject.ToString());
                };
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            ChampionLogic.Initialize();
            MarksmanMenu.LoadMenu();
        }
    }
}