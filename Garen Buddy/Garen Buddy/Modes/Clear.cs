using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using Garen_Buddy.Internal;
using Utility = Garen_Buddy.Internal.Utility;

namespace Garen_Buddy.Modes
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

            if (!Menu.Clear.E || !Spells.E.IsReady() || Player.Instance.HasBuff("GarenQ") ||
                Player.Instance.HasBuff("GarenE"))
            {
                return;
            }

            var minionObj =
                ObjectManager.Get<Obj_AI_Minion>()
                    .Where(minion => minion.IsValidTarget(Spells.E.Range) && !minion.IsAlly);
            var objAiMinions = minionObj as Obj_AI_Minion[] ?? minionObj.ToArray();
            if (objAiMinions.Count() < 2)
            {
                return;
            }

            Utility.Debug("Used E on minions/mobs (Lane/Jungle Clear Mode).");
            Spells.E.Cast();
        }
    }
}