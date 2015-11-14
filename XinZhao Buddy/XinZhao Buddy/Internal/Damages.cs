using EloBuddy;
using EloBuddy.SDK;

namespace XinZhao_Buddy.Internal
{
    internal static class Damages
    {
        public static float GetDamage(this Spell spell, Obj_AI_Base target)
        {
            var damage = new Damage {Value = 0, Type = DamageType.True};
            switch (spell)
            {
                case Spell.E:
                    if (Spells.E.IsReady())
                    {
                        damage = new Damage
                        {
                            Value =
                                new float[] {70, 110, 150, 190, 230}[Spells.E.Level - 1] +
                                0.6f*Player.Instance.FlatMagicDamageMod,
                            Type = DamageType.Magical
                        };
                    }
                    break;
                case Spell.R:
                    if (Spells.R.IsReady())
                    {
                        damage = new Damage
                        {
                            Value =
                                new float[] {75, 175, 275}[Spells.R.Level - 1] + 1*Player.Instance.FlatPhysicalDamageMod +
                                0.15f*target.Health,
                            Type = DamageType.Physical
                        };
                    }
                    break;
                case Spell.Ignite:
                    var igniteSpell = Player.Instance.Spellbook.GetSpell(Spells.Ignite);
                    if (igniteSpell != null && igniteSpell.IsReady)
                    {
                        damage = new Damage
                        {
                            Value = 50 + 20*Player.Instance.Level - (target.HPRegenRate/5*3),
                            Type = DamageType.True
                        };
                    }
                    break;
                case Spell.Smite:
                    var smiteSpell = Player.Instance.Spellbook.GetSpell(Spells.Smite);
                    if ((smiteSpell != null && smiteSpell.Name.Equals("s5_summonersmiteplayerganker") &&
                         smiteSpell.IsReady))
                    {
                        damage = new Damage
                        {
                            Value = 20 + 8*Player.Instance.Level,
                            Type = DamageType.True
                        };
                    }
                    break;
            }

            return Player.Instance.CalculateDamageOnUnit(target, damage.Type, damage.Value);
        }

        internal enum Spell
        {
            E,
            R,
            Ignite,
            Smite
        }

        private class Damage
        {
            public float Value { get; set; }
            public DamageType Type { get; set; }
        }
    }
}