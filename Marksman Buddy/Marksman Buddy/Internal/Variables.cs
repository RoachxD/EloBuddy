using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu;

namespace Marksman_Buddy.Internal
{
    class Variables
    {
        public static Menu Config;
		public static Menu Settings;
        public static bool ComboMode = Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo;
        public static bool HarassMode = Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass;
        public static bool LaneClearMode = Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LaneClear;
        public static bool LastHitMode = Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LastHit;
		public static HitChance HitChance = HitChance.High;

		internal static void OnValueChange(EloBuddy.SDK.Menu.Values.ValueBase<int> sender, EloBuddy.SDK.Menu.Values.ValueBase<int>.ValueChangeArgs args)
		{
			HitChance = (HitChance)sender.CurrentValue;
			sender.DisplayName = "Hitchance: " + Variables.HitChance.ToString();
		}
	}
}
