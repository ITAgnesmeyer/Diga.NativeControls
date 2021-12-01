﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using CoreWindowsWrapper;
using Diga.Core.Api.Win32;
using Diga.WebView2.Interop;

using Diga.WebView2.Wrapper;
using Diga.WebView2.Wrapper.EventArguments;
using Diga.WebView2.Wrapper.Types;
using MimeTypeExtension;

namespace Diga.NativeControls.WebBrowser
{
    public class NativeWebBrowser : NativeControlBase
    {
        private const string JAVASCRIPT_CANNOT_BE_NULL_OR_EMPTY = "javaScript cannot be NULL or empty";
        private NativeWindow _ParentNativeWindow = null;
        private WebView2Control _WebViewControl;
        private bool _DefaultContextMenusEnabled = true;
        private string _Url;

        private bool _DefaultScriptDialogsEnabled = true;
        private bool _DevToolsEnabled = true;

        private bool _RemoteObjectsAllowed = true;
        private bool _IsZoomControlEnabled = true;

        private bool _IsScriptEnabled = true;
        private bool _IsStatusBarEnabled;
        private bool _IsWebMessageEnabled = true;

        private bool _AreBrowserAcceleratorKeysEnabled = true;
        private bool _IsGeneralAutoFillEnabled = true;
        private bool _IsPasswordAutosaveEnabled = true;
        private string _HtmlContent;
        public event EventHandler<NavigationStartingEventArgs> NavigationStart;
        public event EventHandler<ContentLoadingEventArgs> ContentLoading;
        public event EventHandler<SourceChangedEventArgs> SourceChanged;
        public event EventHandler<WebView2EventArgs> HistoryChanged;
        public event EventHandler<NavigationCompletedEventArgs> NavigationCompleted;
        public event EventHandler<WebResourceRequestedEventArgs> WebResourceRequested;
        public event EventHandler<AcceleratorKeyPressedEventArgs> AcceleratorKeyPressed;
        public event EventHandler<WebView2EventArgs> WebViewGotFocus;
        public event EventHandler<WebView2EventArgs> WebViewLostFocus;
        public event EventHandler<MoveFocusRequestedEventArgs> MoveFocusRequested;
        public event EventHandler<WebView2EventArgs> ZoomFactorChanged;
        public event EventHandler<WebView2EventArgs> DocumentTitleChanged;
        //public event EventHandler<DocumentStateChangedEventArgs> DocumentStateChanged;
        public event EventHandler<WebView2EventArgs> ContainsFullScreenElementChanged;
        public event EventHandler<NewWindowRequestedEventArgs> NewWindowRequested;
        public event EventHandler<PermissionRequestedEventArgs> PermissionRequested;
        public event EventHandler<NavigationCompletedEventArgs> FrameNavigationCompleted;
        public event EventHandler<NavigationStartingEventArgs> FrameNavigationStarting;
        public event EventHandler<ExecuteScriptCompletedEventArgs> ExecuteScriptCompleted;
        public event EventHandler<ProcessFailedEventArgs> ProcessFailed;
        public event EventHandler<ScriptDialogOpeningEventArgs> ScriptDialogOpening;
        public event EventHandler<WebMessageReceivedEventArgs> WebMessageReceived;
        public event EventHandler<AddScriptToExecuteOnDocumentCreatedCompletedEventArgs>
            ScriptToExecuteOnDocumentCreatedCompleted;
        public event EventHandler<EnvironmentCompletedHandlerArgs> BeforeEnvironmentCompleted;
        public event EventHandler<DownloadStartingEventArgs> DownloadStarting;
        public event EventHandler<FrameCreatedEventArgs> FrameCreated;
        public event EventHandler<WebView2EventArgs> RasterizationScaleChanged;

        public event EventHandler<DOMContentLoadedEventArgs> DOMContentLoaded;
        public event EventHandler<WebResourceResponseReceivedEventArgs> WebResourceResponseReceived;

        public event EventHandler WebViewCreated;

        public event EventHandler BeforeWebViewDestroy;

        public event EventHandler<WebView2EventArgs> WindowCloseRequested;

