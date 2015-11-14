using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using SharpDX;
using XinZhao_Buddy.Internal;
using Color = System.Drawing.Color;

namespace Warwick_Buddy.Internal
{
    internal static class DamageIndicator
    {
        public delegate float DamageToUnitDelegate(AIHeroClient hero);

        private const int BarWidth = 104;
        private const int LineThickness = 9;
        private static readonly Vector2 BarOffset = new Vector2(1, 0); // -9, 11
        private static Dictionary<DamageToUnitDelegate, Color> _spells;

        private static float EDamage(AIHeroClient hero)
        {
            return Damages.Spell.E.GetDamage(hero);
        }

        private static float RDamage(AIHeroClient hero)
        {
            return Damages.Spell.R.GetDamage(hero);
        }

        private static float IgniteDamage(AIHeroClient hero)
        {
            return Damages.Spell.Ignite.GetDamage(hero);
        }

        private static float SmiteDamage(AIHeroClient hero)
        {
            return Damages.Spell.Smite.GetDamage(hero);
        }

        public static void Initialize()
        {
            _spells = new Dictionary<DamageToUnitDelegate, Color>
            {
                {EDamage, Color.FromArgb(255, 57, 37, 72)},
                {RDamage, Color.FromArgb(255, 72, 72, 116)},
                {IgniteDamage, Color.FromArgb(255, 120, 56, 28)},
                {SmiteDamage, Color.FromArgb(255, 70, 130, 156)}
            };

            Drawing.OnEndScene += Drawing_OnEndScene;
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            if (!Menu.Draw.DamageHpBar)
            {
                return;
            }

            foreach (
                var enemy in EntityManager.Heroes.Enemies.Where(enemy => enemy.IsValidTarget() && enemy.IsHPBarRendered)
                )
            {
                var damage = _spells.Sum(v => v.Key(enemy));
                if (damage <= 0)
                {
                    continue;
                }

                foreach (var spell in _spells)
                {
                    var damagePercentage = ((enemy.Health - damage) > 0 ? (enemy.Health - damage) : 0)/
                                           (enemy.MaxHealth + enemy.AllShield + enemy.AttackShield + enemy.MagicShield);
                    var healthPercentage = enemy.Health/
                                           (enemy.MaxHealth + enemy.AllShield + enemy.AttackShield + enemy.MagicShield);
                    var startPoint = new Vector2(
                        (int) (enemy.HPBarPosition.X + BarOffset.X + damagePercentage*BarWidth),
                        (int) (enemy.HPBarPosition.Y + BarOffset.Y) - 5);
                    var endPoint =
                        new Vector2((int) (enemy.HPBarPosition.X + BarOffset.X + healthPercentage*BarWidth) + 1,
                            (int) (enemy.HPBarPosition.Y + BarOffset.Y) - 5);
                    Drawing.DrawLine(startPoint, endPoint, LineThickness, spell.Value);

                    damage -= spell.Key(enemy);
                }
            }
        }
    }
}