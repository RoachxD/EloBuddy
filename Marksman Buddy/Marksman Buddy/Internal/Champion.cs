using System;

namespace Marksman_Buddy.Internal
{
    internal class Champion : PluginBase
    {
        public Champion()
        {
            _SetupMenu();
        }

        protected override void _SetupSpells()
        {
        }

        protected override void Game_OnTick(EventArgs args)
        {
        }

        protected override void _Combo()
        {
        }

        protected override void _Harass()
        {
        }

        protected override sealed void _SetupMenu()
        {
            Variables.Config.AddGroupLabel("Champion not Supported");
            Variables.Config.AddLabel("The champion is probably not a Marksman or not yet implemented.");
        }
    }
}