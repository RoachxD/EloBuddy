#region

using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;

#endregion

namespace Marksman_Buddy.Internal
{
    internal class Champion
    {
        public Menu Config;
        public bool ComboActive = Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Combo;
        public bool HarassActive = Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.Harass;
        public bool LaneClearActive = Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LaneClear;
        public bool LastHit = Orbwalker.ActiveModesFlags == Orbwalker.ActiveModes.LastHit;

        public virtual bool ComboMenu(Menu config)
        {
            return false;
        }

        public virtual bool HarassMenu(Menu config)
        {
            return false;
        }

        public virtual bool LaneClearMenu(Menu config)
        {
            return false;
        }

        public virtual bool MiscMenu(Menu config)
        {
            return false;
        }

        public virtual bool ExtrasMenu(Menu config)
        {
            return false;
        }

        public virtual bool DrawingMenu(Menu config)
        {
            return false;
        }

        public virtual bool MainMenu(Menu config)
        {
            return false;
        }

        public virtual void Drawing_OnDraw(EventArgs args)
        {
        }

        public virtual void Game_OnGameUpdate(EventArgs args)
        {
        }

        public virtual void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
        }

        public virtual void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
        }
    }
}
