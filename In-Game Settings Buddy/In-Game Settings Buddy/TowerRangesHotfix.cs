using System;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Rendering;
using System.Linq;

namespace In_Game_Settings_Buddy
{
	class TowerRangesHotfix
	{
		public bool Enabled;
		public TowerRangesHotfix()
		{
			Drawing.OnDraw += Drawing_OnDraw;
			Enabled = false;
		}

		private void Drawing_OnDraw(EventArgs args)
		{
			int counter = 0;
			if (!Enabled)
				return;
			foreach(var tower in ObjectManager.Get<Obj_AI_Turret>().Where(tower=>tower.Team != Player.Instance.Team))
			{
				Console.WriteLine(tower.Name + "->" +  tower.Position.Distance(Player.Instance).ToString());
				if (!(tower.Position.Distance(Player.Instance) > 2000.0f))
				{
					counter++;
					if (tower.Name.ToLower().Contains("chaos"))
						Circle.Draw(new SharpDX.ColorBGRA(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B, System.Drawing.Color.Red.A), 1475.0f, new[] { new SharpDX.Vector3(tower.Position.X, tower.Position.Y, tower.Position.Z - 20.0f) });
					else
						Circle.Draw(new SharpDX.ColorBGRA(System.Drawing.Color.Red.R, System.Drawing.Color.Red.G, System.Drawing.Color.Red.B, System.Drawing.Color.Red.A), 855.0f, new[] { new SharpDX.Vector3(tower.Position.X, tower.Position.Y, tower.Position.Z - 20.0f) });

				}
			}
		}
	}
}
