using EloBuddy;
using EloBuddy.SDK;
using XinZhao_Buddy.Internal;
using Utility = XinZhao_Buddy.Internal.Utility;

namespace XinZhao_Buddy.Modes
{
    internal class Harass
    {
        public static void Execute()
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Harass))
            {
                return;
            }

            if (Menu.Harass.E && Spells.E.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Magical, Player.Instance.Position);
                if (target != null &&
                    (!Player.Instance.IsInAutoAttackRange(target) || Player.Instance.Health < target.Health))
                {
                    Utility.Debug(string.Format("Used E on {0} (Harass Mode).", target.Name));
                    Spells.E.Cast(target);
                }
            }

            if (Menu.Harass.Hydra)
            {
                var hydra = new Item((int) ItemId.Ravenous_Hydra_Melee_Only, 250);
                var tiamat = new Item((int) ItemId.Tiamat_Melee_Only, 250);
                var item = hydra.IsReady() ? hydra : tiamat;
                var target = TargetSelector.GetTarget(item.Range, DamageType.Physical);
                if ((Item.HasItem(hydra.Id, Player.Instance) ||
                     Item.HasItem(tiamat.Id, Player.Instance)) && item.IsReady() &&
                    target.Distance(Player.Instance) < item.Range - 80)
                {
                    Utility.Debug("Used Hydra/Tiamat (Harass Mode).");
                    item.Cast();
                }
            }
        }
    }
}