using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Marksman_Buddy
{
    class MarksmanMenu
    {
        #region Main Menu
        //This function will create the Main Menu 
        private static Menu
            _commonMenu;
        #endregion

        #region Sub Menu
        /*This function will create the Sub Menu
        CommonCombo = Here goes the entire Combo Settings
        CommonDraw = Here goes the entire Draw Settings
        CommonHarass = Here goes the entire Harass Settings
        CommonFarm = Here goes the entire Farm/Jungle Settings
        CommonOtherFunctions = Here goes the entire Misc Settings*/

        public static Menu
            CommonCombo,
            CommonDraw,
            CommonHarass,
            CommonFarm;
        #endregion

        #region Menu Loading
        /*This allow the file to be charged in Program
        The function will load with this command in Program
        OwlyzeMenu.LoadMenu();
        Where OwlyzeMenu is the Class and LoadMenu is this function
        CommonPage = Will fill the _commonMenu as First Page
        CommonDrawPage = Will fill the CommonDraw as Second Page 
        CommonComboPage = Will fill the CommonCombo as Third Page
        CommonFarmPage = Will fill the CommonFarm as Fourth Page
        CommonHarassPage = Will fill the CommonHarass as Fifth Page*/
        public static void LoadMenu()
        {
            CommonPage();
            CommonDrawPage();
            CommonComboPage();
            CommonFarmPage();
            CommonHarassPage();
        }
        #endregion

        #region Info Page (Main)
        private static void CommonPage()
        {
        
        _commonMenu = MainMenu.AddMenu("Marksman AIO", "main");
            _commonMenu.AddGroupLabel("About this AIO:");
            _commonMenu.AddLabel(" Integers more the 20 champions");
            _commonMenu.AddLabel(" Made by iRaxe && Roach_ && NewChild");
            _commonMenu.AddSeparator();
            _commonMenu.AddGroupLabel("Hotkeys");
            _commonMenu.AddLabel(" - Use SpaceBar for Combo");
            _commonMenu.AddLabel(" - Use the key V For LaneClear/JungleClear");
            _commonMenu.AddLabel(" - Use the key T For Flee");
        }
        #endregion

        #region Drawings Page (Second Page)
        private static void CommonDrawPage()
        {
            CommonDraw = _commonMenu.AddSubMenu("Draw  settings", "Draw");
            CommonDraw.AddGroupLabel("Draw Settings:");
            CommonDraw.Add("nodraw",
                new CheckBox("No Display Drawing", false));
            CommonDraw.Add("onlyReady",
                new CheckBox("Display Only Ready"));
            CommonDraw.AddSeparator();
            CommonDraw.Add("draw.Q",
                new CheckBox("Draw Q Ability Range"));
            CommonDraw.Add("draw.W",
                new CheckBox("Draw W Ability Range"));
            CommonDraw.Add("draw.E",
                new CheckBox("Draw E Ability Range"));
            CommonDraw.Add("draw.R",
                new CheckBox("Draw R Ability Range"));
            CommonDraw.Add("draw.HP",
                  new CheckBox("Draw Predicted Damage"));
        }
        #endregion

        #region Combo Page (Third Page)
        private static void CommonComboPage()
        {
            CommonCombo = _commonMenu.AddSubMenu("Combo settings", "Combo");
            CommonCombo.AddGroupLabel("Combo settings:");
            CommonCombo.Add("combo.Q",
                new CheckBox("Use Q Ability"));
            CommonCombo.Add("combo.W",
                new CheckBox("Use W Ability"));
            CommonCombo.Add("combo.E",
                new CheckBox("Use E Ability"));
            CommonCombo.Add("combo.R",
                new CheckBox("Use R Ability"));
            CommonCombo.AddSeparator();
            CommonCombo.AddGroupLabel("Combo preferences:");
            CommonCombo.Add("combo.Q1",
                new Slider("Q Ability Overkill", 60, 0, 500));
            CommonCombo.Add("combo.W1",
                new Slider("W Ability Overkill", 60, 0, 500));
            CommonCombo.Add("combo.E1",
                new Slider("E Ability Overkill", 60, 0, 500));
        }
        #endregion

        #region Farm Page (Fourth Page)
        private static void CommonFarmPage()
        {
            CommonFarm = _commonMenu.AddSubMenu("Lane Clear Settings", "laneclear");
            CommonFarm.AddGroupLabel("Lane clear settings:");
            CommonFarm.Add("lc.Q",
                new CheckBox("Use Q Ability", false));
            CommonFarm.Add("lc.W",
                new CheckBox("Use W Ability", false));
            CommonFarm.Add("lc.E",
                new CheckBox("Use E Ability", false));
            CommonFarm.Add("lc.M",
                new Slider("Min. Mana for LaneClear Spells %", 30));

            _commonMenu.AddSubMenu("LastHit Settings", "lasthit");
            CommonFarm.Add("lc.H",
                new CheckBox("Prioritize Harass instead of LastHit"));
            CommonFarm.Add("lc.Q1",
                new CheckBox("Use Q Ability for LastHit", false));
            CommonFarm.Add("lc.W1",
                new CheckBox("Use W Ability for LastHit", false));
            CommonFarm.Add("lc.E1",
                new CheckBox("Use E Ability for LastHit", false));

            _commonMenu.AddSubMenu("JungleClear Settings", "jungle");
            CommonFarm.Add("jungle.Q",
                new CheckBox("Use Q Ability in Jungle (Q Spell)", false));
            CommonFarm.Add("jungle.W",
                new CheckBox("Use W Ability in Jungle (W Spell)", false));
            CommonFarm.Add("jungle.E",
                new CheckBox("Use E Ability in Jungle (E Spell)", false));
            CommonFarm.Add("jungle.M",
                new Slider("Min. Mana for JungleClear Spells %", 30));
        }
        #endregion

        #region Harass/KillSteal Page (Fifth Page)
        private static void CommonHarassPage()
        {
            CommonHarass = _commonMenu.AddSubMenu("Harass Settings", "hsettings");
            CommonHarass.Add("harass.Q",
                new CheckBox("Use Q Ability", false));
            CommonHarass.Add("harass.W",
                new CheckBox("Use W Ability", false));
            CommonHarass.Add("harass.E",
                new CheckBox("Use E Ability", false));
            CommonHarass.AddSeparator();
            CommonHarass.AddGroupLabel("Harass preferences:");
            CommonHarass.Add("harass.A",
                new CheckBox("Auto Harass", false));
            CommonHarass.Add("harass.M",
                new Slider("Min. Mana for Harass Spells %", 35));
            CommonHarass = _commonMenu.AddSubMenu("Killsteal Settings", "ksettings");
            CommonHarass.Add("killsteal.Q",
                new CheckBox("Use Q Ability", false));
            CommonHarass.Add("killsteal.W",
                new CheckBox("Use W Ability", false));
            CommonHarass.Add("killsteal.E",
                new CheckBox("Use E Ability", false));
        }
        #endregion
        
        #region Info
        //Now we will check every CheckBox and Slider of each page and value here
        #endregion

        #region Drawings
        //Checkbox for Nodraw
        public static bool Nodraw()
        {
            return CommonDraw["nodraw"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for OnlyReady Spells in Drawing
        public static bool OnlyReady()
        {
            return CommonDraw["onlyReady"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Draw the Q Spell Range
        public static bool DrawingsQ()
        {
            return CommonDraw["draw.Q"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Draw the W Spell Range
        public static bool DrawingsW()
        {
            return CommonDraw["draw.W"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Draw the E Spell Range
        public static bool DrawingsE()
        {
            return CommonDraw["draw.E"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Draw the R spell Range
        public static bool DrawingsR()
        {
            return CommonDraw["draw.R"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Draw the ideal positions of trinkets
        public static bool DrawingsHp()
        {
            return CommonDraw["draw.HP"].Cast<CheckBox>().CurrentValue;
        }
        #endregion

        #region Combo Preferences
        //Checkbox for Q Ability (Q Spell)
        public static bool ComboQ()
        {
            return CommonCombo["combo.Q"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for W Ability (W Spell)
        public static bool ComboW()
        {
            return CommonCombo["combo.W"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for E Ability (E Spell)
        public static bool ComboE()
        {
            return CommonCombo["combo.E"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for R Ability (R Spell)
        public static bool ComboR()
        {
            return CommonCombo["combo.R"].Cast<CheckBox>().CurrentValue;
        }
        //Slider for Q Ability (Q Spell) Overkill
        public static float ComboQ1()
        {
            return CommonCombo["combo.Q1"].Cast<Slider>().CurrentValue;
        }
        //Slider for W Ability (W Spell) Overkill
        public static float ComboW1()
        {
            return CommonCombo["combo.W1"].Cast<Slider>().CurrentValue;
        }
        //Slider for E Ability (E Spell) Overkill
        public static float ComboE1()
        {
            return CommonCombo["combo.E1"].Cast<Slider>().CurrentValue;
        }
        #endregion

        #region Laneclear
        //Checkbox for Prioritize Harass instead of LastHit
        public static bool LcH()
        {
            return CommonFarm["lc.H"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Q Ability (Q Spell) in Farm
        public static bool LcQ()
        {
            return CommonFarm["lc.Q"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Q Ability (Q Spell) in Lasthit 
        public static bool LcQ1()
        {
            return CommonFarm["lc.Q1"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for W Ability (W Spell) in Farm
        public static bool LcW()
        {
            return CommonFarm["lc.W"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for W Ability (W Spell) in Lasthit
        public static bool LcW1()
        {
            return CommonFarm["lc.W1"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for E Ability (E Spell) in Farm
        public static bool LcE()
        {
            return CommonFarm["lc.E"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for E Ability (E Spell) in Lasthit
        public static bool LcE1()
        {
            return CommonFarm["lc.E1"].Cast<CheckBox>().CurrentValue;
        }
        //Slider for limit mana in Farm
        public static float LcM()
        {
            return CommonFarm["lc.M"].Cast<Slider>().CurrentValue;
        }
        #endregion

        #region Jungle
        //Checkbox for Q Ability (Q Spell) in Jungle
        public static bool JungleQ()
        {
            return CommonFarm["jungle.Q"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for W Ability (W Spell) in Jungle
        public static bool JungleW()
        {
            return CommonFarm["jungle.W"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for E Ability (E Spell) in Jungle
        public static bool JungleE()
        {
            return CommonFarm["jungle.E"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for Limit passive stacks in Jungle
        public static bool JungleM()
        {
            return CommonFarm["jungle.M"].Cast<CheckBox>().CurrentValue;
        }
        #endregion

        #region Harass
        //Checkbox for Q Ability (Q Spell)
        public static bool HarassQ()
        {
            return CommonHarass["harass.Q"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for W Ability (W Spell)
        public static bool HarassW()
        {
            return CommonHarass["harass.W"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for E Ability (E Spell)
        public static bool HarassE()
        {
            return CommonHarass["harass.E"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for AutoHarass
        public static bool HarassA()
        {
            return CommonHarass["harass.A"].Cast<CheckBox>().CurrentValue;
        }
        //Slider for Limit mana in Harass
        public static float HarassM()
        {
            return CommonHarass["harass.M"].Cast<Slider>().CurrentValue;
        }
       #endregion

        #region KillSteal
        //Checkbox for Q Ability (Q Spell) in KillSteal
        public static bool KillstealQ()
        {
            return CommonHarass["killsteal.Q"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for W Ability (W Spell) in KillSteal
        public static bool KillstealW()
        {
            return CommonHarass["killsteal.W"].Cast<CheckBox>().CurrentValue;
        }
        //Checkbox for E Ability (E Spell) in KillSteal
        public static bool KillstealE()
        {
            return CommonHarass["killsteal.E"].Cast<CheckBox>().CurrentValue;
        }
        #endregion
    }
}
