using System;
using EllieMae.Encompass.Reporting;
using EllieMae.Encompass.BusinessObjects.Loans;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;

namespace ACaN2
{
    class LoanMaintenance
    {
        public static void LoanCleanUp()
        {
            CheckForPriorLoanSave();
            CheckForNegativeLoanAmount();
            CheckForStatementOfDenialReason();
            ACaN.loan.Fields["CX.GLOBAL.LASTMODIFIED"].Value = DateTime.Now.ToString();
        }
        private static void CheckForPriorLoanSave()
        {
            if (ACaN.loan.Fields["CX.GLOBAL.LASTMODIFIED"].Value.Equals(""))
            {
                ACaN.loan.Fields["11"].Value = "TBD";
            }
        }
        private static void CheckForNegativeLoanAmount()
        {
            string _loanAmount = ACaN.loan.Fields["1109"].FormattedValue;
            if (_loanAmount.Contains("-"))
            {
                ACaN.loan.Fields["1335"].Value = "";
                ACaN.loan.Fields["1109"].Value = "";
            }
        }
        private static void CheckForStatementOfDenialReason()
        {
            if (!StatementOfDenialReasonSelected())
            {
                //Credit
                if (ACaN.loan.Fields["CX.CANREQ.CREDIT.INELIGIBLE"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X39"].Value = "Y";
                }
                if (ACaN.loan.Fields["CX.CANREQ.NOCREDIT"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X30"].Value = "Y";
                }
                if (ACaN.loan.Fields["CX.CANREQ.INSUFFICIENTCREDIT"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X32"].Value = "Y";
                }
                if (ACaN.loan.Fields["CX.CANREQ.INSUFFICIENTRADE"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X31"].Value = "Y";
                }
                if (ACaN.loan.Fields["CX.CANREQ.BANKRUPTCY"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X40"].Value = "Y";
                }
                if (ACaN.loan.Fields["CX.CANREQ.FORECLOSURE"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X34"].Value = "Y";
                }
                if (ACaN.loan.Fields["CX.CANREQ.JUDGEMENT"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X34"].Value = "Y";
                }
                if (ACaN.loan.Fields["CX.CANREQ.COLLECTION"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X39"].Value = "Y";
                }
                if (ACaN.loan.Fields["CX.CANREQ.REVOLVINGHISTORY"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X39"].Value = "Y";
                }
                if (ACaN.loan.Fields["CX.CANREQ.MORTGAGEHISTORY"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X37"].Value = "Y";
                }
                //Collateral
                if (ACaN.loan.Fields["CX.CANREQ.APPRAISALSHORTAGE"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X58"].Value = "Y";
                    ACaN.loan.Fields["DENIAL.X80"].Value = "Y";
                }
                if (ACaN.loan.Fields["CX.CANREQ.DEFICIENTAPPRAISAL"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X58"].Value = "Y";
                    ACaN.loan.Fields["DENIAL.X61"].Value = "Y";
                }
                if (ACaN.loan.Fields["CX.CANREQ.NONPRIMARYRES"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X63"].Value = "Y";
                    ACaN.loan.Fields["DENIAL.X65"].Value = "Y";
                    ACaN.loan.Fields["DENIAL.X66"].Value = "the terms and conditions you have requested";
                }
                if (ACaN.loan.Fields["CX.CANREQ.USEDHOME"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X63"].Value = "Y";
                    ACaN.loan.Fields["DENIAL.X65"].Value = "Y";
                    ACaN.loan.Fields["DENIAL.X66"].Value = "the terms and conditions you have requested";
                }
                if (ACaN.loan.Fields["CX.CANREQ.TITLECOMMIT"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X58"].Value = "Y";
                    ACaN.loan.Fields["DENIAL.X59"].Value = "Y";
                }
                //Employment
                if (ACaN.loan.Fields["CX.CANREQ.EMPLOYMENTLENGTH"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X43"].Value = "Y";
                }
                if (ACaN.loan.Fields["CX.CANREQ.LOSTJOB"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X42"].Value = "Y";
                }
                if (ACaN.loan.Fields["CX.CANREQ.IRREGULAREMPLOY"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X44"].Value = "Y";
                }
                //Income/Down Payment
                if (ACaN.loan.Fields["CX.CANREQ.DTI.RATIO"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X45"].Value = "Y";
                }
                if (ACaN.loan.Fields["CX.CANREQ.INSUFFICIENTASSETS"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X65"].Value = "Y";
                    ACaN.loan.Fields["DENIAL.X66"].Value.Equals("Insufficient Funds to Close the Loan");
                }
                if (ACaN.loan.Fields["CX.CANREQ.RESIDUALINCOME.1"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X45"].Value = "Y";
                }
                //Other
                if (ACaN.loan.Fields["CX.CANREQ.INCONSISTENCY"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X65"].Value = "Y";
                    ACaN.loan.Fields["DENIAL.X66"].Value.Equals("Inconsistencies in loan file");
                }
                if (ACaN.loan.Fields["CX.CANREQ.COLLATERAL"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X58"].Value = "Y";
                }
                if (ACaN.loan.Fields["CX.CANREQ.STATEORPROD"].Value.Equals("Y"))
                {
                    ACaN.loan.Fields["DENIAL.X63"].Value = "Y";
                    ACaN.loan.Fields["DENIAL.X65"].Value = "Y";
                    ACaN.loan.Fields["DENIAL.X66"].Value = "the terms and conditions you have requested";
                }
            }
        }
        private static bool StatementOfDenialReasonSelected()
        {
            return (ACaN.loan.Fields["DENIAL.X30"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X31"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X32"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X33"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X34"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X35"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X36"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X37"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X39"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X40"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X77"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X78"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X79"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X42"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X43"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X44"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X45"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X46"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X47"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X48"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X49"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X57"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X58"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X59"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X60"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X61"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X62"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X80"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X63"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X65"].Value.Equals("Y") ||
                ACaN.loan.Fields["DENIAL.X67"].Value.Equals("Y"));
        }
        public static bool CreditInfoMissing()
        {
            return (!ACaN.loan.Fields["67"].Value.Equals("") &&
                ACaN.loan.Fields["DISCLOSURE.X13"].Value.Equals("")) ||

                (!ACaN.loan.Fields["60"].Value.Equals("") &&
                ACaN.loan.Fields["DISCLOSURE.X17"].Value.Equals("")) ||

                (!ACaN.loan.Fields["1450"].Value.Equals("") &&
                ACaN.loan.Fields["DISCLOSURE.X33"].Value.Equals("")) ||

                (!ACaN.loan.Fields["1452"].Value.Equals("") &&
                ACaN.loan.Fields["DISCLOSURE.X37"].Value.Equals("")) ||

                (!ACaN.loan.Fields["1414"].Value.Equals("") &&
                ACaN.loan.Fields["DISCLOSURE.X53"].Value.Equals("")) ||

                (!ACaN.loan.Fields["1415"].Value.Equals("") &&
                ACaN.loan.Fields["DISCLOSURE.X57"].Value.Equals(""));
        }
        public static bool ValidAddress()
        {
            return (ACaN.loan.Fields["CX.VALID.MAILINGADDRESS"].Value.Equals("Y") ||
                ACaN.loan.Fields["CX.VALID.CURRENTADDRESS"].Value.Equals("Y"));
        }
        public static bool NoEmptyBorrowerPairs()
        {
            return (!ACaN.loan.Fields["CX.EMPTY.BORR.PAIR.DETECTED"].Value.Equals("Y"));
        }
        public static bool LOisNotBlank()
        {
            return (!ACaN.loan.Fields["317"].Value.Equals(""));
        }
        public static bool LOisValid()
        {
            return (ACaN.loan.Fields["317"].FormattedValue.Contains("McManus") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Krebs") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Nelles") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Denton") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Stakes") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("O'Connor") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Mauch") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Cummard") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("McKamy") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Brudnicki") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Sager") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Beanblossom") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Thompson") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Rabon") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("King") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Shaw") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Pomeroy") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Larivee") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Gould") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Jepsen") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Geiger") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Jensen") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Vigil") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Williams") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Govea") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Abrams") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Melzer") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Janis") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Nelson") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Almarji") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Barrineau") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Gustafson") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Salinas") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Gomez") ||
                ACaN.loan.Fields["317"].FormattedValue.Contains("Torres"));
        }
    }
}
