using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QuickMemo.Config
{
    public class Setting
    {
        #region Binding parameter

        public double Width { get; set; }
        public double Height { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        #endregion




        #region Save,Load

        private static string _settingFilePath = null;

        public static Setting Load()
        {
            _settingFilePath ??= Path.Combine(
                Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                "Setting.json");

            try
            {
                using (var sr = new StreamReader(_settingFilePath, Encoding.UTF8))
                {
                    var setting = JsonSerializer.Deserialize<Setting>(sr.ReadToEnd());
                    return setting;
                }
            }
            catch
            {
                var setting = new Setting();
                setting.Init();
                return setting;
            }
        }

        public void Save()
        {
            _settingFilePath ??= Path.Combine(
                Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                "Setting.json");

            using (var sw = new StreamWriter(_settingFilePath, false, Encoding.UTF8))
            {
                var json = JsonSerializer.Serialize(this, new JsonSerializerOptions()
                {
                    WriteIndented = true,
                });
                sw.Write(json);
            }
        }

        public void Init()
        {
            this.Width = 550;
            this.Height = 800;
            this.X = 50;
            this.Y = 50;
        }

        #endregion
    }
}
