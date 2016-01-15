using System;
using EloBuddy;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Utils;

// ReSharper disable UnusedParameter.Local

namespace In_Game_Settings_Buddy
{
    internal class Program
    {
        private static Menu _config;
		private static MovementHackHotfix MovementHotFix;
		private static TowerRangesHotfix TowerHotFix;
		private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
            AppDomain.CurrentDomain.UnhandledException +=
                delegate(object sender, UnhandledExceptionEventArgs args1)
                {
                    Logger.Log(LogLevel.Error, args1.ExceptionObject.ToString());
                };
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            _config = MainMenu.AddMenu("In-Game Settings Buddy", "ISB");
			_config.AddLabel("Movement Hack HotFix by newchild");
            _config.Add("AntiAFK", new KeyBind("Enable Anti AFK", Hacks.AntiAFK, KeyBind.BindTypes.PressToggle, 112));
            _config.Add("ExtendedZoom",
                new KeyBind("Enable Extended Zoom", Hacks.ZoomHack, KeyBind.BindTypes.PressToggle, 113));
            _config.Add("ExtendedZoomValue", new Slider("Set the max Zoom Value:", 2250, 2250, 5000));
            _config.Add("MovementHack",
                new KeyBind("Enable Movement Hack", Hacks.MovementHack, KeyBind.BindTypes.PressToggle, 114));
            _config.Add("TowerRanges", new KeyBind("Show Tower Ranges", false, KeyBind.BindTypes.PressToggle, 120));
            _config.Add("InGameChat",
                new KeyBind("Enable InGame Chat", Hacks.IngameChat, KeyBind.BindTypes.PressToggle, 118));
            _config.Add("Watermark",
                new KeyBind("Draw Watermark", Hacks.RenderWatermark, KeyBind.BindTypes.PressToggle, 115));
			MovementHotFix  = new MovementHackHotfix();
			TowerHotFix  = new TowerRangesHotfix();
			MovementHotFix.Enabled = false;
			TowerHotFix.Enabled = false;
            Game.OnTick += Game_OnTick;
            _config["ExtendedZoom"].Cast<KeyBind>().OnValueChange += ExtendedZoom_OnValueChange;
            _config["ExtendedZoomValue"].Cast<Slider>().OnValueChange += ExtendedZoomValue_OnValueChange;
        }

        private static void Game_OnTick(EventArgs args)
        {
            Hacks.AntiAFK = _config["AntiAFK"].Cast<KeyBind>().CurrentValue;
            Hacks.ZoomHack = _config["ExtendedZoom"].Cast<KeyBind>().CurrentValue;
			MovementHotFix.Enabled = _config["MovementHack"].Cast<KeyBind>().CurrentValue;
			TowerHotFix.Enabled = _config["TowerRanges"].Cast<KeyBind>().CurrentValue;
            Hacks.IngameChat = _config["InGameChat"].Cast<KeyBind>().CurrentValue;
            Hacks.RenderWatermark = _config["Watermark"].Cast<KeyBind>().CurrentValue;
        }

        private static void ExtendedZoom_OnValueChange(ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs args)
        {
            Camera.SetZoomDistance(args.NewValue ? _config["ExtendedZoomValue"].Cast<Slider>().CurrentValue : 2250);
        }

        private static void ExtendedZoomValue_OnValueChange(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
        {
            if (!_config["ExtendedZoom"].Cast<KeyBind>().CurrentValue)
            {
                return;
            }

            Camera.SetZoomDistance(args.NewValue);
        }
    }
}