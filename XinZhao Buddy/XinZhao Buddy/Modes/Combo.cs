using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using XinZhao_Buddy.Internal;
using Utility = XinZhao_Buddy.Internal.Utility;

namespace XinZhao_Buddy.Modes
{
    internal class Combo
    {
        public static void Execute()
        {
            if (!Orbwalker.ActiveModesFlags.HasFlag(Orbwalker.ActiveModes.Combo))
            {
                return;
            }

            if (Menu.Combo.R && Spells.R.IsReady() && !Player.Instance.IsDashing())
            {
                var targets =
                    EntityManager.Heroes.Enemies.Where(enemy => enemy != null && enemy.IsValidTarget(Spells.R.Range))
                        .ToList();
                if ((targets.Count > 1 &&
                     targets.Any(target => target != null && target.Health < Damages.Spell.R.GetDamage(target))) ||
                    targets.Any(target => target != null && target.HealthPercent < Menu.Combo.RHp) ||
                    targets.Count >= Menu.Combo.RCount)
                {
                    Utility.Debug(string.Format("Used R (Combo Mode) [Targets Count: {0}].", targets.Count));
                    Spells.R.Cast();
                }
            }

            if (Menu.Combo.E && Spells.E.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Magical);
                if (target != null &&
                    (!Player.Instance.IsInAutoAttackRange(target) || Player.Instance.Health < target.Health))
                {
                    Utility.Debug(string.Format("Used E on {0} (Combo Mode).", target.Name));
                    Spells.E.Cast(target);
                }
            }

            if (Menu.Combo.Hydra)
            {
                var hydra = new Item((int) ItemId.Ravenous_Hydra_Melee_Only, 250);
                var tiamat = new Item((int) ItemId.Tiamat_Melee_Only, 250);
                var item = hydra.IsReady() ? hydra : tiamat;
                var target = TargetSelector.GetTarget(item.Range, DamageType.Physical);
                if ((Item.HasItem(hydra.Id, Player.Instance) ||
                     Item.HasItem(tiamat.Id, Player.Instance)) && item.IsReady() &&
                    target.Distance(Player.Instance) < item.Range - 80)
                {
                    Utility.Debug("Used Hydra/Tiamat (Combo Mode).");
                    item.Cast();
                }
            }
        }
    }
}