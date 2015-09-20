using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Menu.Values;

namespace Marksman_Buddy.Internal
{
	abstract class PluginBase
	{
        public virtual void _SetupSpells()
        {
        }

        public virtual void _SetupMenu()
        {
        }
        public virtual void Game_OnTick(EventArgs args)
	    {
	    }

	    public virtual void _Combo()
	    {
	        
	    }

	    public virtual void _Harass()
	    {
	        
	    }
    }
}
