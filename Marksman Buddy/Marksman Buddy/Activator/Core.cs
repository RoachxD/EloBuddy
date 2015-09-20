using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK.Menu.Values;
using Marksman_Buddy.Internal;

namespace Marksman_Buddy.Activator
{
    internal class Core
    {
        //private int _UseManaPotsPercent => Variables.Activator["MBActivator.UseMPPotPercent"].Cast<Slider>().CurrentValue;

        public Core()
        {
            Game.OnTick += _Game_OnTick;
        }

        private static bool _UseHeal => Variables.Activator["MBActivator.UseHeal"].Cast<CheckBox>().CurrentValue;

        private static int _UseHealPercent
            => Variables.Activator["MBActivator.UseHealPercent"].Cast<Slider>().CurrentValue;

        private static bool _UseHealPots => Variables.Activator["MBActivator.UseHPPot"].Cast<CheckBox>().CurrentValue;

        private int _UseHealPotsPercent => Variables.Activator["MBActivator.UseHPPotPercent"].Cast<Slider>().CurrentValue;

        private static bool _UseManaPots => Variables.Activator["MBActivator.UseMPPot"].Cast<CheckBox>().CurrentValue;

		private int _UseHealManaPercent => Variables.Activator["MBActivator.UseMPPotPercent"].Cast<Slider>().CurrentValue;

        private static void _Game_OnTick(EventArgs args)
        {
            if (_UseHeal && Player.Instance.HealthPercent <= _UseHealPercent)
            {
                var healSlot = Player.Spells.FirstOrDefault(spell => spell.Name.ToLower().Contains("summonerheal"));
                if (healSlot != null)
                {
                    Player.CastSpell(healSlot.Slot);
                }
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
