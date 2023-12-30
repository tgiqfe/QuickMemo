
namespace QuickMemo.Lib
{
    /// <summary>
    /// グローバルホットキー登録用クラス
    /// </summary>
    internal class GlobalHotKey : IDisposable
    {
        private class HotKeyItem
        {
            public System.Windows.Input.ModifierKeys ModifierKeys { get; set; }
            public System.Windows.Input.Key Key { get; set; }
            public EventHandler Handler { get; set; }
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int RegisterHotKey(IntPtr hWnd, int id, int modKey, int vKey);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int UnregisterHotKey(IntPtr hWnd, int id);

        private IntPtr _windowHandle;
        private Dictionary<int, HotKeyItem> _hotkeyItems = new Dictionary<int, HotKeyItem>();

        private int _hotkeyID = 0x0000;
        private const int MAX_HOTKEY_ID = 0xC000;
        private const int WM_HOTKEY = 0x0312;

        public GlobalHotKey(System.Windows.Window window)
        {
            var host = new System.Windows.Interop.WindowInteropHelper(window);
            _windowHandle = host.Handle;
            System.Windows.Interop.ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;
        }

        private void ComponentDispatcher_ThreadPreprocessMessage(ref System.Windows.Interop.MSG msg, ref bool handled)
        {
            if (msg.message != WM_HOTKEY)
            {
                return;
            }
            var id = msg.wParam.ToInt32();
            var hotkey = this._hotkeyItems[id];
            hotkey?.Handler?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// ホットキーを登録
        /// </summary>
        /// <param name="modKey"></param>
        /// <param name="key"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        public bool Register(System.Windows.Input.ModifierKeys modKey, System.Windows.Input.Key key, EventHandler handler)
        {
            var modKeyNum = (int)modKey;
            var vKey = System.Windows.Input.KeyInterop.VirtualKeyFromKey(key);

            while (_hotkeyID < MAX_HOTKEY_ID)
            {
                var ret = RegisterHotKey(this._windowHandle, _hotkeyID, modKeyNum, vKey);
                if (ret != 0)
                {
                    var hotKey = new HotKeyItem() { ModifierKeys = modKey, Key = key, Handler = handler };
                    this._hotkeyItems[_hotkeyID] = hotKey;
                    this._hotkeyID++;
                    return true;
                }
                _hotkeyID++;
            }
            return false;
        }

        /// <summary>
        /// ホットキー登録を解除。HotkeyIDを指定
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Unregister(int id)
        {
            var ret = UnregisterHotKey(_windowHandle, id);
            return ret == 0;
        }

        /// <summary>
        /// ホットキー登録を解除。ModifierKeysとKeyを指定
        /// </summary>
        /// <param name="modifierKeys"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Unregister(System.Windows.Input.ModifierKeys modifierKeys, System.Windows.Input.Key key)
        {
            var ret = false;
            var item = _hotkeyItems.
                FirstOrDefault(x => x.Value.ModifierKeys == modifierKeys && x.Value.Key == key);
            var isFound = !item.Equals(default(KeyValuePair<int, HotKeyItem>));

            if (isFound)
            {
                ret = Unregister(item.Key);
                if (ret)
                {
                    _hotkeyItems.Remove(item.Key);
                }
            }
            return ret;
        }

        /// <summary>
        /// ホットキー登録を解除。全て
        /// </summary>
        /// <returns></returns>
        public bool Unregister()
        {
            return _hotkeyItems.Keys.Select(x => Unregister(x)).All(x => x);
        }

        /// <summary>
        /// 終了
        /// </summary>
        public void Close()
        {
            Unregister();
        }

        #region Dispose

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.Close();
                }
                disposedValue = true;
            }
        }

        ~GlobalHotKey()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
