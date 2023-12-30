using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuickMemo.Lib.Functions
{
    internal class SpecialKeyDown
    {
        /// <summary>
        /// Ctrlチェック
        /// </summary>
        /// <returns></returns>
        public static bool IsCtrlPressed()
        {
            return
                (Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down ||
                (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down;
        }

        /// <summary>
        /// Shiftチェック
        /// </summary>
        /// <returns></returns>
        public static bool IsShiftPressed()
        {
            return
                (Keyboard.GetKeyStates(Key.LeftShift) & KeyStates.Down) == KeyStates.Down ||
                (Keyboard.GetKeyStates(Key.RightShift) & KeyStates.Down) == KeyStates.Down;
        }

        /// <summary>
        /// Altチェック
        /// </summary>
        /// <returns></returns>
        public static bool IsAltPressed()
        {
            return
                (Keyboard.GetKeyStates(Key.LeftAlt) & KeyStates.Down) == KeyStates.Down ||
                (Keyboard.GetKeyStates(Key.RightAlt) & KeyStates.Down) == KeyStates.Down;
        }
    }
}
