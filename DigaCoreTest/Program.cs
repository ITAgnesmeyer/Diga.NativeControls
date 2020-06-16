using ConsoleCaller;
using CoreWindowsWrapper;
using System;

namespace DigaCore
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            BrowserWindow browserWindow = new BrowserWindow();
            NativeApp.Run(browserWindow);
        }
    }
}
