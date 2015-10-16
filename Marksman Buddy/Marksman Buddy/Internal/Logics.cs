using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace Marksman_Buddy.Internal
{
    internal class Logics
    {
        public static void KS(Spell.SpellBase spell, float damage)
        {
            foreach (var hero in
                EntityManager.Heroes.Enemies
                    .Where(x => x.Position.Distance(ObjectManager.Player) < spell.Range))
            {
                if (!hero.IsDead && !hero.IsZombie && damage > hero.Health)
                {
                    spell.Cast(hero);
                }
            }
        }
    }
}