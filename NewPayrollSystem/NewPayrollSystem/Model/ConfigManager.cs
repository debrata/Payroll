using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPayrollSystem.Model
{
   public class ConfigManager
    {

        internal static CCommonConstants GetConfig<T1>()
        {
            CCommonConstants aCommonConostant = new CCommonConstants();
            aCommonConostant.DBConnection = ConfigurationSettings.AppSettings["ConnectionString"];

            return aCommonConostant;
        }
    }
}
