using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Garen_Buddy.Internal;
using Utility = Garen_Buddy.Internal.Utility;

namespace Garen_Buddy.Modes
{
    internal class Harass
    {
        public static void Execute()
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                return;
            }

            if (Menu.Harass.Q && Spells.Q.IsReady())
            {
                if (Menu.Misc.QAfterAa)
                {
                    return;
                }

                var target = TargetSelector.GetTarget(700, DamageType.Physical);
                if (target.IsValidTarget(700) && target.PossibleToReachQ())
                {
                    Utility.Debug(string.Format("Used Q to attack {0} (Harass Mode).", target.ChampionName));
                    Spells.Q.Cast();
                }
            }

            if (Menu.Harass.W && Spells.W.IsReady())
            {
                foreach (
                    var enemy in
                        EntityManager.Heroes.Enemies.Where(
                            enemy =>
                                enemy.IsValidTarget() && enemy.IsInAutoAttackRange(Player.Instance) &&
                                enemy.IsAttackingPlayer && enemy.IsFacing(Player.Instance)))
                {
                    Utility.Debug(string.Format("Used W to defend from {0} (Harass Mode).", enemy.ChampionName));
                    Spells.E.Cast();
                }
            }

            if (Menu.Harass.E && Spells.E.IsReady())
            {
                if (Player.Instance.HasBuff("GarenQ") || Player.Instance.HasBuff("GarenE"))
                {
                    return;
                }

                var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Physical);
                if (target.IsValidTarget(Spells.E.Range))
                {
                    Utility.Debug(string.Format("Used E to attack {0} (Harass Mode).", target.ChampionName));
                    Spells.E.Cast();
                }
            }
        }
    }
}