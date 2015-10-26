using System;
using System.Collections.Generic;

namespace XinZhao_Buddy.Internal
{
    internal static class Extensions
    {
        public static bool CanSmiteMob(string name)
        {
            var baron = Menu.Smite.Baron != null && (bool) Menu.Smite.Baron;
            if (baron && name.StartsWith("SRU_Baron"))
            {
                return true;
            }

            var dragon = Menu.Smite.Dragon != null && (bool) Menu.Smite.Dragon;
            if (dragon && name.StartsWith("SRU_Dragon"))
            {
                return true;
            }

            if (name.Contains("Mini"))
            {
                return false;
            }

            var red = Menu.Smite.Red != null && (bool) Menu.Smite.Red;
            if (red && name.StartsWith("SRU_Red"))
            {
                return true;
            }

            var blue = Menu.Smite.Blue != null && (bool) Menu.Smite.Blue;
            if (blue && name.StartsWith("SRU_Blue"))
            {
                return true;
            }

            var krug = Menu.Smite.Krug != null && (bool) Menu.Smite.Krug;
            if (krug && name.StartsWith("SRU_Krug"))
            {
                return true;
            }

            var gromp = Menu.Smite.Gromp != null && (bool) Menu.Smite.Gromp;
            if (gromp && name.StartsWith("SRU_Gromp"))
            {
                return true;
            }

            var raptor = Menu.Smite.Raptor != null && (bool) Menu.Smite.Raptor;
            if (raptor && name.StartsWith("SRU_Razorbeak"))
            {
                return true;
            }

            var wolf = Menu.Smite.Wolf != null && (bool) Menu.Smite.Wolf;
            if (wolf && name.StartsWith("SRU_Murkwolf"))
            {
                return true;
            }

            return false;
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