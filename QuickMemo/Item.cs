using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickMemo
{
    internal class Item
    {
        public static BindingParam BindingParam { get; set; }

        public static MainWindow MainWindow { get; set; }

        public static double SizeChangeStep = 80;
    }
}
