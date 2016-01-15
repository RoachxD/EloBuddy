using System;
using EloBuddy;
using EloBuddy.SDK;

namespace In_Game_Settings_Buddy
{
	class MovementHackHotfix
	{
		Random generator = new Random();
		float lastTick = 0.0f;
		public bool Enabled;
		private bool RButtonDown;
		public MovementHackHotfix()
		{
			Enabled = false;
			RButtonDown = false;
			Game.OnWndProc += Game_OnWndProc;
			Game.OnTick += Game_OnTick;
			Player.OnIssueOrder += Player_OnIssueOrder;
			lastTick = Environment.TickCount;
		}

		private void Player_OnIssueOrder(Obj_AI_Base sender, PlayerIssueOrderEventArgs args)
		{
			lastTick = Environment.TickCount;
		}

		private void Game_OnTick(EventArgs args)
		{
			if (!RButtonDown)
				return;
			if ((Environment.TickCount - lastTick) > (float)generator.Next(43, 145))
			{
				Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos, false);
			}
				
		}

		private void Game_OnWndProc(EloBuddy.WndEventArgs args)
		{
			switch (args.Msg)
			{
				case 0x0204:
					//RButtonDown
					RButtonDown = true;
					break;
				case 0x0205:
					//RButtonUp
					RButtonDown = false;
					break;
				default:
					break;
			}
		}
	}
}
