using System;
using CoreWindowsWrapper;

namespace Diga.NativeControls.WebBrowser
{
    public partial class NativeWebBrowser
    {
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

        //public void ExecuteScript(string javaScript)
        //{
        //    if (this.CheckIsCreatedOrEnded)
        //        this._WebViewControl.ExecuteScript(javaScript);
        //}

        //public string InvokeScript(string javaScript, Action<string, int, string> actionToInvoke)
        //{
        //    if (this.CheckIsCreatedOrEnded)
        //        return this._WebViewControl.InvokeScript(javaScript, actionToInvoke);
        //    return "";
        //}

        public void OpenDevToolsWindow()
        {
            if (this.CheckIsCreatedOrEnded)
                this._WebViewControl.OpenDevToolsWindow();
        }

    }
}