using System;
using EloBuddy;
using EloBuddy.SDK;

namespace Garen_Buddy.Internal
{
    internal static class Extensions
    {
        public static bool CanSmiteMob(this string name)
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

        public static bool PossibleToReachQ(this AIHeroClient target)
        {
            var distance = Player.Instance.Distance(target);
            var diff = Math.Abs(Player.Instance.MoveSpeed*1.30 - target.MoveSpeed);
            var duration = new[] {1.5f, 2f, 2.5f, 3f, 3.5f}[Spells.Q.Level - 1];
            return diff*duration > distance;
        }
    }
}