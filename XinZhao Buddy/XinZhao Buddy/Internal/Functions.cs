using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace XinZhao_Buddy.Internal
{
    internal static class Functions
    {
        private static readonly int[] SmitePurple = {3713, 3726, 3725, 3724, 3723, 3933};
        private static readonly int[] SmiteGrey = {3711, 3722, 3721, 3720, 3719, 3932};
        private static readonly int[] SmiteRed = {3715, 3718, 3717, 3716, 3714, 3931};
        private static readonly int[] SmiteBlue = {3706, 3710, 3709, 3708, 3707, 3930};

        public static void SmiteMob()
        {
            var smite = Menu.ClearMenu["Smite.Enable"].Cast<CheckBox>().CurrentValue;
            var smiteSpell = Player.Instance.Spellbook.GetSpell(Spells.Smite);
            if (!smite || smiteSpell == null || !smiteSpell.IsReady)
            {
                return;
            }

            var obj =
                EntityManager.MinionsAndMonsters.GetJungleMonsters(Player.Instance.Position, 760)
                    .FirstOrDefault(mob => CanSmiteMob(mob.Name));
            if (obj == null)
            {
                return;
            }

            if (obj.IsValidTarget(760) &&
                obj.Health < Player.Instance.GetSummonerSpellDamage(obj, DamageLibrary.SummonerSpells.Smite))
            {
                Player.Instance.Spellbook.CastSpell(Spells.Smite, obj);
            }
        }

        private static bool CanSmiteMob(string name)
        {
            var baron = Menu.ClearMenu["Smite.Baron"].Cast<CheckBox>().CurrentValue;
            if (baron && name.StartsWith("SRU_Baron"))
            {
                return true;
            }

            var dragon = Menu.ClearMenu["Smite.Dragon"].Cast<CheckBox>().CurrentValue;
            if (dragon && name.StartsWith("SRU_Dragon"))
            {
                return true;
            }

            if (name.Contains("Mini"))
            {
                return false;
            }

            var red = Menu.ClearMenu["Smite.Red"].Cast<CheckBox>().CurrentValue;
            if (red && name.StartsWith("SRU_Red"))
            {
                return true;
            }

            var blue = Menu.ClearMenu["Smite.Blue"].Cast<CheckBox>().CurrentValue;
            if (blue && name.StartsWith("SRU_Blue"))
            {
                return true;
            }

            var krug = Menu.ClearMenu["Smite.Krug"].Cast<CheckBox>().CurrentValue;
            if (krug && name.StartsWith("SRU_Krug"))
            {
                return true;
            }

            var gromp = Menu.ClearMenu["Smite.Gromp"].Cast<CheckBox>().CurrentValue;
            if (gromp && name.StartsWith("SRU_Gromp"))
            {
                return true;
            }

            var raptor = Menu.ClearMenu["Smite.Raptor"].Cast<CheckBox>().CurrentValue;
            if (raptor && name.StartsWith("SRU_Razorbeak"))
            {
                return true;
            }

            var wolf = Menu.ClearMenu["Smite.Wolf"].Cast<CheckBox>().CurrentValue;
            if (wolf && name.StartsWith("SRU_Murkwolf"))
            {
                return true;
            }

            return false;
        }

        public static void SetSummonerSlots()
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
            else if (
                SmiteGrey.Any(
                    x => Player.Instance.InventoryItems.FirstOrDefault(a => a.Id == (ItemId) x) != null))
            {
                Spells.Smite = Player.Instance.GetSpellSlotFromName("s5_summonersmitequick");
            }
            else if (
                SmitePurple.Any(
                    x => Player.Instance.InventoryItems.FirstOrDefault(a => a.Id == (ItemId) x) != null))
            {
                Spells.Smite = Player.Instance.GetSpellSlotFromName("itemsmiteaoe");
            }
            else
            {
                Spells.Smite = Player.Instance.GetSpellSlotFromName("summonersmite");
            }

            Spells.Ignite = Player.Instance.GetSpellSlotFromName("summonerdot");
        }

        public static T MinOrDefault<T, TR>(this IEnumerable<T> container, Func<T, TR> valuingFoo)
            where TR : IComparable
        {
            var enumerator = container.GetEnumerator();
            if (!enumerator.MoveNext())
            {
                return default(T);
            }

            var minElem = enumerator.Current;
            var minVal = valuingFoo(minElem);
            while (enumerator.MoveNext())
            {
                var currVal = valuingFoo(enumerator.Current);
                if (currVal.CompareTo(minVal) >= 0)
                {
                    continue;
                }

                minVal = currVal;
                minElem = enumerator.Current;
            }

            return minElem;
        }
    }
}