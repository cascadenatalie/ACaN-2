using iText.Licensing.Base;
using System.IO;

namespace ACaN2
{
    class Program
    {
        static void Main(string[] args)
        {
            LicenseKey.LoadLicenseFile(new FileInfo(Resources.iTextKeyPath));

            new EllieMae.Encompass.Runtime.RuntimeServices().Initialize();
            var encompassSession = new EncompassSession();
            var acan = new ACaN();

            encompassSession.StartApplication();
            
            acan.RunACaN();

            encompassSession.CloseApplication();
        }
    }
}
