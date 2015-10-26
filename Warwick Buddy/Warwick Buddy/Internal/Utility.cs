using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace Warwick_Buddy.Internal
{
    internal class Utility
    {
        public static void Debug(string text)
        {
            if (!Menu.Misc.DebugMode)
            {
                return;
            }

            text = string.Format("Warwick Buddy: <font color=\"#FFFFFF\"> {0}</font>", text);
            Chat.Print(text, Color.FromArgb(255, 70, 69, 83));
        }

        internal class AutoQ
        {
            public static void Execute()
            {
                if (!Menu.Harass.AutoQ || Player.Instance.ManaPercent < Menu.Harass.AutoQMana)
                {
                    return;
                }

                var target = TargetSelector.GetTarget(Spells.Q.Range, DamageType.Magical, Player.Instance.Position);
                if (target.IsValidTarget(Spells.Q.Range))
                {
                    Debug(string.Format("Used Q on {0} (Auto Q).", target.ChampionName));
                    Spells.Q.Cast(target);
                }
            }
        }

        internal class AutoR
        {
            public static void Execute()
            {
                if (!Menu.Misc.TowerR || !Spells.R.IsReady())
                {
                    return;
                }

                var target =
                    EntityManager.Heroes.Enemies.Where(i => i.IsValidTarget(Spells.R.Range))
                        .MinOrDefault(i => i.Distance(Player.Instance.Position));
                var tower =
                    ObjectManager.Get<Obj_AI_Turret>()
                        .FirstOrDefault(i => i.IsAlly && !i.IsDead && i.Distance(Player.Instance.Position) <= 850);
                if (target.IsValidTarget() && tower.IsValid() && target.Distance(tower) <= 850)
                {
                    Debug(string.Format("Used R on {0} (Auto R under Tower).", target.ChampionName));
                    Spells.R.Cast(target);
                }
            }
        }

        internal class KillSteal
        {
            public static void Execute()
            {
                foreach (var enemy in EntityManager.Heroes.Enemies.Where(enemy => enemy.IsValid))
                {
                    var igniteSpell = Player.Instance.Spellbook.GetSpell(Spells.Ignite);
                    if (Menu.KillSteal.Ignite && igniteSpell != null && igniteSpell.IsReady)
                    {
                        if (enemy.IsValidTarget(600) && enemy.Health < Damages.Spell.Ignite.GetDamage(enemy))
                        {
                            Debug(string.Format("Used Ignite on {0} (Kill Steal).", enemy.ChampionName));
                            Player.Instance.Spellbook.CastSpell(Spells.Ignite, enemy);
                        }
                    }

                    var smiteSpell = Player.Instance.Spellbook.GetSpell(Spells.Smite);
                    if (Menu.KillSteal.Smite && smiteSpell != null &&
                        smiteSpell.Name.Equals("s5_summonersmiteplayerganker") && smiteSpell.IsReady)
                    {
                        if (enemy.IsValidTarget(760) && enemy.Health < Damages.Spell.Smite.GetDamage(enemy))
                        {
                            Debug(string.Format("Used Smite on {0} (Kill Steal).", enemy.ChampionName));
                            Player.Instance.Spellbook.CastSpell(Spells.Smite, enemy);
                        }
                    }

                    if (Menu.KillSteal.Q && Spells.Q.IsReady())
                    {
                        if (enemy.IsValidTarget(Spells.Q.Range) && enemy.Health < Damages.Spell.Q.GetDamage(enemy))
                        {
                            Debug(string.Format("Used Q on {0} (Kill Steal).", enemy.ChampionName));
                            Spells.Q.Cast(enemy);
                        }
                    }

                    if (Menu.KillSteal.R && Spells.R.IsReady())
                    {
                        if (enemy.IsValidTarget(Spells.R.Range) && enemy.Health < Damages.Spell.R.GetDamage(enemy))
                        {
                            Debug(string.Format("Used R on {0} (Kill Steal).", enemy.ChampionName));
                            Spells.R.Cast(enemy);
                        }
                    }
                }
            }
        }

        internal class Smite
        {
            public static void Execute()
            {
                var smiteSpell = Player.Instance.Spellbook.GetSpell(Spells.Smite);
                if (Menu.Smite.Enable == null || (bool) !Menu.Smite.Enable || smiteSpell == null || !smiteSpell.IsReady)
                {
                    return;
                }

                var obj =
                    EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, 760)
                        .FirstOrDefault(mob => mob.Name.CanSmiteMob());
                if (obj == null)
                {
                    return;
                }

                if (obj.IsValidTarget(760) &&
                    obj.Health < Player.Instance.GetSummonerSpellDamage(obj, DamageLibrary.SummonerSpells.Smite))
                {
                    Debug(string.Format("Used Smite on {0}.", obj.Name));
                    Player.Instance.Spellbook.CastSpell(Spells.Smite, obj);
                }
            }
        }
    }
}