using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using XinZhao_Buddy.Internal;

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
                    Spells.R.Cast();
                }
            }

            if (Menu.Combo.E && Spells.E.IsReady())
            {
                var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Magical);
                if (target != null &&
                    (!Player.Instance.IsInAutoAttackRange(target) || Player.Instance.Health < target.Health))
                {
                    Spells.E.Cast(target);
                }
            }

            if (Menu.Combo.Hydra)
            {
                var hydra = new Item((int) ItemId.Ravenous_Hydra_Melee_Only, 250);
                var tiamat = new Item((int) ItemId.Tiamat_Melee_Only, 250);
                var item = hydra.IsReady() ? hydra : tiamat;
                var target = TargetSelector.GetTarget(item.Range, DamageType.Physical);
                if ((Item.HasItem((int) ItemId.Ravenous_Hydra_Melee_Only, Player.Instance) ||
                     Item.HasItem((int) ItemId.Tiamat_Melee_Only, Player.Instance)) && item.IsReady() &&
                    target.Distance(Player.Instance) < item.Range - 80)
                {
                    item.Cast();
                }
            }
        }
    }
}