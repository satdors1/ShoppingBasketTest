using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasket.Helpers
{
    public static class SettingHelper
    {
        public static string GetAppSettingValue(string key)
        {
            try
            {
                return ConfigurationManager.AppSettings[key].ToString();
            }
            catch
            {
                throw new SettingsPropertyNotFoundException($"{key} configuration not found.");
            }
        }
    }
}
