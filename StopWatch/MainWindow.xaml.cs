using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Windows.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace StopWatch
{
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Stopwatch stopWatch = new Stopwatch();
        private int idCount;
        private string currentTime;

        public string documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int HOTKEY_ID = 9000;

        //Modifiers:
        private const uint MOD_NONE = 0x0000; //(none)
        private const uint MOD_ALT = 0x0001; //ALT
        private const uint MOD_CONTROL = 0x0002; //CTRL
        private const uint MOD_SHIFT = 0x0004; //SHIFT
        private const uint MOD_WIN = 0x0008; //WINDOWS
        //CAPS LOCK:
        private const uint VK_CAPITAL = 0x14;
        public MainWindow()
        {
            InitializeComponent();
            Timer_Loader();
            TimerStage = eTimerStage.Paussed;
            Path_label.Content = $"{documentPath}";
        }

        void Timer_Loader()
        {
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);

            void dispatcherTimer_Tick(object sender, EventArgs e)
            {
                TimeSpan customTimeSpan = stopWatch.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}:{2:00}",
                customTimeSpan.Hours, customTimeSpan.Minutes, customTimeSpan.Seconds);

                CurTimer.Content = currentTime;
            }

        }

        public enum eTimerStage
        {
            Stopped,
            Started,
            Paussed
        }

        public eTimerStage TimerStage
        {
            get { return (eTimerStage)GetValue(TimerStageProperty); }
            set { SetValue(TimerStageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TimerStage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimerStageProperty =
            DependencyProperty.Register("TimerStage", typeof(eTimerStage), typeof(MainWindow), new PropertyMetadata(eTimerStage.Stopped));

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            Items item = new Items(++idCount, currentTime);
            ListItems1.Items.Add(item);
            btSave.IsEnabled = true;
        }

        private void btStart_Click(object sender, RoutedEventArgs e)
        {
            TimerStage = eTimerStage.Started;
            dispatcherTimer.Start();
            stopWatch.Start();
        }

        private void btPause_Click(object sender, RoutedEventArgs e)
        {
            TimerStage = eTimerStage.Paussed;
            dispatcherTimer.Stop();
            stopWatch.Stop();
        }

        private void btReset_Click(object sender, RoutedEventArgs e)
        {
            TimerStage = eTimerStage.Paussed;
            stopWatch.Reset();
            ListItems1.Items.Clear();
            CurTimer.Content = "00:00:00";
            idCount = 0;
        }

        private void btSave_Click(object sender, RoutedEventArgs e)
        {
            // Create a string array with the lines of text
            // Append text to an existing file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(documentPath, "WriteLines.txt")))
           
            foreach (Items item in ListItems1.Items)
            {
                outputFile.WriteLine($"{item.Id + "." + " " + item.Time}");
            }
        }

        private void Path_label3_KeyDown(object sender, KeyEventArgs e)
        {
            {
                if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift && e.Key == Key.F1)
                    Path_label3.Content = MessageBox.Show("HELLO");
            }
        }

        private IntPtr _windowHandle;
        private HwndSource _source;
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            _windowHandle = new WindowInteropHelper(this).Handle;
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);

            RegisterHotKey(_windowHandle, HOTKEY_ID, MOD_CONTROL, VK_CAPITAL); //CTRL + CAPS_LOCK
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    switch (wParam.ToInt32())
                    {
                        case HOTKEY_ID:
                            int vkey = (((int)lParam >> 16) & 0xFFFF);
                            if (vkey == VK_CAPITAL)
                            {
                                Path_label3.Content += "CapsLock was pressed" + Environment.NewLine;
                            }
                            handled = true;
                            break;
                    }
                    break;
            }
            return IntPtr.Zero;
        }

        protected override void OnClosed(EventArgs e)
        {
            _source.RemoveHook(HwndHook);
            UnregisterHotKey(_windowHandle, HOTKEY_ID);
            base.OnClosed(e);
        }

    }

    public class Items
    {
        public int Id { get; set; }
        public string Time { get; set; }

        public Items(int Id, string Time)
        {
            this.Id = Id;
            this.Time = Time;
        }
    }



}