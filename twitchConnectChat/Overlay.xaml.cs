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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace twitchConnectChat
{
    /// <summary>
    /// Logica di interazione per Overlay.xaml
    /// </summary>
    public partial class Overlay : Window
    {
        DispatcherTimer dispatcherTimer;
        public Overlay()
        {
            InitializeComponent();

            Update();
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0,0,30);
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            Update();
        }

        private void Update() 
        {
            LB_MinBits.Content = "Min bits donation: " + GestioneFileXml.ReadConfig().MinBits;
            dispatcherTimer = new DispatcherTimer();
        }

        private void StopTimer(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dispatcherTimer.Stop();
        }
    }
}
