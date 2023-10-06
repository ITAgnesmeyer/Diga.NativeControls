using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using CoreWindowsWrapper;
using Diga.Core.Api.Win32;
using Diga.Core.Threading;
using Diga.WebView2.Monitoring;
using Diga.WebView2.Scripting;
using Diga.WebView2.Wrapper;
using Diga.WebView2.Wrapper.EventArguments;

namespace Diga.NativeControls.WebBrowser
{



    public partial class NativeWebBrowser : NativeControlBase
    {
        //private readonly RpcHandler _RpcHandler;
        //private const string JAVASCRIPT_CANNOT_BE_NULL_OR_EMPTY = "javaScript cannot be NULL or empty";
        private NativeWindow _ParentNativeWindow = null;
        private WebView2Control _WebViewControl;
        private static int ControlCounter = 0;

        protected override void Initialize()
        {
            this._RpcHandler = new RpcHandler();
            this._RpcHandler.RpcEvent += OnRpcEventIntern;
            this._RpcHandler.RpcDomUnloadEvent += OnRpcDomUnloadEvent;
            this._MonitoringActionList = new MonitoringActionList();
            base.Initialize();
            this.ControlType = CoreWindowsWrapper.Win32ApiForm.ControlType.Label;
            this.TypeIdentifier = "static";
            this.Style = WindowStylesConst.WS_VISIBLE | WindowStylesConst.WS_CHILD | StaticControlStyles.SS_NOTIFY;
            this.BackColor = CoreWindowsWrapper.Tools.ColorTool.White;
            this.ForeColor = CoreWindowsWrapper.Tools.ColorTool.Black;
        }
        private int HiWord(int number)
        {
            if ((number & 0x80000000) == 0x80000000)
                return (number >> 16);
            return (number >> 16) & 0xffff;
        }

        private int LoWord(int number)
        {
            return number & 0xffff;
        }
        protected override bool ControlProc(IntPtr hWndParent, IntPtr hWndControl, int controlId, uint command, IntPtr wParam, IntPtr lParam)
        {
            if (command == 0x210)
            {
                if (wParam.ToInt32() == 0x204)
                {
                    int lp = lParam.ToInt32();
                    int x = HiWord(lp);
                    int y = LoWord(lp);
                    OnMouseButtonDown(new WebViewButtonDownEventArgs(new System.Drawing.Point(x,y)));
                }
            }

            return base.ControlProc(hWndParent, hWndControl, controlId, command, wParam, lParam);
        }

        private void CreateWebViewControl(IntPtr parent)
        {
            try
            {
                if (UIDispatcher.FilnalDisposed)
                {
                    UIDispatcher.FilnalDisposed = false;
                    
//#if !NETCOREAPP3_1_OR_GREATER
                    //AppDomain.CurrentDomain.ProcessExit -= BeforeProcessExitCatch;
//#endif
                }
                    

                this._WebViewControl = new WebView2Control(parent,this.BrowserExecutableFolder, this.BrowserUserDataFolder, this.BrowserAdditionArgs);
                WireEvents(this._WebViewControl);
                ControlCounter++;
            }
            catch (Exception ex)
            {
                Debug.Print(nameof(CreateWebViewControl) + " Exception:" + ex);
            }
        }
        public override bool Create(IntPtr parentId)
        {
            bool created = base.Create(parentId);
            if (this.Handle != IntPtr.Zero)
            {
                CreateWebViewControl(this.Handle);

            }
            else
            {
                throw new Exception("Cannot create WebView");
            }

            return created;

        }

        public override void Destroy()
        {
            OnBeforeWebViewDestroy();
            base.Destroy();
        }

        private void WebWindowInitSettings(BeforeCreateEventArgs e)
        {
            e.Settings.AreBrowserAcceleratorKeysEnabled = this._AreBrowserAcceleratorKeysEnabled;
            e.Settings.AreDefaultContextMenusEnabled = this._DefaultContextMenusEnabled;
            e.Settings.AreDefaultScriptDialogsEnabled = this._DefaultScriptDialogsEnabled;
            e.Settings.AreDevToolsEnabled = this._DevToolsEnabled;
            e.Settings.AreHostObjectsAllowed = this._RemoteObjectsAllowed;
            e.Settings.IsGeneralAutofillEnabled = this._IsGeneralAutoFillEnabled;
            e.Settings.IsPasswordAutosaveEnabled = this._IsPasswordAutosaveEnabled;
            e.Settings.IsScriptEnabled = this._IsScriptEnabled;
            e.Settings.IsStatusBarEnabled = this._IsStatusBarEnabled;
            e.Settings.IsWebMessageEnabled = this._IsWebMessageEnabled;
            e.Settings.IsZoomControlEnabled = this._IsZoomControlEnabled;
            e.Settings.IsBuiltInErrorPageEnabled = true;
            e.Settings.IsPinchZoomEnabled = true;
        }


