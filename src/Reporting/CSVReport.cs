using System;
using System.IO;


namespace ACaN2
{
    class CSVReport
    {
        public static string acanReport = Resources.acanReport;
        public static StreamWriter acanReportcsv { get; private set; }
        private static int count = 0;
        private static int maxTries = 3;

        public static void PrepareCSV()
        {
            while(true)
            {
                try
                {
                    acanReportcsv = new StreamWriter(acanReport, append: true);
                    acanReportcsv.AutoFlush = true;
                    acanReportcsv.WriteLine("LoanNumber,LoanOpened,Reason,UpdateStartTime,UpdateDuration,NOAResponse,DocutechResponse");
                    break;
                }
                catch (Exception ar)
                {
                    Console.WriteLine($"Failed to make ACaN Report, {ar.Message}\n{ar.InnerException}");
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
