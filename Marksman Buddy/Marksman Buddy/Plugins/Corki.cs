using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using Marksman_Buddy.Internal;

namespace Marksman_Buddy.Plugins
{
    internal class Corki : PluginBase
    {
        private readonly Spell.Skillshot _Q = new Spell.Skillshot(SpellSlot.Q, 825, SkillShotType.Circular, 300, 1000,
            250);

        private readonly Spell.Skillshot _R1 = new Spell.Skillshot(SpellSlot.R, 1300, SkillShotType.Linear, 200, 2000,
            40);

        private readonly int[] _RDamage = {100, 180, 260};
        private readonly float[] _RDamageScale = {0.2f, 0.3f, 0.4f};
        private readonly Spell.Skillshot _W = new Spell.Skillshot(SpellSlot.W, 800, SkillShotType.Linear);
        private Spell.Active _E = new Spell.Active(SpellSlot.E, 600);
        //private Spell.Skillshot _R2 = new Spell.Skillshot(SpellSlot.R, 1500, EloBuddy.SDK.Enumerations.SkillShotType.Linear, 200, 2000, 40);

        public Corki()
        {
            _SetupMenu();
            Game.OnTick += Game_OnTick;
            Gapcloser.OnGapCloser += Gapcloser_OnGapCloser;
        }

        private void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapCloserEventArgs e)
        {
            if (!Variables.Config["useWAntigapcloser"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            if (sender.Distance(ObjectManager.Player) < 1400
                && (e.End.Distance(ObjectManager.Player) < sender.Distance(ObjectManager.Player)))
            {
                _W.Cast((ObjectManager.Player.Position.Extend(e.End, -1*_W.Range)).To3D());
            }
        }

        private void Game_OnTick(EventArgs args)
        {
            if (Variables.ComboMode)
            {
                _Combo();
            }

            if (Variables.HarassMode)
            {
                _Harrass();
            }

            _KS();
        }

        private void _KS()
        {
            foreach (var hero in
                HeroManager.Enemies
                    .Where(x => x.Position.Distance(ObjectManager.Player) < _R1.Range))
            {
                if (_RCanKill(hero, _R1.Level) && Variables.Config["useRKS"].Cast<CheckBox>().CurrentValue)
                {
                    _R1.Cast(hero);
                }
            }
        }

        private void _Harrass()
        {
            var QTarget = TargetSelector.GetTarget(_Q.Range, DamageType.Magical);
            var RTarget = TargetSelector.GetTarget(_R1.Range, DamageType.Magical);
            if (QTarget.IsValidTarget() && Variables.Config["useQHarass"].Cast<CheckBox>().CurrentValue)
            {
                _Q.Cast(QTarget);
            }

            if (RTarget.IsValidTarget() && Variables.Config["useRHarass"].Cast<CheckBox>().CurrentValue
                && Variables.Config["useRHarassStacks"].Cast<Slider>().CurrentValue < _R1.Handle.Ammo)
            {
                _R1.Cast(RTarget);
            }
        }

        private bool _RCanKill(Obj_AI_Base target, int Level)
        {
            var EDamage =
                _RDamage[Level] +
                ObjectManager.Player.TotalAttackDamage*_RDamageScale[Level]
                + ObjectManager.Player.TotalMagicalDamage*0.3f - 20.0f; //Damage Calc is off

            return ObjectManager.Player.CalculateDamageOnUnit(target, DamageType.Magical, EDamage) > target.Health;
        }

        private void _Combo()
        {
            var QTarget = TargetSelector.GetTarget(_Q.Range, DamageType.Magical);
            var RTarget = TargetSelector.GetTarget(_R1.Range, DamageType.Magical);
            if (QTarget.IsValidTarget() && Variables.Config["useQCombo"].Cast<CheckBox>().CurrentValue)
            {
                _Q.Cast(QTarget);
            }

            if (RTarget.IsValidTarget() && Variables.Config["useRCombo"].Cast<CheckBox>().CurrentValue
                && Variables.Config["useRComboStacks"].Cast<Slider>().CurrentValue < _R1.Handle.Ammo)
            {
                _R1.Cast(RTarget);
            }
        }

        private void _SetupMenu()
        {
            Variables.Config.AddGroupLabel("Corki");
            Variables.Config.AddGroupLabel("Combo");
            Variables.Config.Add("useQCombo", new CheckBox("Use Q in Combo"));
            Variables.Config.Add("useRCombo", new CheckBox("Use R in Combo"));
            Variables.Config.Add("useRComboStacks", new Slider("Save x Rockets", 3, 0, 7));
            Variables.Config.AddGroupLabel("Harass");
            Variables.Config.Add("useQHarass", new CheckBox("Use Q in Harass", false));
            Variables.Config.Add("useRHarass", new CheckBox("Use R in Harass"));
            Variables.Config.Add("useRHarassStacks", new Slider("Save x Rockets", 5, 0, 7));
            Variables.Config.AddGroupLabel("Misc");
            Variables.Config.Add("useWAntigapcloser", new CheckBox("Use W upon Gapcloser", false));
            Variables.Config.Add("useRKS", new CheckBox("Use R to KS (ignoring Stack limitation)"));
        }
    }
}