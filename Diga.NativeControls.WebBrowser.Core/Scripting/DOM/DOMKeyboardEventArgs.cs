using System;

namespace Diga.NativeControls.WebBrowser.Scripting.DOM
{
    public class DOMKeyboardEventArgs : EventArgs
    {
        public DOMKeyboardEvent Event { get; }

        public DOMKeyboardEventArgs(DOMKeyboardEvent ev)
        {
            this.Event = ev;
        }
    }
}