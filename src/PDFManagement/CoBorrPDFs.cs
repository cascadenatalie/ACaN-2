using System;
using System.IO;
using iTextSharp.text.pdf;


namespace ACaN2
{
    class CoBorrPDFs
    {
        static string coBorrSODSource = Resources.CoBorrowerSODSource;
        static string coBorrSODOutput = Resources.TempDestinationPath + Resources.CoBorrowerSODTemp;

        static string coBorrH5EqSource = Resources.CoBorrowerH5EquifaxSource;
        static string coBorrH5EqOutput = Resources.TempDestinationPath + Resources.CoBorrowerH5EquifaxTemp;

        static string coBorrH5ExpSource = Resources.CoBorrowerH5EExperianSource;
        static string coBorrH5ExpOutput = Resources.TempDestinationPath + Resources.CoBorrowerH5EExperianTemp;

        static string coBorrH5TUSource = Resources.CoBorrowerH5TransUnionSource;
        static string coBorrH5TUOutput = Resources.TempDestinationPath + Resources.CoBorrowerH5TransUnionTemp;

        private static string coBorrMailingAddress;

        public static void GenerateCoBorrTempPackage()
        {
            PrintCoBorrTempSOD();
            PrintCoBorrTempH5Eq();
            PrintCoBorrTempH5Ex();
            PrintCoBorrTempH5TU();
        }
        private static void PrintCoBorrTempSOD()
        {
            try
            {
                using (FileStream output = new FileStream(coBorrSODOutput, FileMode.Create))
                {
                    PdfReader pdfReader = new PdfReader(coBorrSODSource);
                    PdfStamper pdfStamper = new PdfStamper(pdfReader, output);
                    AcroFields sodFields = pdfStamper.AcroFields;

                    DetermineCoBorrMailingAddress();

                    #region Fields
                    #region Mailing Cover Letter
                    sodFields.SetField("4002", $"{ACaN.loan.Fields["4002"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("LoanNum364", $"{ACaN.loan.Fields["364"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("CoBorrName", $"{ACaN.loan.Fields["4004"].GetValueForBorrowerPair(ACaN.Pair)} {ACaN.loan.Fields["4006"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("CoBorrNameComma", $"{ACaN.loan.Fields["4004"].GetValueForBorrowerPair(ACaN.Pair)} {ACaN.loan.Fields["4006"].GetValueForBorrowerPair(ACaN.Pair)},");
                    sodFields.SetField("MailingAddress", $"{coBorrMailingAddress}");
                    sodFields.SetField("LoanTeamMember.Name.Loan Officer", $"{ACaN.loan.Fields["LoanTeamMember.Name.Loan Officer"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("LoanTeamMember.Phone.Loan Officer", $"{ACaN.loan.Fields["LoanTeamMember.Phone.Loan Officer"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("LoanTeamMember.Phone.Loan OfficerPeriod", $"{ACaN.loan.Fields["LoanTeamMember.Phone.Loan Officer"].GetValueForBorrowerPair(ACaN.Pair)}.");
                    sodFields.SetField("3238", $"{ACaN.loan.Fields["3238"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("MailedOnDate", $"{DateTime.Now.ToString("MM/dd/yyyy")}");
                    #endregion

                    #region Header
                    sodFields.SetField("CoBorrAddress", $"{ACaN.loan.Fields["DENIAL.X87"].GetValueForBorrowerPair(ACaN.Pair)}\n{ACaN.loan.Fields["DENIAL.X88"].GetValueForBorrowerPair(ACaN.Pair)}, {ACaN.loan.Fields["DENIAL.X89"].GetValueForBorrowerPair(ACaN.Pair)} {ACaN.loan.Fields["DENIAL.X90"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X69", $"{ACaN.loan.Fields["DENIAL.X69"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("LoanNumber", $"{ACaN.loan.Fields["364"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("LoanAmount", $"{ACaN.loan.Fields["2"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("InterestRate", $"{ACaN.loan.Fields["3"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("Term", $"{ACaN.loan.Fields["1347"].GetValueForBorrowerPair(ACaN.Pair)} years");
                    #endregion

                    #region Descriptions
                    sodFields.SetField("DENIAL.X71", $"{ACaN.loan.Fields["DENIAL.X71"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X72", $"{ACaN.loan.Fields["DENIAL.X72"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X73", $"{ACaN.loan.Fields["DENIAL.X73"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X74", $"{ACaN.loan.Fields["DENIAL.X74"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X75", $"{ACaN.loan.Fields["DENIAL.X75"].GetValueForBorrowerPair(ACaN.Pair)}");
                    #endregion

                    #region Credit
                    sodFields.SetField("DENIAL.X30", $"{ACaN.loan.Fields["DENIAL.X30"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X31", $"{ACaN.loan.Fields["DENIAL.X31"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X32", $"{ACaN.loan.Fields["DENIAL.X32"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X33", $"{ACaN.loan.Fields["DENIAL.X33"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X34", $"{ACaN.loan.Fields["DENIAL.X34"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X35", $"{ACaN.loan.Fields["DENIAL.X35"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X36", $"{ACaN.loan.Fields["DENIAL.X36"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X37", $"{ACaN.loan.Fields["DENIAL.X37"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X39", $"{ACaN.loan.Fields["DENIAL.X39"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X40", $"{ACaN.loan.Fields["DENIAL.X40"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X77", $"{ACaN.loan.Fields["DENIAL.X77"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X78", $"{ACaN.loan.Fields["DENIAL.X78"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X79", $"{ACaN.loan.Fields["DENIAL.X79"].GetValueForBorrowerPair(ACaN.Pair)}");
                    #endregion

                    #region Employment Status
                    sodFields.SetField("DENIAL.X42", $"{ACaN.loan.Fields["DENIAL.X42"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X43", $"{ACaN.loan.Fields["DENIAL.X43"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X44", $"{ACaN.loan.Fields["DENIAL.X44"].GetValueForBorrowerPair(ACaN.Pair)}");
                    #endregion

                    #region Income
                    sodFields.SetField("DENIAL.X45", $"{ACaN.loan.Fields["DENIAL.X45"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X46", $"{ACaN.loan.Fields["DENIAL.X46"].GetValueForBorrowerPair(ACaN.Pair)}");
                    #endregion

                    #region Residency
                    sodFields.SetField("DENIAL.X47", $"{ACaN.loan.Fields["DENIAL.X47"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X48", $"{ACaN.loan.Fields["DENIAL.X48"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X49", $"{ACaN.loan.Fields["DENIAL.X49"].GetValueForBorrowerPair(ACaN.Pair)}");
                    #endregion

                    #region Other
                    sodFields.SetField("DENIAL.X57", $"{ACaN.loan.Fields["DENIAL.X57"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X58", $"{ACaN.loan.Fields["DENIAL.X58"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X59", $"{ACaN.loan.Fields["DENIAL.X59"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X60", $"{ACaN.loan.Fields["DENIAL.X60"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X61", $"{ACaN.loan.Fields["DENIAL.X61"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X62", $"{ACaN.loan.Fields["DENIAL.X62"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X80", $"{ACaN.loan.Fields["DENIAL.X80"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X63", $"{ACaN.loan.Fields["DENIAL.X63"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X65", $"{ACaN.loan.Fields["DENIAL.X65"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X67", $"{ACaN.loan.Fields["DENIAL.X67"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X66", $"{ACaN.loan.Fields["DENIAL.X66"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DENIAL.X68", $"{ACaN.loan.Fields["DENIAL.X68"].GetValueForBorrowerPair(ACaN.Pair)}");
                    #endregion

                    #region Part II
                    sodFields.SetField("DENIAL.X12", $"{ACaN.loan.Fields["DENIAL.X12"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("CreditAgencyName", $"{ACaN.loan.Fields["624"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("CreditAgencyAddress", $"{ACaN.loan.Fields["626"].GetValueForBorrowerPair(ACaN.Pair)}, {ACaN.loan.Fields["627"].GetValueForBorrowerPair(ACaN.Pair)}, {ACaN.loan.Fields["1245"].GetValueForBorrowerPair(ACaN.Pair)} {ACaN.loan.Fields["628"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("CreditAgencyPhone", $"{ACaN.loan.Fields["629"].GetValueForBorrowerPair(ACaN.Pair)}");
                    #endregion

                    #region Experian
                    sodFields.SetField("DENIAL.X76", $"{ACaN.loan.Fields["DENIAL.X76"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("60", $"{ACaN.loan.Fields["60"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X12", $"{ACaN.loan.Fields["DISCLOSURE.X12"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X9", $"{ACaN.loan.Fields["DISCLOSURE.X9"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X10", $"{ACaN.loan.Fields["DISCLOSURE.X10"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X17", $"{ACaN.loan.Fields["DISCLOSURE.X17"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X18", $"{ACaN.loan.Fields["DISCLOSURE.X18"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X19", $"{ACaN.loan.Fields["DISCLOSURE.X19"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X20", $"{ACaN.loan.Fields["DISCLOSURE.X20"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X176", $"{ACaN.loan.Fields["DISCLOSURE.X176"].GetValueForBorrowerPair(ACaN.Pair)}");
                    #endregion

                    #region TransUnion
                    sodFields.SetField("1452", $"{ACaN.loan.Fields["1452"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X32", $"{ACaN.loan.Fields["DISCLOSURE.X32"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X29", $"{ACaN.loan.Fields["DISCLOSURE.X29"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X30", $"{ACaN.loan.Fields["DISCLOSURE.X30"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X37", $"{ACaN.loan.Fields["DISCLOSURE.X37"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X38", $"{ACaN.loan.Fields["DISCLOSURE.X38"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X39", $"{ACaN.loan.Fields["DISCLOSURE.X39"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X40", $"{ACaN.loan.Fields["DISCLOSURE.X40"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X177", $"{ACaN.loan.Fields["DISCLOSURE.X177"].GetValueForBorrowerPair(ACaN.Pair)}");
                    #endregion

                    #region Equifax
                    sodFields.SetField("1415", $"{ACaN.loan.Fields["1415"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X52", $"{ACaN.loan.Fields["DISCLOSURE.X52"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X49", $"{ACaN.loan.Fields["DISCLOSURE.X49"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X50", $"{ACaN.loan.Fields["DISCLOSURE.X50"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X57", $"{ACaN.loan.Fields["DISCLOSURE.X57"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X58", $"{ACaN.loan.Fields["DISCLOSURE.X58"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X59", $"{ACaN.loan.Fields["DISCLOSURE.X59"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X60", $"{ACaN.loan.Fields["DISCLOSURE.X60"].GetValueForBorrowerPair(ACaN.Pair)}");
                    sodFields.SetField("DISCLOSURE.X178", $"{ACaN.loan.Fields["DISCLOSURE.X178"].GetValueForBorrowerPair(ACaN.Pair)}");
                    #endregion

                    #region Lender and ECOA
                    sodFields.SetField("LenderContactInformation", $"{ACaN.loan.Fields["315"]}\n{ACaN.loan.Fields["319"]}, {ACaN.loan.Fields["313"]}, {ACaN.loan.Fields["321"]} {ACaN.loan.Fields["323"]}\n{ACaN.loan.Fields["324"]}");
                    sodFields.SetField("ECOANoticeFields", $"{ACaN.loan.Fields["ECOA_NAME"]}\n{ACaN.loan.Fields["ECOA_ADDR"]}, {ACaN.loan.Fields["ECOA_ADDR2"]}, {ACaN.loan.Fields["ECOA_CITY"]}, {ACaN.loan.Fields["ECOA_ST"]} {ACaN.loan.Fields["ECOA_ZIP"]}\n{ACaN.loan.Fields["ECOA_PHONE"]}");
                    sodFields.SetField("DENIAL.X11", $"{ACaN.loan.Fields["DENIAL.X11"]}");
                    #endregion
                    #endregion

                    pdfStamper.FormFlattening = true;
                    pdfStamper.Close();
                    pdfReader.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{ACaN.loan.LoanNumber}, Failed while creating CoBorrTempSOD, {e.Message}\n{e.InnerException}");
                Console.ReadLine();
                Environment.Exit(99);
            }
        }
        private static void PrintCoBorrTempH5Eq()
        {
            try
            {
                using (var output = new FileStream(coBorrH5EqOutput, FileMode.Create))
                {
                    var pdfReader = new iTextSharp.text.pdf.PdfReader(coBorrH5EqSource);
                    var pdfStamper = new PdfStamper(pdfReader, output);
                    var h5EqFields = pdfStamper.AcroFields;

                    h5EqFields.SetField("CoBorrName", $"{ACaN.loan.Fields["4004"].GetValueForBorrowerPair(ACaN.Pair)} {ACaN.loan.Fields["4006"].GetValueForBorrowerPair(ACaN.Pair)}");
                    h5EqFields.SetField("MailedOnDate", $"{DateTime.Now.ToString("MM/dd/yyyy")}");
                    h5EqFields.SetField("LoanNum364", $"{ACaN.loan.Fields["364"].GetValueForBorrowerPair(ACaN.Pair)}");

                    pdfStamper.FormFlattening = true;
                    pdfStamper.Close();
                    pdfReader.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{ACaN.loan.LoanNumber}, Failed while creating CoBorrTempEquifax, {e.Message}\n{e.InnerException}");
                Console.ReadLine();
                Environment.Exit(99);
            }
        }
        private static void PrintCoBorrTempH5Ex()
        {
            try
            {
                using (var output = new FileStream(coBorrH5ExpOutput, FileMode.Create))
                {
                    var pdfReader = new iTextSharp.text.pdf.PdfReader(coBorrH5ExpSource);
                    var pdfStamper = new PdfStamper(pdfReader, output);
                    var h5ExFields = pdfStamper.AcroFields;

                    h5ExFields.SetField("CoBorrName", $"{ACaN.loan.Fields["4004"].GetValueForBorrowerPair(ACaN.Pair)} {ACaN.loan.Fields["4006"].GetValueForBorrowerPair(ACaN.Pair)}");
                    h5ExFields.SetField("MailedOnDate", $"{DateTime.Now.ToString("MM/dd/yyyy")}");
                    h5ExFields.SetField("LoanNum364", $"{ACaN.loan.Fields["364"].GetValueForBorrowerPair(ACaN.Pair)}");

                    pdfStamper.FormFlattening = true;
                    pdfStamper.Close();
                    pdfReader.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{ACaN.loan.LoanNumber}, Failed while creating CoBorrTempExperian, {e.Message}\n{e.InnerException}");
                Console.ReadLine();
                Environment.Exit(99);
            }
        }
        private static void PrintCoBorrTempH5TU()
        {
            try
            {
                using (var output = new FileStream(coBorrH5TUOutput, FileMode.Create))
                {
                    var pdfReader = new iTextSharp.text.pdf.PdfReader(coBorrH5TUSource);
                    var pdfStamper = new PdfStamper(pdfReader, output);
                    var h5TUFields = pdfStamper.AcroFields;

                    h5TUFields.SetField("CoBorrName", $"{ACaN.loan.Fields["4004"].GetValueForBorrowerPair(ACaN.Pair)} {ACaN.loan.Fields["4006"].GetValueForBorrowerPair(ACaN.Pair)}");
                    h5TUFields.SetField("MailedOnDate", $"{DateTime.Now.ToString("MM/dd/yyyy")}");
                    h5TUFields.SetField("LoanNum364", $"{ACaN.loan.Fields["364"].GetValueForBorrowerPair(ACaN.Pair)}");

                    pdfStamper.FormFlattening = true;
                    pdfStamper.Close();
                    pdfReader.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{ACaN.loan.LoanNumber}, Failed while creating CoBorrTempTransUnion, {e.Message}\n{e.InnerException}");
                Console.ReadLine();
                Environment.Exit(99);
            }
        }
        private static bool CoBorrMailingAddress()
        {
            return (!ACaN.loan.Fields["URLA.X198"].GetValueForBorrowerPair(ACaN.Pair).Equals(""));
        }
        private static void DetermineCoBorrMailingAddress()
        {
            if (CoBorrMailingAddress())
            {
                coBorrMailingAddress = $"{ACaN.loan.Fields["URLA.X198"].GetValueForBorrowerPair(ACaN.Pair)} {ACaN.loan.Fields["URLA.X10"].GetValueForBorrowerPair(ACaN.Pair)}\n{ACaN.loan.Fields["1520"].GetValueForBorrowerPair(ACaN.Pair)}, {ACaN.loan.Fields["1521"].GetValueForBorrowerPair(ACaN.Pair)} {ACaN.loan.Fields["1522"].GetValueForBorrowerPair(ACaN.Pair)}";
            }
            else
            {
                coBorrMailingAddress = $"{ACaN.loan.Fields["FR0226"].GetValueForBorrowerPair(ACaN.Pair)} {ACaN.loan.Fields["FR0227"].GetValueForBorrowerPair(ACaN.Pair)}\n{ACaN.loan.Fields["FR0206"].GetValueForBorrowerPair(ACaN.Pair)}, {ACaN.loan.Fields["FR0207"].GetValueForBorrowerPair(ACaN.Pair)} {ACaN.loan.Fields["FR0208"].GetValueForBorrowerPair(ACaN.Pair)}";
            }
        }
    }
}
