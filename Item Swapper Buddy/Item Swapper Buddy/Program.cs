using System;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Utils;

namespace Item_Swapper_Buddy
{
    internal class Program
    {
        private static int _firstKey = 0x60;
        private static readonly int[] Keys = {0x64, 0x65, 0x66, 0x61, 0x62, 0x63};
        // ReSharper disable once UnusedParameter.Local
        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
            AppDomain.CurrentDomain.UnhandledException +=
                delegate(object sender, UnhandledExceptionEventArgs args1)
                {
                    Logger.Log(LogLevel.Error, args1.ExceptionObject.ToString());
                };
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
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

            var inventorySlot = new[]
            {
                new InventorySlot((uint) Player.Instance.NetworkId, Array.IndexOf(Keys, _firstKey)),
                new InventorySlot((uint) Player.Instance.NetworkId, Array.IndexOf(Keys, key))
            };
            if (inventorySlot[0].DisplayName.Equals("Unknown") && inventorySlot[1].DisplayName.Equals("Unknown"))
            {
                _firstKey = 0x60;
                return;
            }

            if (inventorySlot[0].DisplayName.Equals("Unknown"))
            {
                Player.SwapItem(Array.IndexOf(Keys, key), Array.IndexOf(Keys, _firstKey));
                _firstKey = 0x60;
                return;
            }

            Player.SwapItem(Array.IndexOf(Keys, _firstKey), Array.IndexOf(Keys, key));
            _firstKey = 0x60;
        }
    }
}