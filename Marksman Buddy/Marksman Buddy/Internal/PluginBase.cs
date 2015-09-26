using System;

namespace Marksman_Buddy.Internal
{
    internal abstract class PluginBase
    {
		protected abstract void _SetupSpells();

		protected abstract void _SetupMenu();
        

		protected abstract void Game_OnTick(EventArgs args);
        

		protected abstract void _Combo();
        

		protected abstract void _Harass();
    }
}