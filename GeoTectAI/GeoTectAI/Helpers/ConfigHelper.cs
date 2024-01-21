using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoTectAI.Helpers
{
    internal class ConfigHelper
    {
        //读取配置文件,传入关键字，返回值
        public static string ReadConfig(string key)
        {
            string value = System.Configuration.ConfigurationManager.AppSettings[key];
            return value;
        }

        //修改配置文件，传入关键这和值，返回布尔值
        public static bool WriteConfig(string key, string value)
        {
            try
            {
                System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                config.AppSettings.Settings[key].Value = value;
                config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
