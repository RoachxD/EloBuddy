using System;
using System.Drawing;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Rendering;
using EloBuddy.SDK.Utils;
using Garen_Buddy.Internal;
using Garen_Buddy.Modes;
using Utility = Garen_Buddy.Internal.Utility;

// ReSharper disable UnusedParameter.Local

namespace Garen_Buddy
{
    internal class Program
    {
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
            if (!Player.Instance.ChampionName.Equals("Garen"))
            {
                return;
            }

            Spells.Initialize();

            Menu.Initialize();

            DamageIndicator.Initialize();

            Game.OnTick += Game_OnTick;
            Drawing.OnDraw += Drawing_OnDraw;
            Orbwalker.OnPostAttack += Orbwalker_OnPostAttack;
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

            Utility.KillSteal.Execute();
            Utility.Smite.Execute();
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.Instance.IsDead)
            {
                return;
            }

            if (Menu.Draw.E && (Spells.E.IsReady() || Player.Instance.HasBuff("GarenE")))
            {
                new Circle {Color = Color.FromArgb(255, 161, 100, 88), Radius = Spells.E.Range}.Draw(
                    Player.Instance.Position);
            }

            if (Menu.Draw.R && Spells.R.IsReady())
            {
                new Circle {Color = Color.FromArgb(255, 155, 131, 90), Radius = Spells.R.Range}.Draw(
                    Player.Instance.Position);
            }

            var smiteSpell = Player.Instance.Spellbook.GetSpell(Spells.Smite);
            if (Menu.Draw.Smite && smiteSpell != null)
            {
                var barPos = Player.Instance.HPBarPosition;
                Drawing.DrawText(barPos.X - 10, barPos.Y - 8, Color.White,
                    "Smite: " + (Menu.Smite.Enable != null && (bool) Menu.Smite.Enable ? "Enabled" : "Disabled"));
            }
        }

        private static void Orbwalker_OnPostAttack(AttackableUnit target, EventArgs args)
        {
            if (!Spells.Q.IsReady())
            {
                return;
            }

            var aiHeroClientQ = (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo) && Menu.Combo.Q) ||
                                (Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass) && Menu.Harass.Q);
            if (aiHeroClientQ && Menu.Misc.QAfterAa)
            {
                Utility.Debug("Used Q on OnPostAttack Callback.");
                Spells.Q.Cast();
            }
        }
    }
}