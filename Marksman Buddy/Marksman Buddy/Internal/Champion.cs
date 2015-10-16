using System.Drawing;
using EloBuddy;

namespace Marksman_Buddy.Internal
{
    internal class Champion : PluginBase
    {
		protected override void _SetupSpells()
		{

		}

		protected override void Game_OnTick(System.EventArgs args)
		{

		}

		protected override void _Combo()
		{

		}

		protected override void _Harass()
		{

		}

        public Champion()
        {
            _SetupMenu();

            Chat.Print("Marksman Buddy - <font color=\"#FFFFFF\">{0} is not supported</font>",
                Color.FromArgb(255, 210, 68, 74), ObjectManager.Player.ChampionName);
        }

        protected override sealed void _SetupMenu()
        {
            Variables.Config.AddGroupLabel("Champion not Supported");
            Variables.Config.AddLabel("The champion is probably not a Marksman or not yet implemented.");
        }
    }
}