        public bool AutoDock { get; set; } = false;
        public string BrowserExecutableFolder { get; set; } = "";
        public string BrowserUserDataFolder { get; set; } = "";
        public string BrowserAdditionArgs { get; set; } = "";
        public string MonitoringFolder { get; set; } = "";
        public string MonitoringUrl { get; set; } = "";
        public bool EnableMonitoring { get; set; } = false;
        public bool IsCreated { get; set; }
        public bool IsBrowserEnded { get; private set; } = false;

        private Color _DefaultBackgroundColor = Color.Empty;

        public Color DefaultBackgroundColor
        {
            get
            {


                if (this.CheckIsCreatedOrEnded)
                {
                    this._DefaultBackgroundColor = this._WebViewControl.DefaultBackgroundColor;
                }

                return _DefaultBackgroundColor;
            }
            set
            {
                _DefaultBackgroundColor = value;
                if (this.CheckIsCreatedOrEnded)
                {
                    this._WebViewControl.DefaultBackgroundColor = _DefaultBackgroundColor;
                }

            }

        }

        public string Url
        {
            get => this._Url;
            set => this._Url = value;
        }
        public string HtmlContent
        {
            get => this._HtmlContent;
            set
            {

                this.NavigateToString(value);

            }
        }
        protected override void Initialize()
        {
            base.Initialize();
            this.ControlType = CoreWindowsWrapper.Win32ApiForm.ControlType.Label;
            this.TypeIdentifier = "static";
            this.Style = WindowStylesConst.WS_VISIBLE | WindowStylesConst.WS_CHILD | StaticControlStyles.SS_NOTIFY;
            this.BackColor = CoreWindowsWrapper.Tools.ColorTool.White;
            this.ForeColor = CoreWindowsWrapper.Tools.ColorTool.Black;
        }


        public override bool Create(IntPtr parentId)
        {
            bool created = base.Create(parentId);
            if (this.Handle != IntPtr.Zero)
            {
                this._WebViewControl = new WebView2Control(this.Handle, this.BrowserExecutableFolder, this.BrowserUserDataFolder, this.BrowserAdditionArgs);
                this._WebViewControl.Created += OnWebWindowCreated;
                this._WebViewControl.BeforeCreate += OnWebWindowBeforeCreate;
                this._WebViewControl.NavigateStart += OnNavigationStartIntern;
                this._WebViewControl.AcceleratorKeyPressed += OnAcceleratorKeyPressedIntern;
                this._WebViewControl.GotFocus += OnWebViewGotFocusIntern;
                this._WebViewControl.LostFocus += OnWebViewLostFocusIntern;
                this._WebViewControl.MoveFocusRequested += OnMoveFocusRequestedIntern;
                this._WebViewControl.ZoomFactorChanged += OnZoomFactorChangedIntern;
                this._WebViewControl.ContainsFullScreenElementChanged += OnContainsFullScreenElementChangedIntern;

                this._WebViewControl.NewWindowRequested += OnNewWindowRequestedIntern;
                this._WebViewControl.PermissionRequested += OnPermissionRequestedIntern;
                this._WebViewControl.DocumentTitleChanged += OnDocumentTitleChangedIntern;
                this._WebViewControl.FrameNavigationCompleted += OnFrameNavigationCompletedIntern;
                this._WebViewControl.FrameNavigationStarting += OnFrameNavigationStartingIntern;
                this._WebViewControl.ProcessFailed += OnProcessFailedIntern;
                this._WebViewControl.ScriptDialogOpening += OnScriptDialogOpeningIntern;
                this._WebViewControl.WebMessageReceived += OnWebMessageReceivedIntern;
                this._WebViewControl.ScriptToExecuteOnDocumentCreatedCompleted += ScriptToExecuteOnDocumentCreatedCompletedIntern;

                this._WebViewControl.WindowCloseRequested += OnWindowCloseRequestedIntern;
                this._WebViewControl.ExecuteScriptCompleted += OnExecuteScriptCompletedIntern;
                this._WebViewControl.WebResourceRequested += OnWebResourceRequestedIntern;
                this._WebViewControl.ContentLoading += OnContentLoadingIntern;
                this._WebViewControl.SourceChanged += OnSourceChangedIntern;
                this._WebViewControl.HistoryChanged += OnHistoryChangedIntern;
                this._WebViewControl.NavigationCompleted += OnNavigationCompletedIntern;
                this._WebViewControl.NewBrowserVersionAvailable += OnNewBrowserVersionAvailableIntern;
                this._WebViewControl.DOMContentLoaded += OnDOMContentLoadedIntern;
                this._WebViewControl.WebResourceResponseReceived += WebResourceResponseReceivedIntern;
                this._WebViewControl.DownloadStarting += OnDownalodStartingIntern;
                this._WebViewControl.FrameCreated += OnFrameCreatedIntern;
                this._WebViewControl.RasterizationScaleChanged += OnRasterizationScaleChangedIntern;

            }
            else
            {
                throw new Exception("Cannot create WebView");
            }

            return created;

        }

