
using System;
using System.Collections.Generic;
using System.Linq;
using Marksman_Buddy.Internal;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace Marksman_Buddy.Activator
{
	class Core
	{

		private bool _UseHeal
		{
			get
			{
				return Variables.Activator["MBActivator.UseHeal"].Cast<CheckBox>().CurrentValue;
			}
		}

		private int _UseHealPercent
		{
			get
			{
				return Variables.Activator["MBActivator.UseHealPercent"].Cast<Slider>().CurrentValue;
			}
		}

		private bool _UseHealPots
		{
			get
			{
				return Variables.Activator["MBActivator.UseHPPot"].Cast<CheckBox>().CurrentValue;
			}
		}

		private int _UseHealPotsPercent
		{
			get
			{
				return Variables.Activator["MBActivator.UseHPPotPercent"].Cast<Slider>().CurrentValue;
			}
		}

		private bool _UseManaPots
		{
			get
			{
				return Variables.Activator["MBActivator.UseMPPot"].Cast<CheckBox>().CurrentValue;
			}
		}

		private int _UseManaPotsPercent
		{
			get
			{
				return Variables.Activator["MBActivator.UseMPPotPercent"].Cast<Slider>().CurrentValue;
			}
		}

		public Core()
		{
			Game.OnTick += _Game_OnTick;
		}

		private void _Game_OnTick(EventArgs args)
		{
			if (_UseHeal && Player.Instance.HealthPercent <= _UseHealPercent)
			{
				var HealSlot = Player.Spells.Where(spell => spell.Name.ToLower().Contains("summonerheal")).FirstOrDefault();
				if (HealSlot != null)
				{
					Player.CastSpell(HealSlot.Slot);
				}
			}

			var HasHealPots = (Player.Instance.InventoryItems.Where(item => item.Name == "RegenerationPotion").FirstOrDefault() != null);

			if (_UseHealPots && HasHealPots && !Player.HasBuff("RegenerationPotion"))
			{
				Player.Instance.InventoryItems.Where(item => item.Name == "RegenerationPotion").FirstOrDefault().Cast();
			}

			var HasManaPots = (Player.Instance.InventoryItems.Where(item => item.Name == "FlaskOfCrystalWater").FirstOrDefault() != null);

			if (_UseManaPots && HasManaPots && !Player.HasBuff("FlaskOfCrystalWater"))
			{
				Player.Instance.InventoryItems.Where(item => item.Name == "RegenerationPotion").FirstOrDefault().Cast();
			}
		}
	}
}