using System;
using System.Drawing;
using System.Threading;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace In_Game_Settings_Buddy
{
    internal class Program
    {
        private static Menu _config;

        private static void Main(string[] args)
        {
            try
            {
                Loading.OnLoadingComplete += delegate
                {
                    var onLoadingComplete = new Thread(Loading_OnLoadingComplete);
                    onLoadingComplete.Start();
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void Loading_OnLoadingComplete()
        {
            _config = MainMenu.AddMenu("In-Game Settings Buddy", "ISB");

            _config.Add("AntiAFK", new KeyBind("Enable Anti AFK", Hacks.AntiAFK, KeyBind.BindTypes.PressToggle, 112));
            _config.Add("ExtendedZoom",
                new KeyBind("Enable Extended Zoom", Hacks.ZoomHack, KeyBind.BindTypes.PressToggle, 113));
            _config.Add("MovementHack",
                new KeyBind("Enable Movement Hack", Hacks.MovementHack, KeyBind.BindTypes.PressToggle, 114));
            _config.Add("InGameChat",
                new KeyBind("Enable InGame Chat", Hacks.IngameChat, KeyBind.BindTypes.PressToggle, 118));
            _config.Add("Watermark",
                new KeyBind("Draw Watermark", Hacks.RenderWatermark, KeyBind.BindTypes.PressToggle, 115));

            Chat.Print("In-Game Settings: <font color=\"#FFFFFF\">Loaded</font>", Color.FromArgb(255, 210, 68, 74));

            Game.OnTick += Game_OnTick;
        }

        private static void Game_OnTick(EventArgs args)
        {
            Hacks.AntiAFK = _config["AntiAFK"].Cast<KeyBind>().CurrentValue;
            Hacks.ZoomHack = _config["ExtendedZoom"].Cast<KeyBind>().CurrentValue;
            Hacks.MovementHack = _config["MovementHack"].Cast<KeyBind>().CurrentValue;
            Hacks.IngameChat = _config["InGameChat"].Cast<KeyBind>().CurrentValue;
            Hacks.RenderWatermark = _config["Watermark"].Cast<KeyBind>().CurrentValue;
        }
    }
}