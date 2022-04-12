using System;
using EllieMae.Encompass.Client;
using System.Configuration;

namespace ACaN2
{

    class EncompassSession
    {
        public static Session session;
        private static string _encompassEnviornment = ConfigurationManager.AppSettings["EncompassEnvironment"];
        private static string _prodServer = ConfigurationManager.AppSettings["PRODServer"];
        private static string _uatServer = ConfigurationManager.AppSettings["UATServer"];
        private static string _user = ConfigurationManager.AppSettings["UserName"];
        private static string _pw = ConfigurationManager.AppSettings["PW"];
        private static bool _connected = false;


        public void StartApplication()
        {
            if (_encompassEnviornment.Equals("PROD"))
            {
                StartProdSession();
                ConfirmSession();
            }
            if (_encompassEnviornment.Equals("UAT"))
            {
                StartUATSession();
                ConfirmSession();
            }
            if (!_encompassEnviornment.Equals("PROD") && !_encompassEnviornment.Equals("UAT"))
            {
                Console.WriteLine("Encompass Environment has not been determined");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }
        private static void StartProdSession()
        {
            session = new Session();
            session.Start(_prodServer, _user, _pw);
        }

        private static void StartUATSession()
        {
            session = new Session();
            session.Start(_uatServer, _user, _pw);
        }

        private static void ConfirmSession()
        {
            Console.WriteLine($"Successful Encompass Connection = {session.IsConnected.ToString()}");
            if (session.IsConnected)
            {
                Console.WriteLine($"Connected to {session.ClientID} as {session.GetCurrentUser()}\n");
                _connected = true;
            }
        }
        public void CloseApplication()
        {
            if (_connected)
            {
                Console.WriteLine("Disconnecting...");
                session.End();
            }
        }
    }
}
