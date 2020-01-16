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

namespace StopWatch
{
    public partial class MainWindow : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        Stopwatch stopWatch = new Stopwatch();
        private int idCount;
        private string currentTime;

        public MainWindow()
        {
            InitializeComponent();
            Timer_Loader();
            TimerStage = eTimerStage.Paussed;
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
            // Set a variable to the Documents path.
            string documentPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Append text to an existing file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(documentPath, "WriteLines.txt")))
                foreach (Items item in ListItems1.Items)
                {
                    outputFile.WriteLine($"{item.Id + "." + " " + item.Time}");
                }
        }

        // как винда понимает какая кнопка нажата через user32 библиотеку

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