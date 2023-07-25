using System;
using System.Collections.Generic;
using System.Linq;
using Utils.Extentions;

namespace Utils.MessageHooking.Wpf
{
    public sealed class MessageHooker
    {
        public class KeyPair
        {
            public WindowMessage WindowMessage { get; set; } = WindowMessage.None;
            public WindowMessage VirtualKey { get; set; } = WindowMessage.None;
            public bool IsHandled { get; set; } = true;
        }

        private readonly Dictionary<KeyPair, Action> _dictionary;

        public MessageHooker(Dictionary<KeyPair, Action> keyValuePairs)
        {
            if (keyValuePairs == null) throw new ArgumentNullException();
            if (keyValuePairs.Count == 0) throw new InvalidOperationException();

            _dictionary = keyValuePairs;
        }

        public IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var message = _dictionary.FirstOrDefault(x => (int)x.Key.WindowMessage == msg);

            if (message.IsDefault() == false)
            {
                // -1이면 미사용을 의미
                if ((message.Key.VirtualKey == WindowMessage.None) || (message.Key.VirtualKey == (WindowMessage)wParam.ToInt32()))
                {
                    message.Value.Invoke();
                    handled = message.Key.IsHandled;
                }
            }

            return IntPtr.Zero;
        }
    }
}