       private void WebWindowInitAction()
        {
            this.IsCreated = true;
            AddRemoteObject("RpcHandler", this._RpcHandler);
            AddScriptToExecuteOnDocumentCreated(
                "class ScriptErrorObject{constructor(e,t,r,n,i,c){this.name=e,this.message=t,this.fileName=r,this.lineNumber=n,this.columnNumber=i,this.stack=c}}window.external={sendMessage:function(e){window.chrome.webview.postMessage(e)},receiveMessage:function(e){window.chrome.webview.addEventListener(\"message\",(function(t){e(t.data)}))},evalScript:function(e){try{return eval(e)}catch(e){let t=new ScriptErrorObject(e.name,e.message,e.fileName,e.lineNumber,e.columnNumber,e.stack);return JSON.stringify(t)}},executeScript:function(e){try{return new Function(e)()}catch(e){let t=new ScriptErrorObject(e.name,e.message,e.fileName,e.lineNumber,e.columnNumber,e.stack);return JSON.stringify(t)}}};");
            AddScriptToExecuteOnDocumentCreated(
                "window.external.raiseRpcEvent= async function(action, obj,objName,eventObj) { if(window.diga == undefined) window.diga = new Object(); try { const rpcHandler = window.chrome.webview.hostObjects.RpcHandler;const rpcObj = await rpcHandler.GetNewRpc(); let vn=await rpcObj.idName;window.diga[vn]=eventObj;rpcObj.objId = obj.id;rpcObj.action = action;rpcObj.varname=objName; rpcObj.param = \"empty\";rpcObj.item=document.getElementById(obj.id);let r = await rpcHandler.Handle(await rpcObj.id, await rpcObj.action, rpcObj);let b = await rpcHandler.ReleaseObject(rpcObj); } catch (e) { alert(e); } }; console.log('script_loaded'); window.addEventListener(\"beforeunload\",(e)=>{ window.chrome.webview.hostObjects.sync.RpcHandler.UnloadDom(); });");

            //"window.external = { sendMessage: function(message) { window.chrome.webview.postMessage(message); }, receiveMessage: function(callback) { window.chrome.webview.addEventListener('message', function(e) { callback(e.data); }); } };");
            if (this._DefaultBackgroundColor != Color.Empty)
                this._WebViewControl.DefaultBackgroundColor = this._DefaultBackgroundColor;
            if (!string.IsNullOrEmpty(this._Url))
                Navigate(this.Url);
            if (!string.IsNullOrEmpty(this._HtmlContent))
                NavigateToString(this._HtmlContent);
            if (this._ZoomFactor != 0)
                this.ZoomFactor = this._ZoomFactor;
            //this._WebViewControl.SetVirtualHostNameToFolderMapping("diga.resources","c:\\temp",COREWEBVIEW2_HOST_RESOURCE_ACCESS_KIND.COREWEBVIEW2_HOST_RESOURCE_ACCESS_KIND_ALLOW);
        }

        private void OnParentSize(object sender, SizeEventArgs e)
        {
            this.DoDock();
        }

        public void DoDock()
        {
            if (this.CheckIsCreatedOrEnded)
            {

                if (this.AutoDock)
                {
                    if (NativeWindow.TryGetWindow(this.ParentHandle, out NativeWindow wnd))
                    {
                        Rect rect = wnd.GetClientRect();
                        this.Left = rect.Left;
                        this.Top = rect.Top;
                        this.Width = rect.Width;
                        this.Height = rect.Height;
                    }
                    else
                    {
                        if (User32.GetClientRect(this.ParentHandle, out Rect rect))
                        {
                            this.Left = rect.Left;
                            this.Top = rect.Top;
                            this.Width = rect.Width;
                            this.Height = rect.Height;
                        }

                    }
                }
                this._WebViewControl.DockToParent();
            }


        }

        private bool CheckIsCreatedOrEnded
        {
            get
            {
                if (!this.IsCreated) return false;
                if (this.IsBrowserEnded) return false;
                return true;
            }
        }


        private void CheckIsCreatedOrEndedWithThrow()
        {
            if (!this.CheckIsCreatedOrEnded)
                throw new InvalidOperationException("Browser not Created or Crashed");
        }

        private void CheckIsInUiThread([CallerMemberName] string member = "")
        {
            if (!this.UIDispatcher.CheckAccess())
                throw new InvalidOperationException($"method ({member}) can only execute on UI-Thread");
        }

        private UIDispatcher UIDispatcher => UIDispatcher.UIThread;


    }
}
