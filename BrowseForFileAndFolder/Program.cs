using System;
using CoreWindowsWrapper;

namespace BrowseForFileAndFolder
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {

            MainWindow mw = new MainWindow();
            NativeApp.Run(mw);

        }
    }
}
