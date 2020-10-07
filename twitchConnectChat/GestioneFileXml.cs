using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Windows;
using System.Reflection;

namespace twitchConnectChat
{
    class GestioneFileXml
    {
        public static string path = @"..\";
        public static void SaveConfig(Dash U)
        {
            string NomeFile = "DashConfig.xml";
            try
            {
                XmlSerializer xmls = new XmlSerializer(typeof(Dash));
                StreamWriter Sw = new StreamWriter(path + NomeFile, false);
                xmls.Serialize(Sw, U);
                Sw.Close();
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static Dash ReadConfig()
        {
            string NomeFile = "DashConfig.xml";
            try
            {
                XmlSerializer Xmls = new XmlSerializer(typeof(Dash));
                StreamReader Sr = new StreamReader(path + NomeFile);
                Dash L = (Dash)Xmls.Deserialize(Sr);
                Sr.Close();
                return L;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
