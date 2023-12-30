using QuickMemo.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickMemo
{
    public class BindingParam
    {
        public Setting Setting { get; set; }

        public Content Content { get; set; }


        public BindingParam()
        {
            this.Setting = Setting.Load();
            this.Content = Content.Load();
        }


    }
}
