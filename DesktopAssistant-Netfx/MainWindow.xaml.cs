using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;
using Image = System.Windows.Controls.Image;

namespace DesktopAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] filenames;
        string[] icons;
        string[] commandargs;
        string[] names;
        private string ShowText;
        private string HideText;
        const string userRoot = "HKEY_CURRENT_USER";
        const string subkey = "\\Software\\DesktopAssistant";
        public MainWindow()
        {
            InitializeComponent();
            AppInit();
            InitLinks();
            InitIcons();
            InitStrings();
            Microsoft.Win32.SystemEvents.DisplaySettingsChanged += SystemEvents_DisplaySettingsChanged;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void AppInit()
        {
            double height = SystemParameters.PrimaryScreenHeight;
            this.Width = height * (0.445);
            this.Height = height * (0.445) / 1.2;
            this.Left = SystemParameters.WorkArea.Width*0.994 - this.Width;
            this.Top = SystemParameters.WorkArea.Height*0.993 - this.Height;
            this.line.Y1 = this.Height *0.122;
            this.line.Y2 = this.Height *0.122;
            this.TitleBox.MaxHeight = this.Height/18;
            this.TitleBox.Margin = new Thickness(this.Height/16, this.Height*3/80, 0,0);
            this.Column1.MaxHeight = this.Height / 20;
            this.Column1.Margin = new Thickness(this.Height *(0.08), this.Height *(0.09), 0, 0);
            this.PinnedItems.Margin = new Thickness(this.Height * (0.05), this.Height * (-0.003), 0, 0);
            this.TopPanel.Margin= new Thickness(0,this.Height * (0.025),  0, 0);
            this.svgbtn1.Height = this.Height / 15;
            this.svgbtn1.Width = this.Height /15;
            this.svgbtn1.Margin = new Thickness(0, 0, this.Height * (0.03), 0);
            this.svgbtn2.Height = this.Height / 15;
            this.svgbtn2.Width = this.Height / 15;
            this.svgbtn2.Margin = new Thickness(0, 0, this.Height * (0.03), 0);
            this.svgbtn3.Height = this.Height / 15;
            this.svgbtn3.Width = this.Height /15;
            this.svgbtn3.Margin = new Thickness(0, 0, this.Height * (0.03), 0);
            this.ChangeViewBtn.Height = this.Height / 12;
            this.ChangeViewBtn.Width = this.Height / 3;
            this.ChangeViewBtn.HorizontalOffset = this.Height * (-0.27);
            this.ChangeViewBtn.VerticalOffset = this.Height * (0.01);
            this.ButtonPopupBox.MaxHeight = this.Height / 24;

            InitObject(this.btn1, (Image)this.btn1.FindName("Image1"), (Viewbox)this.btn1.FindName("Text1"));
            InitObject(this.btn2, (Image)this.btn2.FindName("Image2"), (Viewbox)this.btn2.FindName("Text2"));
            InitObject(this.btn3, (Image)this.btn3.FindName("Image3"), (Viewbox)this.btn3.FindName("Text3"));
            InitObject(this.btn4, (Image)this.btn4.FindName("Image4"), (Viewbox)this.btn4.FindName("Text4"));
            InitObject(this.btn5, (Image)this.btn5.FindName("Image5"), (Viewbox)this.btn5.FindName("Text5"));

            this.Column2.MaxHeight = this.Height / 20;
            this.Column2.Margin = new Thickness(this.Height * (0.08), this.Height * (0.07), 0, 0);

            this.FrequentApps.Margin = new Thickness(this.Height * (0.05), this.Height * (-0.01), 0, 0);
            InitObject(this.btn2_1, (Image)this.btn2_1.FindName("Image2_1"), (Viewbox)this.btn2_1.FindName("Text2_1"));
            InitObject(this.btn2_2, (Image)this.btn2_2.FindName("Image2_2"), (Viewbox)this.btn2_2.FindName("Text2_2"));
            InitObject(this.btn2_3, (Image)this.btn2_3.FindName("Image2_3"), (Viewbox)this.btn2_3.FindName("Text2_3"));
            InitObject(this.btn2_4, (Image)this.btn2_4.FindName("Image2_4"), (Viewbox)this.btn2_4.FindName("Text2_4"));
            InitObject(this.btn2_5, (Image)this.btn2_5.FindName("Image2_5"), (Viewbox)this.btn2_5.FindName("Text2_5"));
        }
        private void InitObject(Button btn,Image img, Viewbox box)
        {
            btn.Width = this.Width * 0.185;
            btn.Height = this.Width * 0.22;
            btn.Margin = new Thickness(0, this.Height * (0.03), 0, 0);
            img.Width = btn.Width * 0.68;
            img.Height = btn.Height * 0.68;
            box.Margin = new Thickness(0, 0, 0, this.Height * (-0.03));
            box.MaxHeight = this.Height / 20;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            HideAltTab();
            SetBottom();
        }
        private void HideAltTab()
        {
            var windowInterop = new WindowInteropHelper(this);
            long exStyle = (long)GetWindowLong(windowInterop.Handle, (-20));
            exStyle |= 0x00000080L;
            SetWindowLong(windowInterop.Handle, (-20), (IntPtr)exStyle);
        }
        private void SetBottom()
        {
            const int GWL_STYLE = (-16);
            const UInt64 WS_CHILD = 0x40000000;
            IntPtr hWnd = new WindowInteropHelper(this).Handle;
            UInt64 iWindowStyle = (ulong)GetWindowLong(hWnd, GWL_STYLE);
            SetWindowLong(hWnd, GWL_STYLE, (iWindowStyle | WS_CHILD));
            hWnd = new WindowInteropHelper(this).Handle;
            SetWindowPos(hWnd, HWND_BOTTOM, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE  |SWP_NOACTIVATE);
        }
        #region Window styles
        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, UInt64 dwNewLong);
        [DllImport("user32.dll")]
        static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpWindowClass, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        public static IntPtr SearchDesktopHandle()
        {
            IntPtr hRoot = GetDesktopWindow();
            IntPtr hShellDll = IntPtr.Zero;
            IntPtr hDesktop = FindWindowEx(hRoot, IntPtr.Zero, "WorkerW", String.Empty);
            while (true)
            {
                hShellDll = FindWindowEx(hDesktop, IntPtr.Zero, "SHELLDLL_DefView", String.Empty);
                if (hShellDll != IntPtr.Zero)
                {
                    return hDesktop;
                }
                hDesktop = FindWindowEx(hRoot, hDesktop, "WorkerW", String.Empty);
            }
            return IntPtr.Zero;
        }

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        public const UInt32 SWP_NOSIZE = 0x0001;
        public const UInt32 SWP_NOMOVE = 0x0002;
        public const UInt32 SWP_NOACTIVATE = 0x0010;
        public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            int error = 0;
            IntPtr result = IntPtr.Zero;
            // Win32 SetWindowLong doesn't clear error on success
            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                // use SetWindowLong
                Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(tempResult);
            }
            else
            {
                // use SetWindowLongPtr
                result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

        private static int IntPtrToInt32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int dwErrorCode);
        #endregion
        #region Window Icons
        void InitIcons()
        {
            DoImageInitialize(btn1, Image1, TextBlock1, filenames[0], names[0], icons[0]);
            DoImageInitialize(btn2, Image2, TextBlock2, filenames[1], names[1], icons[1]);
            DoImageInitialize(btn3, Image3, TextBlock3, filenames[2], names[2], icons[2]);
            DoImageInitialize(btn4, Image4, TextBlock4, filenames[3], names[3], icons[3]);
            DoImageInitialize(btn5, Image5, TextBlock5, filenames[4], names[4], icons[4]);

            DoImageInitialize(btn2_1, Image2_1, TextBlock2_1, filenames[5], names[5], icons[5]);
            DoImageInitialize(btn2_2, Image2_2, TextBlock2_2, filenames[6], names[6], icons[6]);
            DoImageInitialize(btn2_3, Image2_3, TextBlock2_3, filenames[7], names[7], icons[7]);
            DoImageInitialize(btn2_4, Image2_4, TextBlock2_4, filenames[8], names[8], icons[8]);
            DoImageInitialize(btn2_5, Image2_5, TextBlock2_5, filenames[9], names[9], icons[9]);
        }
        void InitStrings()
        {
            string tmp = "";
            tmp=(string)Registry.GetValue(userRoot + subkey + "\\" , "Title", "");
            if(tmp!=null && tmp.Length > 0) this.Title.Text = tmp;
            tmp = (string)Registry.GetValue(userRoot + subkey + "\\", "Title1", "");
            if (tmp != null && tmp.Length > 0) this.Title1.Text = tmp;
            tmp = (string)Registry.GetValue(userRoot + subkey + "\\", "Title2", "");
            if (tmp != null && tmp.Length > 0) this.Title2.Text = tmp;
            tmp = (string)Registry.GetValue(userRoot + subkey + "\\", "Show", "");
            if (tmp != null && tmp.Length > 0) this.ShowText = tmp;
            tmp = (string)Registry.GetValue(userRoot + subkey + "\\", "Hide", "");
            if (tmp != null && tmp.Length > 0) this.HideText = tmp;
            this.ButtonPopupText.Text = this.HideText + this.Title1.Text;
        }
        void InitLinks()
        {
            this.filenames = new string[10];
            this.icons = new string[10];
            this.names = new string[10];
            this.commandargs = new string[10];
            for (int i = 1; i<= 10; i++)
            {
                this.filenames[i - 1] =(string)Registry.GetValue( userRoot + subkey +"\\" + i.ToString(),"","");
                this.icons[i - 1] = (string)Registry.GetValue(userRoot + subkey + "\\" + i.ToString(), "Icon", "");
                this.names[i - 1] = (string)Registry.GetValue(userRoot + subkey + "\\" + i.ToString(), "Name", "");
                this.commandargs[i - 1] = (string)Registry.GetValue(userRoot + subkey + "\\" + i.ToString(), "Parameters", "");
            }
        }
        void DoImageInitialize(Button button,Image image,TextBlock block, string path,string name,string iconpath="")
        {
            if (path==null || path.Length == 0)
            {
                button.Visibility = Visibility.Hidden;//Change to Collapsed
                return;
            }
            if (iconpath==null || iconpath.Length==0)
            {

                image.Source = ExtractIconToImage(path);
            }
            else
            {
                Uri uri = new Uri(iconpath);
                image.Source = new BitmapImage(uri);
            }
            block.Text= name;
        }
        BitmapImage ExtractIconToImage(string filepath,int index=0)
        {
             
            //var iconTotalCount = PrivateExtractIcons(filepath, index, 0, 0, null, null, 0, 0);
            //用于接收获取到的图标指针
            IntPtr[] hIcons = new IntPtr[1];
            //对应的图标id
            int[] ids = new int[1];
            int successCount = 0;
            //成功获取到的图标个数
            try
            {
                successCount = PrivateExtractIcons(filepath, index, 256, 256, hIcons, ids, 1, 0);
            }
            catch { successCount = 0; }
            if (hIcons[0] == (IntPtr)null)
            {
                DestroyIcon(hIcons[0]);
                PrivateExtractIcons("%Windir%\\System32\\imageres.dll", 3, 256, 256, hIcons, ids, 1, 0);
            }
            Icon ico = System.Drawing.Icon.FromHandle(hIcons[0]);
            Bitmap bm = ico.ToBitmap();
            ico.Dispose();
            DestroyIcon(hIcons[0]);
            BitmapImage img = BitmapToBitmapImage(bm);
            bm.Dispose();
            return img;
        }
        public static BitmapImage BitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png); // 坑点：格式选Bmp时，不带透明度

                stream.Position = 0;
                BitmapImage result = new BitmapImage();
                result.BeginInit();
                // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
                // Force the bitmap to load right now so we can dispose the stream.
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = stream;
                result.EndInit();
                result.Freeze();
                return result;
            }
        }

        [DllImport("User32.dll")]
        public static extern int PrivateExtractIcons(
             string lpszFile, //file name
             int nIconIndex,  //The zero-based index of the first icon to extract.
             int cxIcon,      //The horizontal icon size wanted.
             int cyIcon,      //The vertical icon size wanted.
             IntPtr[] phicon, //(out) A pointer to the returned array of icon handles.
             int[] piconid,   //(out) A pointer to a returned resource identifier.
             int nIcons,      //The number of icons to extract from the file. Only valid when *.exe and *.dll
             int flags        //Specifies flags that control this function.
         );

        //details:https://msdn.microsoft.com/en-us/library/windows/desktop/ms648063(v=vs.85).aspx
        //Destroys an icon and frees any memory the icon occupied.
        [DllImport("User32.dll")]
        public static extern bool DestroyIcon(
            IntPtr hIcon //A handle to the icon to be destroyed. The icon must not be in use.
        );
