using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using TwitchLib.Client;
using System.Reflection;

namespace twitchConnectChat
{
    /// <summary>
    /// Logica di interazione per Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void Inizialize(object sender, EventArgs e)
        {
            Dash temp = new Dash();
            temp = GestioneFileXml.ReadConfig();

            if (File.Exists(@"..\DashConfig.xml"))
            {
                EntryEnable.IsChecked = temp.EntryEnable;
                MouseEnable.IsChecked = temp.MouseEnable;
                SL_MouseSensibility.Value = temp.MouseSens;
                ShootEnable.IsChecked = temp.MouseShootEnable;
                SL_MinBits.Value = temp.MinBits;

                TB_MouseSensibility.Text = "Mouse Sensibility: " + (int)SL_MouseSensibility.Value;
                TB_MinBits.Text = "Min Bits Donation: " + (int)SL_MinBits.Value;

                if (MouseEnable.IsChecked == false)
                {
                    ShootEnable.IsEnabled = false;
                    SL_MouseSensibility.IsEnabled = false;
                }
                if (MouseEnable.IsChecked == true)
                {
                    ShootEnable.IsEnabled = true;
                    SL_MouseSensibility.IsEnabled = true;
                }
            }
        }

        private void UpdateMinBitsValue(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TB_MinBits.Text = "Min Bits Donation: " + (int)SL_MinBits.Value;
        }

        private void UpdateMouseSensibilityValue(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
                TB_MouseSensibility.Text = "Mouse Sensibility: " + (int)SL_MouseSensibility.Value;
        }

        private void BT_Save_Click(object sender, RoutedEventArgs e)
        {
            Dash temp = new Dash();
            temp.EntryEnable = (bool)EntryEnable.IsChecked;
            temp.MouseEnable = (bool)MouseEnable.IsChecked;
            temp.MouseSens = (int)SL_MouseSensibility.Value;
            temp.MouseShootEnable = (bool)ShootEnable.IsChecked;
            temp.MinBits = (int)SL_MinBits.Value;
            GestioneFileXml.SaveConfig(temp);
        }

        private void MouseEnabled(object sender, RoutedEventArgs e)
        {
            if (MouseEnable.IsChecked == false)
            {
                ShootEnable.IsEnabled = false;
                SL_MouseSensibility.IsEnabled = false;
            }
            if (MouseEnable.IsChecked == true)
            {
                ShootEnable.IsEnabled = true;
                SL_MouseSensibility.IsEnabled = true;
            }
        }
    }
}
