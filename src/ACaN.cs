using System;
using EllieMae.Encompass.Reporting;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;

namespace ACaN2
{
    class ACaN : EncompassSession
    {
        public static Loan loan;
        public static BorrowerPair Pair { get; set; }
        public static SendDisclosureRecord sendDisclosureRecord = new SendDisclosureRecord();
        public static int updatedCount = 0;
        private static int currentLoanCount = 1;
        private static DateTime updateEndTime;
        private static DateTime updateStartTime;
        private static TimeSpan updateTotalTime;
        private static string reportText;
        private static string docuTechResults;
        private static string loanSkippedReason = null;

        public void RunACaN()
        {
            SessionPrep();
            ACaNWorkload();
            SessionClose();
        }
        private static void ACaNWorkload()
        {
            Console.WriteLine($"Identified {Prep.acanLoanCount} loans to be updated....\n");

            foreach (LoanReportData item in Prep.acanLoansReportData)
            {
                Console.WriteLine($"{currentLoanCount} of {Prep.numLoansToUpdate}\nGUID: {item.Guid}: {item["fields.364"]}");
                SetUpdateStarttime();
                loan = session.Loans.Open(item.Guid);
                if (loan.GetCurrentLock() == null && ReadyForACaN())
                {
                    loan.Lock();
                    LoanCleanup();
                    GenerateDocuments();
                    UpdateLoan();
                    LoanSaveAndClose();
                    Console.WriteLine("Loan Tasks Complete\n");
                    loan.Close();
                    currentLoanCount++;
                    updatedCount++;
                    SetUpdateEndtime();
                    GetTimeDifference();
                    CSVReport.acanReportcsv.WriteLine($"{item["fields.364"]},True,n/a, {updateStartTime.ToString()}, {updateTotalTime.ToString("mm\\:ss")},{reportText},{docuTechResults}");
                }
                else
                {
                    SetLoanSkippedReason();
                    CSVReport.acanReportcsv.WriteLine($"{item["fields.364"]}, False, {loanSkippedReason}, {DateTime.Now.ToString()}");
                    Console.WriteLine($"Cannot update due to {loanSkippedReason}... Moving on to the next one\n");
                    currentLoanCount++;
                }
            }
        }
        private static void SessionPrep()
        {
            Prep.PrepareSession();
            PrepareLogging();
        }
        private static void PrepareLogging()
        {
            CSVReport.PrepareCSV();
            SessionLog.PrepareSessionLog();
        }
        private static void LoanCleanup()
        {
            LoanMaintenance.LoanCleanUp();
        }
        public static bool NOANeeded()
        {
            return (!loan.Fields["CX.CANREQ.10DAYLETTER"].Value.Equals("Y") && 
                !loan.Fields["CX.CANREQ.BEATBYCOMP"].Value.Equals("Y") &&
                !loan.Fields["CX.CANREQ.LANDHOME"].Value.Equals("Y") &&
                !loan.Fields["CX.CANREQ.WITHDRAWN"].Value.Equals("Y") &&
                !loan.Fields["CX.CANREQ.DUPLICATELOAN"].Value.Equals("Y") &&
                !loan.Fields["CX.CANREQ.REMOVECOBORR"].Value.Equals("Y"));
        }
        private static void CompleteCancellation()
        {
            Console.WriteLine("Completing Cancellation...");

            //Cancel Pulled
            loan.Fields["CX.CANREQ.PULLED"].Value = "X";
            loan.Fields["CX.CANREQ.PULLED.BY"].Value = $"{session.GetCurrentUser()}";
            loan.Fields["CX.CANREQ.PULLED.DATE"].Value = DateTime.Now.ToString();

            //Cancel Completed
            loan.Fields["CX.CANREQ.COMPLETED"].Value = "X";
            loan.Fields["CX.CANREQ.COMPLETED.BY"].Value = $"{session.GetCurrentUser()}";
            loan.Fields["CX.CANREQ.COMPLETED.DATE"].Value = DateTime.Now.ToString();

            loan.Fields["CX.GLOBAL.LASTMODIFIED"].Value = DateTime.Now.ToString();
            Console.WriteLine("Done");
        }
        private static void LoanSaveAndClose()
        {
            Console.WriteLine("Saving and closing the loan....");
            //loan.Commit();
            //loan.Unlock();
            loan.Close();
        }
        private static void GenerateDocuments()
        {
            if (NOANeeded())
            {
                Console.WriteLine("Generating NOA's and Uploading to eFolder");

                int numOfPairs = loan.BorrowerPairs.Count;
                for (int i = 0; i < numOfPairs; i++)
                {
                    ACaN.Pair = loan.BorrowerPairs[i];
                    GenerateNOAs.GenerateNOAPackages();
                }
                Console.WriteLine("Done");
                //SendToDocutech();

            }
            else
            {
                Console.WriteLine("NOA Documents not needed\n");
            }
        }
        private static void SessionClose()
        {
            Closing.ClosingActions();
        }
        private static void UpdateConversationLog()
        {
            Console.WriteLine("Updating Conversation Log...");
            string conLogBody = SetConLogBody();

            Conversation cLog = loan.Log.Conversations.Add(DateTime.Now);
            cLog.DisplayInLog = true;
            cLog.ContactMethod = ConversationContactMethod.Email;
            cLog.Company = "";
            cLog.HeldWith = $"{session.GetCurrentUser()}";
            cLog.NewComments = conLogBody;
            Console.WriteLine("Done");
        }
        private static string SetConLogBody()
        {
            if (!NOANeeded())
            {
                string conLogBody = $"File cancel completed by {session.GetCurrentUser()} - Cancellation Reason does not require NOA";
                return conLogBody;
            }
            else
            {
                string conLogBody = $"File cancel completed by {session.GetCurrentUser()} - NOA generated - NOA Added to Statement of Denial Container - NOA Prepared for Mailing";
                return conLogBody;
            }
        }
        private static void MoveToApplicableFolder()
        {
            Console.WriteLine("Updating Loan Folder...");
            LoanFolder inactiveProspects = session.Loans.Folders["Inactive Prospects"];
            LoanFolder adverseLoans = session.Loans.Folders["Adverse Loans"];

            if (!NOANeeded())
            {
                if (loan.LoanFolder != "Inactive Prospects")
                {
                    loan.MoveToFolder(inactiveProspects);
                    loan.Fields["CX.GLOBAL.FOLDER"].Value = "Inactive Prospects";
                }
            }
            else
            {
                if (loan.LoanFolder != "Adverse Loans")
                {
                    loan.MoveToFolder(adverseLoans);
                    loan.Fields["CX.GLOBAL.FOLDER"].Value = "Adverse Loans";
                }
            }
            Console.WriteLine("Done");
        }
        private static void UpdateLoan()
        {
            CompleteCancellation();
            UpdateConversationLog();
            MoveToApplicableFolder();
            UpdateReportText();
        }
        private static void UpdateReportText()
        {
            if (NOANeeded())
            {
                reportText = "NOA Generated Successfully";
                docuTechResults = SendDisclosureRecord.DocutechResults;
            }
            else
            {
                reportText = "NOA Not Needed";
                docuTechResults = "n/a";
            }
        }
        private static void SetUpdateStarttime()
        {
            updateStartTime = DateTime.Now;
        }
        private static void SetUpdateEndtime()
        {
            updateEndTime = DateTime.Now;
        }
        private static void GetTimeDifference()
        {

            updateTotalTime = updateEndTime - updateStartTime;
        }
        private static void SendToDocutech()
        {
            Console.WriteLine("Sending to DocuTech...");
            sendDisclosureRecord.PostToDocuTech(DisclosureTypes.NOA).Wait();
        }
        private static bool ReadyForACaN()
        {
            return 
                LoanMaintenance.CreditInfoMissing().Equals(false) &&
                LoanMaintenance.NoEmptyBorrowerPairs() &&
                LoanMaintenance.ValidAddress();
        }

        private static void SetLoanSkippedReason()
        {
            if (loan.GetCurrentLock() != null)
            {
                loanSkippedReason = "Loan Locked";
            }
            if (LoanMaintenance.CreditInfoMissing())
            {
                loanSkippedReason = "Missing Credit Information";
            }
            if(LoanMaintenance.NoEmptyBorrowerPairs().Equals(false))
            {
                loanSkippedReason = "Empty Borrower Pair Detected";
            }
            if(LoanMaintenance.ValidAddress().Equals(false))
            {
                loanSkippedReason = "Valid Address Not Found";
            }
        }
    }
}
