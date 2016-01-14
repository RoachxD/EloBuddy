using System.Linq;
using EloBuddy;
using EloBuddy.SDK;

namespace Garen_Buddy.Internal
{
    internal class Spells
    {
        public static Spell.Active Q;
        public static Spell.Active W;
        public static Spell.Active E;
        public static Spell.Targeted R;
        public static SpellSlot Smite;
        public static SpellSlot Ignite;

        public static void Initialize()
        {
            Q = new Spell.Active(SpellSlot.Q);
            W = new Spell.Active(SpellSlot.W);
            E = new Spell.Active(SpellSlot.E, 325);
            R = new Spell.Targeted(SpellSlot.R, 400);

            SummonerSpells.Initialize();
        }
    }

    internal class SummonerSpells
    {
        private static readonly int[] SmiteRed = {3715, 1415, 1414, 1413, 1412};
        private static readonly int[] SmiteBlue = {3706, 1403, 1402, 1401, 1400};

        public static void Initialize()
        {
            SetSummonerSlots();
        }

        private static void SetSummonerSlots()
        {
            if (SmiteBlue.Any(x => Player.Instance.InventoryItems.FirstOrDefault(a => a.Id == (ItemId) x) != null))
            {
                Spells.Smite = Player.Instance.GetSpellSlotFromName("s5_summonersmiteplayerganker");
            }
            else if (
                SmiteRed.Any(
                    x => Player.Instance.InventoryItems.FirstOrDefault(a => a.Id == (ItemId) x) != null))
            {
                Spells.Smite = Player.Instance.GetSpellSlotFromName("s5_summonersmiteduel");
            }
            else
            {
                Spells.Smite = Player.Instance.GetSpellSlotFromName("summonersmite");
            }

            Spells.Ignite = Player.Instance.GetSpellSlotFromName("summonerdot");
        }
    }
}