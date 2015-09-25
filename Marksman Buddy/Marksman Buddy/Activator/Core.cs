using System;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;
using Marksman_Buddy.Internal;

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
            get { return Variables.Activator["Activator.UseHeal"].Cast<CheckBox>().CurrentValue; }
        }

        private bool _UseBotrK
        {
            get { return Variables.Activator["Activator.UseBotrK"].Cast<CheckBox>().CurrentValue; }
        }

        private bool _UseYoumuus
        {
            get { return Variables.Activator["Activator.UseYoumuus"].Cast<CheckBox>().CurrentValue; }
        }

        private bool _UseCutlass
        {
            get { return Variables.Activator["Activator.UseCutlass"].Cast<CheckBox>().CurrentValue; }
        }

        private int _UseHealPercent
        {
            get { return Variables.Activator["Activator.UseHealPercent"].Cast<Slider>().CurrentValue; }
        }

        private bool _UseHealPots
        {
            get { return Variables.Activator["Activator.UseHPPot"].Cast<CheckBox>().CurrentValue; }
        }

        private int _UseHealPotsPercent
        {
            get { return Variables.Activator["Activator.UseHPPotPercent"].Cast<Slider>().CurrentValue; }
        }

        private bool _UseManaPots
        {
            get { return Variables.Activator["Activator.UseMPPot"].Cast<CheckBox>().CurrentValue; }
        }

        private int _UseHealManaPercent
        {
            get { return Variables.Activator["Activator.UseMPPotPercent"].Cast<Slider>().CurrentValue; }
        }

        private void _Game_OnTick(EventArgs args)
        {
            if (_UseHeal && Player.Instance.HealthPercent <= _UseHealPercent && !Player.Instance.IsInShopRange())
            {
                var healSlot = Player.Spells.FirstOrDefault(spell => spell.Name.ToLower().Contains("summonerheal"));
                if (healSlot != null)
                {
                    Player.CastSpell(healSlot.Slot);
                }
            }

            var hasCutlass = (Player.Instance.InventoryItems.FirstOrDefault(item => item.Id == ItemId.Bilgewater_Cutlass) !=
                              null);

            if (_UseCutlass && Variables.ComboMode && hasCutlass)
            {
                var inventorySlot = Player.Instance.InventoryItems.FirstOrDefault(item => item.Id == ItemId.Bilgewater_Cutlass);
                if (inventorySlot != null)
                {
                    var firstOrDefault =
                        inventorySlot.SpellSlot;
                    Player.CastSpell(firstOrDefault, Orbwalker.GetTarget());
                }
            }

            var hasYoumuus = (Player.Instance.InventoryItems.FirstOrDefault(item => item.Id ==  ItemId.Youmuus_Ghostblade) !=
                              null);

            if (_UseYoumuus && Variables.ComboMode && hasYoumuus)
            {
                var inventorySlot = Player.Instance.InventoryItems.FirstOrDefault(item => item.Id == ItemId.Youmuus_Ghostblade);
                if (inventorySlot != null)
                {
                    var firstOrDefault =
                        inventorySlot.SpellSlot;
                    Player.CastSpell(firstOrDefault);
                }
            }

            var hasBotrK = (Player.Instance.InventoryItems.FirstOrDefault(item => item.Id == ItemId.Blade_of_the_Ruined_King) !=
                            null);

            if (_UseBotrK && Variables.ComboMode && hasBotrK)
            {
                var inventorySlot = Player.Instance.InventoryItems.FirstOrDefault(item => item.Id == ItemId.Blade_of_the_Ruined_King);
                if (inventorySlot != null)
                {
                    var firstOrDefault =
                        inventorySlot.SpellSlot;
                    Player.CastSpell(firstOrDefault, Orbwalker.GetTarget());
                }
            }

            var hasHealPots = (Player.Instance.InventoryItems.FirstOrDefault(item => item.Id == ItemId.Health_Potion) !=
                               null);
            if (_UseHealPots && hasHealPots && !Player.HasBuff("RegenerationPotion") &&
                _UseHealPotsPercent > Player.Instance.HealthPercent)
            {
                var inventorySlot =
                    Player.Instance.InventoryItems.FirstOrDefault(item => item.Id == ItemId.Health_Potion);
                if (inventorySlot != null)
                {
                    var firstOrDefault =
                        inventorySlot.SpellSlot;
                    Player.CastSpell(firstOrDefault);
                }
            }

            var hasManaPots = (Player.Instance.InventoryItems.FirstOrDefault(item => item.Id == ItemId.Mana_Potion) !=
                               null);
            if (_UseManaPots && hasManaPots && !Player.HasBuff("FlaskOfCrystalWater") &&
                _UseHealManaPercent > Player.Instance.ManaPercent)
            {
                var inventorySlot = Player.Instance.InventoryItems.FirstOrDefault(item => item.Id == ItemId.Mana_Potion);
                if (inventorySlot != null)
                {
                    var firstOrDefault =
                        inventorySlot.SpellSlot;
                    Player.CastSpell(firstOrDefault);
                }
            }
        }
    }
}