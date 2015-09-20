using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Marksman_Buddy.Internal;
using SharpDX;
using Color = System.Drawing.Color;

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
            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Orbwalker.OnPreAttack += Orbwalker_OnPreAttack;
            Gapcloser.OnGapCloser += Gapcloser_OnGapCloser;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
        }

        private void _SetupSpells()
        {
            _Q = new Spell.Active(SpellSlot.Q);
            _W = new Spell.Skillshot(SpellSlot.W, 1240, SkillShotType.Linear, 250, 1200, 50);
            //_E = new Spell.Skillshot(SpellSlot.E, 2500, SkillShotType.Linear, 250, 1400, 299);
            _R = new Spell.Skillshot(SpellSlot.R, 3000, SkillShotType.Linear, 250, 1600, 130);
        }

        private void _SetupMenu()
        {
            Variables.Config.AddGroupLabel("Combo");
            Variables.Config.Add("Ashe.CastQCombo", new CheckBox("Cast Q in Combo"));
            Variables.Config.Add("Ashe.StacksQCombo", new Slider("Cast Q at x Stacks", 5, 0, 5));
            Variables.Config.Add("Ashe.CastWCombo", new CheckBox("Cast W in Combo"));
            Variables.Config.Add("Ashe.CastRCombo", new CheckBox("Cast R in Combo"));
            Variables.Config.AddGroupLabel("Harass");
            Variables.Config.Add("Ashe.CastQHarass", new CheckBox("Cast Q in Harass", false));
            Variables.Config.Add("Ashe.StacksQHarass", new Slider("Cast Q at x Stacks", 5, 0, 5));
            Variables.Config.Add("Ashe.CastWHarass", new CheckBox("Cast W in Harass"));
            Variables.Config.AddGroupLabel("Farm");
            Variables.Config.Add("Ashe.CastQLane", new CheckBox("Cast Q in Lane Clear"));
            Variables.Config.Add("Ashe.CastWLane", new CheckBox("Cast W in Lane Clear"));
            Variables.Config.Add("Ashe.CastQJungle", new CheckBox("Cast Q in Jungle Clear"));
            Variables.Config.Add("Ashe.CastWJungle", new CheckBox("Cast W in Jungle Clear"));
            Variables.Config.AddGroupLabel("Misc");
            //Variables.Config.Add("Ashe.AutoE", new CheckBox("Auto E"));
            Variables.Config.Add("Ashe.CastRAOE", new CheckBox("Cast R on AOE"));
            Variables.Config.Add("Ashe.CastRInterrupt", new CheckBox("Cast R to Interrupt"));
            Variables.Config.Add("Ashe.CastRGapCloser", new CheckBox("Cast R on GapCloser"));
            Variables.Config.AddGroupLabel("Draw");
            Variables.Config.Add("Ashe.DrawAvailableSpells", new CheckBox("Draw only Available Spells"));
            Variables.Config.Add("Ashe.DrawW", new CheckBox("Draw W"));
        }

        private void Game_OnTick(EventArgs args)
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
        }

        private void _Combo()
        {
            if (Variables.Config["Ashe.CastWCombo"].Cast<CheckBox>().CurrentValue && _W.IsReady())
            {
                var target = TargetSelector.GetTarget(_W.Range, DamageType.Physical);
                if (ObjectManager.Player.CountEnemiesInRange(700) > 0)
                {
                    target = TargetSelector.GetTarget(700, DamageType.Physical);
                }

                if (target.IsValidTarget())
                {
                    var pred = Prediction.Position.PredictConeSpell(target, _W.Range, 50, 250);
                    var col = pred.CollisionObjects.Count(colObj => colObj.IsEnemy && colObj.IsMinion && !colObj.IsDead);
                    if (target.IsDead || col > 0 || pred.HitChance < HitChance.High)
                    {
                        return;
                    }

                    _W.Cast(pred.CastPosition);
                }
            }

            if (Variables.Config["Ashe.CastRCombo"].Cast<CheckBox>().CurrentValue)
            {
                foreach (var target in HeroManager.Enemies.Where(target => target.IsValidTarget(_R.Range)))
                {
                    var predictedHealth = target.Health + target.HPRegenRate*2;
                    var RDmg = GetDamage(_R);
                    if (target.CountEnemiesInRange(250) > 2 &&
                        Variables.Config["Ashe.CastRAOE"].Cast<CheckBox>().CurrentValue)
                    {
                        _R.Cast(target);
                    }

                    if (RDmg > predictedHealth && CountAlliesInRange(600, target) == 0 &&
                        target.Distance(ObjectManager.Player.Position) > 1000)
                    {
                        var cast = true;
                        var output = Prediction.Position.PredictLinearMissile(target, _R.Range, _R.Width, _R.CastDelay,
                            _R.Speed, 0);
                        var direction = output.CastPosition.To2D() - ObjectManager.Player.Position.To2D();
                        direction.Normalize();
                        var enemies = HeroManager.Enemies.Where(x => x.IsValidTarget()).ToList();
                        foreach (var enemy in enemies)
                        {
                            if (enemy.ChampionName == target.ChampionName || !cast)
                            {
                                continue;
                            }

                            var prediction = Prediction.Position.PredictLinearMissile(enemy, _R.Range, _R.Width,
                                _R.CastDelay, _R.Speed, 0);
                            var predictedPosition = prediction.CastPosition;
                            var v = output.CastPosition - ObjectManager.Player.ServerPosition;
                            var w = predictedPosition - ObjectManager.Player.ServerPosition;
                            double c1 = Vector3.Dot(w, v);
                            double c2 = Vector3.Dot(v, v);
                            var b = c1/c2;
                            var pb = ObjectManager.Player.ServerPosition + ((float) b*v);
                            var length = Vector3.Distance(predictedPosition, pb);
                            if (length < (_R.Width + 150 + enemy.BoundingRadius/2) &&
                                ObjectManager.Player.Distance(predictedPosition) <
                                ObjectManager.Player.Distance(target.ServerPosition))
                            {
                                cast = false;
                            }
                        }

                        if (cast)
                        {
                            _R.Cast(target);
                        }
                    }
                }
            }

            foreach (var enemy in HeroManager.Enemies.Where(enemy => enemy.IsValidTarget(_R.Range)))
            {
                if (ObjectManager.Player.Health < ObjectManager.Player.MaxHealth*0.4 && enemy.IsValidTarget(270) &&
                    enemy.IsMelee && Variables.Config["Ashe.CastRGapCloser"].Cast<CheckBox>().CurrentValue)
                {
                    _R.Cast(enemy);
                }
            }
        }

        private void _Harass()
        {
            if (!Variables.Config["Ashe.CastWHarass"].Cast<CheckBox>().CurrentValue || !_W.IsReady())
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

            var pred = Prediction.Position.PredictConeSpell(target, _W.Range, 50, 250);
            var col = pred.CollisionObjects.Count(colObj => colObj.IsEnemy && colObj.IsMinion && !colObj.IsDead);
            if (target.IsDead || col > 0 || pred.HitChance < HitChance.High)
            {
                return;
            }

            _W.Cast(pred.CastPosition);
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

        private void Gapcloser_OnGapCloser(AIHeroClient sender, Gapcloser.GapCloserEventArgs e)
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

        private void Interrupter_OnInterruptableSpell(Obj_AI_Base sender, InterruptableSpellEventArgs e)
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

        private double GetDamage(Spell.Skillshot slot)
        {
            if (slot == _W)
            {
                return new double[] {20, 35, 50, 65, 80}[_W.Level] +
                       1*(ObjectManager.Player.BaseAttackDamage + ObjectManager.Player.FlatPhysicalDamageMod);
            }

            if (slot == _R)
            {
                return new double[] {250, 425, 600}[_R.Level] + 1*ObjectManager.Player.FlatMagicDamageMod;
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
