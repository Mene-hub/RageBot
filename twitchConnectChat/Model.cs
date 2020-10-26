using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
//using System.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TwitchLib.Api.Helix.Models.Subscriptions;
using TwitchLib.Client;
using TwitchLib.Api;
using TwitchLib.PubSub;
using TwitchLib.Api.V5;
using TwitchLib.Api.Core;
using TwitchLib.Api.Core.Common;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using TwitchLib.Communication.Services;
using WindowsInput;
using WindowsInput.Native;
using System.Web.UI.HtmlControls;
using System.Net;
using System.IO;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using AudioSwitcher.AudioApi.CoreAudio;
using System.Reflection;
using System.Runtime.InteropServices;

namespace twitchConnectChat
{
    public class Model : INotifyPropertyChanged
    {
        private Config configgo;
        public TwitchClient client;
        private Dash DashConfig;
        private string Channel;
        MainWindow main;
        WebBrowser WebQuery;

        //Cose

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

        const int WM_COMMAND = 0x111;
        const int MIN_ALL = 419;
        const int MIN_ALL_UNDO = 416;

        //---------------
        public Model(string channel, MainWindow Main, WebBrowser wb)
        {
            configgo = new Config();
            DashConfig = new Dash();
            configgo.SetConfig(channel);
            Channel = channel;
            main = Main;
            WebQuery = wb;
            connect();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propretyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propretyName));
        }

        private void connect()
        {
            ConnectionCredentials credentials = new ConnectionCredentials("TheMeneBot", "gnudhq7j8ik6yek3y0e9vn5gi3lx77");
            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 999, ThrottlingPeriod = TimeSpan.FromSeconds(3)
            };
            WebSocketClient costumClient = new WebSocketClient(clientOptions);
            
            client = new TwitchClient(costumClient);
            client.Initialize(credentials, Channel);
            client.OnConnected += Client_OnConnected;
            client.OnDisconnected += Client_OnDisconnected;
            client.OnMessageReceived += Client_OnMessageReceived;
            client.Connect();
            while (client.JoinedChannels.Count<=0) ;
            var tmp = client.JoinedChannels.FirstOrDefault().Channel;
            if(GestioneFileXml.ReadConfig().TestEnable==false)
                WebQuery.Navigate("http://ragebot.unaux.com/Files/Query.php?Name=" + tmp);
        }

        private void Client_OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            update();
            CommandRecived(e.ChatMessage.Message);
            try
            {
                if (e.ChatMessage.Bits >= DashConfig.MinBits)
                {
                    CommandRecived("!" + e.ChatMessage.Message.Split('!')[1]);
                }
            }
            catch (Exception E) { }

            if(GestioneFileXml.ReadConfig().TestEnable==true)
                CommandRecived(e.ChatMessage.ToString());
        }

        private void Client_OnDisconnected(object sender, TwitchLib.Communication.Events.OnDisconnectedEventArgs e)
        {
            //client.SendMessage(Channel, "Oh no mi sono disconnesso");
        }

        private void Client_OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
        {
            //client.SendMessage(client.JoinedChannels.FirstOrDefault(), "Eccomi! Pronto a rovinarti la giornata!");
        }

        public void Close() 
        {
            if (client.IsConnected)
            {
                client.Disconnect();
            }
        }

        public void update() 
        {
            DashConfig = GestioneFileXml.ReadConfig();
        }

        public void Chancge_Volume(int Value) 
        {
            CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            defaultPlaybackDevice.Volume = Value;
        }

        public void Minimize()
        {
            IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
            SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL, IntPtr.Zero);
        }

        public void AltTab()
        {
            int random = new Random().Next(1, 100);
            InputSimulator input = new InputSimulator();
            for (int i = 0; i < random; i++)
            {
               
                input.Keyboard.KeyDown(VirtualKeyCode.MENU);
                input.Keyboard.KeyDown(VirtualKeyCode.TAB);
                Thread.Sleep(10);
                input.Keyboard.KeyUp(VirtualKeyCode.MENU);
                input.Keyboard.KeyUp(VirtualKeyCode.TAB);
            }
            
            input.Keyboard.KeyDown(VirtualKeyCode.RETURN);

        }

        public void CommandRecived(string e) 
        {
            e = e.ToLower();
            InputSimulator input = new InputSimulator();
            update();

            if (e.Split(' ')[0] == "!minimize")
                Minimize();

            if (e == "!altab")
                AltTab();

            if (e.Split(' ')[0] == "!volume")
                Chancge_Volume(int.Parse(e.Split(' ')[1]) > 100 ? 100 : int.Parse(e.Split(' ')[1]) < 0 ? 0 : int.Parse(e.Split(' ')[1]));

            //su cod non va
            if (e.Split(' ')[0] == "!entry" && DashConfig.EntryEnable)
            {
                for (int i = 1; i < e.Split(' ').Length; i++)
                    input.Keyboard.TextEntry(e.Split(' ')[i] + " ");
            }
            
            /*if(e=="!stop")
                System.Diagnostics.Process.Start("shutdown", "/s /t 0");*/

            if (e.Split(' ')[0] == "!mouse" && DashConfig.MouseEnable)
            {
                try
                {

                    if (e.Split(' ')[1] == "shoot" && DashConfig.MouseShootEnable)
                    {
                        input.Mouse.LeftButtonDown();
                        Thread.Sleep(1000);
                        input.Mouse.LeftButtonUp();
                    }

                    //int tempo = int.Parse(e.Split(' ')[3]);
                    int x = int.Parse(e.Split(' ')[1]);
                    int y = int.Parse(e.Split(' ')[2]);

                    /*if (int.Parse(e.Split(' ')[3]) > 2)
                        tempo = 2;*/

                    if (x > DashConfig.MouseSens)
                        x = DashConfig.MouseSens;

                    if (y > DashConfig.MouseSens)
                        y = DashConfig.MouseSens;

                    for (int i = 0; i < 50; i++)
                    {
                        input.Mouse.MoveMouseBy(x, y);
                        Thread.Sleep(10);
                    }
                }
                catch (Exception E) { }
            }
        }
    }
}