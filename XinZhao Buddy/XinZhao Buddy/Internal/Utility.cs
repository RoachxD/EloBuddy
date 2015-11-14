using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace XinZhao_Buddy.Internal
{
    internal class Utility
    {
        public static void Debug(string text)
        {
            if (!Menu.Misc.DebugMode)
            {
                return;
            }

            text = string.Format("XinZhao Buddy: <font color=\"#FFFFFF\"> {0}</font>", text);
            Chat.Print(text, Color.FromArgb(255, 123, 94, 84));
        }

        internal class KillSteal
        {
            public static void Execute()
            {
                var igniteSpell = Player.Instance.Spellbook.GetSpell(Spells.Ignite);
                if (Menu.KillSteal.Ignite && igniteSpell != null && igniteSpell.IsReady)
                {
                    var target = TargetSelector.GetTarget(600, DamageType.True);
                    if (target != null &&
                        target.Health <
                        Player.Instance.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Ignite))
                    {
                        Player.Instance.Spellbook.CastSpell(Spells.Ignite, target);
                    }
                }

                var smiteSpell = Player.Instance.Spellbook.GetSpell(Spells.Smite);
                if (Menu.KillSteal.Smite && smiteSpell != null && smiteSpell.Name.Equals("s5_summonersmiteplayerganker") &&
                    smiteSpell.IsReady)
                {
                    var target = TargetSelector.GetTarget(760, DamageType.True);
                    if (target != null &&
                        target.Health <
                        Player.Instance.GetSummonerSpellDamage(target, DamageLibrary.SummonerSpells.Smite))
                    {
                        Player.Instance.Spellbook.CastSpell(Spells.Smite, target);
                    }
                }

                if (Menu.KillSteal.E && Spells.E.IsReady())
                {
                    var target = TargetSelector.GetTarget(Spells.E.Range, DamageType.Magical, Player.Instance.Position);
                    if (target != null && target.Health < Damages.Spell.E.GetDamage(target))
                    {
                        Spells.E.Cast(target);
                    }
                }

                if (Menu.KillSteal.R && Spells.R.IsReady())
                {
                    var target = TargetSelector.GetTarget(Spells.R.Range, DamageType.Physical, Player.Instance.Position);
                    if (target != null && target.Health < Damages.Spell.R.GetDamage(target))
                    {
                        Spells.R.Cast();
                    }
                }
            }
        }

        internal class Smite
        {
            public static void Execute()
            {
                var smiteSpell = Player.Instance.Spellbook.GetSpell(Spells.Smite);
                if (smiteSpell == null || !smiteSpell.IsReady || Menu.Smite.Enable == null || (bool) !Menu.Smite.Enable)
                {
                    return;
                }

                var obj =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, 760)
                        .FirstOrDefault(mob => Extensions.CanSmiteMob(mob.Name));
                if (obj == null)
                {
                    return;
                }

                if (obj.IsValidTarget(760) &&
                    obj.Health < Player.Instance.GetSummonerSpellDamage(obj, DamageLibrary.SummonerSpells.Smite))
                {
                    Player.Instance.Spellbook.CastSpell(Spells.Smite, obj);
                }
            }
        }
    }
}