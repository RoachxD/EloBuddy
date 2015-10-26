using System;
using System.Collections.Generic;

namespace Warwick_Buddy.Internal
{
    internal static class Extensions
    {
        public static bool CanSmiteMob(this string name)
        {
            if (Menu.Smite.Baron && name.StartsWith("SRU_Baron"))
            {
                return true;
            }

            if (Menu.Smite.Dragon && name.StartsWith("SRU_Dragon"))
            {
                return true;
            }

            if (name.Contains("Mini"))
            {
                return false;
            }

            if (Menu.Smite.Red && name.StartsWith("SRU_Red"))
            {
                return true;
            }

            if (Menu.Smite.Blue && name.StartsWith("SRU_Blue"))
            {
                return true;
            }

            if (Menu.Smite.Krug && name.StartsWith("SRU_Krug"))
            {
                return true;
            }

            if (Menu.Smite.Gromp && name.StartsWith("SRU_Gromp"))
            {
                return true;
            }

            if (Menu.Smite.Raptor && name.StartsWith("SRU_Razorbeak"))
            {
                return true;
            }

            if (Menu.Smite.Wolf && name.StartsWith("SRU_Murkwolf"))
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