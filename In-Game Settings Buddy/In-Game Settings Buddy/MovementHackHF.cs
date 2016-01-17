using System;
using EloBuddy;

namespace In_Game_Settings_Buddy
{
    internal class MovementHackHF
    {
        private readonly Random _generator = new Random();
        private float _lastTick;
        private bool _rButtonDown;
        private bool _shouldMove;
        public bool Enabled;

        public MovementHackHF()
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
            if (args.Target != null)
            {
                _shouldMove = false;
                return;
            }

            _lastTick = Environment.TickCount;
            _shouldMove = true;
        }

        private void Game_OnTick(EventArgs args)
        {
            if (!_rButtonDown || !Enabled)
            {
                return;
            }

            if ((Environment.TickCount - _lastTick) > _generator.Next(43, 145) && _shouldMove)
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