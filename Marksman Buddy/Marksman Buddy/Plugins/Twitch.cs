using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy;
using System.Text;

namespace Marksman_Buddy.Plugin
{
	class Twitch: Internal.PluginBase
	{
		private static Spell.Skillshot _W;
		private static Spell.Active _E;
		private static int[] _EDamage = new int[] { 20, 35, 50, 65, 80 };
		
		public Twitch()
		{
			_SetupMenu();
			_SetupSpells();
			Game.OnTick += Game_OnTick;
		}

		private void _SetupSpells()
		{
			_W = new Spell.Skillshot(SpellSlot.W, 900, EloBuddy.SDK.Enumerations.SkillShotType.Circular, 250, 1400, 275);
			_E = new Spell.Active(SpellSlot.E, 1200);
		}

		private static void _SetupMenu()
		{
			Internal.Variables.Config.AddGroupLabel("Combo");
			Internal.Variables.Config.Add("Twitch.UseECombo", new Slider("Cast E at x Stacks", 5, 1, 5));
			Internal.Variables.Config.Add("Twitch.UseWCombo", new CheckBox("Use W in Comco"));
			Internal.Variables.Config.AddGroupLabel("Harrass");
			Internal.Variables.Config.Add("Twitch.UseEHarrass", new Slider("Cast E at x Stacks", 3, 1, 5));
			Internal.Variables.Config.Add("Twitch.UseWHarrass", new CheckBox("Use W in Harras", false));
			Internal.Variables.Config.AddGroupLabel("Misc");
			Internal.Variables.Config.Add("Twitch.KS", new CheckBox("Use E to KS"));

		}

		static void Game_OnTick(EventArgs args)
		{
			if(Internal.Variables.ComboMode)
				_Combo();
			if (Internal.Variables.HarassMode)
				_Harrass();

			_KS();
		}

		private static void _Harrass()
		{
			var WTarget = TargetSelector.GetTarget(_W.Range, DamageType.True);
			if (Internal.Variables.Config["Twitch.UseWHarrass"].Cast<CheckBox>().CurrentValue && !_W.IsOnCooldown)
				_W.Cast(WTarget);
			foreach (var Hero in ObjectManager.Get<AIHeroClient>().Where(x => x.Position.Distance(ObjectManager.Player.Position) < 1200))
			{
				if (Hero.GetBuffCount("twitchdeadlyvenom") >= Internal.Variables.Config["Twitch.UseEHarrass"].Cast<Slider>().CurrentValue)
				{
					_E.Cast();
				}
			}
		}

		private static void _KS()
		{
			foreach (var Hero in ObjectManager.Get<AIHeroClient>().Where(x => x.Position.Distance(ObjectManager.Player.Position) < 1200))
			{
				if (_ECanKill(Hero, _E) && Internal.Variables.Config["Twitch.KS"].Cast<CheckBox>().CurrentValue)
					_E.Cast();
			}
		}



		private static void _Combo()
		{
			var WTarget = TargetSelector.GetTarget(_W.Range, DamageType.True);
			if (Internal.Variables.Config["Twitch.UseWCombo"].Cast<CheckBox>().CurrentValue && !_W.IsOnCooldown)
				_W.Cast(WTarget);
			foreach (var Hero in ObjectManager.Get<AIHeroClient>().Where(x => x.Position.Distance(ObjectManager.Player.Position) < 1200))
			{
				if (Hero.GetBuffCount("twitchdeadlyvenom") >= Internal.Variables.Config["Twitch.UseECombo"].Cast<Slider>().CurrentValue)
				{
					_E.Cast();
				}
			}

		}

		private static bool _ECanKill(AIHeroClient Hero, Spell.Active _E)
		{
			float EDamage = Convert.ToSingle(Hero.GetBuffCount("twitchdeadlyvenom") * (_EDamage[_E.Level] + ObjectManager.Player.TotalAttackDamage * 0.25 + ObjectManager.Player.TotalMagicalDamage * 0.2)) - 20.0f; //Damage Calc is off
			if (Damage.CalculateDamageOnUnit(ObjectManager.Player, Hero, DamageType.Physical, EDamage) > Hero.Health)
				return true;
			return false;

		}
	}
}
