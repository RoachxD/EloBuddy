using System;
using System.Collections.Generic;
using System.Linq;
using Marksman_Buddy.Internal;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu.Values;

namespace Marksman_Buddy.Activator
{
	class Core
	{

		private bool _UseHeal
		{
			get
			{
				Variables.Activator["MBActivator.UseHeal"].Cast<CheckBox>().CurrentValue;
			}
		}

		private int _UseHealPercent
		{
			get
			{
				Variables.Activator["MBActivator.UseHealPercent"].Cast<Slider>().CurrentValue;
			}
		}

		public Core()
		{
			Game.OnTick += _Game_OnTick;
		}

		private void _Game_OnTick(EventArgs args)
		{
			
		}
	}
}
