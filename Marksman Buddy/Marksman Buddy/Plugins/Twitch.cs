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
	class Twitch : Internal.PluginBase
	{
		private static Spell.Skillshot _W;
		private static Spell.Active _E;
		private static int[] _EDamage = new int[] { 20, 35, 50, 65, 80 };
		private static string[] _Minions = new string[] { "SRU_Dragon", "SRU_Baron", "Sru_Crab", "Siege" };

		public Twitch()
		{
			_SetupMenu();
			_SetupSpells();
			Game.OnTick += Game_OnTick;
		}

		private static void _SetupSpells()
		{
			_W = new Spell.Skillshot(SpellSlot.W, 900, EloBuddy.SDK.Enumerations.SkillShotType.Circular, 250, 1400, 275);
			_E = new Spell.Active(SpellSlot.E, 1200);
		}

		private static void _SetupMenu()
		{
			Variables.Config.AddGroupLabel("Twitch");
			Variables.Config.AddGroupLabel("Combo");
			Variables.Config.Add("Twitch.UseECombo", new CheckBox("Use E in Combo"));
			Variables.Config.Add("Twitch.UseEComboStacks", new Slider("Cast E at x Stacks", 5, 1, 5));
			Variables.Config.Add("Twitch.UseWCombo", new CheckBox("Use W in Combo"));
			Variables.Config.AddGroupLabel("Harrass");
			Variables.Config.Add("Twitch.UseEHarass", new CheckBox("Use E in Harass"));
			Variables.Config.Add("Twitch.UseEHarassStacks", new Slider("Cast E at x Stacks", 3, 1, 5));
			Variables.Config.Add("Twitch.UseWHarass", new CheckBox("Use W in Harass", false));
			Variables.Config.AddGroupLabel("Misc");
			Variables.Config.Add("Twitch.KS", new CheckBox("Use E to KS"));
			Variables.Config.Add("Twitch.EExecute", new CheckBox("Use E to execute Large Minions")); //Kappa
		}

		static void Game_OnTick(EventArgs args)
		{
			if (Variables.ComboMode)
			{
				_Combo();
			}
			if (Variables.HarassMode)
			{
				_Harrass();
			}
			_KS();
			_Execute();
		}

		private static void _Execute()
		{
			foreach (var minion in 
				ObjectManager.Get<Obj_AI_Minion>()
					.Where(x => x.Position.Distance(ObjectManager.Player.Position) < 1200))
			{
				if (minion.Team != ObjectManager.Player.Team &&
						_ContainsSimiliar(_Minions, minion.Name))
				{
					if (_ECanKill(minion, _E) &&
						Variables.Config["Twitch.EExecute"].Cast<CheckBox>().CurrentValue)
					{
						_E.Cast();
					}
						
				}
			}
		}

		private static bool _ContainsSimiliar(string[] array, string simily)//Who needs comparators Kappa
		{
			foreach (string element in array)
			{
				if (element.ToLower().Contains(simily.ToLower()))
					return true;
			}
			return false;
		}

		private static void _Harrass()
		{
			var WTarget = TargetSelector.GetTarget(_W.Range, DamageType.True);
			if (Variables.Config["Twitch.UseWHarass"].Cast<CheckBox>().CurrentValue
				&& !_W.IsOnCooldown)
			{
				_W.Cast(WTarget);
			}

			if (!Variables.Config["Twitch.UseEHarass"].Cast<CheckBox>().CurrentValue)
				return;
			foreach (var Hero in
				ObjectManager.Get<AIHeroClient>()
					.Where(x => x.Position.Distance(ObjectManager.Player.Position) < 1200))
			{
				if (Hero.GetBuffCount("twitchdeadlyvenom") >=
					Variables.Config["Twitch.UseEHarassStacks"].Cast<Slider>().CurrentValue)
				{
					_E.Cast();
				}
			}
		}

		private static void _KS()
		{
			foreach (var Hero in
				ObjectManager.Get<AIHeroClient>()
					.Where(x => x.Position.Distance(ObjectManager.Player.Position) < 1200))
			{
				if (_ECanKill(Hero, _E) && Variables.Config["Twitch.KS"].Cast<CheckBox>().CurrentValue)
				{
					_E.Cast();
				}
					
			}
		}



		private static void _Combo()
		{
			var WTarget = TargetSelector.GetTarget(_W.Range, DamageType.True);
			if (Variables.Config["Twitch.UseWCombo"].Cast<CheckBox>().CurrentValue
				&& !_W.IsOnCooldown)
			{
				_W.Cast(WTarget);
			}

			if (!Variables.Config["Twitch.UseECombo"].Cast<CheckBox>().CurrentValue)
			{
				return;
			}
				
			foreach (var Hero in
				ObjectManager.Get<AIHeroClient>()
					.Where(x => x.Position.Distance(ObjectManager.Player.Position) < 1200))
			{
				if (Hero.GetBuffCount("twitchdeadlyvenom") >=
					Variables.Config["Twitch.UseEComboStacks"].Cast<Slider>().CurrentValue)
				{
					_E.Cast();
				}
			}

		}

		private static bool _ECanKill(Obj_AI_Base Hero, Spell.Active _E)
		{
			float EDamage = Convert.ToSingle(Hero.GetBuffCount("twitchdeadlyvenom") *
				(_EDamage[_E.Level] +
				ObjectManager.Player.TotalAttackDamage * 0.25
				+ ObjectManager.Player.TotalMagicalDamage * 0.2)) - 20.0f; //Damage Calc is off
			return Damage.CalculateDamageOnUnit(ObjectManager.Player, Hero, DamageType.Physical, EDamage) > Hero.Health;

		}
	}
}
