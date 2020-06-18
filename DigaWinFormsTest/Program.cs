using CoreWindowsWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace DigaWinFormsTest
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            BrowserWindow bw = new BrowserWindow();
            NativeApp.Run(bw);
        }
    }
}
