using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace Item_Swapper_Buddy
{
    internal class Program
    {
        private static int _firstKey = 0x60;
        private static readonly int[] Keys = {0x64, 0x65, 0x66, 0x61, 0x62, 0x63};

        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += delegate
            {
                var onLoadingComplete = new Thread(Loading_OnLoadingComplete);
                onLoadingComplete.Start();
            };
        }

        private static void Loading_OnLoadingComplete()
        {
            Chat.Print("Item Swapper Buddy - <font color=\"#FFFFFF\">Loaded</font>", Color.FromArgb(255, 210, 68, 74));

            Game.OnWndProc += Game_OnWndProc;
        }

        private static void Game_OnWndProc(WndEventArgs args)
        {
            if (args.Msg == 0x0100 && args.WParam == 0x60)
            {
                _firstKey = 0x60;
            }

            if (args.Msg != 0x0100 || !Keys.ToList().Contains((byte) args.WParam))
            {
                return;
            }

            var key = (int) args.WParam;
            if (_firstKey == 0x60)
            {
                _firstKey = key;
            }

            if (_firstKey == key)
            {
                return;
            }
			
            Player.SwapItem(Array.IndexOf(Keys, _firstKey), Array.IndexOf(Keys, key));
            _firstKey = 0x60;
        }
    }
}