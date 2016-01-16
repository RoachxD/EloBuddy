using System;
using EloBuddy;

namespace In_Game_Settings_Buddy
{
    internal class MovementHackHotfix
    {
        private readonly Random _generator = new Random();
        private float _lastTick;
        private bool _rButtonDown;
        public bool Enabled;

        public MovementHackHotfix()
        {
            Enabled = false;
            _rButtonDown = false;
            Game.OnWndProc += Game_OnWndProc;
            Game.OnTick += Game_OnTick;
            Player.OnIssueOrder += Player_OnIssueOrder;
            _lastTick = Environment.TickCount;
        }

        private void Player_OnIssueOrder(Obj_AI_Base sender, PlayerIssueOrderEventArgs args)
        {
            _lastTick = Environment.TickCount;
        }

        private void Game_OnTick(EventArgs args)
        {
            if (!_rButtonDown)
            {
                return;
            }

            if ((Environment.TickCount - _lastTick) > _generator.Next(43, 145))
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos, false);
            }
        }

        private void Game_OnWndProc(WndEventArgs args)
        {
            switch (args.Msg)
            {
                case 0x0204:
                    //RButtonDown
                    _rButtonDown = true;
                    break;
                case 0x0205:
                    //RButtonUp
                    _rButtonDown = false;
                    break;
            }
        }
    }
}