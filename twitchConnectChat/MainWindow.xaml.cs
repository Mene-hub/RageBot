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
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using MaterialDesignThemes.Wpf;
using TwitchLib.Api.Services.Events.LiveStreamMonitor;
using TwitchLib;
using System.Net;
using System.Media;
using System.Reflection;
using System.Net.Http;
using System.Diagnostics;

namespace twitchConnectChat
{
    /// <summary>
    /// Logica di interazione per MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool BotStart;
        Model model;
        string file;
        public MainWindow()
        {
            InitializeComponent();
            BotStart = false;
            this.DataContext = model;

            if (File.Exists(@"..\botconfig.txt"))
            {
                file = File.ReadAllText(@"..\botconfig.txt");
                TwitchChannelName.Text = file;
            }
            else
            {
                File.Create(@"..\botconfig.txt");
            }

            ConnectDot.Fill = Brushes.Red;
            ConnectStatus.Foreground = Brushes.Red;

            CheckVersion();
        }

        private async void CheckVersion()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "C# console program");

            var content = await client.GetStringAsync("http://real4dmotion.altervista.org/site/TwitchBot/Version.php");

            float projectVersion = float.Parse(File.ReadAllText("Version.txt"));

            if (projectVersion < float.Parse(content))
            {
                var Downloadoption = MessageBox.Show("There is an update!\ndo you want download it?", "Update", MessageBoxButton.YesNo);

                if (Downloadoption == MessageBoxResult.Yes)
                {
                    //MessageBox.Show("Start the Updater!");
                    System.Diagnostics.Process.Start(@"..\Updater");
                    /*Process[] runingProcess = Process.GetProcesses();
                    for (int i = 0; i < runingProcess.Length; i++)
                    {
                        // compare equivalent process by their name
                        if (runingProcess[i].ProcessName == "TwitchConnectChat")
                        {
                            // kill  running process
                            runingProcess[i].Kill();
                        }
                    }
                    //this.Close();
                }*/

                /*using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFileAsync(new Uri("http://real4dmotion.altervista.org/site/TwitchBot/TwitchBotPlus/TwitchBot.zip"), @"..\TwitchBot.zip");*/
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (BotStart == true)
            {
                BT_Start.Content = "START BOT";
                model.Close();
                model = null;
                BotStart = !BotStart;
                ConnectStatus.Content = "Offline";
                ConnectDot.Fill = Brushes.Red;
                ConnectStatus.Foreground = Brushes.Red;
            }
            else
            if (BotStart == false && TwitchChannelName.Text != "")
            {
                BT_Start.Content = "STOP BOT";
                model = new Model(TwitchChannelName.Text, this);
                BotStart = !BotStart;
                ConnectStatus.Content = "Online";
                ConnectDot.Fill = Brushes.Green;
                ConnectStatus.Foreground = Brushes.Green;
                FindAvatar();
            }
            else
                MessageBox.Show("Inserisci un canle");

        }

        private async void FindAvatar() 
        {
            File.WriteAllText("CurlCommand.bat", "curl -H \"Client-ID:la81ubesu8iud3aeckoekrd733apij\" -H \"Authorization:Bearer um48yvg9ezylg28i4sa5kbxseecgx6\" \"https://api.twitch.tv/helix/users?login=" + TwitchChannelName.Text + "\"");
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "CurlCommand.bat";
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            //MessageBox.Show(output);

            string ImageUrl = output.Split('"')[35];

            ImageProfile.Source = new BitmapImage(new Uri(ImageUrl, UriKind.RelativeOrAbsolute));

        }

        //private void Update(object sender, RoutedEventArgs e)
        //{
        //    string Json = TwitchChannelName.Text;
        //    File.WriteAllText(GestioneFileXml.path + @"..\botconfig.txt", Json);
        //}

        private void BT_DashboardOpener_Click(object sender, RoutedEventArgs e)
        {
            new Dashboard().Show();
        }

        //private void OpenTwitchBotCreator(object sender, RoutedEventArgs e)
        //{
        //    System.Diagnostics.Process.Start("https://twitchtokengenerator.com/");
        //}

        private void OpenCredits(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.instagram.com/4d_motion/");
        }

        public void StopBot() 
        {
            BT_Start.Content = "START BOT";
            model = null;
            BotStart = !BotStart;
            ConnectStatus.Content = "Offline";
            ConnectDot.Fill = Brushes.Red;
            ConnectStatus.Foreground = Brushes.Red;
        }

        private void BT_OverlayOpener_Click(object sender, RoutedEventArgs e)
        {
            new Overlay().Show();
        }

        private void OpenDonation(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.paypal.com/paypalme/MeneBot");
        }
    }
}
