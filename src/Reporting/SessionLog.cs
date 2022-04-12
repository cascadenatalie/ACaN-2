using System;
using System.IO;


namespace ACaN2
{
    class SessionLog
    {
        public static string acanLog = Resources.acanLog;
        public static StreamWriter sessionLog = null;
        private static int count = 0;
        private static int maxTries = 3;

        public static void PrepareSessionLog()
        {
            while(true)
            {
                try
                {
                    sessionLog = new StreamWriter(acanLog, true);
                    sessionLog.AutoFlush = true;
                    sessionLog.WriteLine($"ACaN Session Start Time {DateTime.Now.ToString()}\nLoan Count: {Prep.acanLoanCount}");
                    break;
                }
                catch (Exception al)
                {
                    Console.WriteLine($"Failed to make ACaN Log, {al.Message}\n{al.InnerException}");
                    Console.ReadLine();
                    if (++count == maxTries)
                    {
                        Environment.Exit(99);
                    }
                }
            }
        }
    }
}
