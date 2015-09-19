using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy;
using Marksman_Buddy.Internal;

namespace Marksman_Buddy.Plugins
{
	class Corki: PluginBase
	{
		private static Spell.Active _E = new Spell.Active(SpellSlot.E, 600);
		private static Spell.Skillshot _Q = new Spell.Skillshot(SpellSlot.Q, 825, EloBuddy.SDK.Enumerations.SkillShotType.Circular, 300, 1000, 250);
		private static Spell.Skillshot _R1 = new Spell.Skillshot(SpellSlot.R, new Spell.Skillshot(SpellSlot.R, 1300, EloBuddy.SDK.Enumerations.SkillShotType.Linear, 200, 2000, 40));
		private static Spell.Skillshot _R2 = new Spell.Skillshot(SpellSlot.R, new Spell.Skillshot(SpellSlot.R, 1500, EloBuddy.SDK.Enumerations.SkillShotType.Linear, 200, 2000, 40));

		public Corki()
		{
			_SetupMenu();
			Game.OnTick += Game_OnTick;
		}

		private void _SetupMenu()
		{
			Variables.Config.AddGroupLabel("Corki");
			Variables.Config.AddGroupLabel("Combo");
			Variables.Config.Add("useQCombo", new CheckBox("Use Q in Combo"));
			Variables.Config.Add("useRCombo", new CheckBox("Use R in Combo"));
			Variables.Config.AddGroupLabel("Harass");
			Variables.Config.Add("useQHarass", new CheckBox("Use Q in Combo", false));
			Variables.Config.Add("useRHarass", new CheckBox("Use R in Combo"));
		}
	}
}
