using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace Warwick_Buddy.Internal
{
    internal class Spells
    {
        public static Spell.Targeted Q;
        public static Spell.Active W;
        public static Spell.Targeted R;
        public static SpellSlot Smite;
        public static SpellSlot Ignite;

        public static void Initialize()
        {
            Q = new Spell.Targeted(SpellSlot.Q, 400);
            W = new Spell.Active(SpellSlot.W, 1250);
            R = new Spell.Targeted(SpellSlot.R, 700);

            SummonerSpells.Initialize();
        }
    }

    internal class SummonerSpells
    {
        private static readonly int[] SmitePurple = { 3713, 3726, 3725, 3724, 3723, 3933 };
        private static readonly int[] SmiteGrey = { 3711, 3722, 3721, 3720, 3719, 3932 };
        private static readonly int[] SmiteRed = { 3715, 3718, 3717, 3716, 3714, 3931 };
        private static readonly int[] SmiteBlue = { 3706, 3710, 3709, 3708, 3707, 3930 };

        public static void Initialize()
        {
            SetSummonerSlots();
        }
        private static void SetSummonerSlots()
        {
            if (SmiteBlue.Any(x => Player.Instance.InventoryItems.FirstOrDefault(a => a.Id == (ItemId)x) != null))
            {
                Spells.Smite = Player.Instance.GetSpellSlotFromName("s5_summonersmiteplayerganker");
            }
            else if (
                SmiteRed.Any(
                    x => Player.Instance.InventoryItems.FirstOrDefault(a => a.Id == (ItemId)x) != null))
            {
                Spells.Smite = Player.Instance.GetSpellSlotFromName("s5_summonersmiteduel");
            }
            else if (
                SmiteGrey.Any(
                    x => Player.Instance.InventoryItems.FirstOrDefault(a => a.Id == (ItemId)x) != null))
            {
                Spells.Smite = Player.Instance.GetSpellSlotFromName("s5_summonersmitequick");
            }
            else if (
                SmitePurple.Any(
                    x => Player.Instance.InventoryItems.FirstOrDefault(a => a.Id == (ItemId)x) != null))
            {
                Spells.Smite = Player.Instance.GetSpellSlotFromName("itemsmiteaoe");
            }
            else
            {
                Spells.Smite = Player.Instance.GetSpellSlotFromName("summonersmite");
            }

            Spells.Ignite = Player.Instance.GetSpellSlotFromName("summonerdot");
        }
    }
}