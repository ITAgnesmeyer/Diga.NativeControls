using System;
using System.Drawing;

namespace Diga.NativeControls.WebBrowser
{


    public class WebViewButtonDownEventArgs : EventArgs
    {
        public Point Location { get; }

        public WebViewButtonDownEventArgs(Point location)
        {
            this.Location = location;
        }
    }
}