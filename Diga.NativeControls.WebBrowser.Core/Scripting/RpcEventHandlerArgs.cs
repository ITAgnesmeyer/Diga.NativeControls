﻿using System;

namespace Diga.NativeControls.WebBrowser.Scripting
{
    public class RpcEventHandlerArgs : EventArgs
    {
        public IRpcObject RpcObject { get; }
        public string EventName { get; }
        public string ObjectId { get; }

        public RpcEventHandlerArgs(string id, string eventName, IRpcObject rpc)
        {
            this.ObjectId = id;
            this.EventName=eventName;
            this.RpcObject = rpc;
        }
    }
}