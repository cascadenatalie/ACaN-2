using System;
using EllieMae.Encompass.BusinessObjects.Loans.Logging;


namespace ACaN2
{
    class GenerateNOAs
    {
        private static string _borrDocName = null;
        private static string _coBorrDocName = null;

        public static void GenerateNOAPackages()
        {
            BorrowerNOAPackage();
            CoBorrowerNOAPackage();
        }
        private static void BorrowerNOAPackage()
        {
            BorrPDFs.GenerateBorrTempPackage();
            Merge.MergeBorrTempDocuments();
            BorrowerNOAPackageToEFolder();
        }
        private static void CoBorrowerNOAPackage()
        {
            if (CoBorr())
            {
                CoBorrPDFs.GenerateCoBorrTempPackage();
                Merge.MergeCoBorrTempDocuments();
                CoBorrowerNOAPackageToEFolder();
            }
        }
        private static bool CoBorr()
        {
            return (!ACaN.loan.Fields["4004"].GetValueForBorrowerPair(ACaN.Pair).Equals(""));
        }
        private static string SetBorrDocumentName()
        {
            _borrDocName = $"{ACaN.loan.Fields["4000"].GetValueForBorrowerPair(ACaN.Pair)} {ACaN.loan.Fields["4002"].GetValueForBorrowerPair(ACaN.Pair)} - NOA Package";
            return _borrDocName;
        }
        private static void BorrowerNOAPackageToEFolder()
        {
            Console.WriteLine("Uploading Borr Package to eFolder...");
            try
            {
                SetBorrDocumentName();
                var statementOfDenialContainer = ACaN.loan.Log.TrackedDocuments.GetDocumentsByTitle("Statement of Denial");
                var attachment = ACaN.loan.Attachments.Add(Merge.BorrFileName);
                var document = statementOfDenialContainer.Count >= 1
                    ? (TrackedDocument)statementOfDenialContainer[0]
                    : ACaN.loan.Log.TrackedDocuments.Add("Statement of Denial", ACaN.loan.Log.MilestoneEvents.NextEvent.MilestoneName);
                attachment.Title = _borrDocName;
                document.Attach(attachment);
                Console.WriteLine("Upload to eFolder Done");
            }
            catch (Exception e)
            {
                Console.WriteLine($"{ACaN.loan.LoanNumber}, Failed while uploading Borr NOA Package to eFolder, {e.Message}\n{e.InnerException}");
                Console.ReadLine();
                Environment.Exit(99);
            }
            
        }

        private static string SetCoBorrDocumentName()
        {
            _coBorrDocName = $"{ACaN.loan.Fields["4004"].GetValueForBorrowerPair(ACaN.Pair)} {ACaN.loan.Fields["4006"].GetValueForBorrowerPair(ACaN.Pair)} - NOA Package";
            return _coBorrDocName;
        }
        private static void CoBorrowerNOAPackageToEFolder()
        {
            try
            {
                SetCoBorrDocumentName();
                var statementOfDenialContainer = ACaN.loan.Log.TrackedDocuments.GetDocumentsByTitle("Statement of Denial");
                var attachment = ACaN.loan.Attachments.Add(Merge.CoBorrFileName);
                var document = statementOfDenialContainer.Count >= 1
                    ? (TrackedDocument)statementOfDenialContainer[0]
                    : ACaN.loan.Log.TrackedDocuments.Add("Statement of Denial", ACaN.loan.Log.MilestoneEvents.NextEvent.MilestoneName);
                attachment.Title = _coBorrDocName;
                document.Attach(attachment);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{ACaN.loan.LoanNumber}, Failed while uploading CoBorr NOA Package to eFolder, {e.Message}\n{e.InnerException}");
                Console.ReadLine();
                Environment.Exit(99);
            }
        }
    }
}
