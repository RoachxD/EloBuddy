using System.Drawing;
using System.Globalization;
using System.Threading;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using Marksman_Buddy.Internal;

namespace Marksman_Buddy
{
    internal class Program
    {
        public static Internal.PluginBase ChampionPlugin;
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
            Variables.Config.AddGroupLabel("Marksman Buddy");
            Variables.Config.AddLabel("Version: " + "1.0.0.0");
            Variables.Config.AddSeparator();
            Variables.Config.AddLabel("Creators: " + "Roach, newchild");


            Chat.Print("Marksman Buddy - <font color=\"#FFFFFF\">Loaded</font>", Color.FromArgb(255, 210, 68, 74));

            var championName = ObjectManager.Player.ChampionName.ToLower(CultureInfo.InvariantCulture);
            switch (championName)
            {
                case "ashe":
                    ChampionPlugin = new Internal.Champion();
                    break;
                case "caitlyn":
                    ChampionPlugin = new Internal.Champion();
                    break;
                case "corki":
                    ChampionPlugin = new Internal.Champion();
                    break;
                case "draven":
                    ChampionPlugin = new Internal.Champion();
                    break;
                case "ezreal":
                    ChampionPlugin = new Internal.Champion();
                    break;
                case "graves":
                    ChampionPlugin = new Internal.Champion();
                    break;
                case "gnar":
                    ChampionPlugin = new Internal.Champion();
                    break;
                case "jinx":
                    ChampionPlugin = new Internal.Champion();
                    break;
                case "kalista":
                    ChampionPlugin = new Internal.Champion();
                    break;
                case "kindred":
                    ChampionPlugin = new Internal.Champion();
                    break;
                case "kogmaw":
                    ChampionPlugin = new Internal.Champion();
                    break;
                case "lucian":
                    ChampionPlugin = new Internal.Champion();
                    break;
                case "missfortune":
                    ChampionPlugin = new Internal.Champion();
                    break;
                case "quinn":
                    ChampionPlugin = new Internal.Champion();
                    break;
                case "sivir":
                    ChampionPlugin = new Internal.Champion();
                    break;
                case "teemo":
                    ChampionPlugin = new Internal.Champion();
                    break;
                case "tristana":
                    ChampionPlugin = new Internal.Champion();
                    break;
                case "twitch":
					ChampionPlugin = new Plugin.Twitch();
                    break;
                case "urgot":
                    ChampionPlugin = new Internal.Champion();
                    break;
                case "vayne":
                    ChampionPlugin = new Internal.Champion();
                    break;
                case "varus":
                    ChampionPlugin = new Internal.Champion();
                    break;
            }
        }
    }
}