

namespace ACaN2
{
    class Program
    {
        static void Main(string[] args)
        {
            new EllieMae.Encompass.Runtime.RuntimeServices().Initialize();
            var encompassSession = new EncompassSession();
            var acan = new ACaN();

            encompassSession.StartApplication();
            
            acan.RunACaN();

            encompassSession.CloseApplication();
        }
    }
}
