using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
//using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pomodoro {

    public partial class MainWindow : Window {

        private DispatcherTimer timer = new DispatcherTimer();
        private MediaPlayer mediaPlayer = new MediaPlayer();

        private System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();
        private System.Windows.Forms.ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();


        private uint counter = 0;

        public MainWindow() {
            InitializeComponent();

            MinimizeToTray();

            timer.Tick += new EventHandler(TimerTick);

            SwitchVisibility();

            timer.Start();
        }

        private void InitContectMenu() {
            System.Windows.Forms.MenuItem menuItemAbout = new System.Windows.Forms.MenuItem { Text = "A&bout" };
            menuItemAbout.Click += delegate (object sender, EventArgs args) { };
            menuItemAbout.Enabled = false;
            contextMenu.MenuItems.Add(menuItemAbout);

            System.Windows.Forms.MenuItem menuItemExit = new System.Windows.Forms.MenuItem { Text = "E&xit" };
            menuItemExit.Click += delegate (object sender, EventArgs args) {
                notifyIcon.Visible = false;
                Application.Current.Shutdown();
            };
            menuItemExit.Enabled = true;
            contextMenu.MenuItems.Add(menuItemExit);
        }

        private void MinimizeToTray() {
            notifyIcon.Icon = new System.Drawing.Icon(@"F:\C#\Project\Pomodoro\Pomodoro\icon.ico");
            notifyIcon.Visible = true;
            
            InitContectMenu();

            notifyIcon.ContextMenu = contextMenu;
        }

        protected override void OnStateChanged(EventArgs e) {
            if (WindowState == WindowState.Minimized) {
                //Hide();
            }

            base.OnStateChanged(e);
        }


        private void TimerTick(object sender, EventArgs e) {
            SwitchVisibility();
        }

        private void PlaySound(string uri) {
            mediaPlayer.Open(new Uri(uri));
            mediaPlayer.Play();
        }

        private void SwitchVisibility() {
            mediaPlayer.Stop();

            if (IsVisible || counter == 0) {
                PlaySound(@"F:\C#\Project\Pomodoro\Pomodoro\Audio\clock.wav");
                window.WindowState = WindowState.Minimized;
                Hide();
                timer.Interval = new TimeSpan(0, 25, 0);
                counter++;
            } else {
                labelCounter.Content = string.Format("{0:d2}", counter);
                PlaySound(@"F:\C#\Project\Pomodoro\Pomodoro\Audio\relax.wav");
                Show();
                window.WindowState = WindowState.Maximized;

                if (counter % 4 == 0) {
                    labelCurrentTime.Content = "25";
                    timer.Interval = new TimeSpan(0, 25, 0);
                } else {
                    labelCurrentTime.Content = "5";
                    timer.Interval = new TimeSpan(0, 5, 0);
                }

            }

        }

    }

}
