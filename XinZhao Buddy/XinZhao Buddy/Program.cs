using System;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
using Warwick_Buddy.Internal;
using XinZhao_Buddy.Internal;
using XinZhao_Buddy.Modes;
using Utility = XinZhao_Buddy.Internal.Utility;

namespace XinZhao_Buddy
{
    internal class Program
    {
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
            AppDomain.CurrentDomain.UnhandledException +=
                delegate(object sender, UnhandledExceptionEventArgs args1)
                {
                    Logger.Log(LogLevel.Error, args1.ExceptionObject.ToString());
                };
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            if (!Player.Instance.ChampionName.Equals("XinZhao"))
            {
                return;
            }

            Spells.Initialize();

            Menu.Initialize();

            DamageIndicator.Initialize();

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Orbwalker.OnAttack += Orbwalker_OnAttack;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
            Interrupter.OnInterruptableSpell += Interrupter_OnInterruptableSpell;
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

            Utility.Smite.Execute();
            Utility.KillSteal.Execute();
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.Instance.IsDead)
            {
                return;
            }

            if (Menu.Draw.E && Spells.E.IsReady())
            {
                new Circle {Color = Color.FromArgb(255, 57, 37, 72), Radius = Spells.E.Range}.Draw(
                    Player.Instance.Position);
            }

            if (Menu.Draw.R && Spells.R.IsReady())
            {
                new Circle {Color = Color.FromArgb(255, 72, 72, 116), Radius = Spells.R.Range}.Draw(
                    Player.Instance.Position);
            }

            var smiteSpell = Player.Instance.Spellbook.GetSpell(Spells.Smite);
            if (Menu.Draw.Smite && smiteSpell != null)
            {
                var barPos = Player.Instance.HPBarPosition;
                var smiteStatus = Menu.Smite.Enable != null && (bool) Menu.Smite.Enable;
                Drawing.DrawText(barPos.X - 10, barPos.Y - 8, Color.White,
                    "Smite: " + (smiteStatus ? "Enabled" : "Disabled"));
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
                  Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) &&
                 aiHeroClientW && target is AIHeroClient) ||
                (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) && Menu.Clear.W &&
                 target is Obj_AI_Minion))
            {
                Utility.Debug("Used W on OnAttack Callback.");
                Spells.W.Cast();
            }
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (!Spells.Q.IsReady())
            {
                return;
            }

            var aiHeroClientQ = Menu.Combo.Q || Menu.Harass.Q;
            if (((!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) &&
                  !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass)) || !aiHeroClientQ ||
                 !(target is AIHeroClient)) &&
                (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear) || !Menu.Clear.Q ||
                 !(target is Obj_AI_Minion)))
            {
                return;
            }

            if (Spells.Q.Cast())
            {
                Utility.Debug("Used Q on OnPostAttack Callback.");
                Orbwalker.ResetAutoAttack();
            }
        }

        private static void Interrupter_OnInterruptableSpell(Obj_AI_Base sender,
            Interrupter.InterruptableSpellEventArgs e)
        {
            if (!Menu.Misc.InterruptR || !Spells.R.IsReady() || !sender.IsEnemy || sender.HasBuff("xenzhaointimidate"))
            {
                return;
            }

            if (sender.IsValidTarget(Spells.R.Range))
            {
                Utility.Debug(string.Format("Used R on {0} (OnInterruptableSpell).",
                    ((AIHeroClient) sender).ChampionName));
                Spells.R.Cast(sender.Position);
            }
            else
            {
                var erManaCost = Spells.E.Handle.SData.Mana + Spells.R.Handle.SData.Mana;
                if (Spells.E.IsReady() && sender.IsValidTarget(Spells.E.Range) && Player.Instance.Mana >= erManaCost)
                {
                    Utility.Debug(string.Format("Used E on {0} (OnInterruptableSpell).",
                        ((AIHeroClient) sender).ChampionName));
                    Spells.E.Cast(sender);
                }
            }
        }
    }
}