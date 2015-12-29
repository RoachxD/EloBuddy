using System.Collections.Generic;
using Marksman_Buddy.Champions;

namespace Marksman_Buddy
{
    internal class ChampionLogic
    {
        private static List<ChampionBase> AvailableChampions { get; set; }

        public static void Initialize()
        {
            AvailableChampions = new List<ChampionBase>()
            {
                new Ashe()
            };

            AvailableChampions.ForEach(mode =>
            {
                if (mode.ShouldBeExecuted())
                {
                    mode.Execute();
                }
            });
        }
    }
}