using EloBuddy;
using EloBuddy.SDK;

namespace Garen_Buddy.Internal
{
    internal static class Damages
    {
        public static float GetDamage(this Spell spell, Obj_AI_Base target)
        {
            var damage = new Damage {Value = 0, Type = DamageType.True};
            switch (spell)
            {
                case Spell.Q:
                    if (Spells.Q.IsReady() || Player.Instance.HasBuff("GarenQ"))
                    {
                        damage = new Damage
                        {
                            Value =
                                new float[] {30, 55, 80, 105, 130}[Spells.Q.Level - 1] +
                                1.4f*(Player.Instance.BaseAttackDamage + Player.Instance.FlatPhysicalDamageMod),
                            Type = DamageType.Physical
                        };
                    }
                    break;
                case Spell.E:
                    if (Spells.E.IsReady())
                    {
                        damage = new Damage
                        {
                            Value =
                                new float[] {20, 45, 70, 95, 120}[Spells.E.Level - 1] +
                                new float[] {70, 80, 90, 100, 110}[Spells.E.Level - 1]/100*
                                (Player.Instance.BaseAttackDamage + Player.Instance.FlatPhysicalDamageMod),
                            Type = DamageType.Physical
                        };
                    }
                    break;
                case Spell.R:
                    if (Spells.R.IsReady())
                    {
                        damage = new Damage
                        {
                            Value =
                                new float[] {175, 350, 525}[Spells.R.Level - 1] +
                                new[] {28.57f, 33.33f, 40}[Spells.R.Level - 1]/100*(target.MaxHealth - target.Health),
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