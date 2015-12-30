using EloBuddy;
using EloBuddy.SDK.Events;
using MarksmanAIO.Champions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarksmanAIO
{
    class MarksmanAIO
    {
        public static AIOChampion champion;
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
            Events.Init();
        }
        static void Loading_OnLoadingComplete(EventArgs args)
        {
            switch (Player.Instance.ChampionName)
            {
                case "Ashe": champion = new Ashe(); break;
                case "Caitlyn": champion = new Caitlyn(); break;
                case "Corki": champion = new Corki(); break;
                case "Draven": champion = new Draven(); break;
                case "Ezreal": champion = new Ezreal(); break;
                case "Graves": champion = new Graves(); break;
                case "Jinx": champion = new Jinx(); break;
                case "Kalista": champion = new Kalista(); break;
                case "Kindred": champion = new Kindred(); break;
                case "Kog'Maw": champion = new KogMaw(); break;
                case "Lucian": champion = new Lucian(); break;
                case "MissFortune": champion = new MissFortune(); break;
                case "Quinn": champion = new Quinn(); break;
                case "Sivir": champion = new Sivir(); break;
                case "Teemo": champion = new Teemo(); break;
                case "Tristana": champion = new Tristana(); break;
                case "Twitch": champion = new Twitch(); break;
                case "Urgot": champion = new Urgot(); break;
                case "Varus": champion = new Varus(); break;
                case "Vayne": champion = new Vayne(); break;
                //case "Vayne": champion = new Vayne(); break; //etc
            }
            if (champion != null)
            {
                champion.Init();
            }
            else
            {
                Chat.Print("MarksmanAIO doesn't support this Champion!");
            }
        }
    }
}
