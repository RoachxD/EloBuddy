using EloBuddy.SDK;
using EloBuddy.SDK.Menu;

namespace Marksman_Buddy.Internal
{
    internal class Variables
    {
        public static Menu InfoMenu;
        public static Menu Config;
        public static Menu Activator;
        public static bool ComboMode = Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo;
        public static bool HarassMode = Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass;
        public static bool LaneClearMode = Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LaneClear;
        public static bool LastHitMode = Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LastHit;
    }
}