using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MagmaLightWeb.NaplampaService;
using System.Configuration;
using System.Threading;

namespace MagmaLightWeb.Common
{
    public static class ServiceManager
    {
        private static NaplampaServiceClient naplampaServiceClient;
        private static DateTime lastActivity;
        private static Timer inactivityTimer;

        public static NaplampaServiceClient NaplampaService
        {
            get
            {
                lastActivity = DateTime.UtcNow;

                lock (inactivityTimer)
                {
                    if ((naplampaServiceClient == null) || (naplampaServiceClient.State != System.ServiceModel.CommunicationState.Opened))
                    {
                        naplampaServiceClient = new NaplampaServiceClient();
                        naplampaServiceClient.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["NaplampaUsername"];
                        naplampaServiceClient.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["NaplampaPassword"];
                    }
                }

                return naplampaServiceClient;
            }
        }


        static ServiceManager()
        {
            inactivityTimer = new Timer(new TimerCallback(InactivityCallBack), 0, 0, 1000);
        }

        private static void InactivityCallBack(Object stateObject)
        {
            lock (inactivityTimer)
            {
                if ((lastActivity.AddSeconds(50) < DateTime.UtcNow) && (naplampaServiceClient != null))
                {
                    naplampaServiceClient.Close();
                    naplampaServiceClient = null;
                }
            }
        }

    }
}
