using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace ARKNotifierService
{
    static class Program
    {
        //Rename to Main when deploying as a service.
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ServiceCore()
            };
            ServiceBase.Run(ServicesToRun);
        }

        [STAThread]
        //Rename to Main when debugging.
        static void Main2()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DebugForm());
        }

    }
}
