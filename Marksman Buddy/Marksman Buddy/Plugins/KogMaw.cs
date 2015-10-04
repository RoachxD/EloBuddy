using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;
using Marksman_Buddy.Internal;

namespace Marksman_Buddy.Plugins
{
    internal class KogMaw : PluginBase
    {
		private readonly int[] _RDamage = { 160, 240, 320 };
		private readonly int[] _RRange = { 1200, 1500, 1800 };
        private Spell.Skillshot _E;
        private Spell.Skillshot _Q;
        private Spell.Skillshot _R;
        private Spell.Active _W;

        public KogMaw()
        {
            _SetupMenu();
            _SetupSpells();
            Game.OnTick += Game_OnTick;
        }

        protected override void Game_OnTick(EventArgs args)
        {
            if (Variables.ComboMode)
            {
                _Combo();
            }

            if (Variables.HarassMode)
            {
                _Harass();
            }

            _KS();
        }

        protected override void _Harass()
        {
            var _ETarget = TargetSelector.GetTarget(_E.Range, DamageType.Magical);
            if (Variables.Config["UseEInHarass"].Cast<CheckBox>().CurrentValue && _ETarget.IsValidTarget() &&
                !_ETarget.IsZombie)
            {
                _E.Cast(_ETarget);
            }

			var _RTarget = TargetSelector.GetTarget(_RRange[_R.Level - 1], DamageType.Magical);
            if (Variables.Config["UseRInHarass"].Cast<CheckBox>().CurrentValue &&
                Variables.Config["UseRInHarassStacks"].Cast<Slider>().CurrentValue < _R.Handle.Ammo &&
                _RTarget.IsValidTarget() &&
                !_RTarget.IsZombie)
            {
                _R.Cast(_RTarget);
            }
        }

        protected void _KS()
        {
            foreach (
                var enemy in
                    HeroManager.Enemies.Where(
                        enemy =>
                            enemy != null && !enemy.IsDead && !enemy.IsZombie &&
							enemy.Distance(Player.Instance) < _RRange[_R.Level - 1] && _RSpellDamage(enemy) > enemy.Health))
            {
                _R.Cast(enemy);
            }
        }

        protected override void _Combo()
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
            if (Variables.Config["UseEInCombo"].Cast<CheckBox>().CurrentValue && _ETarget.IsValidTarget() &&
                !_ETarget.IsZombie)
            {
                _E.Cast(_ETarget);
            }
			var _RTarget = TargetSelector.GetTarget(_RRange[_R.Level - 1], DamageType.Magical);
            if (Variables.Config["UseRInCombo"].Cast<CheckBox>().CurrentValue &&
                Variables.Config["UseRInComboStacks"].Cast<Slider>().CurrentValue < _R.Handle.Ammo &&
                _RTarget.IsValidTarget() &&
                _RTarget.IsZombie)
            {
                _R.Cast(_RTarget);
            }
        }

        protected override sealed void _SetupSpells()
        {
            _Q = new Spell.Skillshot(SpellSlot.Q, 1200, SkillShotType.Linear, 250, 1650, 70);
            _W = new Spell.Active(SpellSlot.W);
            _E = new Spell.Skillshot(SpellSlot.E, 1360, SkillShotType.Linear, 250, 1400, 120);
            _R = new Spell.Skillshot(SpellSlot.R, 1800, SkillShotType.Circular, 1200, int.MaxValue, 150);
        }

        protected override sealed void _SetupMenu()
        {
            Variables.Config.AddGroupLabel("Combo");
            Variables.Config.Add("UseQInCombo", new CheckBox("Use Q in Combo"));
            Variables.Config.Add("UseWInCombo", new CheckBox("Use W in Combo"));
            Variables.Config.Add("UseEInCombo", new CheckBox("Use E in Combo"));
            Variables.Config.Add("UseRInCombo", new CheckBox("Use R in Combo"));
            Variables.Config.Add("UseRInComboStacks", new Slider("Limit Ultimate Stacks", 3, 0, 9));
            Variables.Config.AddGroupLabel("Harass");
            Variables.Config.Add("UseEInHarass", new CheckBox("Use E in Harass"));
            Variables.Config.Add("UseRInHarass", new CheckBox("Use R in Harass"));
            Variables.Config.Add("UseRInHarassStacks", new Slider("Limit Ultimate Stacks", 3, 0, 6));
        }

        protected float _RSpellDamage(Obj_AI_Base target)
        {
            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical,
                (_RDamage[_R.Level] + 0.5f*Player.Instance.TotalAttackDamage + 0.3f*Player.Instance.TotalMagicalDamage));
        }
    }
}
