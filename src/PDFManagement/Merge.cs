using System;
using iText.Kernel.Utils;
using iText.Kernel.Pdf;
using System.IO;

namespace ACaN2
{
    class Merge
    {
        static string borrSODOutput = Resources.TempDestinationPath + Resources.BorrowerSODTemp;
        static string borrH5EqOutput = Resources.TempDestinationPath + Resources.BorrowerH5EquifaxTemp;
        static string borrH5ExpOutput = Resources.TempDestinationPath + Resources.BorrowerH5EExperianTemp;
        static string borrH5TUOutput = Resources.TempDestinationPath + Resources.BorrowerH5TransUnionTemp;
        static string coBorrSODOutput = Resources.TempDestinationPath + Resources.CoBorrowerSODTemp;
        static string coBorrH5EqOutput = Resources.TempDestinationPath + Resources.CoBorrowerH5EquifaxTemp;
        static string coBorrH5ExpOutput = Resources.TempDestinationPath + Resources.CoBorrowerH5EExperianTemp;
        static string coBorrH5TUOutput = Resources.TempDestinationPath + Resources.CoBorrowerH5TransUnionTemp;

        static string path = Resources.FinalDestinationPath;

        public static string BorrFileName = null;
        public static string CoBorrFileName = null;

        //Borrower
        public static void MergeBorrTempDocuments()
        {
            try
            {
                PdfDocument pdf = null;
                PdfMerger merger = null;
                PdfDocument sodSource = null;
                PdfDocument h5EqSource = null;
                PdfDocument h5ExpSource = null;
                PdfDocument h5TUSource = null;

                SetBorrFileName();

                //Create Merge Document
                pdf = new PdfDocument(new PdfWriter(BorrFileName));
                merger = new PdfMerger(pdf);

                //add statement of denial
                sodSource = new PdfDocument(new PdfReader(borrSODOutput));
                merger.Merge(sodSource, 1, sodSource.GetNumberOfPages());

                //add h5 Equifax
                h5EqSource = new PdfDocument(new PdfReader(borrH5EqOutput));
                if (BorrEquifaxScoreIsNull())
                {
                    merger.Merge(h5EqSource, 1, h5EqSource.GetNumberOfPages());
                }

                //add h5 Experian
                h5ExpSource = new PdfDocument(new PdfReader(borrH5ExpOutput));
                if (BorrExperianScoreIsNull())
                {
                    merger.Merge(h5ExpSource, 1, h5ExpSource.GetNumberOfPages());
                }

                //add h5 TransUnion
                h5TUSource = new PdfDocument(new PdfReader(borrH5TUOutput));
                if (BorrTransUnionScoreIsNull())
                {
                    merger.Merge(h5TUSource, 1, h5TUSource.GetNumberOfPages());
                }

                sodSource.Close();
                h5EqSource.Close();
                h5ExpSource.Close();
                h5TUSource.Close();
                pdf.Close();

                File.Delete(borrSODOutput);
                File.Delete(borrH5EqOutput);
                File.Delete(borrH5ExpOutput);
                File.Delete(borrH5TUOutput);
            }
            catch (Exception b)
            {
                Console.WriteLine($"{ACaN.loan.LoanNumber}, Failed while merging Borr PDFs, {b.Message}\n{b.InnerException}");
                Console.ReadLine();
                Environment.Exit(99);
            }
        }
        private static string SetBorrFileName()
        {
            BorrFileName = $"{path}{ACaN.loan.Fields["364"]}-{ACaN.loan.Fields["4000"].GetValueForBorrowerPair(ACaN.Pair).ToString().Trim()}-{ACaN.loan.Fields["4002"].GetValueForBorrowerPair(ACaN.Pair).ToString().Trim()}-NOA Disclosures.pdf";
            return BorrFileName;
        }
        private static bool BorrExperianScoreIsNull()
        {
            return (ACaN.loan.Fields["67"].GetValueForBorrowerPair(ACaN.Pair).Equals(""));
        }
        private static bool BorrEquifaxScoreIsNull()
        {
            return (ACaN.loan.Fields["1414"].GetValueForBorrowerPair(ACaN.Pair).Equals(""));
        }
        private static bool BorrTransUnionScoreIsNull()
        {
            return (ACaN.loan.Fields["1450"].GetValueForBorrowerPair(ACaN.Pair).Equals(""));
        }

        //CoBorrower
        public static void MergeCoBorrTempDocuments()
        {
            try
            {
                PdfDocument pdf = null;
                PdfMerger merger = null;
                PdfDocument sodSource = null;
                PdfDocument h5EqSource = null;
                PdfDocument h5ExpSource = null;
                PdfDocument h5TUSource = null;

                //string coBorrFileName = SetCoBorrFileName();
                SetCoBorrFileName();

                //Create Merge Document
                pdf = new PdfDocument(new PdfWriter(CoBorrFileName));
                merger = new PdfMerger(pdf);

                //add statement of denial
                sodSource = new PdfDocument(new PdfReader(coBorrSODOutput));
                merger.Merge(sodSource, 1, sodSource.GetNumberOfPages());

                //add h5 Equifax
                h5EqSource = new PdfDocument(new PdfReader(coBorrH5EqOutput));
                if (CoBorrEquifaxScoreIsNull())
                {
                    merger.Merge(h5EqSource, 1, h5EqSource.GetNumberOfPages());
                }

                //add h5 Experian
                h5ExpSource = new PdfDocument(new PdfReader(coBorrH5ExpOutput));
                if (CoBorrExperianScoreIsNull())
                {
                    merger.Merge(h5ExpSource, 1, h5ExpSource.GetNumberOfPages());
                }

                //add h5 TransUnion
                h5TUSource = new PdfDocument(new PdfReader(coBorrH5TUOutput));
                if (CoBorrTransUnionScoreIsNull())
                {
                    merger.Merge(h5TUSource, 1, h5TUSource.GetNumberOfPages());
                }

                sodSource.Close();
                h5EqSource.Close();
                h5ExpSource.Close();
                h5TUSource.Close();
                pdf.Close();

                File.Delete(coBorrSODOutput);
                File.Delete(coBorrH5EqOutput);
                File.Delete(coBorrH5ExpOutput);
                File.Delete(coBorrH5TUOutput);
            }
            catch (Exception c)
            {
                Console.WriteLine($"{ACaN.loan.LoanNumber}, Failed while merging CoBorr PDFs, {c.Message}\n{c.InnerException}");
                Console.ReadLine();
                Environment.Exit(99);
            }
        }
        private static string SetCoBorrFileName()
        {
            CoBorrFileName = $"{path}{ACaN.loan.Fields["364"]}-{ACaN.loan.Fields["4004"].GetValueForBorrowerPair(ACaN.Pair).ToString().Trim()}-{ACaN.loan.Fields["4006"].GetValueForBorrowerPair(ACaN.Pair).ToString().Trim()}-NOA Disclosures.pdf";
            return CoBorrFileName;
        }
        private static bool CoBorrExperianScoreIsNull()
        {
            return (ACaN.loan.Fields["60"].GetValueForBorrowerPair(ACaN.Pair).Equals(""));
        }
        private static bool CoBorrEquifaxScoreIsNull()
        {
            return (ACaN.loan.Fields["1415"].GetValueForBorrowerPair(ACaN.Pair).Equals(""));
        }
        private static bool CoBorrTransUnionScoreIsNull()
        {
            return (ACaN.loan.Fields["1452"].GetValueForBorrowerPair(ACaN.Pair).Equals(""));
        }
    }
}
