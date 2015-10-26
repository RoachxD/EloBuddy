using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using Warwick_Buddy.Internal;
using Utility = Warwick_Buddy.Internal.Utility;

namespace Warwick_Buddy.Modes
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
                var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical, Player.Instance.Position);
                if (target.IsValidTarget(Spells.Q.Range))
                {
                    Utility.Debug(string.Format("Used Q on {0} (Combo Mode).", target.ChampionName));
                    Spells.Q.Cast(target);
                }
            }

            if (Menu.Combo.R && Spells.R.IsReady())
            {
                var targets =
                    EntityManager.Heroes.Enemies.Where(
                        enemy =>
                            Menu.ComboMenu["R." + enemy.ChampionName].Cast<CheckBox>().CurrentValue &&
                            Player.Instance.Distance(enemy) < Spells.R.Range);
                foreach (var target in targets)
                {
                    if (!target.IsValidTarget())
                    {
                        continue;
                    }

                    if (Menu.Combo.Smite)
                    {
                        Utility.Debug(string.Format("Used Smite on {0} (Combo Mode).", target.ChampionName));
                        Player.Instance.Spellbook.CastSpell(Spells.Smite, target);
                    }

                    Utility.Debug(string.Format("Used R on {0} (Combo Mode).", target.ChampionName));
                    Spells.R.Cast(target);
                }
            }

            if (Menu.Combo.W && Spells.W.IsReady() &&
                EntityManager.Heroes.Allies.Any(
                    ally => !ally.IsMe && ally.IsValidTarget(Spells.W.Range) && ally.IsAttackingPlayer))
            {
                Utility.Debug("Used W to help a teammate (Combo Mode).");
                Spells.W.Cast();
            }
        }
    }
}