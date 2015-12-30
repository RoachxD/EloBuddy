namespace Marksman_Buddy.Champions
{
    public class Ashe : ChampionBase
    {
        public override bool ShouldBeExecuted()
        {
            return Player.ChampionName.Equals("Ashe");
        }

        public override void Execute()
        {
        }
    }
}