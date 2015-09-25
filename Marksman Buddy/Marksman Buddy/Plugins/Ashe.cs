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
    internal class Ashe : PluginBase
    {
        private static Spell.Active _Q;
        private static Spell.Skillshot _W, _R;

        public Ashe()
        {
            _SetupMenu();
            _SetupSpells();
            Drawing.OnDraw += Drawing_OnDraw;
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            Gapcloser.OnGapcloser += Gapcloser_OnGapCloser;
            Game.OnTick += Game_OnTick;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
        }

        public override sealed void _SetupSpells()
        {
            _Q = new Spell.Active(SpellSlot.Q);
            _W = new Spell.Skillshot(SpellSlot.W, 1240, SkillShotType.Linear, 250, 1200, 50);
            //_E = new Spell.Skillshot(SpellSlot.E, 2500, SkillShotType.Linear, 250, 1400, 299);
            _R = new Spell.Skillshot(SpellSlot.R, 3000, SkillShotType.Linear, 250, 1600, 130);
        }

        public override sealed void _SetupMenu()
        {
            Variables.Config.AddGroupLabel("Combo");
            Variables.Config.Add("Ashe.CastQCombo", new CheckBox("Cast Q in Combo"));
            Variables.Config.Add("Ashe.StacksQCombo", new Slider("Cast Q at x Stacks", 5, 0, 5));
            Variables.Config.Add("Ashe.CastWCombo", new CheckBox("Cast W in Combo"));
            Variables.Config.Add("Ashe.CastRCombo",
                new KeyBind("Cast R on Press", false, KeyBind.BindTypes.HoldActive, 'T'));
            Variables.Config.AddGroupLabel("Harass");
            Variables.Config.Add("Ashe.CastQHarass", new CheckBox("Cast Q in Harass", false));
            Variables.Config.Add("Ashe.StacksQHarass", new Slider("Cast Q at x Stacks", 5, 0, 5));
            Variables.Config.Add("Ashe.CastWHarass", new CheckBox("Cast W in Harass"));
            Variables.Config.AddGroupLabel("Misc");
            //Variables.Config.Add("Ashe.AutoE", new CheckBox("Auto E"));
            Variables.Config.Add("Ashe.CastRAOE", new CheckBox("Cast R on AOE"));
            Variables.Config.Add("Ashe.CastRInterrupt", new CheckBox("Cast R to Interrupt"));
            Variables.Config.Add("Ashe.CastRGapCloser", new CheckBox("Cast R on GapCloser"));
            Variables.Config.AddGroupLabel("Draw");
            Variables.Config.Add("Ashe.DrawAvailableSpells", new CheckBox("Draw only Available Spells"));
            Variables.Config.Add("Ashe.DrawW", new CheckBox("Draw W"));
        }

        public override void Game_OnTick(EventArgs args)
        {
            GetQStacks();

            if (Variables.ComboMode)
            {
                _Combo();
            }

            if (Variables.HarassMode)
            {
                _Harass();
            }

            _RLogic();
        }

        public override void _Combo()
        {
            if (!Variables.Config["Ashe.CastWCombo"].Cast<CheckBox>().CurrentValue || !_W.IsReady() ||
                ObjectManager.Player.IsAttackingPlayer)
            {
                return;
            }

            var target = TargetSelector.GetTarget(_W.Range, DamageType.Physical);
            if (ObjectManager.Player.CountEnemiesInRange(700) > 0)
            {
                target = TargetSelector.GetTarget(700, DamageType.Physical);
            }

            if (!target.IsValidTarget())
            {
                return;
            }

            var pred = Prediction.Position.PredictConeSpell(target, _W.Range, 50, 250, 1500);
            var col = pred.CollisionObjects.Count(colObj => colObj.IsEnemy && colObj.IsMinion && !colObj.IsDead);
            if (target.IsDead || col > 0 || pred.HitChance < HitChance.High)
            {
                return;
            }

            _W.Cast(pred.CastPosition);
        }

        public override void _Harass()
        {
            if (!Variables.Config["Ashe.CastWHarass"].Cast<CheckBox>().CurrentValue || !_W.IsReady() ||
                ObjectManager.Player.IsAttackingPlayer)
            {
                return;
            }

            var target = TargetSelector.GetTarget(_W.Range, DamageType.Physical);
            if (ObjectManager.Player.CountEnemiesInRange(700) > 0)
            {
                target = TargetSelector.GetTarget(700, DamageType.Physical);
            }

            if (!target.IsValidTarget())
            {
                return;
            }

            var pred = Prediction.Position.PredictConeSpell(target, _W.Range, 50, 250, 1500);
            var col = pred.CollisionObjects.Count(colObj => colObj.IsEnemy && colObj.IsMinion && !colObj.IsDead);
            if (target.IsDead || col > 0 || pred.HitChance < HitChance.High)
            {
                return;
            }

            _W.Cast(pred.CastPosition);
        }

        public void _RLogic()
        {
            if (!_R.IsReady())
            {
                return;
            }

            if (Variables.Config["Ashe.CastRCombo"].Cast<KeyBind>().CurrentValue)
            {
                var target = TargetSelector.GetTarget(2000, DamageType.Physical);
                if (target.IsValidTarget())
                {
                    _R.Cast(target);
                }
            }


            foreach (var enemy in HeroManager.Enemies.Where(target => target.IsValidTarget(_R.Range)))
            {
                if (enemy.CountEnemiesInRange(250) > 2 &&
                    Variables.Config["Ashe.CastRAOE"].Cast<CheckBox>().CurrentValue)
                {
                    _R.Cast(enemy);
                }

                if (ObjectManager.Player.Health < ObjectManager.Player.MaxHealth*0.4 && enemy.IsValidTarget(270) &&
                    enemy.IsMelee && Variables.Config["Ashe.CastRGapCloser"].Cast<CheckBox>().CurrentValue)
                {
                    _R.Cast(enemy);
                }
            }
        }

        private void Orbwalker_OnPreAttack(AttackableUnit target, Orbwalker.PreAttackArgs args)
        {
            if (target == null)
            {
                return;
            }

            var heroTarget = (Obj_AI_Base) target;
            var requiredStacks = Variables.ComboMode
                ? Variables.Config["Ashe.StacksQCombo"].Cast<Slider>().CurrentValue
                : Variables.Config["Ashe.StacksQHarass"].Cast<Slider>().CurrentValue;
            if (GetQStacks() < requiredStacks || !heroTarget.IsValid || !(heroTarget is AIHeroClient))
            {
                return;
            }

            if ((Variables.ComboMode && Variables.Config["Ashe.CastQCombo"].Cast<CheckBox>().CurrentValue) ||
                (Variables.HarassMode && Variables.Config["Ashe.CastQHarass"].Cast<CheckBox>().CurrentValue))
            {
                _Q.Cast();
            }
        }

        private void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs e)
        {
            if (!Variables.Config["Ashe.CastRGapCloser"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            if (!_R.IsReady())
            {
                return;
            }

            var target = sender ?? e.Sender;
            if (!target.IsValidTarget(800))
            {
                return;
            }

            _R.Cast(target.ServerPosition);
        }

        private void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, Interrupter.InterruptableSpellEventArgs e)
        {
            if (Variables.Config["Ashe.CastRInterrupt"].Cast<CheckBox>().CurrentValue && _R.IsReady() &&
                sender.IsValidTarget(_R.Range))
            {
                _R.Cast(sender);
            }
        }

        private void Drawing_OnDraw(EventArgs args)
        {
            if (!Variables.Config["Ashe.DrawW"].Cast<CheckBox>().CurrentValue)
            {
                return;
            }

            if ((Variables.Config["Ashe.DrawAvailableSpells"].Cast<CheckBox>().CurrentValue && _W.IsReady()) ||
                !Variables.Config["Ashe.DrawAvailableSpells"].Cast<CheckBox>().CurrentValue)
            {
                new Circle {Color = Color.Orange, Radius = _W.Range}.Draw(ObjectManager.Player.Position);
            }
        }

        private int GetQStacks()
        {
            foreach (var buff in ObjectManager.Player.Buffs)
            {
                switch (buff.Name)
                {
                    case "asheqcastready":
                        return buff.Count;
                    case "AsheQ":
                        return buff.Count;
                }
            }

            return 0;
        }

        public int CountAlliesInRange(float range, Obj_AI_Base originalunit = null)
        {
            if (originalunit != null)
            {
                return HeroManager.Allies
                    .Count(x => x.NetworkId != originalunit.NetworkId && x.IsValidTarget(range));
            }

            return 0;
        }
    }
}