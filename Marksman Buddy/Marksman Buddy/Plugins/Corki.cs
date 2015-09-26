using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Marksman_Buddy.Internal;

namespace Marksman_Buddy.Plugins
{
    internal class Corki : PluginBase
    {
        private readonly int[] _RDamage = {100, 180, 260};
        private readonly float[] _RDamageScale = {0.2f, 0.3f, 0.4f};
        private Spell.Active _E;
        private Spell.Skillshot _Q;
        private Spell.Skillshot _R1;
        private Spell.Skillshot _W;
        //private Spell.Skillshot _R2 = new Spell.Skillshot(SpellSlot.R, 1500, EloBuddy.SDK.Enumerations.SkillShotType.Linear, 200, 2000, 40);

        public Corki()
        {
            _SetupMenu();
            _SetupSpells();
            Drawing.OnDraw += Drawing_OnDraw;
            Gapcloser.OnGapcloser += Gapcloser_OnGapCloser;
            Game.OnTick += Game_OnTick;
        }

		protected override sealed void _SetupSpells()
        {
            _E = new Spell.Active(SpellSlot.E, 600);
            _Q = new Spell.Skillshot(SpellSlot.Q, 825, SkillShotType.Circular, 300, 1000,
                250);
            _R1 = new Spell.Skillshot(SpellSlot.R, 1300, SkillShotType.Linear, 200, 2000,
                40);
            _W = new Spell.Skillshot(SpellSlot.W, 800, SkillShotType.Linear);
        }

        private void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
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

        private void Drawing_OnDraw(EventArgs args)
        {
            if (((Variables.Config["Corki.DrawAvailableSpells"].Cast<CheckBox>().CurrentValue && _Q.IsReady()) ||
                 !Variables.Config["Corki.DrawAvailableSpells"].Cast<CheckBox>().CurrentValue) &&
                Variables.Config["Corki.DrawQ"].Cast<CheckBox>().CurrentValue)
            {
                new Circle {Color = Color.DarkRed, Radius = _Q.Range}.Draw(ObjectManager.Player.Position);
            }

            if (((Variables.Config["Corki.DrawAvailableSpells"].Cast<CheckBox>().CurrentValue && _E.IsReady()) ||
                 !Variables.Config["Corki.DrawAvailableSpells"].Cast<CheckBox>().CurrentValue) &&
                Variables.Config["Corki.DrawR"].Cast<CheckBox>().CurrentValue)
            {
                new Circle {Color = Color.DarkBlue, Radius = _R1.Range}.Draw(ObjectManager.Player.Position);
            }
        }

        private void _KS()
        {
            foreach (var hero in
                HeroManager.Enemies
                    .Where(x => x.Position.Distance(ObjectManager.Player) < _R1.Range))
            {
                Console.WriteLine(hero.ChampionName);
                if (!hero.IsDead && !hero.IsZombie && _RCanKill(hero) &&
                    Variables.Config["useRKS"].Cast<CheckBox>().CurrentValue)
                {
                    _R1.Cast(hero);
                }
            }
        }

		protected override void _Harass()
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

        private bool _RCanKill(Obj_AI_Base target)
        {
            var RDamage = (_RDamage[_R1.Level] +
                           ObjectManager.Player.TotalAttackDamage*_RDamageScale[_R1.Level]
                           + ObjectManager.Player.TotalMagicalDamage*0.3f) - 20.0f; //Damage Calc is off    

            return ObjectManager.Player.CalculateDamageOnUnit(target, DamageType.Magical, RDamage) > target.Health;
        }

		protected override void _Combo()
        {
            var QTarget = TargetSelector.GetTarget(_Q.Range, DamageType.Magical);
            var RTarget = TargetSelector.GetTarget(_R1.Range, DamageType.Magical);
            if (QTarget.IsValidTarget() && Variables.Config["useQCombo"].Cast<CheckBox>().CurrentValue)
            {
                _Q.Cast(QTarget);
            }
            var inERange = false;
            ;
            foreach (var target in HeroManager.Enemies)
            {
                if (target.Distance(Player.Instance) <= 550)
                    inERange = true;
            }
            if (inERange && Variables.Config["useECombo"].Cast<CheckBox>().CurrentValue)
            {
                _E.Cast();
            }
            if (RTarget.IsValidTarget() && Variables.Config["useRCombo"].Cast<CheckBox>().CurrentValue
                && Variables.Config["useRComboStacks"].Cast<Slider>().CurrentValue < _R1.Handle.Ammo)
            {
                _R1.Cast(RTarget);
            }
        }

		protected override sealed void _SetupMenu()
        {
            Variables.Config.AddGroupLabel("Combo");
            Variables.Config.Add("useQCombo", new CheckBox("Use Q in Combo"));
            Variables.Config.Add("useECombo", new CheckBox("Use E in Combo"));
            Variables.Config.Add("useRCombo", new CheckBox("Use R in Combo"));
            Variables.Config.Add("useRComboStacks", new Slider("Save x Rockets", 3, 0, 7));
            Variables.Config.AddGroupLabel("Harass");
            Variables.Config.Add("useQHarass", new CheckBox("Use Q in Harass", false));
            Variables.Config.Add("useRHarass", new CheckBox("Use R in Harass"));
            Variables.Config.Add("useRHarassStacks", new Slider("Save x Rockets", 5, 0, 7));
            Variables.Config.AddGroupLabel("Misc");
            Variables.Config.Add("useWAntigapcloser", new CheckBox("Use W upon Gapcloser", false));
            Variables.Config.Add("useRKS", new CheckBox("Use R to KS (ignoring Stack limitation)"));
            Variables.Config.AddGroupLabel("Draw");
            Variables.Config.Add("Corki.DrawAvailableSpells", new CheckBox("Draw only Available Spells"));
            Variables.Config.Add("Corki.DrawQ", new CheckBox("Draw Q"));
            Variables.Config.Add("Corki.DrawR", new CheckBox("Draw R"));
        }
    }
}