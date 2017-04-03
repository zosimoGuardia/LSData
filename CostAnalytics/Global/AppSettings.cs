using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dell.CostAnalytics.Global
{
    public static class AppSettings
    {
        #region Properties
        public static string DBServer
        {
            get { return Global.Default.DBServer; }
        }

        public static string DBName
        {
            get { return Global.Default.DBName; }
        }

        public static string DBAccountName
        {
            get { return Global.Default.DBAccountName; }
        }

        public static string DBPassword
        {
            get { return Global.Default.DBPassword; }
        }

        public static bool DBPersistentSecurity
        {
            get { return Global.Default.DBPersistentSecurity; }
        }

        public static int DBPacketSize
        {
            get { return Global.Default.DBPacketSize; }
        }

        public static int DBNumberOfRetries
        {
            get { return Global.Default.DBNumberOfRetries; }
        }

        public static int DBDefaultTimeout
        {
            get { return Global.Default.DBDefaultTimeout; }
        }
        #endregion
    }
}
