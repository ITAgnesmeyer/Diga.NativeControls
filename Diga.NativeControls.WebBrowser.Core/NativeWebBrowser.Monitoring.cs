﻿using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using Diga.WebView2.Monitoring;
using Diga.WebView2.Wrapper;
using Diga.WebView2.Wrapper.EventArguments;
using MimeTypeExtension;

namespace Diga.NativeControls.WebBrowser
{
    public partial class NativeWebBrowser
    {
        //private readonly ConcurrentDictionary<string, ResponseInfo>
        //    _Responses = new ConcurrentDictionary<string, ResponseInfo>();
        private MonitoringActionList _MonitoringActionList;

        private void CheckMonitoring(WebResourceRequestedEventArgs e)
        {
            string uri = e.Request.Uri;
            if (this._MonitoringActionList.IsAnyActiveWithUrl(uri))
            {
                var action = this._MonitoringActionList.GetFirstActiveWithUrl(uri);
                RequestInfo requestInfo = new RequestInfo(e.Request);
                try
                {
                    if (action.Run(requestInfo, out var responseInfo))
                    {
                        var respose = this.CreateResponse(responseInfo);
                        e.Response = respose;

                    }

                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }

        //private void CheckMonitoring(WebResourceRequestedEventArgs e)
        //{

        //    string uri = e.Request.Uri;
        //    bool isMonitroingUrl = IsMonitoringUrl(uri);
        //    bool isResourceUrl = IsResorceUrl(uri);
        //    if (isMonitroingUrl || isResourceUrl)
        //    {
        //        RequestInfo requestInfo = new RequestInfo(e.Request);

        //        if (isMonitroingUrl)
        //        {
        //            if (GetMonitoringFile(requestInfo, out var responseInfo))
        //            {
        //                var response = this.CreateResponse(responseInfo);
        //                e.Response = response;
        //                return;
        //            }
        //        }


        //        if (isResourceUrl)
        //        {
        //            if (GetResource(requestInfo, out var responseInfoRes))
        //            {
        //                var response = this.CreateResponse(responseInfoRes);
        //                e.Response = response;

        //                //return;
        //            }
        //        }
        //    }
        //}

        //private bool IsResorceUrl(string url)
        //{
        //    if (string.IsNullOrEmpty(url))
        //        return false;
        //    if (url.StartsWith("diga://"))
        //        return true;
        //    return false;

        //}

        //private bool IsMonitoringUrl(string url)
        //{
        //    if (!this.EnableMonitoring)
        //        return false;
        //    if (string.IsNullOrEmpty(url))
        //        return false;
        //    if (url.StartsWith(this.MonitoringUrl))
        //        return true;
        //    return false;
        //}
        //private bool GetResource(RequestInfo requestInfo, out ResponseInfo responseInfo)
        //{
        //    string url = requestInfo.Uri;
        //    string monUrl = "diga://";

        //    if (!url.StartsWith(monUrl))
        //    {
        //        responseInfo = null;
        //        return false;
        //    }

        //    if (url.Length <= monUrl.Length)
        //    {
        //        responseInfo = null;
        //        return false;
        //    }
        //    string resName = url.Substring(monUrl.Length);

        //    if (string.IsNullOrEmpty(resName))
        //    {
        //        responseInfo = null;
        //        return false;
        //    }

        //    try
        //    {
        //        string resString = Resources.ResourceManager.GetString(resName);
        //        if (resString == null)
        //        {
        //            responseInfo = new ResponseInfo("<h1>Server Error</h1><h5>file not found:" + url + "</h5>");
        //            responseInfo.Header.Add("content-type", "text/html; charset=utf-8");
        //            responseInfo.ContentType = "content-type: text/html; charset=utf-8";
        //            responseInfo.StatusCode = 404;
        //            responseInfo.StatusText = "Not Found";
        //            return false;
        //        }

        //        int index = resString.IndexOf("base64,", StringComparison.Ordinal);
        //        string mime = resString.Substring(5, index - 5);

        //        string contentBase64 = resString.Substring(index + 7);

        //        byte[] contentBase64Bytes = Convert.FromBase64String(contentBase64);


        //        responseInfo = new ResponseInfo(contentBase64Bytes);
        //        responseInfo.Header.Add("content-type", mime);
        //        responseInfo.ContentType = mime;
        //        responseInfo.StatusCode = 200;
        //        responseInfo.StatusText = "OK";
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        string message = "Error:" + e.Message;
        //        responseInfo = new ResponseInfo(message);
        //        responseInfo.Header.Add("content-type", "text/html; charset=utf-8");
        //        responseInfo.ContentType = "content-type; charset=utf-8";
        //        responseInfo.StatusCode = 500;
        //        responseInfo.StatusText = "Internal Server Error";
        //        return true;
        //    }

        //}
        ////private void CleanUpResponses(WebResourceResponseReceivedEventArgs e)
        ////{
        ////    //try
        ////    //{
        ////    //    if (this._Responses.ContainsKey(e.Request.Uri))
        ////    //    {
        ////    //        if (this._Responses.TryRemove(e.Request.Uri, out var resp))
        ////    //        {
        ////    //            resp.Dispose();
        ////    //        }

        ////    //    }

        ////    //}
        ////    //catch (Exception exception)
        ////    //{
        ////    //    Debug.Print("CleanUpResponse Error:" + exception.Message);
        ////    //}
        ////}

        //private bool MonitoringFileExist(string file)
        //{
        //    if (file.StartsWith("/"))
        //        file = file.Substring(1);
        //    file = file.Replace("/", "\\");
        //    string fullName = Path.Combine(this.MonitoringFolder, file);
        //    return File.Exists(fullName);
        //}
        //private bool GetMonitoringFile(RequestInfo requestInfo, out ResponseInfo responseInfo)
        //{
        //    string url = requestInfo.Uri;
        //    if (!url.StartsWith(this.MonitoringUrl))
        //    {
        //        responseInfo = null;
        //        return false;
        //    }

        //    Uri uri = new Uri(url);


        //    //TestMimeTypes();
        //    string baseDirectory = this.MonitoringFolder;

        //    string file = MonitoringFileExist(uri.AbsolutePath) ? uri.AbsolutePath : "";

        //    if (file == "/")
        //        file = "";
        //    if (file.StartsWith("/"))
        //        file = file.Substring(1);
        //    if (string.IsNullOrEmpty(file))
        //        file = "index.html";
        //    file = file.Replace("/", "\\");
        //    file = Path.Combine(baseDirectory, file);
        //    FileInfo fileInfo = new FileInfo(file);
        //    if (!fileInfo.Exists)
        //    {
        //        responseInfo = new ResponseInfo("<h1>Server Error</h1><h5>file not found:" + file + "</h5>");
        //        responseInfo.Header.Add("content-type", "text/html; charset=utf-8");
        //        responseInfo.ContentType = "content-type: text/html; charset=utf-8";
        //        responseInfo.StatusCode = 404;
        //        responseInfo.StatusText = "Not Found";

        //        return false;
        //    }

        //    string contentType = fileInfo.MimeTypeOrDefault();
        //    if (contentType == "document")
        //        Debug.Print(contentType);
        //    try
        //    {
        //        byte[] bytes = File.ReadAllBytes(file);
        //        responseInfo = new ResponseInfo(bytes);
        //        string utf8Extension = GetUtf8IfNeeded(contentType);

        //        responseInfo.Header.Add("content-type", contentType + utf8Extension);

        //        responseInfo.ContentType = "content-type: " + contentType + utf8Extension;
        //        responseInfo.StatusCode = 200;
        //        responseInfo.StatusText = "OK";

        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        string message = "Error:" + e.Message;
        //        responseInfo = new ResponseInfo(message);
        //        responseInfo.Header.Add("content-type", "text/html; charset=utf-8");
        //        responseInfo.ContentType = "content-type; charset=utf-8";
        //        responseInfo.StatusCode = 500;
        //        responseInfo.StatusText = "Internal Server Error";
        //        return true;
        //    }
        //}

        //private static string GetUtf8IfNeeded(string contentType)
        //{
        //    if (string.IsNullOrEmpty(contentType))
        //        return "";

        //    bool needUtf8 = false;

        //    switch (contentType)
        //    {
        //        case "application/x-javascript":
        //        case "text/html":
        //        case "text/css":
        //        case "application/javascript":
        //        case "application/json":
        //            needUtf8 = true;
        //            break;
        //    }

        //    if (needUtf8)
        //        return "; charset=utf-8";
        //    return "";
        //}
    }

}
