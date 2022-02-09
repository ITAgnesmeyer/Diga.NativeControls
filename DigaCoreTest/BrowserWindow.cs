using System;
using System.Diagnostics;
using CoreWindowsWrapper;
using Diga.Core.Api.Win32;
using Diga.NativeControls.WebBrowser;
using Diga.WebView2.Wrapper;
using Diga.WebView2.Wrapper.EventArguments;

namespace ConsoleCaller
{
    class BrowserWindow : NativeWindow
    {
        private NativeWebBrowser _Browser;
        protected override void InitControls()
        {
            this.Text = "WebBrowser";
            this.Name = "BrowserWindow";
            this.StatusBar = false;
            this.IconFile = "Browser.ico";
            this.Width = 800;
            this.Height = 600;
            this.StartUpPosition = WindowsStartupPosition.CenterScreen;


            this._Browser = new NativeWebBrowser()
            {
                Width = this.Width,
                Height = this.Height,
                Url = "http://localhost:1",
                IsStatusBarEnabled = true,
                DefaultContextMenusEnabled = true,
                DevToolsEnabled = true,
                EnableMonitoring = true,
                MonitoringFolder = ".\\wwwroot",
                MonitoringUrl = "http://localhost:1/",
                AutoDock = true

            };
            this._Browser.DocumentTitleChanged += OnDocumentTitleChanged;
            this._Browser.NavigationStart += OnNavigationStart;
            this._Browser.NavigationCompleted += OnNaviationCompleted;
            this._Browser.WebResourceRequested += OnWebResourceRequested;
            this._Browser.WebMessageReceived += OnWebMessageReceived;
            this._Browser.AcceleratorKeyPressed += OnWebBrowserAcceleratorKeyPressed;


            this.Controls.Add(this._Browser);
        }

        private uint LastStype = 0;
        private uint LastExStyle = 0;
        private void OnWebBrowserAcceleratorKeyPressed(object sender, AcceleratorKeyPressedEventArgs e)
        {
            
            if (e.VirtualKey == 122 && e.KeyVentType == KeyEventType.KeyDown)
            {
                IntPtr style = User32.GetWindowLongPtr(this.Handle, GWL.GWL_STYLE);
                IntPtr exStyle = User32.GetWindowLongPtr(this.Handle, GWL.GWL_EXSTYLE);
                uint currentStyle = unchecked((uint)style.ToInt64());
                uint currentExStyle = unchecked((uint)exStyle.ToInt64());
                if (currentStyle == 385941504 && currentExStyle == 327680)
                {
                    IntPtr p = new IntPtr(this.LastStype);
                    IntPtr pp = new IntPtr(this.LastExStyle);
                    User32.ShowWindow(this.Handle, 1);
                    User32.SetWindowLongPtr(this.Handle, GWL.GWL_STYLE, p);
                    User32.SetWindowLongPtr(this.Handle, GWL.GWL_EXSTYLE, pp);
                    User32.SetWindowPos(this.Handle, IntPtr.Zero, 0, 0, 0, 0,
                        (uint)DeferWindowPosCommands.SWP_FRAMECHANGED | (uint)DeferWindowPosCommands.SWP_NOMOVE |
                        (uint)DeferWindowPosCommands.SWP_NOSIZE | (uint)DeferWindowPosCommands.SWP_NOZORDER |
                        (uint)DeferWindowPosCommands.SWP_NOOWNERZORDER);
                }
                else
                {


                    this.LastStype = unchecked((uint)style.ToInt64());
                    this.LastExStyle = unchecked((uint)exStyle.ToInt64());
                    //369164288;327680
                    //385941504;327680
                    uint newStyle = 385941504;
                    uint newExStyle = 327680;
                    IntPtr p = new IntPtr(newStyle);
                    IntPtr pp = new IntPtr(newExStyle);
                    User32.ShowWindow(this.Handle, 3);
                    User32.SetWindowLongPtr(this.Handle, GWL.GWL_STYLE, p);
                    User32.SetWindowLongPtr(this.Handle, GWL.GWL_EXSTYLE, pp);
                    User32.SetWindowPos(this.Handle, IntPtr.Zero, 0, 0, 0, 0,
                        (uint)DeferWindowPosCommands.SWP_FRAMECHANGED | (uint)DeferWindowPosCommands.SWP_NOMOVE |
                        (uint)DeferWindowPosCommands.SWP_NOSIZE | (uint)DeferWindowPosCommands.SWP_NOZORDER |
                        (uint)DeferWindowPosCommands.SWP_NOOWNERZORDER);
                }
            }
        }


        private void OnWebMessageReceived(object sender, WebMessageReceivedEventArgs e)
        {

        }

        private void OnWebResourceRequested(object sender, WebResourceRequestedEventArgs e)
        {
            Debug.Print(e.Request.Uri);
        }

        private void OnNaviationCompleted(object sender, NavigationCompletedEventArgs e)
        {
            if (e.IsSuccess)
                this.Text = e.IsSuccess + "->" + this._Browser.DocumentTitle;
            else
                this.Text = "Navigation-Error=>" + e.GetErrorText();

        }
        private void OnNavigationStart(object sender, NavigationStartingEventArgs e)
        {

            this.Text = "Start-Navigate" + e.uri;
        }
        private void OnDocumentTitleChanged(object sender, WebView2EventArgs e)
        {
            this.Text = this._Browser.DocumentTitle;
        }

        //protected override void OnSize(SizeEventArgs e)
        //{
        //    if (e.Width == 0) return;
        //    base.OnSize(e);
        //    this._Browser.Left = e.X;
        //    this._Browser.Top = e.Y;
        //    this._Browser.Width = e.Width;
        //    this._Browser.Height = e.Height;
        //    this._Browser.DoDock();
        //}
    }
}