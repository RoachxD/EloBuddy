using System;
using System.Collections.Generic;
using EloBuddy.SDK;
using EloBuddy;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marksman_Buddy.Internal
{
	class Logics
	{
		public static void KS(Spell.SpellBase spell, float damage){
			foreach (var hero in
                HeroManager.Enemies
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
