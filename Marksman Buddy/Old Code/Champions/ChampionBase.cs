using EloBuddy;

namespace Marksman_Buddy.Champions
{
    public abstract class ChampionBase
    {
        protected static readonly AIHeroClient Player = EloBuddy.Player.Instance;
        public abstract bool ShouldBeExecuted();
        public abstract void Execute();
    }
}