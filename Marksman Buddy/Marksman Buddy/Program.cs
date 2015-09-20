using System;
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
using Core = Marksman_Buddy.Activator.Core;

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


            Variables.InfoMenu = MainMenu.AddMenu("Marksman Buddy", "MarksmanBuddy");
            Variables.InfoMenu.AddGroupLabel("Marksman Buddy");
            Variables.InfoMenu.AddLabel("Version: " + "1.0.0.0");
            Variables.InfoMenu.AddSeparator();
            Variables.InfoMenu.AddLabel("Creators: " + "Roach, newchild");

			
            Variables.Activator = Variables.InfoMenu.AddSubMenu("MB Activator", "MBActivator");
            Variables.Activator.AddGroupLabel("Summoner Spells");
            Variables.Activator.Add("Activator.UseHeal", new CheckBox("Use Heal"));
            Variables.Activator.Add("Activator.UseHealPercent", new Slider("Use Heal when under X Percent Health", 35));
            Variables.Activator.AddGroupLabel("Potions");
            Variables.Activator.Add("Activator.UseHPPot", new CheckBox("Use Healing Potions"));
            Variables.Activator.Add("Activator.UseHPPotPercent",
                new Slider("Use Healing Potions when under X Percent Health", 60));
            Variables.Activator.Add("Activator.UseMPPot", new CheckBox("Use Mana Potions"));
            Variables.Activator.Add("Activator.UseMPPotPercent",
                new Slider("Use Mana Potions when under X Percent Mana", 40));
            Variables.Activator.AddGroupLabel("Items");
            Variables.Activator.Add("Activator.UseCutlass", new CheckBox("Use Cutlass in Combo"));
            Variables.Activator.Add("Activator.UseYoumuus", new CheckBox("Use Youmuu's in Combo"));
            Variables.Activator.Add("Activator.UseBotrK", new CheckBox("Use Blade of the Ruined King in Combo"));
			
            var _Activator = new Core();
            Chat.Print("Marksman Buddy - <font color=\"#FFFFFF\">Loaded</font>", Color.FromArgb(255, 210, 68, 74));
            var championName = ObjectManager.Player.ChampionName.ToLower(CultureInfo.InvariantCulture);
            if (championName == "")
            {
                championName = "Core Error";
            }

            Variables.Config = Variables.InfoMenu.AddSubMenu(Player.Instance.ChampionName, Player.Instance.ChampionName);
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

            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            Variables.ComboMode = Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo);
            Variables.HarassMode = Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass);
            Variables.LaneClearMode = Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear);
            Variables.LastHitMode = Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit);
        }
    }
}
