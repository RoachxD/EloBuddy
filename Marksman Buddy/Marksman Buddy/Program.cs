using System.Drawing;
using System.Globalization;
using System.Threading;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace Marksman_Buddy
{
    internal class Program
    {
        public static Internal.Champion ChampionClass;
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
            Chat.Print("Marksman Buddy - <font color=\"#FFFFFF\">Loaded</font>", Color.FromArgb(255, 210, 68, 74));

            var championName = ObjectManager.Player.ChampionName.ToLower(CultureInfo.InvariantCulture);

            switch (championName)
            {
                case "ashe":
                    ChampionClass = new Internal.Champion();
                    break;
                case "caitlyn":
                    ChampionClass = new Internal.Champion();
                    break;
                case "corki":
                    ChampionClass = new Internal.Champion();
                    break;
                case "draven":
                    ChampionClass = new Internal.Champion();
                    break;
                case "ezreal":
                    ChampionClass = new Internal.Champion();
                    break;
                case "graves":
                    ChampionClass = new Internal.Champion();
                    break;
                case "gnar":
                    ChampionClass = new Internal.Champion();
                    break;
                case "jinx":
                    ChampionClass = new Internal.Champion();
                    break;
                case "kalista":
                    ChampionClass = new Internal.Champion();
                    break;
                case "kindred":
                    ChampionClass = new Internal.Champion();
                    break;
                case "kogmaw":
                    ChampionClass = new Internal.Champion();
                    break;
                case "lucian":
                    ChampionClass = new Internal.Champion();
                    break;
                case "missfortune":
                    ChampionClass = new Internal.Champion();
                    break;
                case "quinn":
                    ChampionClass = new Internal.Champion();
                    break;
                case "sivir":
                    ChampionClass = new Internal.Champion();
                    break;
                case "teemo":
                    ChampionClass = new Internal.Champion();
                    break;
                case "tristana":
                    ChampionClass = new Internal.Champion();
                    break;
                case "twitch":
                    ChampionClass = new Internal.Champion();
                    break;
                case "urgot":
                    ChampionClass = new Internal.Champion();
                    break;
                case "vayne":
                    ChampionClass = new Internal.Champion();
                    break;
                case "varus":
                    ChampionClass = new Internal.Champion();
                    break;
            }
        }
    }
}