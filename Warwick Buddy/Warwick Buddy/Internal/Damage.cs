using System;
using EloBuddy;
using EloBuddy.SDK;

namespace Warwick_Buddy.Internal
{
    internal static class Damage
    {
        public static float GetDamage(this SpellSlot spell, Obj_AI_Base target)
        {
            var damage = 0f;
            switch (spell)
            {
                case SpellSlot.Q:
                    damage = Math.Max(new float[] {75, 125, 175, 225, 275}[Spells.Q.Level - 1],
                        new float[] {8, 10, 12, 14, 16}[Spells.Q.Level - 1]/100*target.MaxHealth) +
                             1*Player.Instance.FlatMagicDamageMod;
                    break;
                case SpellSlot.R:
                    damage = new float[] {150, 250, 350}[Spells.R.Level - 1] + 2*Player.Instance.FlatPhysicalDamageMod;
                    break;
            }

            return Player.Instance.CalculateDamageOnUnit(target, DamageType.Magical, damage);
        }
    }
}