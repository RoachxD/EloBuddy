using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Warwick_Buddy.Internal;
using Utility = Warwick_Buddy.Internal.Utility;

namespace Warwick_Buddy.Modes
{
    internal class Clear
    {
        public static void Execute()
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.JungleClear) &&
                !Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.LaneClear))
            {
                return;
            }

            if (!Menu.Clear.Q || !Spells.Q.IsReady())
            {
                return;
            }

            var minionObj = ObjectManager.Get<Obj_AI_Minion>()
                .Where(minion => !minion.IsAlly && minion.Distance(Player.Instance) < Spells.Q.Range);
            var objAiMinions = minionObj as Obj_AI_Minion[] ?? minionObj.ToArray();
            var obj = objAiMinions.FirstOrDefault(minion => minion.Health < Damages.Spell.Q.GetDamage(minion)) ??
                      objAiMinions.MinOrDefault(minion => minion.Health);
            if (obj.IsValidTarget())
            {
                return;
            }

            Utility.Debug("Used Q on a minion/mob (Lane/Jungle Clear Mode).");
            Spells.Q.Cast(obj);
        }
    }
}