using System;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;

// ReSharper disable MemberHidesStaticFromOuterClass

namespace Item_Swapper_Buddy.Internal
{
    internal class MenuManager
    {
        public static Menu MainMenu;
        public static Menu HowToUseMenu;
        public static Menu HumanizerMenu;

        public static void Initialize()
        {
            MainMenu = EloBuddy.SDK.Menu.MainMenu.AddMenu("Item Swapper Buddy", "ItemSwapperBuddy");
            MainMenu.AddGroupLabel("Changelog");
            MainMenu.AddLabel("  6.17.0.1");
            MainMenu.AddLabel("    <font color=\"#716842\">- Added a Menu and a Humanizer.</font>", 23);
            MainMenu.AddLabel("  6.17.0.0");
            MainMenu.AddLabel("    - Added \"Reverse - Swapping\".", 23);
            MainMenu.AddLabel("  6.16.0.0 - \"Back on track!\"");
            MainMenu.AddLabel("    - Fixed in case it didn't work.", 23);
            MainMenu.AddLabel("  1.0.0.0");
            MainMenu.AddLabel("    - First Release.", 23);

            HowToUse.Initialize();
            Humanizer.Initialize();
        }

        public class HowToUse
        {
            public static void Initialize()
            {
                HowToUseMenu = MainMenu.AddSubMenu("How to Use");
                HowToUseMenu.AddGroupLabel("Key-binds");
                HowToUseMenu.AddLabel("  Numpad 4 - Item Slot 1");
                HowToUseMenu.AddLabel("  Numpad 5 - Item Slot 2");
                HowToUseMenu.AddLabel("  Numpad 6 - Item Slot 3");
                HowToUseMenu.AddLabel("  Numpad 1 - Item Slot 4");
                HowToUseMenu.AddLabel("  Numpad 2 - Item Slot 5");
                HowToUseMenu.AddLabel("  Numpad 3 - Item Slot 6");
                HowToUseMenu.AddLabel("  Numpad 0 - Reset");
            }
        }

        public class Humanizer
        {
            private static CheckBox _enable;
            private static ComboBox _type;
            private static Slider _value;
            private static Slider _minValue;
            private static Slider _maxValue;

            public static bool Enable
            {
                get { return _enable.CurrentValue; }
            }

            public static int Type
            {
                get { return _type.SelectedIndex; }
            }

            public static int Value
            {
                get { return _value.CurrentValue; }
            }

            public static int MinValue
            {
                get { return _minValue.CurrentValue; }
            }

            public static int MaxValue
            {
                get { return _maxValue.CurrentValue; }
            }

            public static void Initialize()
            {
                HumanizerMenu = MainMenu.AddSubMenu("Humanizer");
                HumanizerMenu.AddGroupLabel("Humanizer");
                _enable = HumanizerMenu.Add("Enable", new CheckBox("Enable", false));
                _type = HumanizerMenu.Add("Type", new ComboBox("Choose type:", new[] {"Exact", "Randomized"}, 1));
                if (_type.SelectedIndex == 0)
                {
                    _value = HumanizerMenu.Add("Value", new Slider("Delay in seconds:", 2, 1, 5));
                }
                else
                {
                    _minValue = HumanizerMenu.Add("MinValue", new Slider("Minimum delay in seconds:", 1, 1, 5));
                    _maxValue = HumanizerMenu.Add("MaxValue", new Slider("Maximum delay in seconds:", 5, 1, 5));
                }

                _type.OnValueChange += delegate(ValueBase<int> sender, ValueBase<int>.ValueChangeArgs args)
                {
                    if (args.NewValue == args.OldValue)
                    {
                        return;
                    }

                    if (args.NewValue == 0)
                    {
                        HumanizerMenu.Remove("MinValue");
                        HumanizerMenu.Remove("MaxValue");
                        _value = HumanizerMenu.Add("Value", new Slider("Delay in seconds:", 2, 1, 5));
                    }

                    if (args.NewValue == 1)
                    {
                        HumanizerMenu.Remove("Value");
                        _minValue = HumanizerMenu.Add("MinValue", new Slider("Minimum delay in seconds:", 1, 1, 5));
                        _maxValue = HumanizerMenu.Add("MaxValue", new Slider("Maximum delay in seconds:", 5, 1, 5));
                    }
                };
            }
        }
    }
}