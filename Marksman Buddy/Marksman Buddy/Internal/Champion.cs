using System;
using System.Collections.Generic;
using System.Linq;
using EloBuddy;
using System.Text;

namespace Marksman_Buddy.Internal
{
    class Champion: PluginBase
    {
		public Champion()
		{
			Chat.Print("{0} is not supported", ObjectManager.Player.ChampionName);
		}
    }
}
