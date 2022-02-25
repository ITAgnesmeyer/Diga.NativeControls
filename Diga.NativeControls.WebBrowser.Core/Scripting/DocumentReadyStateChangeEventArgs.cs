using System;
using Diga.NativeControls.WebBrowser.Scripting.DOM;

namespace Diga.NativeControls.WebBrowser.Scripting
{
    public class DocumentReadyStateChangeEventArgs : EventArgs
    {
        public DOMDocument Document { get; }
        public string State { get; }

        public DocumentReadyStateChangeEventArgs(DOMDocument doc, string state)
        {
            this.Document = doc;
            this.State = state;
        }
    }
}