        private void OnRasterizationScaleChangedIntern(object sender, WebView2EventArgs e)
        {
            OnRasterizationScaleChanged(e);
        }

        private void OnFrameCreatedIntern(object sender, FrameCreatedEventArgs e)
        {
            OnFrameCreated(e);
        }

        private void OnDownalodStartingIntern(object sender, DownloadStartingEventArgs e)
        {
            OnDownloadStarting(e);
        }

        private void WebResourceResponseReceivedIntern(object sender, WebResourceResponseReceivedEventArgs e)
        {
            OnWebResourceResponseReceived(e);
        }

        private void OnDOMContentLoadedIntern(object sender, DOMContentLoadedEventArgs e)
        {
            OnDomContentLoaded(e);
        }

        private void OnNewBrowserVersionAvailableIntern(object sender, WebView2EventArgs e)
        {
            MessageBox.Show(this.ParentHandle, "New Browser-Version available", "Information",
                MessageBoxOptions.OkOnly | MessageBoxOptions.IconInformation);
        }

        private void OnWindowCloseRequestedIntern(object sender, WebView2EventArgs e)
        {
            OnWindowCloseRequested(e);
        }

        private void ScriptToExecuteOnDocumentCreatedCompletedIntern(object sender, AddScriptToExecuteOnDocumentCreatedCompletedEventArgs e)
        {
            OnScriptToExecuteOnDocumentCreatedCompleted(e);
        }

        private void OnFrameNavigationCompletedIntern(object sender, NavigationCompletedEventArgs e)
        {
            OnFrameNavigationCompleted(e);
        }

       

        private void OnNavigationStartIntern(object sender, NavigationStartingEventArgs e)
        {
            OnNavigationStart(e);

        }

        private void OnDownloadStartingIntern(object sender, DownloadStartingEventArgs e)
        {
            OnDownloadStarting(e);
        }

        private void OnZoomFactorChangedIntern(object sender, WebView2EventArgs e)
        {
            OnZoomFactorChanged(e);
        }

        private void OnWebResourceRequestedIntern(object sender, WebResourceRequestedEventArgs e)
        {
            OnWebResourceRequested(e);
        }

        private void OnWebMessageReceivedIntern(object sender, WebMessageReceivedEventArgs e)
        {
            OnWebMessageReceived(e);
        }

        private void OnSourceChangedIntern(object sender, SourceChangedEventArgs e)
        {
            OnSourceChanged(e);
        }

        private void OnScriptToExecuteOnDocumentCreatedCompletedIntern(object sender, AddScriptToExecuteOnDocumentCreatedCompletedEventArgs e)
        {
            OnScriptToExecuteOnDocumentCreatedCompleted(e);
        }

        private void OnScriptDialogOpeningIntern(object sender, ScriptDialogOpeningEventArgs e)
        {
            OnScriptDialogOpening(e);
        }

        private void OnProcessFailedIntern(object sender, ProcessFailedEventArgs e)
        {
            if (e.ProcessFailedKind == ProcessFailedKind.BrowserProcessExited)
            {
                this.IsBrowserEnded = true;
            }
            OnProcessFailed(e);
        }

        private void OnPermissionRequestedIntern(object sender, PermissionRequestedEventArgs e)
        {
            OnPermissionRequested(e);
        }

        private void OnNewWindowRequestedIntern(object sender, NewWindowRequestedEventArgs e)
        {
            OnNewWindowRequested(e);
        }

        private void OnNavigationCompletedIntern(object sender, NavigationCompletedEventArgs e)
        {
            OnNavigationCompleted(e);
        }

        private void OnNavigateStartIntern(object sender, NavigationStartingEventArgs e)
        {
            OnNavigationStart(e);
        }

