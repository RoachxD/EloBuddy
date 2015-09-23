using System;

namespace Marksman_Buddy.Internal
{
    internal abstract class PluginBase
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