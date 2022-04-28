using System.Configuration;
using System;


namespace ACaN2
{
    class Resources
    {
        public static int maxLoansToUpdate = int.Parse(ConfigurationManager.AppSettings["MaxLoansToUpdate"]);
        public static string acanReport = ConfigurationManager.AppSettings["ACaNReport"];
        public static string acanReportSend = ConfigurationManager.AppSettings["PostgresInFile"] + DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + ".csv";
        public static string acanLog = ConfigurationManager.AppSettings["ACaNLog"];

        //Borrower
        public static string BorrowerSODSource = ConfigurationManager.AppSettings["BorrowerSODSource"];
        public static string BorrowerH5EquifaxSource = ConfigurationManager.AppSettings["BorrowerH5EquifaxSource"];
        public static string BorrowerH5EExperianSource = ConfigurationManager.AppSettings["BorrowerH5EExperianSource"];
        public static string BorrowerH5TransUnionSource = ConfigurationManager.AppSettings["BorrowerH5TransUnionSource"];

        //CoBorrower
        public static string CoBorrowerSODSource = ConfigurationManager.AppSettings["CoBorrowerSODSourceSource"];
        public static string CoBorrowerH5EquifaxSource = ConfigurationManager.AppSettings["CoBorrowerH5EquifaxSource"];
        public static string CoBorrowerH5EExperianSource = ConfigurationManager.AppSettings["CoBorrowerH5EExperianSource"];
        public static string CoBorrowerH5TransUnionSource = ConfigurationManager.AppSettings["CoBorrowerH5TransUnionSource"];

        //Temp PDF Names
        public static string BorrowerSODTemp = ConfigurationManager.AppSettings["BorrowerSODTemp"];
        public static string BorrowerH5EquifaxTemp = ConfigurationManager.AppSettings["BorrowerH5EquifaxTemp"];
        public static string BorrowerH5EExperianTemp = ConfigurationManager.AppSettings["BorrowerH5EExperianTemp"];
        public static string BorrowerH5TransUnionTemp = ConfigurationManager.AppSettings["BorrowerH5TransUnionTemp"];

        public static string CoBorrowerSODTemp = ConfigurationManager.AppSettings["CoBorrowerSODTemp"];
        public static string CoBorrowerH5EquifaxTemp = ConfigurationManager.AppSettings["CoBorrowerH5EquifaxTemp"];
        public static string CoBorrowerH5EExperianTemp = ConfigurationManager.AppSettings["CoBorrowerH5EExperianTemp"];
        public static string CoBorrowerH5TransUnionTemp = ConfigurationManager.AppSettings["CoBorrowerH5TransUnionTemp"];

        //Temp Folder Location
        public static string TempDestinationPath = ConfigurationManager.AppSettings["TempDestinationPath"];

        //Final Folder Location
        public static string FinalDestinationPath = ConfigurationManager.AppSettings["FinalDestinationPath"];

        //iText Key Location
        public static string iTextKeyPath = @".\Keys\iTextKey.json";
    }
}
