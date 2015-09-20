using System;
using System.Drawing;
using System.Globalization;
using System.Threading;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using Marksman_Buddy.Internal;
using Marksman_Buddy.Plugins;
using Champion = Marksman_Buddy.Internal.Champion;

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
			Chat.Print("Don't try to sneak peak Kappa");
			return;
			
			Bootstrap.Init(null);

			Variables.Activator = MainMenu.AddMenu("MB Activator", "MarksmanBuddy");
			Variables.Activator.AddGroupLabel("Summoner Spells");
			Variables.Activator.Add("MBActivator.UseHeal", new CheckBox("Use Heal"));
			Variables.Activator.Add("MBActivator.UseHealPercent", new Slider("Use Heal when under X Percent Health"));
			Variables.Activator.AddGroupLabel("Potions");
			Variables.Activator.Add("MBActivator.UseHPPot", new CheckBox("Use Healing Potions"));
			Variables.Activator.Add("MBActivator.UseHPPotPercent", new Slider("Use Healing Potions when under X Percent Health"));
			Variables.Activator.Add("MBActivator.UseMPPot", new CheckBox("Use Mana Potions"));
			Variables.Activator.Add("MBActivator.UseMPPotPercent", new Slider("Use Mana Potions when under X Percent Mana"));
			Variables.Activator.AddGroupLabel("Items");
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
            Variables.ComboMode = Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo;
            Variables.HarassMode = Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass;
            Variables.LaneClearMode = Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LaneClear;
            Variables.LastHitMode = Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LastHit;
        }
    }
}
