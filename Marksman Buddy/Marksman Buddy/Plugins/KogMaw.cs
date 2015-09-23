using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using Marksman_Buddy.Internal;

namespace Marksman_Buddy.Plugins
{
    internal class KogMaw : PluginBase
    {
        private Spell.Skillshot _E;
        private Spell.Skillshot _Q;
        private Spell.Skillshot _R;
        private Spell.Active _W;

        public KogMaw()
        {
            _LoadMenu();
            _LoadSpells();
            Game.OnTick += Game_OnTick;
        }

        public override void Game_OnTick(EventArgs args)
        {
            if (Variables.ComboMode)
            {
                _Combo();
            }

            _KS();
        }

        private void _KS()
        {
            throw new NotImplementedException();
        }

        public override void _Combo()
        {
            if (Variables.Config["UseQInCombo"].Cast<CheckBox>().CurrentValue && Orbwalker.GetTarget() != null)
            {
                _Q.Cast((Obj_AI_Base) Orbwalker.GetTarget());
            }
            if (Variables.Config["UseWInCombo"].Cast<CheckBox>().CurrentValue && Orbwalker.GetTarget() != null)
            {
                _W.Cast();
            }
            var _ETarget = TargetSelector.GetTarget(_E.Range, DamageType.Magical);
            if (Variables.Config["UseEInCombo"].Cast<CheckBox>().CurrentValue && _ETarget != null && _ETarget.IsZombie)
            {
                _E.Cast(_ETarget);
            }
            var _RTarget = TargetSelector.GetTarget(_R.Range, DamageType.Magical);
            if (Variables.Config["UseRInCombo"].Cast<CheckBox>().CurrentValue &&
                Variables.Config["UseRInComboStacks"].Cast<Slider>().CurrentValue < _R.Handle.Ammo && _ETarget != null &&
                _ETarget.IsZombie)
            {
                _R.Cast(_ETarget);
            }
        }

        private void _LoadSpells()
        {
            _Q = new Spell.Skillshot(SpellSlot.Q, 1200, SkillShotType.Linear, 250, 1650, 70);
            _W = new Spell.Active(SpellSlot.W);
            _E = new Spell.Skillshot(SpellSlot.E, 1360, SkillShotType.Linear, 250, 1400, 120);
            _R = new Spell.Skillshot(SpellSlot.R, 1800, SkillShotType.Circular, 1200, int.MaxValue, 150);
        }

        private void _LoadMenu()
        {
            Variables.Config.AddGroupLabel("Combo");
            Variables.Config.Add("UseQInCombo", new CheckBox("Use Q in Combo"));
            Variables.Config.Add("UseWInCombo", new CheckBox("Use W in Combo"));
            Variables.Config.Add("UseEInCombo", new CheckBox("Use E in Combo"));
            Variables.Config.Add("UseRInCombo", new CheckBox("Use R in Combo"));
            Variables.Config.Add("UseRInComboStacks", new Slider("Limit Ultimate Stacks", 3, 0, 7));
        }
    }
}