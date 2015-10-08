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
using XinZhao_Buddy.Internal;
using Menu = XinZhao_Buddy.Internal.Menu;

namespace XinZhao_Buddy
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
                if (!Player.Instance.ChampionName.Equals("XinZhao"))
                {
                    return;
                }

                Spells.Q = new Spell.Active(SpellSlot.Q);
                Spells.W = new Spell.Active(SpellSlot.W);
                Spells.E = new Spell.Targeted(SpellSlot.E, 650);
                Spells.R = new Spell.Active(SpellSlot.R, 500);

                Functions.SetSummonerSlots();

                Menu.InfoMenu = MainMenu.AddMenu("XinZhao Buddy", "XinZhaoBuddy");
                Menu.InfoMenu.AddGroupLabel("XinZhao Buddy");
                Menu.InfoMenu.AddLabel("Version: " + "1.0.0.0");
                Menu.InfoMenu.AddSeparator();
                Menu.InfoMenu.AddLabel("Creators: " + "Roach");

                Menu.ComboMenu = Menu.InfoMenu.AddSubMenu("Combo", "Combo");
                Menu.ComboMenu.AddGroupLabel("Combo Options");
                Menu.ComboMenu.Add("Combo.Q", new CheckBox("Use Q"));
                Menu.ComboMenu.Add("Combo.W", new CheckBox("Use W"));
                Menu.ComboMenu.Add("Combo.E", new CheckBox("Use E"));
                Menu.ComboMenu.Add("Combo.R", new CheckBox("Use R"));
                Menu.ComboMenu.Add("Combo.Hydra", new CheckBox("Use Tiamat/Hydra"));
                Menu.ComboMenu.AddGroupLabel("R Options");
                Menu.ComboMenu.Add("Combo.R.HP", new Slider("If Enemy HP is less than:", 50));
                Menu.ComboMenu.Add("Combo.R.Count", new Slider("If there are X or more Enemies:", 2, 1, 5));

                Menu.HarassMenu = Menu.InfoMenu.AddSubMenu("Harass", "Harass");
                Menu.HarassMenu.AddGroupLabel("Harass Options");
                Menu.HarassMenu.Add("Harass.Q", new CheckBox("Use Q"));
                Menu.HarassMenu.Add("Harass.W", new CheckBox("Use W"));
                Menu.HarassMenu.Add("Harass.E", new CheckBox("Use E"));
                Menu.HarassMenu.Add("Harass.Hydra", new CheckBox("Use Tiamat/Hydra", false));

                Menu.ClearMenu = Menu.InfoMenu.AddSubMenu("Clear", "Clear");
                Menu.ClearMenu.AddGroupLabel("Clear Options");
                Menu.ClearMenu.Add("Clear.Q", new CheckBox("Use Q"));
                Menu.ClearMenu.Add("Clear.W", new CheckBox("Use W"));
                Menu.ClearMenu.Add("Clear.E", new CheckBox("Use E"));
                Menu.ClearMenu.Add("Clear.Hydra", new CheckBox("Use Tiamat/Hydra"));
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

                Menu.MiscMenu = Menu.InfoMenu.AddSubMenu("Misc", "Misc");
                Menu.MiscMenu.AddGroupLabel("Misc Options");
                Menu.MiscMenu.Add("Misc.InterruptR", new CheckBox("Auto R to Interrupt Spells"));
                Menu.MiscMenu.AddGroupLabel("Kill Steal");
                Menu.MiscMenu.Add("KillSteal.E", new CheckBox("Use E"));
                Menu.MiscMenu.Add("KillSteal.R", new CheckBox("Use R"));
                Menu.MiscMenu.Add("KillSteal.Ignite", new CheckBox("Use Ignite"));
                Menu.MiscMenu.Add("KillSteal.Smite", new CheckBox("Use Smite"));

                Menu.DrawMenu = Menu.InfoMenu.AddSubMenu("Draw", "Draw");
                Menu.DrawMenu.AddGroupLabel("Draw Options");
                Menu.DrawMenu.Add("Draw.E", new CheckBox("E Range"));
                Menu.DrawMenu.Add("Draw.R", new CheckBox("R Range"));
                Menu.DrawMenu.Add("Draw.Smite", new CheckBox("Draw Smite Status"));

                Chat.Print("XinZhao Buddy - <font color=\"#FFFFFF\">Loaded</font>", Color.FromArgb(255, 210, 68, 74));

                Game.OnTick += Game_OnTick;
                Drawing.OnDraw += Drawing_OnDraw;
                Orbwalker.OnAttack += Orbwalker_OnAttack;
                Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
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

            Functions.SmiteMob();
            KillSteal();
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            try
            {
                if (Player.Instance.IsDead)
                {
                    return;
                }

                var drawE = Menu.DrawMenu["Draw.E"].Cast<CheckBox>().CurrentValue;
                if (drawE && Spells.E.IsReady())
                {
                    new Circle {Color = Color.SkyBlue, Radius = Spells.E.Range}.Draw(Player.Instance.Position);
                }

                var drawR = Menu.DrawMenu["Draw.R"].Cast<CheckBox>().CurrentValue;
                if (drawR && Spells.R.IsReady())
                {
                    new Circle {Color = Color.YellowGreen, Radius = Spells.R.Range}.Draw(Player.Instance.Position);
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
                  Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) &&
                 aiHeroClientW && target is AIHeroClient) ||
                (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) && objAiMinionW &&
                 target is Obj_AI_Minion))
            {
                Spells.W.Cast();
            }
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (!Spells.Q.IsReady())
            {
                return;
            }

            var aiHeroClientQ = Menu.ComboMenu["Combo.Q"].Cast<CheckBox>().CurrentValue ||
                                Menu.ComboMenu["Harass.Q"].Cast<CheckBox>().CurrentValue;
            var objAiMinionQ = Menu.ClearMenu["Clear.Q"].Cast<CheckBox>().CurrentValue;
            if (((!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                  !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) || !aiHeroClientQ ||
                 !(target is AIHeroClient)) &&
                (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) || !objAiMinionQ ||
                 !(target is Obj_AI_Minion)))
            {
                return;
            }

            if (Spells.Q.Cast())
            {
                Orbwalker.ResetAutoAttack();
            }
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs e)
        {
            var interruptR = Menu.MiscMenu["Misc.InterruptR"].Cast<CheckBox>().CurrentValue;
            if (!interruptR || !Spells.R.IsReady() || !sender.IsEnemy || sender.HasBuff("xenzhaointimidate"))
            {
                return;
            }

            if (sender.IsValidTarget(Spells.R.Range))
            {
                Spells.R.Cast(sender.Position);
            }
            else
            {
                var erManaCost = Spells.E.Handle.SData.Mana + Spells.R.Handle.SData.Mana;
                if (Spells.E.IsReady() && sender.IsValidTarget(Spells.E.Range) && Player.Instance.Mana >= erManaCost)
                {
                    Spells.E.Cast(sender);
                }
            }
        }

        private static void Combo()
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                return;
            }
            
            var comboR = Menu.ComboMenu["Combo.R"].Cast<CheckBox>().CurrentValue;
            if (comboR && Spells.R.IsReady() && !Player.Instance.IsDashing())
            {
                var targets =
                    EntityManager.Heroes.Enemies.Where(enemy => enemy != null && enemy.IsValidTarget(Spells.R.Range))
                        .ToList();
                var rHp = Menu.ComboMenu["Combo.R.HP"].Cast<Slider>().CurrentValue;
                var rCount = Menu.ComboMenu["Combo.R.Count"].Cast<Slider>().CurrentValue;
                if ((targets.Count > 1 &&
                     targets.Any(target => target != null && target.Health < SpellSlot.R.GetDamage(target))) ||
                    targets.Any(target => target != null && target.HealthPercent < rHp) || targets.Count >= rCount)
                {
                    Spells.R.Cast();
                }
            }

            var comboE = Menu.ComboMenu["Combo.E"].Cast<CheckBox>().CurrentValue;
            if (comboE && Spells.E.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Magical);
                if (target != null &&
                    (!Player.Instance.IsInAutoAttackRange(target) || Player.Instance.Health < target.Health))
                {
                    Spells.E.Cast(target);
                }
            }

            var comboHydra = Menu.ClearMenu["Combo.Hydra"].Cast<CheckBox>().CurrentValue;
            if (comboHydra)
            {
                var hydra = new Item((int) ItemId.Ravenous_Hydra_Melee_Only, 250);
                var tiamat = new Item((int) ItemId.Tiamat_Melee_Only, 250);
                var item = hydra.IsReady() ? hydra : tiamat;
                var target = TargetSelector.GetTarget(item.Range, DamageType.Physical);
                if ((Item.HasItem((int) ItemId.Ravenous_Hydra_Melee_Only, Player.Instance) ||
                     Item.HasItem((int) ItemId.Tiamat_Melee_Only, Player.Instance)) && item.IsReady() &&
                    target.Distance(Player.Instance) < item.Range - 80)
                {
                    item.Cast();
                }
            }
        }

        private static void Harass()
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                return;
            }

            var harassE = Menu.ComboMenu["Harass.E"].Cast<CheckBox>().CurrentValue;
            if (harassE && Spells.E.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Magical, Player.Instance.Position);
                if (target != null &&
                    (!Player.Instance.IsInAutoAttackRange(target) || Player.Instance.Health < target.Health))
                {
                    Spells.E.Cast(target);
                }
            }

            var harassHydra = Menu.ClearMenu["Harass.Hydra"].Cast<CheckBox>().CurrentValue;
            if (harassHydra)
            {
                var hydra = new Item((int) ItemId.Ravenous_Hydra_Melee_Only, 250);
                var tiamat = new Item((int) ItemId.Tiamat_Melee_Only, 250);
                var item = hydra.IsReady() ? hydra : tiamat;
                var target = TargetSelector.GetTarget(item.Range, DamageType.Physical);
                if ((Item.HasItem((int) ItemId.Ravenous_Hydra_Melee_Only, Player.Instance) ||
                     Item.HasItem((int) ItemId.Tiamat_Melee_Only, Player.Instance)) && item.IsReady() &&
                    target.Distance(Player.Instance) < item.Range - 80)
                {
                    item.Cast();
                }
            }
        }

        private static void Clear()
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) &&
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                return;
            }


            var minionObj =
                ObjectManager.Get<Obj_AI_Minion>()
                    .Where(minion => !minion.IsAlly && minion.Distance(Player.Instance) < Spells.E.Range);
            if (!minionObj.Any())
            {
                return;
            }

            var clearE = Menu.ClearMenu["Clear.E"].Cast<CheckBox>().CurrentValue;
            if (clearE && Spells.E.IsReady())
            {
                var obj = minionObj.FirstOrDefault(minion => minion.Health < SpellSlot.E.GetDamage(minion));
                if (obj == null && !minionObj.Any(minion => Player.Instance.IsInAutoAttackRange(minion)))
                {
                    obj = minionObj.MinOrDefault(minion => minion.Health);
                }

                if (obj != null)
                {
                    Spells.E.Cast(obj);
                }
            }

            var clearHydra = Menu.ClearMenu["Clear.Hydra"].Cast<CheckBox>().CurrentValue;
            if (clearHydra)
            {
                var hydra = new Item((int) ItemId.Ravenous_Hydra_Melee_Only, 250);
                var tiamat = new Item((int) ItemId.Tiamat_Melee_Only, 250);
                var item = hydra.IsReady() ? hydra : tiamat;
                if ((Item.HasItem((int) ItemId.Ravenous_Hydra_Melee_Only, Player.Instance) ||
                     Item.HasItem((int) ItemId.Tiamat_Melee_Only, Player.Instance)) && item.IsReady() &&
                    (minionObj.Count(i => item.IsInRange(i)) > 2 ||
                     minionObj.Any(i => i.MaxHealth >= 1200 && i.Distance(Player.Instance) < item.Range - 80)))
                {
                    item.Cast();
                }
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

            var killStealE = Menu.MiscMenu["KillSteal.E"].Cast<CheckBox>().CurrentValue;
            if (killStealE && Spells.E.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Magical, Player.Instance.Position);
                if (target != null && target.Health < SpellSlot.E.GetDamage(target))
                {
                    Spells.E.Cast(target);
                }
            }

            var killStealR = Menu.MiscMenu["KillSteal.R"].Cast<CheckBox>().CurrentValue;
            if (killStealR && Spells.R.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.R.Range, DamageType.Physical, Player.Instance.Position);
                if (target != null && target.Health < SpellSlot.R.GetDamage(target))
                {
                    Spells.R.Cast();
                }
            }
        }
    }
}