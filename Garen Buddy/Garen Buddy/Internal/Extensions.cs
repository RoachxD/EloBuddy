using System;
using EloBuddy;
using EloBuddy.SDK;

namespace Garen_Buddy.Internal
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

        public static bool PossibleToReachQ(this AIHeroClient target)
        {
            var distance = Player.Instance.Distance(target);
            var diff = Math.Abs(Player.Instance.MoveSpeed*1.30 - target.MoveSpeed);
            var duration = new[] {1.5f, 2f, 2.5f, 3f, 3.5f}[Spells.Q.Level - 1];
            return diff*duration > distance;
        }
    }
}