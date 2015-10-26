﻿using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using XinZhao_Buddy.Internal;

namespace XinZhao_Buddy.Modes
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


            var minionObj =
                ObjectManager.Get<Obj_AI_Minion>()
                    .Where(minion => !minion.IsAlly && minion.Distance(Player.Instance) < Spells.E.Range);
            var objAiMinions = minionObj as Obj_AI_Minion[] ?? minionObj.ToArray();
            if (!objAiMinions.Any())
            {
                return;
            }

            if (Menu.Clear.E && Spells.E.IsReady())
            {
                var obj = objAiMinions.FirstOrDefault(minion => minion.Health < Damages.Spell.E.GetDamage(minion));
                if (obj == null && !objAiMinions.Any(minion => Player.Instance.IsInAutoAttackRange(minion)))
                {
                    obj = objAiMinions.MinOrDefault(minion => minion.Health);
                }

                if (obj != null)
                {
                    Spells.E.Cast(obj);
                }
            }

            if (Menu.Clear.Hydra)
            {
                var hydra = new Item((int) ItemId.Ravenous_Hydra_Melee_Only, 250);
                var tiamat = new Item((int) ItemId.Tiamat_Melee_Only, 250);
                var item = hydra.IsReady() ? hydra : tiamat;
                if ((Item.HasItem((int) ItemId.Ravenous_Hydra_Melee_Only, Player.Instance) ||
                     Item.HasItem((int) ItemId.Tiamat_Melee_Only, Player.Instance)) && item.IsReady() &&
                    (objAiMinions.Count(i => item.IsInRange(i)) > 2 ||
                     objAiMinions.Any(i => i.MaxHealth >= 1200 && i.Distance(Player.Instance) < item.Range - 80)))
                {
                    item.Cast();
                }
            }
        }
    }
}