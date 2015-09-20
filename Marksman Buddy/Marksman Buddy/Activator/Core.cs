using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK.Menu.Values;
using Marksman_Buddy.Internal;
using EloBuddy.SDK;

namespace Marksman_Buddy.Activator
{
    internal class Core
    {
        

        public Core()
        {
            Game.OnTick += _Game_OnTick;
        }

        private bool _UseHeal
		{
			get
			{
				return Variables.Activator["MBActivator.UseHeal"].Cast<CheckBox>().CurrentValue;
			}
		}

		private bool _UseBotrK
		{
			get
			{
				return Variables.Activator["MBActivator.UseBotrK"].Cast<CheckBox>().CurrentValue;
			}
		}

		private bool _UseYoumuus
		{
			get
			{
				return Variables.Activator["MBActivator.UseYoumuus"].Cast<CheckBox>().CurrentValue;
			}
		}

		private bool _UseCutlass
		{
			get
			{
				return Variables.Activator["MBActivator.UseCutlass"].Cast<CheckBox>().CurrentValue;
			}
		}
			

        private int _UseHealPercent
		{
			get
			{
				return Variables.Activator["MBActivator.UseHealPercent"].Cast<Slider>().CurrentValue;
			}
		}

        private bool _UseHealPots{
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
			

		private int _UseHealManaPercent
		{
			get
			{
				return Variables.Activator["MBActivator.UseMPPotPercent"].Cast<Slider>().CurrentValue;
			}
		}

        private void _Game_OnTick(EventArgs args)
        {
            if (_UseHeal && Player.Instance.HealthPercent <= _UseHealPercent)
            {
                var healSlot = Player.Spells.FirstOrDefault(spell => spell.Name.ToLower().Contains("summonerheal"));
                if (healSlot != null)
                {
                    Player.CastSpell(healSlot.Slot);
                }
            }

			var hasCutlass = (Player.Instance.InventoryItems.FirstOrDefault(item => item.Id == (ItemId)3144) !=
                               null);

			if (_UseCutlass && Variables.ComboMode && hasCutlass)
			{
				var firstOrDefault = Player.Instance.InventoryItems.FirstOrDefault(item => item.Id == (ItemId)3142).SpellSlot;
				Player.CastSpell(firstOrDefault, Orbwalker.GetTarget());
			}

			var hasYoumuus = (Player.Instance.InventoryItems.FirstOrDefault(item => item.Id == (ItemId)3142) !=
                               null);

			if (_UseYoumuus && Variables.ComboMode && hasYoumuus)
			{
				var firstOrDefault = Player.Instance.InventoryItems.FirstOrDefault(item => item.Id == (ItemId)3142).SpellSlot;
				Player.CastSpell(firstOrDefault);
			}

			var hasBotrK = (Player.Instance.InventoryItems.FirstOrDefault(item => item.Id == (ItemId)3153) !=
							   null);

			if (_UseBotrK && Variables.ComboMode && hasBotrK)
			{
				var firstOrDefault = Player.Instance.InventoryItems.FirstOrDefault(item => item.Id == (ItemId)3153).SpellSlot;
				Player.CastSpell(firstOrDefault);
			}

            var hasHealPots = (Player.Instance.InventoryItems.FirstOrDefault(item => item.Name == "healthPotion") !=
                               null);
            if (_UseHealPots && hasHealPots && !Player.HasBuff("RegenerationPotion") && _UseHealPotsPercent > Player.Instance.HealthPercent)
            {
                var firstOrDefault = Player.Instance.InventoryItems.FirstOrDefault(item => item.Name == "healthPotion").SpellSlot;
				Player.CastSpell(firstOrDefault);
            }

            var hasManaPots = (Player.Instance.InventoryItems.FirstOrDefault(item => item.Name == "manaPotion") != null);
            if (_UseManaPots && hasManaPots && !Player.HasBuff("FlaskOfCrystalWater") && _UseHealManaPercent > Player.Instance.ManaPercent)
            {
                var firstOrDefault = Player.Instance.InventoryItems.FirstOrDefault(item => item.Name == "manaPotion").SpellSlot;
                Player.CastSpell(firstOrDefault);
            }
        }
    }
}
