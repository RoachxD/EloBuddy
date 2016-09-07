using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Utils;
using Item_Swapper_Buddy.Internal;

namespace Item_Swapper_Buddy
{
    internal class Program
    {
        private static int _firstKey = 0x60;
        private static readonly int[] Keys = {0x64, 0x65, 0x66, 0x61, 0x62, 0x63};
        private static List<Tuple<int, int, float>> _queueList;
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

            _queueList = new List<Tuple<int, int, float>>();
            MenuManager.Initialize();

            Game.OnTick += Game_OnTick;
            Game.OnWndProc += Game_OnWndProc;
        }

        private static void Game_OnTick(EventArgs args)
        {
            if (!MenuManager.Humanizer.Enable)
            {
                return;
            }

            foreach (var item in _queueList.Where(item => item.Item3 <= Game.Time))
            {
                Player.SwapItem(item.Item1, item.Item2);
                _queueList.RemoveAt(_queueList.FindIndex(i => item.Equals(i)));
            }
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
                if (MenuManager.Humanizer.Enable)
                {
                    var delay = MenuManager.Humanizer.Type == 0
                        ? MenuManager.Humanizer.Value
                        : new Random().Next(MenuManager.Humanizer.MinValue, MenuManager.Humanizer.MaxValue);
                    _queueList.Add(new Tuple<int, int, float>(Array.IndexOf(Keys, key), Array.IndexOf(Keys, _firstKey),
                        !_queueList.Any()
                            ? Game.Time + delay
                            : _queueList.Last().Item3 + delay));
                    _firstKey = 0x60;
                }
                else
                {
                    Player.SwapItem(Array.IndexOf(Keys, key), Array.IndexOf(Keys, _firstKey));
                    _firstKey = 0x60;
                }
                return;
            }

            if (MenuManager.Humanizer.Enable)
            {
                var delay = MenuManager.Humanizer.Type == 0
                    ? MenuManager.Humanizer.Value
                    : new Random().Next(MenuManager.Humanizer.MinValue, MenuManager.Humanizer.MaxValue);
                _queueList.Add(new Tuple<int, int, float>(Array.IndexOf(Keys, _firstKey), Array.IndexOf(Keys, key),
                    !_queueList.Any()
                        ? Game.Time + delay
                        : _queueList.Last().Item3 + delay));
                _firstKey = 0x60;
            }
            else
            {
                Player.SwapItem(Array.IndexOf(Keys, _firstKey), Array.IndexOf(Keys, key));
                _firstKey = 0x60;
            }
        }
    }
}