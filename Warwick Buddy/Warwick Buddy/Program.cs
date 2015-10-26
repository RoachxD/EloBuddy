using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using Warwick_Buddy.Internal;
using Warwick_Buddy.Modes;
using Utility = Warwick_Buddy.Internal.Utility;

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

                Spells.Initialize();

                Menu.Initialize();

                DamageIndicator.Initialize();

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

            Combo.Execute();
            
            Harass.Execute();

            Clear.Execute();

            LastHit.Execute();

            Utility.AutoQ.Execute();
            Utility.AutoR.Execute();
            Utility.KillSteal.Execute();
            Utility.Smite.Execute();
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            try
            {
                if (Player.Instance.IsDead)
                {
                    return;
                }

                if (Menu.Draw.Q && Spells.Q.IsReady())
                {
                    new Circle {Color = Color.FromArgb(255, 72, 76, 72), Radius = Spells.Q.Range}.Draw(Player.Instance.Position);
                }

                if (Menu.Draw.R && Spells.R.IsReady())
                {
                    new Circle {Color = Color.FromArgb(255, 74, 68, 54), Radius = Spells.R.Range}.Draw(Player.Instance.Position);
                }

                var smiteSpell = Player.Instance.Spellbook.GetSpell(Spells.Smite);
                if (Menu.Draw.Smite && smiteSpell != null)
                {
                    var barPos = Player.Instance.HPBarPosition;
                    Drawing.DrawText(barPos.X - 10, barPos.Y - 8, Color.White,
                        "Smite: " + (Menu.Smite.Enable != null && (bool) Menu.Smite.Enable ? "Enabled" : "Disabled"));
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

            var aiHeroClientW = Menu.Combo.W || Menu.Harass.W;
            if (((Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) ||
                  Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) && aiHeroClientW &&
                 target is AIHeroClient) ||
                (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) && Menu.Clear.W &&
                 target is Obj_AI_Minion))
            {
                Utility.Debug("Used W on OnAttack Callback.");
                Spells.W.Cast();
            }
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs e)
        {
            if (Menu.Misc.InterruptR && Spells.R.IsReady() && sender.IsValidTarget(Spells.R.Range) && sender.IsEnemy)
            {
                Utility.Debug(string.Format("Used R on {0} (OnInterruptableSpell).", ((AIHeroClient) sender).ChampionName));
                Spells.R.Cast(sender);
            }
        }
    }
}