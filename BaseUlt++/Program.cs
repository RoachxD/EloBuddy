using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace BaseUltPlusPlus
{
    public class Program
    {
        public static Menu BaseUltMenu { get; set; }

        public static void Main(string[] args)
        {
            // Wait till the name has fully loaded
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            //Menu
            BaseUltMenu = MainMenu.AddMenu("BaseUlt++", "BUP");
            BaseUltMenu.AddGroupLabel("BaseUlt++ General");
            BaseUltMenu.AddSeparator();
            BaseUltMenu.Add("baseult", new CheckBox("BaseUlt"));
            BaseUltMenu.Add("showrecalls", new CheckBox("Show Recalls"));
            BaseUltMenu.Add("showallies", new CheckBox("Show Allies"));
            BaseUltMenu.Add("showenemies", new CheckBox("Show Enemies"));
            BaseUltMenu.Add("checkcollision", new CheckBox("Check Collision"));
            BaseUltMenu.AddSeparator();
            BaseUltMenu.Add("timeLimit", new Slider("FOW Time Limit (SEC)", 0, 0, 120));
            BaseUltMenu.AddSeparator();
            BaseUltMenu.Add("nobaseult", new KeyBind("No BaseUlt while", false, KeyBind.BindTypes.HoldActive, 32));
            BaseUltMenu.AddSeparator();
            BaseUltMenu.Add("x", new Slider("Offset X", 0, -500, 500));
            BaseUltMenu.Add("y", new Slider("Offset Y", 0, -500, 500));
            BaseUltMenu.AddGroupLabel("BaseUlt++ Targets");
            foreach (var unit in EntityManager.Heroes.Enemies)
            {
                BaseUltMenu.Add("target" + unit.ChampionName,
                    new CheckBox(string.Format("{0} ({1})", unit.ChampionName, unit.Name)));
            }

            BaseUltMenu.AddGroupLabel("BaseUlt++ Credits");
            BaseUltMenu.AddLabel("By: LunarBlue (Fixed by: Roach_)");
            BaseUltMenu.AddLabel("Testing: FinnDev, MrOwl");

            // Initialize the Addon
            OfficialAddon.Initialize();

            // Listen to the two main events for the Addon
            Game.OnUpdate += args1 => OfficialAddon.Game_OnUpdate();
            Drawing.OnEndScene += args1 => OfficialAddon.Drawing_OnEndScene();
            Teleport.OnTeleport += OfficialAddon.Teleport_OnTeleport;
        }
    }
}