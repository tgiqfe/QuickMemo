using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Collections.ObjectModel;
using ICSharpCode.AvalonEdit.Document;
using System.Windows;

namespace QuickMemo.Config
{
    public class Content
    {
        #region Binding parameter

        public string Text { get; set; }

        #endregion



        #region Save,Load

        private static string _contentFilePath = null;

        public static Content Load()
        {
            _contentFilePath ??= Path.Combine(
                Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                "Content.json");

            try
            {
                using (var sr = new StreamReader(_contentFilePath, Encoding.UTF8))
                {
                    var content = JsonSerializer.Deserialize<Content>(sr.ReadToEnd());
                    return content;
                }
            }
            catch
            {
                var content = new Content();
                content.Init();
                return content;
            }
        }

        public void Save()
        {
            _contentFilePath ??= Path.Combine(
                Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName),
                "Content.json");

            using (var sw = new StreamWriter(_contentFilePath, false, Encoding.UTF8))
            {
                var json = JsonSerializer.Serialize(this, new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                });
                sw.Write(json);
            }
        }

        public void Init()
        {
        }

        #endregion
    }
}
