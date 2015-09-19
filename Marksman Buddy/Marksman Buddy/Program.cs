using System.Drawing;
using System.Globalization;
using System.Threading;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Marksman_Buddy.Internal;
using Marksman_Buddy.Plugins;
using Champion = Marksman_Buddy.Internal.Champion;
using System;


namespace Marksman_Buddy
{
    internal class Program
    {
        public static PluginBase ChampionPlugin;

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += delegate
            {
                var onLoadingComplete = new Thread(Loading_OnLoadingComplete);
                onLoadingComplete.Start();
            };
        }

        private static void Loading_OnLoadingComplete()
        {
            Bootstrap.Init(null);
            Variables.Config = MainMenu.AddMenu("Marksman Buddy", "MarksmanBuddy");
			Variables.Settings = MainMenu.AddMenu("Marksman Buddy Settings", "MarksmanBuddy Settings");
			Variables.Settings.AddGroupLabel("Marksman Buddy");
			Variables.Settings.AddLabel("Version: " + "1.0.0.0");
			Variables.Settings.AddSeparator();
			Variables.Settings.AddLabel("Creators: " + "Roach, newchild");
			Variables.Settings.AddSeparator();
			Variables.Settings.AddGroupLabel("Prediction");
			Variables.Settings.Add("GlobalPrediction", new Slider("Prediction accuracy", Convert.ToInt32(Variables.Hitchance), 0, 8));
			Chat.Print("Marksman Buddy - <font color=\"#FFFFFF\">Loaded</font>", Color.FromArgb(255, 210, 68, 74));
            var championName = ObjectManager.Player.ChampionName.ToLower(CultureInfo.InvariantCulture);
            switch (championName)
            {
                case "ashe":
                    ChampionPlugin = new Ashe();
                    break;
                case "caitlyn":
                    ChampionPlugin = new Champion();
                    break;
                case "corki":
                    ChampionPlugin = new Corki();
                    break;
                case "draven":
                    ChampionPlugin = new Champion();
                    break;
                case "ezreal":
                    ChampionPlugin = new Champion();
                    break;
                case "graves":
                    ChampionPlugin = new Champion();
                    break;
                case "gnar":
                    ChampionPlugin = new Champion();
                    break;
                case "jinx":
                    ChampionPlugin = new Champion();
                    break;
                case "kalista":
                    ChampionPlugin = new Champion();
                    break;
                case "kindred":
                    ChampionPlugin = new Champion();
                    break;
                case "kogmaw":
                    ChampionPlugin = new Champion();
                    break;
                case "lucian":
                    ChampionPlugin = new Champion();
                    break;
                case "missfortune":
                    ChampionPlugin = new Champion();
                    break;
                case "quinn":
                    ChampionPlugin = new Champion();
                    break;
                case "sivir":
                    ChampionPlugin = new Champion();
                    break;
                case "teemo":
                    ChampionPlugin = new Champion();
                    break;
                case "tristana":
                    ChampionPlugin = new Champion();
                    break;
                case "twitch":
                    ChampionPlugin = new Twitch();
                    break;
                case "urgot":
                    ChampionPlugin = new Champion();
                    break;
                case "vayne":
                    ChampionPlugin = new Champion();
                    break;
                case "varus":
                    ChampionPlugin = new Champion();
                    break;
            }
        }
    }
}
