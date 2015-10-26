using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Warwick_Buddy.Internal;
using Utility = Warwick_Buddy.Internal.Utility;

namespace Warwick_Buddy.Modes
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
                var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical, Player.Instance.Position);
                if (target.IsValidTarget(Spells.Q.Range))
                {
                    Utility.Debug(string.Format("Used Q on {0} (Harass Mode).", target.ChampionName));
                    Spells.Q.Cast(target);
                }
            }

            if (Menu.Harass.W && Spells.W.IsReady() &&
                EntityManager.Heroes.Allies.Any(
                    ally => ally != null && !ally.IsMe && ally.IsValidTarget(Spells.W.Range) && ally.IsAttackingPlayer))
            {
                Utility.Debug("Used W to help a teammate (Harass Mode).");
                Spells.W.Cast();
            }
        }
    }
}