#endregion
private void ChangeViwBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height *0.6;
            this.Left = SystemParameters.WorkArea.Width * 0.994 - this.Width;
            this.Top = SystemParameters.WorkArea.Height * 0.993 - this.Height;
            this.Column1.Visibility = Visibility.Collapsed;
            this.Panel1.Visibility = Visibility.Collapsed;
            this.ChangeViewBtn.IsOpen = false;
            this.ChangeViewButton.Click -= ChangeViwBtn_Click;
            this.ChangeViewButton.Click += ChangeViwBtn_Click2;
            this.ButtonPopupText.Text = this.ShowText + this.Title1.Text;
        }
        private void ChangeViwBtn_Click2(object sender, RoutedEventArgs e)
        {
            this.Height = this.Height / 0.6;
            this.Left = SystemParameters.WorkArea.Width * 0.994 - this.Width;
            this.Top = SystemParameters.WorkArea.Height * 0.993 - this.Height;
            this.Column1.Visibility = Visibility.Visible;
            this.Panel1.Visibility = Visibility.Visible;
            this.ChangeViewBtn.IsOpen = false;
            this.ChangeViewButton.Click -= ChangeViwBtn_Click2;
            this.ChangeViewButton.Click += ChangeViwBtn_Click;
            this.ButtonPopupText.Text = this.HideText + this.Title1.Text;
        }
        private void svgbtn3_Click(object sender, RoutedEventArgs e)
        {
            this.ChangeViewBtn.IsOpen = true;
        }
        private void SystemEvents_DisplaySettingsChanged(object sender, EventArgs e)
      {
            AppInit();
      }
        #region OpenCommands
        [DllImport("shell32.dll")]
        static extern IntPtr ShellExecute(
        IntPtr hwnd,
        string lpOperation,
        string lpFile,
        string lpParameters,
        string lpDirectory,
        int nShowCmd);
        #endregion
        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            ShellExecute((IntPtr)null, "Open", this.filenames[0], this.commandargs[0], "", 1);
        }

        private void btn2_Click(object sender, RoutedEventArgs e)
        {

            ShellExecute((IntPtr)null, "Open", this.filenames[1], this.commandargs[1], "", 1);
        }

        private void btn3_Click(object sender, RoutedEventArgs e)
        {

            ShellExecute((IntPtr)null, "Open", this.filenames[2], this.commandargs[2], "", 1);
        }

        private void btn4_Click(object sender, RoutedEventArgs e)
        {

            ShellExecute((IntPtr)null, "Open", this.filenames[3], this.commandargs[3], "", 1);
        }

        private void btn5_Click(object sender, RoutedEventArgs e)
        {

            ShellExecute((IntPtr)null, "Open", this.filenames[4], this.commandargs[4], "", 1);
        }

        private void btn2_1_Click(object sender, RoutedEventArgs e)
        {
            ShellExecute((IntPtr)null, "Open", this.filenames[5], this.commandargs[5], "", 1);

        }

        private void btn2_2_Click(object sender, RoutedEventArgs e)
        {
            ShellExecute((IntPtr)null, "Open", this.filenames[6], this.commandargs[6], "", 1);
        }

        private void btn2_3_Click(object sender, RoutedEventArgs e)
        {
            ShellExecute((IntPtr)null, "Open", this.filenames[7], this.commandargs[7], "", 1);
        }

        private void btn2_4_Click(object sender, RoutedEventArgs e)
        {
            ShellExecute((IntPtr)null, "Open", this.filenames[8], this.commandargs[8], "", 1);
        }

        private void btn2_5_Click(object sender, RoutedEventArgs e)
        {
            ShellExecute((IntPtr)null, "Open", this.filenames[9], this.commandargs[9], "", 1);
        }
    }
}