        private void OnMoveFocusRequestedIntern(object sender, MoveFocusRequestedEventArgs e)
        {
            OnMoveFocusRequested(e);
        }

        private void OnWebViewLostFocusIntern(object sender, WebView2EventArgs e)
        {
            OnWebViewLostFocus(e);
        }

        private void OnHistoryChangedIntern(object sender, WebView2EventArgs e)
        {
            OnHistoryChanged(e);
        }

        private void OnWebViewGotFocusIntern(object sender, WebView2EventArgs e)
        {
            OnWebViewGotFocus(e);
        }

        private void OnFrameNavigationStartingIntern(object sender, NavigationStartingEventArgs e)
        {
            OnFrameNavigationStarting(e);
        }

        private void OnExecuteScriptCompletedIntern(object sender, ExecuteScriptCompletedEventArgs e)
        {
            OnExecuteScriptCompleted(e);
        }

        private void OnDocumentTitleChangedIntern(object sender, WebView2EventArgs e)
        {
            OnDocumentTitleChanged(e);
        }

        //private void OnDocumentStateChangedIntern(object sender, DocumentStateChangedEventArgs e)
        //{
        //    OnDocumentStateChanged(e);
        //}

        private void OnContentLoadingIntern(object sender, ContentLoadingEventArgs e)
        {
            OnContentLoading(e);
        }

        private void OnContainsFullScreenElementChangedIntern(object sender, WebView2EventArgs e)
        {
            OnContainsFullScreenElementChanged(e);
        }

        private void OnAcceleratorKeyPressedIntern(object sender, AcceleratorKeyPressedEventArgs e)
        {
            OnAcceleratorKeyPressed(e);
        }

        private void OnWebWindowBeforeCreate(object sender, BeforeCreateEventArgs e)
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

