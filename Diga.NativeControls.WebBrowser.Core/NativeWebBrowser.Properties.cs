using System.Drawing;
using Diga.WebView2.Wrapper.Types;

namespace Diga.NativeControls.WebBrowser
{
    public partial class NativeWebBrowser
    {
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
        private double _ZoomFactor;

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
        public double ZoomFactor
        {
            get
            {
                if (this.CheckIsCreatedOrEnded)
                {
                    this._ZoomFactor = this._WebViewControl.ZoomFactor;
                }

                return this._ZoomFactor;
            }
            set
            {
                this._ZoomFactor = value;
                if (this.CheckIsCreatedOrEnded)
                {
                    this._WebViewControl.ZoomFactor = this._ZoomFactor;
                }
            }
        }
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
    }
}