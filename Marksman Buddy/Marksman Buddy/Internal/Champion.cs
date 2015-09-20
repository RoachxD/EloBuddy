using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EloBuddy;
using System.Text;

namespace Marksman_Buddy.Internal
{
    class Champion : PluginBase
    {
		public Champion()
		{
			Chat.Print("Marksman Buddy - <font color=\"#FFFFFF\">{0} is not supported</font>", Color.FromArgb(255, 210, 68, 74), ObjectManager.Player.ChampionName);
		}

        public override void _SetupMenu()
        {
            Variables.Config.AddGroupLabel("Champion not Supported");
            Variables.Config.AddLabel("The champion is probably not a Marksman or not yet implemented.");
        }
    }
}
