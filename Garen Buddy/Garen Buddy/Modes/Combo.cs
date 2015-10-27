using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using Garen_Buddy.Internal;
using Utility = Garen_Buddy.Internal.Utility;

namespace Garen_Buddy.Modes
{
    internal class Combo
    {
        public static void Execute()
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                return;
            }

            if (Menu.Combo.Q && Spells.Q.IsReady())
            {
                if (Menu.Misc.QAfterAa)
                {
                    return;
                }

                var target = TargetSelector.GetTarget(700, DamageType.Physical);
                if (target.IsValidTarget(700) && target.PossibleToReachQ())
                {
                    Utility.Debug(string.Format("Used Q to attack {0} (Combo Mode).", target.ChampionName));
                    Spells.Q.Cast();
                }
            }

            if (Menu.Combo.W && Spells.W.IsReady())
            {
                foreach (
                    var enemy in
                        EntityManager.Heroes.Enemies.Where(
                            enemy =>
                                enemy.IsValidTarget() && enemy.IsInAutoAttackRange(Player.Instance) &&
                                enemy.IsAttackingPlayer && enemy.IsFacing(Player.Instance)))
                {
                    Utility.Debug(string.Format("Used W to defend from {0} (Combo Mode).", enemy.ChampionName));
                    Spells.E.Cast();
                }
            }

            if (Menu.Combo.E && Spells.E.IsReady())
            {
                if (Player.Instance.HasBuff("GarenQ") || Player.Instance.HasBuff("GarenE"))
                {
                    return;
                }

                var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Physical);
                if (target.IsValidTarget(Spells.E.Range))
                {
                    Utility.Debug(string.Format("Used E to attack {0} (Combo Mode).", target.ChampionName));
                    Spells.E.Cast();
                }
            }

            if (Menu.Combo.R && Spells.R.IsReady())
            {
                var targets =
                    EntityManager.Heroes.Enemies.Where(
                        enemy =>
                            enemy.IsValidTarget(Spells.R.Range) &&
                            Menu.ComboMenu["R." + enemy.ChampionName].Cast<CheckBox>().CurrentValue &&
                            enemy.Health < Damages.Spell.R.GetDamage(enemy));
                foreach (var target in targets)
                {
                    Utility.Debug(string.Format("Used R on {0} (Combo Mode).", target.ChampionName));
                    Spells.R.Cast(target);
                }
            }
        }
    }
}