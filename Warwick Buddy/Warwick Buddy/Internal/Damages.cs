using System;
using EloBuddy;
using EloBuddy.SDK;

namespace Warwick_Buddy.Internal
{
    internal static class Damages
    {
        public static float GetDamage(this Spell spell, Obj_AI_Base target)
        {
            var damage = new Damage {Value = 0, Type = DamageType.True};
            switch (spell)
            {
                case Spell.Q:
                    if (Spells.Q.IsReady())
                    {
                        damage = new Damage
                        {
                            Value =
                                Math.Max(new float[] {75, 125, 175, 225, 275}[Spells.Q.Level - 1],
                                    new float[] {8, 10, 12, 14, 16}[Spells.Q.Level - 1]/100*target.MaxHealth) +
                                1*Player.Instance.FlatMagicDamageMod,
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
                                new float[] {150, 250, 350}[Spells.R.Level - 1] +
                                2*Player.Instance.FlatPhysicalDamageMod,
                            Type = DamageType.Magical
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
            Q,
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