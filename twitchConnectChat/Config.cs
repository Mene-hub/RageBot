using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace twitchConnectChat
{
    public class Config
    {
        public string Channel;
        
        public void SetConfig(string channel)
        {
            File.WriteAllText(@"..\botconfig.txt", channel);
            Channel = channel;
        }

    }
}
