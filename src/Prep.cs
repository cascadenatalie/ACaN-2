using System;
using System.Collections.Generic;
using EllieMae.Encompass.Query;
using EllieMae.Encompass.Collections;
using EllieMae.Encompass.Reporting;




namespace ACaN2
{
    class Prep : EncompassSession
    {
        //Report Cursors
        public static LoanReportCursor acanLoanReport;
        public static LoanReportCursor remainingACaNLoans;

        //Filter Criteria
        public static StringList acanLoans;
        public static QueryCriterion acanCri;
        public static SortCriterionList acanCriSortBy;
        
        //Report
        public static List<LoanReportData> acanLoansReportData;

        public static int acanLoanCount;
        public static int remainingLoanCount;
        public static int numLoansToUpdate;
        private static int _maxLoansToUpdate = Resources.maxLoansToUpdate;


        public static void PrepareSession()
        {
            Console.WriteLine("Preparing Session");
            SetACaNCriteria();
            ACaNFields();
            SetCursor();
            SetLoanCount();
            GetLoans();
            CreateReport();
        }

        private static void SetACaNCriteria()
        {
            StringFieldCriterion CancelRequested = new StringFieldCriterion
            {
                FieldName = "Fields.CX.CANREQ.REQ.DATE",
                Value = "",
                MatchType = StringFieldMatchType.Exact,
                Include = false
            };

            StringFieldCriterion notCancelCompleted = new StringFieldCriterion
            {
                FieldName = "Fields.CX.CANREQ.COMPLETED.DATE",
                Value = "",
                MatchType = StringFieldMatchType.Exact,
            };

            DateFieldCriterion botRunDateMetorExceeded = new DateFieldCriterion
            {
                FieldName = "Fields.CX.CANREQ.NOABOTRUN.DATE",
                Value = DateTime.Today,
                MatchType = OrdinalFieldMatchType.LessThanOrEquals
            };

            DateFieldCriterion denialDateOlderThan30Days = new DateFieldCriterion
            {
                FieldName = "Fields.DENIAL.X69",
                Value = DateTime.Today.AddDays(-30),
                MatchType = OrdinalFieldMatchType.LessThan
            };

            DateFieldCriterion denialDateisEmpty = new DateFieldCriterion
            {
                FieldName = "Fields.DENIAL.X69",
                Value = DateTime.MinValue,
                MatchType = OrdinalFieldMatchType.Equals,
            };

            StringFieldCriterion testLoanIsNotY = new StringFieldCriterion
            {
                FieldName = "Fields.CX.GLOBAL.TESTLOAN",
                Value = "Y",
                MatchType = StringFieldMatchType.Exact,
                Include = false
            };

            StringFieldCriterion creditPlusInfoProvided = new StringFieldCriterion
            {
                FieldName = "Fields.CX.MISSING.CREDIT.INFO",
                Value = "N",
                MatchType = StringFieldMatchType.Exact,
            };

            StringFieldCriterion cancellationReasonSelected = new StringFieldCriterion
            {
                FieldName = "FIELDS.CX.CANCELLATION.REASON",
                Value = "",
                MatchType = StringFieldMatchType.Exact,
                Include = false
            };

            StringFieldCriterion loIsNotBlank = new StringFieldCriterion
            {
                FieldName = "FIELDS.317",
                Value = "",
                MatchType = StringFieldMatchType.Exact,
                Include = false
            };

            StringFieldCriterion noEmptyBorrPairs = new StringFieldCriterion
            {
                FieldName = "FIELDS.CX.EMPTY.BORR.PAIR.DETECTED",
                Value = "Y",
                MatchType = StringFieldMatchType.Exact,
                Include = false
            };

            StringFieldCriterion validMailingAddress = new StringFieldCriterion
            {
                FieldName = "FIELDS.CX.VALID.MAILINGADDRESS",
                Value = "Y",
                MatchType = StringFieldMatchType.Exact,
            };

            StringFieldCriterion validCurrentAddress = new StringFieldCriterion
            {
                FieldName = "FIELDS.CX.VALID.CURRENTADDRESS",
                Value = "Y",
                MatchType = StringFieldMatchType.Exact,
            };


            //StringFieldCriterion testLoan1 = new StringFieldCriterion
            //{
            //    FieldName = "FIELDS.364",
            //    Value = "2272957",
            //    MatchType = StringFieldMatchType.Exact,
            //};

            //acanCri = testLoan1;

            //filters compiled
            acanCri = CancelRequested
                .And(notCancelCompleted)
                .And(botRunDateMetorExceeded)
                .And(denialDateOlderThan30Days)
                .And(testLoanIsNotY)
                .And(creditPlusInfoProvided)
                .And(cancellationReasonSelected)
                .And(loIsNotBlank)
                .And(noEmptyBorrPairs)
                .And(validCurrentAddress
                .Or(validMailingAddress));

            //sort criteria
            acanCriSortBy = new SortCriterionList();
            acanCriSortBy.Add(new SortCriterion("Fields.CX.CANREQ.NOABOTRUN.DATE", SortOrder.Descending));
        }

        private static void ACaNFields()
        {
            //fields to return (stringlist)
            acanLoans = new StringList();
            acanLoans.Add("Fields.364");
            acanLoans.Add("Fields.CX.CANREQ.REQ");
            acanLoans.Add("Fields.CX.GLOBAL.UDNDEACTIVATE");
            acanLoans.Add("Fields.CX.GLOBAL.UDNACTIVATE");
            acanLoans.Add("Fields.CX.CCS.BORROWER.CONDITIONS");
            acanLoans.Add("Fields.CX.CANREQ.NOABOTRUN.DATE");
        }

        private static void SetCursor()
        {
            acanLoanReport = session.Reports.OpenReportCursor(acanLoans, acanCri, acanCriSortBy);
        }

        private static void CreateReport()
        {
            acanLoansReportData = new List<LoanReportData>();

            for(int i = 0; i < numLoansToUpdate; i++)
            {
                acanLoansReportData.Add(acanLoanReport.GetItem(i));
            }
        }

        private static void SetLoanCount()
        {
            acanLoanCount = acanLoanReport.Count;

            if (acanLoanCount > _maxLoansToUpdate) numLoansToUpdate = _maxLoansToUpdate;
            else numLoansToUpdate = acanLoanCount;
        }

        private static void GetLoans()
        {
            acanLoanReport.GetItems(0, acanLoanCount);
        }

        public static void GetRemainingLoans()
        {
            remainingACaNLoans = session.Reports.OpenReportCursor(acanLoans, acanCri, acanCriSortBy);
            remainingLoanCount = remainingACaNLoans.Count;
        }
    }
}
