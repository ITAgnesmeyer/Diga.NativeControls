using System;

namespace Diga.NativeControls.WebBrowser.Scripting.DOM
{
    public class DOMMouseEventArgs : EventArgs
    {
        public DOMMouseEvent Event { get; }

        public DOMMouseEventArgs(DOMMouseEvent ev)
        {
            this.Event = ev;
        }
    }
}