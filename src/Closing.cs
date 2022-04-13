using System;
using System.IO;


namespace ACaN2
{
    class Closing
    {
        public static void ClosingActions()
        {
            CloseInitialReportCursor();
            GetRemainingLoanCount();
            UpdateAndCloseSessionLog();
            CloseRemainingLoanCountReport();
            CloseCSVReport();
            ClosingMessage();
        }

        private static void CloseInitialReportCursor()
        {
            Prep.acanLoanReport.Close();
        }
        private static void GetRemainingLoanCount()
        {
            Prep.GetRemainingLoans();
        }
        private static void CloseRemainingLoanCountReport()
        {
            Prep.remainingACaNLoans.Close();
        }
        private static void UpdateAndCloseSessionLog()
        {
            SessionLog.sessionLog.WriteLine($"ACaN2 Session End Time {DateTime.Now.ToString()}\nSuccessfully updated {ACaN.updatedCount} Loans. Remaining ACaN Loan Count: {Prep.remainingACaNLoans.Count}");
            SessionLog.sessionLog.WriteLine("");
            SessionLog.sessionLog.Close();
        }
        private static void CloseCSVReport()
        {
            CSVReport.acanReportcsv.Close();
            Console.WriteLine("Submitting ACaN csv to Reporting...");
            File.Move(Resources.acanReport, Resources.acanReportSend);
            Console.WriteLine("Done");
        }
        private static void ClosingMessage()
        {
            Console.WriteLine($"Total Number of Results {Prep.acanLoanCount}");
            Console.WriteLine($"Total Number of results remaining {Prep.remainingLoanCount}");
        }
    }
}
