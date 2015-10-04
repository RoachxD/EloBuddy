using EloBuddy;
using EloBuddy.SDK;

namespace XinZhao_Buddy.Internal
{
    internal static class Damage
    {
        public static float GetDamage(this SpellSlot spell, Obj_AI_Base target)
        {
            var damage = 0f;
            switch (spell)
            {
                case SpellSlot.E:
                    damage = new float[] {70, 110, 150, 190, 230}[Spells.E.Level - 1] +
                             0.6f*Player.Instance.FlatMagicDamageMod;

                    break;
                case SpellSlot.R:
                    damage = new float[] {75, 175, 275}[Spells.R.Level - 1] + 1*Player.Instance.FlatPhysicalDamageMod +
                             0.15f*target.Health;
                    break;
            }

            return Player.Instance.CalculateDamageOnUnit(target,
                spell == SpellSlot.E ? DamageType.Magical : DamageType.Physical, damage);
        }
    }
}