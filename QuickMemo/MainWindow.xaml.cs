using ICSharpCode.AvalonEdit;
using QuickMemo.Lib;
using QuickMemo.Lib.Functions;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuickMemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GlobalHotKey globalhotKey = null;

        public MainWindow()
        {
            InitializeComponent();
            Uri iconUri = new Uri("pack://application:,,,/Resources/Transparent.ico", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);

            this.DataContext = Item.BindingParam;
            this.textEditor.Text = Item.BindingParam.Content.Text;

            //  グローバルホットキーを登録
            globalhotKey = new(this);
            globalhotKey.Register(ModifierKeys.Control | ModifierKeys.Alt,
                Key.PageUp,
                (_, __) => { this.Show(); });

            textEditor.Focus();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            globalhotKey.Dispose();

            KeyEvent_S();
            Item.BindingParam.Setting.Save();
            Item.BindingParam.Content.Save();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left: KeyEvent_Left(e); break;
                case Key.Right: KeyEvent_Right(e); break;
                case Key.Up: KeyEvent_Up(e); break;
                case Key.Down: KeyEvent_Down(e); break;
                case Key.Home: KeyEvent_Home(e); break;
                case Key.End: KeyEvent_End(e); break;
                case Key.PageDown: KeyEvent_PageDown(e); break;
                case Key.S: KeyEvent_S(); break;
            }
        }


        /// <summary>
        /// キー押下時イベント: Left
        /// </summary>
        private void KeyEvent_Left(KeyEventArgs e)
        {
            if (SpecialKeyDown.IsCtrlPressed() &&
                SpecialKeyDown.IsAltPressed())
            {
                double step = Item.SizeChangeStep;
                e.Handled = true;
                if (SpecialKeyDown.IsShiftPressed())
                {
                    //  ウィンドウ右側を小さくする
                    if (this.Width >= step * 2)
                    {
                        this.Width -= step;
                    }
                }
                else
                {
                    //  左に座標を移動させながら、ウィンドウ左側を広げる
                    if (this.Left >= step)
                    {
                        this.Left -= step;
                        this.Width += step;
                    }
                    else
                    {
                        step = this.Left;
                        this.Left = 0;
                        this.Width += step;
                    }
                }
            }
        }

        /// <summary>
        /// キー押下時イベント: Right
        /// </summary>
        private void KeyEvent_Right(KeyEventArgs e)
        {
            if (SpecialKeyDown.IsCtrlPressed() &&
                SpecialKeyDown.IsAltPressed())
            {
                double step = Item.SizeChangeStep;
                e.Handled = true;
                if (SpecialKeyDown.IsShiftPressed())
                {
                    //  座標はそのままに、ウィンドウ右側を広げる
                    if (this.Width + this.Left < (SystemParameters.WorkArea.Width - step))
                    {
                        this.Width += step;
                    }
                    else
                    {
                        this.Width = SystemParameters.WorkArea.Width - this.Left;
                    }
                }
                else
                {
                    //  右に座標を移動させながら、ウィンドウ左側を小さくする
                    if (this.Width >= step * 2)
                    {
                        this.Left += step;
                        this.Width -= step;
                    }
                }
            }
        }

        /// <summary>
        /// キー押下時イベント: Up
        /// </summary>
        /// <param name="e"></param>
        private void KeyEvent_Up(KeyEventArgs e)
        {
            if (SpecialKeyDown.IsCtrlPressed() &&
                SpecialKeyDown.IsAltPressed())
            {
                double step = Item.SizeChangeStep;
                e.Handled = true;
                if (SpecialKeyDown.IsShiftPressed())
                {
                    //  ウィンドウ下側側を小さくする
                    if (this.Height >= step * 2)
                    {
                        this.Height -= step;
                    }
                }
                else
                {
                    //  上に座標を移動させながら、ウィンドウ上側を広げる
                    if (this.Top >= step)
                    {
                        this.Top -= step;
                        this.Height += step;
                    }
                    else
                    {
                        step = this.Top;
                        this.Top = 0;
                        this.Height += step;
                    }
                }
            }
        }

        /// <summary>
        /// キー押下時イベント: Down
        /// </summary>
        /// <param name="e"></param>
        private void KeyEvent_Down(KeyEventArgs e)
        {
            if (SpecialKeyDown.IsCtrlPressed() &&
                SpecialKeyDown.IsAltPressed())
            {
                double step = Item.SizeChangeStep;
                e.Handled = true;
                if (SpecialKeyDown.IsShiftPressed())
                {
                    //  座標はそのままに、ウィンドウ下側を広げる
                    if (this.Height + this.Top < (SystemParameters.WorkArea.Height - step))
                    {
                        this.Height += step;
                    }
                    else
                    {
                        this.Height = SystemParameters.WorkArea.Height - this.Top;
                    }
                }
                else
                {
                    //  下に座標を移動させながら、ウィンドウ上側を小さくする
                    if (this.Height >= step * 2)
                    {
                        this.Height -= step;
                        this.Top += step;
                    }
                }
            }
        }

        /// <summary>
        /// キー押下時イベント: Home
        /// </summary>
        /// <param name="e"></param>
        private void KeyEvent_Home(KeyEventArgs e)
        {
            if (SpecialKeyDown.IsCtrlPressed() &&
                SpecialKeyDown.IsAltPressed())
            {
                e.Handled = true;

                var screenCenter_left = SystemParameters.WorkArea.Width / 2;
                var screenCenter_top = SystemParameters.WorkArea.Height / 2;

                var windowCenter_left = this.Left + this.Width / 2;
                var windowCenter_top = this.Top + this.Height / 2;

                if (windowCenter_left <= screenCenter_left && windowCenter_top <= screenCenter_top)
                {
                    //  左上エリア ⇒ 右上エリア
                    this.Left = SystemParameters.WorkArea.Width - this.Width;
                    this.Top = 0;

                }
                else if (windowCenter_left <= screenCenter_left && windowCenter_top > screenCenter_top)
                {
                    //  左下エリア ⇒ 左上エリア
                    this.Left = 0;
                    this.Top = 0;

                }
                else if (windowCenter_left > screenCenter_left && windowCenter_top <= screenCenter_top)
                {
                    //  右上エリア ⇒ 右下エリア
                    this.Left = SystemParameters.WorkArea.Width - this.Width;
                    this.Top = SystemParameters.WorkArea.Height - this.Height;
                }
                else if (windowCenter_left > screenCenter_left && windowCenter_top > screenCenter_top)
                {
                    //  右下エリア ⇒ 左下エリア
                    this.Left = 0;
                    this.Top = SystemParameters.WorkArea.Height - this.Height;
                }
            }
        }

        /// <summary>
        /// キー押下時イベント: End
        /// </summary>
        /// <param name="e"></param>
        private void KeyEvent_End(KeyEventArgs e)
        {
            if (SpecialKeyDown.IsCtrlPressed() &&
                SpecialKeyDown.IsAltPressed())
            {
                e.Handled = true;

                var screenCenter_left = SystemParameters.WorkArea.Width / 2;
                var screenCenter_top = SystemParameters.WorkArea.Height / 2;

                var windowCenter_left = this.Left + this.Width / 2;
                var windowCenter_top = this.Top + this.Height / 2;

                if (windowCenter_left <= screenCenter_left && windowCenter_top <= screenCenter_top)
                {
                    //  左上エリア ⇒ 左下エリア
                    this.Left = 0;
                    this.Top = SystemParameters.WorkArea.Height - this.Height;
                }
                else if (windowCenter_left <= screenCenter_left && windowCenter_top > screenCenter_top)
                {
                    //  左下エリア ⇒ 右下エリア
                    this.Left = SystemParameters.WorkArea.Width - this.Width;
                    this.Top = SystemParameters.WorkArea.Height - this.Height;
                }
                else if (windowCenter_left > screenCenter_left && windowCenter_top <= screenCenter_top)
                {
                    //  右上エリア ⇒ 左上エリア
                    this.Left = 0;
                    this.Top = 0;
                }
                else if (windowCenter_left > screenCenter_left && windowCenter_top > screenCenter_top)
                {
                    //  右下エリア ⇒ 右上エリア
                    this.Left = SystemParameters.WorkArea.Width - this.Width;
                    this.Top = 0;
                }
            }
        }

        private void KeyEvent_PageDown(KeyEventArgs e)
        {
            if (SpecialKeyDown.IsCtrlPressed() &&
                SpecialKeyDown.IsAltPressed())
            {
                e.Handled = true;
                this.Hide();
            }
        }
        /// <summary>
        /// キー押下時イベント: S
        /// 上書き保存
        /// </summary>
        private void KeyEvent_S()
        {
            Item.BindingParam.Content.Text = this.textEditor.Text;
        }
    }
}