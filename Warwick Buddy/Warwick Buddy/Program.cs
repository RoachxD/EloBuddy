using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy.SDK.Rendering;
using Warwick_Buddy.Internal;
using Menu = Warwick_Buddy.Internal.Menu;

namespace Warwick_Buddy
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                Loading.OnLoadingComplete += delegate
                {
                    var onLoadingComplete = new Thread(Loading_OnLoadingComplete);
                    onLoadingComplete.Start();
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Main:" + Environment.NewLine + e);
            }
        }

        private static void Loading_OnLoadingComplete()
        {
            try
            {
                if (!Player.Instance.ChampionName.Equals("Warwick"))
                {
                    return;
                }

                Spells.Q = new Spell.Targeted(SpellSlot.Q, 400);
                Spells.W = new Spell.Active(SpellSlot.W, 1250);
                Spells.R = new Spell.Targeted(SpellSlot.R, 700);

                Functions.SetSummonerSlots();

                Menu.InfoMenu = MainMenu.AddMenu("Warwick Buddy", "WarwickBuddy");
                Menu.InfoMenu.AddGroupLabel("Warwick Buddy");
                Menu.InfoMenu.AddLabel("Version: " + "1.0.0.1");
                Menu.InfoMenu.AddSeparator();
                Menu.InfoMenu.AddLabel("Creators: " + "Roach");

                Menu.ComboMenu = Menu.InfoMenu.AddSubMenu("Combo", "Combo");
                Menu.ComboMenu.AddGroupLabel("Combo Options");
                Menu.ComboMenu.Add("Combo.Q", new CheckBox("Use Q"));
                Menu.ComboMenu.Add("Combo.W", new CheckBox("Use W"));
                Menu.ComboMenu.Add("Combo.R", new CheckBox("Use R"));
                Menu.ComboMenu.Add("Combo.Smite", new CheckBox("Use Smite"));
                Menu.ComboMenu.AddGroupLabel("R Targets");
                foreach (var hero in EntityManager.Heroes.Enemies)
                {
                    Menu.ComboMenu.Add("R." + hero.ChampionName, new CheckBox(hero.ChampionName));
                }

                Menu.HarassMenu = Menu.InfoMenu.AddSubMenu("Harass", "Harass");
                Menu.HarassMenu.AddGroupLabel("Auto Harass Options");
                Menu.HarassMenu.Add("Harass.AutoQ", new KeyBind("Auto Q", true, KeyBind.BindTypes.PressToggle, 'T'));
                Menu.HarassMenu.Add("Harass.AutoQMana", new Slider("If Mana Percent >=", 50));
                Menu.HarassMenu.AddGroupLabel("Harass Options");
                Menu.HarassMenu.Add("Harass.Q", new CheckBox("Use Q"));
                Menu.HarassMenu.Add("Harass.W", new CheckBox("Use W"));

                Menu.ClearMenu = Menu.InfoMenu.AddSubMenu("Clear", "Clear");
                Menu.ClearMenu.AddGroupLabel("Clear Options");
                Menu.ClearMenu.Add("Clear.Q", new CheckBox("Use Q"));
                Menu.ClearMenu.Add("Clear.W", new CheckBox("Use W"));
                Menu.ClearMenu.AddGroupLabel("Smite Mobs");
                Menu.ClearMenu.Add("Smite.Enable", new CheckBox("Use Smite"));
                Menu.ClearMenu.AddSeparator();
                Menu.ClearMenu.Add("Smite.Baron", new CheckBox("Baron Nashor"));
                Menu.ClearMenu.Add("Smite.Dragon", new CheckBox("Dragon"));
                Menu.ClearMenu.Add("Smite.Red", new CheckBox("Red Brambleback"));
                Menu.ClearMenu.Add("Smite.Blue", new CheckBox("Blue Sentinel"));
                Menu.ClearMenu.Add("Smite.Krug", new CheckBox("Ancient Krug"));
                Menu.ClearMenu.Add("Smite.Gromp", new CheckBox("Gromp"));
                Menu.ClearMenu.Add("Smite.Raptor", new CheckBox("Crimson Raptor"));
                Menu.ClearMenu.Add("Smite.Wolf", new CheckBox("Greater Murk Wolf"));

                Menu.LastHitMenu = Menu.InfoMenu.AddSubMenu("Last Hit", "LastHit");
                Menu.LastHitMenu.AddGroupLabel("Last Hit Options");
                Menu.LastHitMenu.Add("LastHit.Q", new CheckBox("Use Q"));

                Menu.MiscMenu = Menu.InfoMenu.AddSubMenu("Misc", "Misc");
                Menu.MiscMenu.AddGroupLabel("Misc Options");
                Menu.MiscMenu.Add("Misc.InterruptR", new CheckBox("Auto R to Interrupt Spells"));
                Menu.MiscMenu.Add("Misc.TowerR", new CheckBox("Auto R if Enemy under Tower"));
                Menu.MiscMenu.AddGroupLabel("Kill Steal");
                Menu.MiscMenu.Add("KillSteal.Q", new CheckBox("Use Q"));
                Menu.MiscMenu.Add("KillSteal.R", new CheckBox("Use R"));
                Menu.MiscMenu.Add("KillSteal.Ignite", new CheckBox("Use Ignite"));
                Menu.MiscMenu.Add("KillSteal.Smite", new CheckBox("Use Smite"));

                Menu.DrawMenu = Menu.InfoMenu.AddSubMenu("Draw", "Draw");
                Menu.DrawMenu.AddGroupLabel("Draw Options");
                Menu.DrawMenu.Add("Draw.Q", new CheckBox("Q Range"));
                Menu.DrawMenu.Add("Draw.R", new CheckBox("R Range"));
                Menu.DrawMenu.Add("Draw.Smite", new CheckBox("Draw Smite Status"));

                Chat.Print("Warwick Buddy - <font color=\"#FFFFFF\">Loaded</font>", Color.FromArgb(255, 210, 68, 74));

                Game.OnTick += Game_OnTick;
                Drawing.OnDraw += Drawing_OnDraw;
                Orbwalker.OnAttack += Orbwalker_OnAttack;
                Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
            }
            catch (Exception e)
            {
                Console.WriteLine("Loading_OnLoadingComplete:" + Environment.NewLine + e);
            }
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (Player.Instance.IsDead || MenuGUI.IsChatOpen)
            {
                return;
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                Combo();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                Harass();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) ||
                Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                Clear();
            }

            if (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                LastHit();
            }

            Functions.SmiteMob();
            AutoQ();
            KillSteal();
            AutoRUnderTower();
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            try
            {
                if (Player.Instance.IsDead)
                {
                    return;
                }

                var drawQ = Menu.DrawMenu["Draw.Q"].Cast<CheckBox>().CurrentValue;
                if (drawQ && Spells.Q.IsReady())
                {
                    new Circle
                    {
                        Color = Color.SkyBlue,
                        Radius = Spells.Q.Range
                    }.Draw(Player.Instance.Position);
                }

                var drawR = Menu.DrawMenu["Draw.R"].Cast<CheckBox>().CurrentValue;
                if (drawR && Spells.R.IsReady())
                {
                    new Circle
                    {
                        Color = Color.YellowGreen,
                        Radius = Spells.R.Range
                    }.Draw(Player.Instance.Position);
                }

                var drawSmite = Menu.DrawMenu["Draw.Smite"].Cast<CheckBox>().CurrentValue;
                var smiteSpell = Player.Instance.Spellbook.GetSpell(Spells.Smite);
                if (drawSmite && smiteSpell != null)
                {
                    var barPos = Player.Instance.HPBarPosition;
                    var smiteStatus = Menu.ClearMenu["Smite.Enable"].Cast<CheckBox>().CurrentValue;
                    Drawing.DrawText(barPos.X - 10, barPos.Y - 8, Color.White,
                        "Smite: " + (smiteStatus ? "Enabled" : "Disabled"));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Drawing_OnDraw:" + Environment.NewLine + e);
            }
        }

        private static void Orbwalker_OnAttack(AttackableUnit target, EventArgs args)
        {
            if (!Spells.W.IsReady())
            {
                return;
            }

            var aiHeroClientW = Menu.ComboMenu["Combo.W"].Cast<CheckBox>().CurrentValue ||
                                Menu.ComboMenu["Harass.W"].Cast<CheckBox>().CurrentValue;
            var objAiMinionW = Menu.ClearMenu["Clear.W"].Cast<CheckBox>().CurrentValue;
            if (((Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) ||
                  Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) && aiHeroClientW &&
                 target is AIHeroClient) ||
                (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) && objAiMinionW &&
                 target is Obj_AI_Minion))
            {
                Spells.W.Cast();
            }
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs e)
        {
            if (Menu.MiscMenu["Misc.InterruptR"].Cast<CheckBox>().CurrentValue && Spells.R.IsReady() &&
                sender.IsValidTarget(Spells.R.Range) && sender.IsEnemy)
            {
                Spells.R.Cast(sender);
            }
        }

        private static void Combo()
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                return;
            }

            var comboQ = Menu.ComboMenu["Combo.Q"].Cast<CheckBox>().CurrentValue;
            if (comboQ && Spells.Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical, Player.Instance.Position);
                if (target != null)
                {
                    Spells.Q.Cast(target);
                }
            }

            var comboR = Menu.ComboMenu["Combo.R"].Cast<CheckBox>().CurrentValue;
            if (comboR && Spells.R.IsReady())
            {
                var targets = EntityManager.Heroes.Enemies.Where(
                    enemy =>
                        Menu.ComboMenu["R." + enemy.ChampionName].Cast<CheckBox>().CurrentValue &&
                        Player.Instance.Distance(enemy) < Spells.R.Range);
                foreach (var target in targets)
                {
                    if (!target.IsValidTarget())
                    {
                        continue;
                    }

                    var comboSmite = Menu.ComboMenu["Combo.Smite"].Cast<CheckBox>().CurrentValue;
                    if (comboSmite)
                    {
                        Player.Instance.Spellbook.CastSpell(Spells.Smite, target);
                    }

                    Spells.R.Cast(target);
                }
            }

            var comboW = Menu.ComboMenu["Combo.W"].Cast<CheckBox>().CurrentValue;
            if (comboW && Spells.W.IsReady() && EntityManager.Heroes.Allies.Any(
                ally => !ally.IsMe && ally.IsValidTarget(Spells.W.Range) && ally.IsAttackingPlayer))
            {
                Spells.W.Cast();
            }
        }

        private static void Harass()
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                return;
            }

            var harassQ = Menu.ComboMenu["Harass.Q"].Cast<CheckBox>().CurrentValue;
            if (harassQ && Spells.Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical, Player.Instance.Position);
                if (target != null)
                {
                    Spells.Q.Cast(target);
                }
            }

            var harassW = Menu.ComboMenu["Harass.W"].Cast<CheckBox>().CurrentValue;
            if (harassW && Spells.W.IsReady() && EntityManager.Heroes.Allies.Any(
                ally => ally != null && !ally.IsMe && ally.IsValidTarget(Spells.W.Range) && ally.IsAttackingPlayer))
            {
                Spells.W.Cast();
            }
        }

        private static void Clear()
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) &&
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                return;
            }

            var clearQ = Menu.ClearMenu["Clear.Q"].Cast<CheckBox>().CurrentValue;
            if (!clearQ || !Spells.Q.IsReady())
            {
                return;
            }

            var minionObj = ObjectManager.Get<Obj_AI_Minion>()
                .Where(minion => !minion.IsAlly && minion.Distance(Player.Instance) < Spells.Q.Range);
            var obj = minionObj.FirstOrDefault(minion => minion.Health < SpellSlot.Q.GetDamage(minion)) ??
                      minionObj.MinOrDefault(minion => minion.Health);
            if (obj == null)
            {
                return;
            }

            Spells.Q.Cast(obj);
        }

        private static void LastHit()
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                return;
            }

            var lastHitQ = Menu.LastHitMenu["LastHit.Q"].Cast<CheckBox>().CurrentValue;
            if (!lastHitQ || !Spells.Q.IsReady())
            {
                return;
            }

            var obj =
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position,
                    Spells.Q.Range).FirstOrDefault(minion => minion.Health < SpellSlot.Q.GetDamage(minion));
            if (obj == null)
            {
                return;
            }

            Spells.Q.Cast(obj);
        }

        private static void AutoQ()
        {
            var autoQ = Menu.HarassMenu["Harass.AutoQ"].Cast<KeyBind>().CurrentValue;
            var autoQMana = Menu.HarassMenu["Harass.AutoQMana"].Cast<Slider>().CurrentValue;
            if (!autoQ || Player.Instance.ManaPercent < autoQMana)
            {
                return;
            }

            var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical, Player.Instance.Position);
            if (target != null)
            {
                Spells.Q.Cast(target);
            }
        }

        private static void KillSteal()
        {
            var killStealIgnite = Menu.MiscMenu["KillSteal.Ignite"].Cast<CheckBox>().CurrentValue;
            var igniteSpell = Player.Instance.Spellbook.GetSpell(Spells.Ignite);
            if (killStealIgnite && igniteSpell != null && igniteSpell.IsReady)
            {
                var target = TargetSelector.GetTarget(600, DamageType.True);
                if (target != null &&
                    target.Health < Player.Instance.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite))
                {
                    Player.Instance.Spellbook.CastSpell(Spells.Ignite, target);
                }
            }

            var killStealSmite = Menu.MiscMenu["KillSteal.Smite"].Cast<CheckBox>().CurrentValue;
            var smiteSpell = Player.Instance.Spellbook.GetSpell(Spells.Smite);
            if (killStealSmite && smiteSpell != null && smiteSpell.IsReady)
            {
                var target = TargetSelector.GetTarget(760, DamageType.True);
                if (target != null &&
                    target.Health < Player.Instance.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Smite))
                {
                    Player.Instance.Spellbook.CastSpell(Spells.Smite, target);
                }
            }

            var killStealQ = Menu.MiscMenu["KillSteal.Q"].Cast<CheckBox>().CurrentValue;
            if (killStealQ && Spells.Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical, Player.Instance.Position);
                if (target != null && target.Health < SpellSlot.Q.GetDamage(target))
                {
                    Spells.Q.Cast(target);
                }
            }

            var killStealR = Menu.MiscMenu["KillSteal.R"].Cast<CheckBox>().CurrentValue;
            if (killStealR && Spells.R.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.R.Range, DamageType.Magical, Player.Instance.Position);
                if (target != null && target.Health < SpellSlot.R.GetDamage(target))
                {
                    Spells.R.Cast(target);
                }
            }
        }

        private static void AutoRUnderTower()
        {
            var towerR = Menu.MiscMenu["Misc.TowerR"].Cast<CheckBox>().CurrentValue;
            if (!towerR || !Spells.R.IsReady())
            {
                return;
            }

            var target = EntityManager.Heroes.Enemies.Where(i => i.IsValidTarget(Spells.R.Range))
                .MinOrDefault(i => i.Distance(Player.Instance.Position));
            var tower = ObjectManager.Get<Obj_AI_Turret>()
                .FirstOrDefault(i => i.IsAlly && !i.IsDead && i.Distance(Player.Instance.Position) <= 850);

            if (target != null && tower != null && target.Distance(tower) <= 850)
            {
                Spells.R.Cast(target);
            }
        }
    }
}