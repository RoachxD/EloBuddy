using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace Garen_Buddy.Internal
{
    internal class Menu
    {
        public static EloBuddy.SDK.Menu.Menu MainMenu;
        public static EloBuddy.SDK.Menu.Menu ComboMenu;
        public static EloBuddy.SDK.Menu.Menu HarassMenu;
        public static EloBuddy.SDK.Menu.Menu ClearMenu;
        public static EloBuddy.SDK.Menu.Menu MiscMenu;
        public static EloBuddy.SDK.Menu.Menu DrawMenu;

        public static void Initialize()
        {
            MainMenu = EloBuddy.SDK.Menu.MainMenu.AddMenu("Garen Buddy", "GarenBuddy");
            MainMenu.AddGroupLabel("Garen Buddy");
            MainMenu.AddLabel("Version: " + "1.0.0.2");
            MainMenu.AddSeparator();
            MainMenu.AddLabel("Creators: " + "Roach");

            Combo.Initialize();

            Harass.Initialize();

            Clear.Initialize();

            var smiteSpell = Player.Instance.Spellbook.GetSpell(Spells.Smite);
            if (smiteSpell != null)
            {
                Smite.Initialize();
            }

            Misc.Initialize();

            KillSteal.Initialize();

            Draw.Initialize();
        }

        public static class Combo
        {
            private static CheckBox _q;
            private static CheckBox _w;
            private static CheckBox _e;
            private static CheckBox _r;

            public static bool Q
            {
                get { return _q.CurrentValue; }
            }

            public static bool W
            {
                get { return _w.CurrentValue; }
            }

            public static bool E
            {
                get { return _e.CurrentValue; }
            }

            public static bool R
            {
                get { return _r.CurrentValue; }
            }

            public static void Initialize()
            {
                ComboMenu = MainMenu.AddSubMenu("Combo", "Combo");
                ComboMenu.AddGroupLabel("Combo Options");
                _q = ComboMenu.Add("Combo.Q", new CheckBox("Use Q"));
                _w = ComboMenu.Add("Combo.W", new CheckBox("Use W"));
                _e = ComboMenu.Add("Combo.E", new CheckBox("Use E"));
                _r = ComboMenu.Add("Combo.R", new CheckBox("Use R"));
                ComboMenu.AddGroupLabel("R Targets");
                foreach (var hero in EntityManager.Heroes.Enemies)
                {
                    ComboMenu.Add("R." + hero.ChampionName, new CheckBox(hero.ChampionName));
                }
            }
        }

        public static class Harass
        {
            private static CheckBox _q;
            private static CheckBox _w;
            private static CheckBox _e;

            public static bool Q
            {
                get { return _q.CurrentValue; }
            }

            public static bool W
            {
                get { return _w.CurrentValue; }
            }

            public static bool E
            {
                get { return _e.CurrentValue; }
            }

            public static void Initialize()
            {
                HarassMenu = MainMenu.AddSubMenu("Harass", "Harass");
                HarassMenu.AddGroupLabel("Harass Options");
                _q = HarassMenu.Add("Harass.Q", new CheckBox("Use Q"));
                _w = HarassMenu.Add("Harass.W", new CheckBox("Use W"));
                _e = HarassMenu.Add("Harass.E", new CheckBox("Use E"));
            }
        }

        public static class Clear
        {
            private static CheckBox _e;

            public static bool E
            {
                get { return _e.CurrentValue; }
            }

            public static void Initialize()
            {
                ClearMenu = MainMenu.AddSubMenu("Clear", "Clear");
                ClearMenu.AddGroupLabel("Clear Options");
                _e = ClearMenu.Add("Clear.E", new CheckBox("Use E"));
            }
        }

        public static class Smite
        {
            private static CheckBox _enable;
            private static CheckBox _baron;
            private static CheckBox _dragon;
            private static CheckBox _red;
            private static CheckBox _blue;
            private static CheckBox _krug;
            private static CheckBox _gromp;
            private static CheckBox _raptor;
            private static CheckBox _wolf;

            public static bool? Enable
            {
                get { return _enable.CurrentValue; }
            }

            public static bool? Baron
            {
                get { return _baron.CurrentValue; }
            }

            public static bool? Dragon
            {
                get { return _dragon.CurrentValue; }
            }

            public static bool? Red
            {
                get { return _red.CurrentValue; }
            }

            public static bool? Blue
            {
                get { return _blue.CurrentValue; }
            }

            public static bool? Krug
            {
                get { return _krug.CurrentValue; }
            }

            public static bool? Gromp
            {
                get { return _gromp.CurrentValue; }
            }

            public static bool? Raptor
            {
                get { return _raptor.CurrentValue; }
            }

            public static bool? Wolf
            {
                get { return _wolf.CurrentValue; }
            }

            public static void Initialize()
            {
                ClearMenu.AddGroupLabel("Smite Mobs");
                _enable = ClearMenu.Add("Smite.Enable", new CheckBox("Use Smite"));
                ClearMenu.AddSeparator();
                _baron = ClearMenu.Add("Smite.Baron", new CheckBox("Baron Nashor"));
                _dragon = ClearMenu.Add("Smite.Dragon", new CheckBox("Dragon"));
                _red = ClearMenu.Add("Smite.Red", new CheckBox("Red Brambleback"));
                _blue = ClearMenu.Add("Smite.Blue", new CheckBox("Blue Sentinel"));
                _krug = ClearMenu.Add("Smite.Krug", new CheckBox("Ancient Krug"));
                _gromp = ClearMenu.Add("Smite.Gromp", new CheckBox("Gromp"));
                _raptor = ClearMenu.Add("Smite.Raptor", new CheckBox("Crimson Raptor"));
                _wolf = ClearMenu.Add("Smite.Wolf", new CheckBox("Greater Murk Wolf"));
            }
        }

        public static class Misc
        {
            private static CheckBox _qAfterAa;
            private static CheckBox _debugMode;

            public static bool QAfterAa
            {
                get { return _qAfterAa.CurrentValue; }
            }

            public static bool DebugMode
            {
                get { return _debugMode.CurrentValue; }
            }

            public static void Initialize()
            {
                MiscMenu = MainMenu.AddSubMenu("Misc", "Misc");
                MiscMenu.AddGroupLabel("Misc Options");
                _qAfterAa = MiscMenu.Add("Misc.QAfterAA", new CheckBox("Use Q after AA", false));
                _debugMode = MiscMenu.Add("Misc.DebugMode", new CheckBox("Debug Mode", false));
            }
        }

        public static class KillSteal
        {
            private static CheckBox _q;
            private static CheckBox _r;
            private static CheckBox _ignite;
            private static CheckBox _smite;

            public static bool Q
            {
                get { return _q.CurrentValue; }
            }

            public static bool R
            {
                get { return _r.CurrentValue; }
            }

            public static bool Ignite
            {
                get { return _ignite.CurrentValue; }
            }

            public static bool Smite
            {
                get { return _smite.CurrentValue; }
            }

            public static void Initialize()
            {
                MiscMenu.AddGroupLabel("Kill Steal");
                _q = MiscMenu.Add("KillSteal.Q", new CheckBox("Use Q"));
                _r = MiscMenu.Add("KillSteal.R", new CheckBox("Use R"));
                _ignite = MiscMenu.Add("KillSteal.Ignite", new CheckBox("Use Ignite"));
                _smite = MiscMenu.Add("KillSteal.Smite", new CheckBox("Use Smite"));
            }
        }

        public static class Draw
        {
            private static CheckBox _e;
            private static CheckBox _r;
            private static CheckBox _smite;
            private static CheckBox _damageHpBar;

            public static bool E
            {
                get { return _e.CurrentValue; }
            }

            public static bool R
            {
                get { return _r.CurrentValue; }
            }

            public static bool Smite
            {
                get { return _smite.CurrentValue; }
            }

            public static bool DamageHpBar
            {
                get { return _damageHpBar.CurrentValue; }
            }

            public static void Initialize()
            {
                DrawMenu = MainMenu.AddSubMenu("Draw", "Draw");
                DrawMenu.AddGroupLabel("Draw Options");
                _e = DrawMenu.Add("Draw.E", new CheckBox("E Range"));
                _r = DrawMenu.Add("Draw.R", new CheckBox("R Range"));
                _smite = DrawMenu.Add("Draw.Smite", new CheckBox("Draw Smite Status"));
                _damageHpBar = DrawMenu.Add("Draw.DamageHPBar", new CheckBox("Draw Damage on HP Bar"));
            }
        }
    }
}