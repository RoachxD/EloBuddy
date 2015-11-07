using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Warwick_Buddy.Internal;
using Utility = Warwick_Buddy.Internal.Utility;

namespace Warwick_Buddy.Modes
{
    internal class LastHit
    {
        public static void Execute()
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LastHit))
            {
                return;
            }

            if (!Menu.LastHit.Q || !Spells.Q.IsReady())
            {
                return;
            }

            var obj =
                EntityManager.MinionsAndMonsters.GetLaneMinions(EntityManager.UnitTeam.Enemy, Player.Instance.Position,
                    Spells.Q.Range).FirstOrDefault(minion => minion.Health < Damages.Spell.Q.GetDamage(minion));
            if (obj.IsValidTarget(Spells.Q.Range))
            {
                return;
            }

            Utility.Debug("Used Q on a minion/mob (Last Hit Mode).");
            Spells.Q.Cast(obj);
        }
    }
}