        private void OnWebWindowCreated(object sender, EventArgs e)
        {
            this.IsCreated = true;
            this.IsBrowserEnded = false;
            this.AddScriptToExecuteOnDocumentCreated(
                "window.external = { sendMessage: function(message) { window.chrome.webview.postMessage(message); }, receiveMessage: function(callback) { window.chrome.webview.addEventListener('message', function(e) { callback(e.data); }); } };");
            if (!string.IsNullOrEmpty(this._Url))
                this.Navigate(this.Url);
            if (!string.IsNullOrEmpty(this._HtmlContent))
                this.NavigateToString(this._HtmlContent);

            OnWebViewCreated();
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
        public void Navigate(string url)
        {
            this._Url = url;
            if (this.CheckIsCreatedOrEnded && !string.IsNullOrEmpty(this.Url))
            {
                try
                {
                    this._WebViewControl.Navigate(this._Url);
                }
                catch (Exception e)
                {
                    MessageBox.Show(this.Handle, e.Message, "Navigation Error!", MessageBoxOptions.OkOnly | MessageBoxOptions.IconError);

                }

            }

        }

        public void NavigateToString(string htmlContent)
        {
            this._HtmlContent = htmlContent;
            if (this.CheckIsCreatedOrEnded && !string.IsNullOrEmpty(this._HtmlContent))
            {
                try
                {
                    this._WebViewControl.NavigateToString(this._HtmlContent);
                }
                catch (Exception e)
                {
                    MessageBox.Show(this.Handle, e.Message, "Navigation Error!", MessageBoxOptions.OkOnly | MessageBoxOptions.IconError);
                }

            }
        }

        public void GoBack()
        {
            if (this.CheckIsCreatedOrEnded)
                this._WebViewControl.GoBack();
        }

        public void GoForward()
        {
            if (this.CheckIsCreatedOrEnded)
                this._WebViewControl.GoForward();
        }

        public bool CanGoBack
        {
            get
            {
                if (this.CheckIsCreatedOrEnded)
                    return this._WebViewControl.CanGoBack;
                return false;
            }
        }

        public bool CanGoForward
        {
            get
            {
                if (this.CheckIsCreatedOrEnded)
                    return this._WebViewControl.CanGoForward;
                return false;
            }
        }

        public string DocumentTitle
        {
            get
            {
                if (this.CheckIsCreatedOrEnded)
                    return this._WebViewControl.DocumentTitle;
                return "";
            }
        }
        public void SendMessage(string message)
        {
            this._WebViewControl.PostWebMessageAsString(message);
        }
        public void AddScriptToExecuteOnDocumentCreated(string javaScript)
        {
            if (this.CheckIsCreatedOrEnded)
                this._WebViewControl.AddScriptToExecuteOnDocumentCreated(javaScript);
        }
        public void RemoveScriptToExecuteOnDocumentCreated(string id)
        {
            if (this.CheckIsCreatedOrEnded)
                this._WebViewControl.RemoveRemoteObject(id);
        }

        public void PostWebMessageAsJson(string webMessageAsJson)
        {
            if (this.CheckIsCreatedOrEnded)
                this._WebViewControl.PostWebMessageAsJson(webMessageAsJson);
        }
        public void PostWebMessageAsString(string webMessageAsString)
        {
            if (this.CheckIsCreatedOrEnded)
                this._WebViewControl.PostWebMessageAsString(webMessageAsString);
        }

        public void AddRemoteObject(string name, object @object)
        {
            if (this.CheckIsCreatedOrEnded)
                this._WebViewControl.AddRemoteObject(name, @object);
        }

        public void RemoveRemoteObject(string name)
        {
            if (this.CheckIsCreatedOrEnded)
                this._WebViewControl.RemoveRemoteObject(name);
        }

        public void ExecuteScript(string javaScript)
        {
            if (this.CheckIsCreatedOrEnded)
                this._WebViewControl.ExecuteScript(javaScript);
        }

        public string InvokeScript(string javaScript, Action<string, int, string> actionToInvoke)
        {
            if (this.CheckIsCreatedOrEnded)
                return this._WebViewControl.InvokeScript(javaScript, actionToInvoke);
            return "";
        }

        public void OpenDevToolsWindow()
        {
            if (this.CheckIsCreatedOrEnded)
                this._WebViewControl.OpenDevToolsWindow();
        }

        public bool IsWebMessageEnabled
        {
            get => this._IsWebMessageEnabled;
            set
            {
                this._IsWebMessageEnabled = value;
                if (this.CheckIsCreatedOrEnded)
                {
                    this._WebViewControl.Settings.IsWebMessageEnabled = new CBOOL(value);
                }
            }
        }



        public bool IsStatusBarEnabled
        {
            get => this._IsStatusBarEnabled;
            set
            {
                this._IsStatusBarEnabled = value;
                if (this.CheckIsCreatedOrEnded)
                {
                    this._WebViewControl.Settings.IsStatusBarEnabled = value;
                }
            }
        }

        public bool IsGeneralAutoFillEnabled
        {
            get => this._IsGeneralAutoFillEnabled;
            set
            {
                this._IsGeneralAutoFillEnabled = value;
                if (this.CheckIsCreatedOrEnded)
                {
                    this._WebViewControl.Settings.IsGeneralAutofillEnabled = value;
                }
            }
        }

        public bool IsPasswordAutosaveEnabled
        {
            get => this._IsPasswordAutosaveEnabled;
            set
            {
                this._IsPasswordAutosaveEnabled = value;
                if (this.CheckIsCreatedOrEnded)
                {
                    this._WebViewControl.Settings.IsPasswordAutosaveEnabled = value;
                }
            }
        }
        public bool IsScriptEnabled
        {
            get => this._IsScriptEnabled;
            set
            {
                this._IsScriptEnabled = value;
                if (this.CheckIsCreatedOrEnded)
                {
                    this._WebViewControl.Settings.IsScriptEnabled = value;
                }
            }
        }


        public bool DevToolsEnabled
        {
            get => this._DevToolsEnabled;
            set
            {
                this._DevToolsEnabled = value;
                if (this.CheckIsCreatedOrEnded)
                {
                    this._WebViewControl.Settings.AreDevToolsEnabled =value;
                }
            }
        }

        public bool DefaultScriptDialogsEnabled
        {
            get => this._DefaultScriptDialogsEnabled;
            set
            {
                this._DefaultScriptDialogsEnabled = value;
                if (this.CheckIsCreatedOrEnded)
                {
                    this._WebViewControl.Settings.AreDefaultScriptDialogsEnabled = value;
                }
            }
        }

        public bool DefaultContextMenusEnabled
        {
            get => this._DefaultContextMenusEnabled;
            set
            {
                this._DefaultContextMenusEnabled = value;
                if (this.CheckIsCreatedOrEnded)
                {
                    this._WebViewControl.Settings.AreDefaultContextMenusEnabled = value;
                }
            }
        }

        protected virtual void OnNavigationStart(NavigationStartingEventArgs e)
        {
            NavigationStart?.Invoke(this, e);
        }

        protected virtual void OnContentLoading(ContentLoadingEventArgs e)
        {
            ContentLoading?.Invoke(this, e);
        }

        protected virtual void OnSourceChanged(SourceChangedEventArgs e)
        {
            SourceChanged?.Invoke(this, e);
        }

        protected virtual void OnHistoryChanged(WebView2EventArgs e)
        {
            HistoryChanged?.Invoke(this, e);
        }

        protected virtual void OnNavigationCompleted(NavigationCompletedEventArgs e)
        {
            NavigationCompleted?.Invoke(this, e);
        }

        protected virtual void OnWebResourceRequested(WebResourceRequestedEventArgs e)
        {
            if (this.EnableMonitoring)
            {
                if (GetFileStream(e.Request.Uri, out var responseInfo))
                {
                    var response = this.CreateResponse(responseInfo);
                    e.Response = response;
                }
            }
            WebResourceRequested?.Invoke(this, e);
        }
        public WebResourceResponse CreateResponse(ResponseInfo responseInfo)
        {
            WebResourceResponse response = null;
            if (this.CheckIsCreatedOrEnded)
            {
                response = this._WebViewControl.GetResponseStream(responseInfo.Stream, responseInfo.StatusCode,
                    responseInfo.StatusText, responseInfo.HeaderToString(), responseInfo.ContentType);
            }

            return response;
        }
        private bool GetFileStream(string url, out ResponseInfo responseInfo)
        {

            if (string.IsNullOrEmpty(url) || !url.StartsWith(this.MonitoringUrl))
            {
                responseInfo = null;
                return false;
            }

            string baseDirectory = this.MonitoringFolder;
            string file = url.Replace(this.MonitoringUrl, "");
            if (string.IsNullOrEmpty(file))
                file = "index.html";
            file = file.Replace("/", "\\");
            file = Path.Combine(baseDirectory, file);
            FileInfo fileInfo = new FileInfo(file);
            if (!fileInfo.Exists)
            {
                responseInfo = new ResponseInfo("<h1>Server Error</h1><h5>file not found:" + file + "</h5>");
                responseInfo.Header.Add("content-type", "text/html; charset=utf-8");
                responseInfo.ContentType = "content-type: text/html; charset=utf-8";
                responseInfo.StatusCode = 404;
                responseInfo.StatusText = "Not Found";

                return false;
            }

            string contentType = fileInfo.MimeTypeOrDefault();
            if (contentType == "document")
                Debug.Print(contentType);
            try
            {
                byte[] bytes = File.ReadAllBytes(file);
                responseInfo = new ResponseInfo(bytes);
                string utf8Extension = GetUtf8IfNeeded(contentType);

                responseInfo.Header.Add("content-type", contentType + utf8Extension);

                responseInfo.ContentType = "content-type: " + contentType + utf8Extension;
                responseInfo.StatusCode = 200;
                responseInfo.StatusText = "OK";
                return true;
            }
            catch (Exception e)
            {
                string message = "Error:" + e.Message;
                responseInfo = new ResponseInfo(message);
                responseInfo.Header.Add("content-type", "text/html; charset=utf-8");
                responseInfo.ContentType = "content-type; charset=utf-8";
                responseInfo.StatusCode = 500;
                responseInfo.StatusText = "Internal Server Error";
                return true;
            }


        }

        private string GetUtf8IfNeeded(string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
                return "";

            bool needUtf8 = false;

            switch (contentType)
            {
                case "application/x-javascript":
                case "text/html":
                case "text/css":
                case "application/javascript":
                case "application/json":
                    needUtf8 = true;
                    break;
            }

            if (needUtf8)
                return "; charset=utf-8";
            return "";
        }

        protected virtual void OnAcceleratorKeyPressed(AcceleratorKeyPressedEventArgs e)
        {
            AcceleratorKeyPressed?.Invoke(this, e);
        }

        protected virtual void OnWebViewGotFocus(WebView2EventArgs e)
        {
            WebViewGotFocus?.Invoke(this, e);
        }

        protected virtual void OnWebViewLostFocus(WebView2EventArgs e)
        {
            WebViewLostFocus?.Invoke(this, e);
        }

        protected virtual void OnZoomFactorChanged(WebView2EventArgs e)
        {
            ZoomFactorChanged?.Invoke(this, e);
        }

        protected virtual void OnMoveFocusRequested(MoveFocusRequestedEventArgs e)
        {
            MoveFocusRequested?.Invoke(this, e);
        }

        protected virtual void OnDocumentTitleChanged(WebView2EventArgs e)
        {
            DocumentTitleChanged?.Invoke(this, e);
        }

        protected virtual void OnContainsFullScreenElementChanged(WebView2EventArgs e)
        {
            ContainsFullScreenElementChanged?.Invoke(this, e);
        }

        protected virtual void OnNewWindowRequested(NewWindowRequestedEventArgs e)
        {
            NewWindowRequested?.Invoke(this, e);
        }

        protected virtual void OnPermissionRequested(PermissionRequestedEventArgs e)
        {
            PermissionRequested?.Invoke(this, e);
        }

        protected virtual void OnFrameNavigationStarting(NavigationStartingEventArgs e)
        {
            FrameNavigationStarting?.Invoke(this, e);
        }

        protected virtual void OnExecuteScriptCompleted(ExecuteScriptCompletedEventArgs e)
        {
            ExecuteScriptCompleted?.Invoke(this, e);
        }

        protected virtual void OnProcessFailed(ProcessFailedEventArgs e)
        {
            ProcessFailed?.Invoke(this, e);
        }

        protected virtual void OnScriptDialogOpening(ScriptDialogOpeningEventArgs e)
        {
            ScriptDialogOpening?.Invoke(this, e);
        }

        protected virtual void OnWebMessageReceived(WebMessageReceivedEventArgs e)
        {
            WebMessageReceived?.Invoke(this, e);
        }

        protected virtual void OnScriptToExecuteOnDocumentCreatedCompleted(AddScriptToExecuteOnDocumentCreatedCompletedEventArgs e)
        {
            ScriptToExecuteOnDocumentCreatedCompleted?.Invoke(this, e);
        }

        //protected virtual void OnDocumentStateChanged(DocumentStateChangedEventArgs e)
        //{
        //    DocumentStateChanged?.Invoke(this, e);
        //}

        protected virtual void OnWebViewCreated()
        {
            this.DoDock();
            if (this.AutoDock)
            {
                if (NativeWindow.TryGetWindow(this.ParentHandle, out NativeWindow wnd))
                {
                    this._ParentNativeWindow = wnd;
                    this._ParentNativeWindow.Size += OnParentSize;
                }
            }
            WebViewCreated?.Invoke(this, EventArgs.Empty);
        }

        private void OnParentSize(object sender, SizeEventArgs e)
        {
            this.DoDock();
        }

        protected virtual void OnDownloadStarting(DownloadStartingEventArgs e)
        {
            DownloadStarting?.Invoke(this, e);
        }

        protected virtual void OnBeforeEnvironmentCompleted(EnvironmentCompletedHandlerArgs e)
        {
            BeforeEnvironmentCompleted?.Invoke(this, e);
        }

        protected virtual void OnFrameCreated(FrameCreatedEventArgs e)
        {
            FrameCreated?.Invoke(this, e);

        }

        protected virtual void OnRasterizationScaleChanged(WebView2EventArgs e)
        {
            RasterizationScaleChanged?.Invoke(this, e);

        }

        protected virtual void OnDomContentLoaded(DOMContentLoadedEventArgs e)
        {
            DOMContentLoaded?.Invoke(this, e);

        }

        protected virtual void OnWebResourceResponseReceived(WebResourceResponseReceivedEventArgs e)
        {
            WebResourceResponseReceived?.Invoke(this, e);

        }

        protected virtual void OnBeforeWebViewDestroy()
        {
            BeforeWebViewDestroy?.Invoke(this, EventArgs.Empty);

        }

        protected virtual void OnWindowCloseRequested(WebView2EventArgs e)
        {
            WindowCloseRequested?.Invoke(this, e);

        }

        protected virtual void OnFrameNavigationCompleted(NavigationCompletedEventArgs e)
        {
            FrameNavigationCompleted?.Invoke(this, e);

        }
    }
}
