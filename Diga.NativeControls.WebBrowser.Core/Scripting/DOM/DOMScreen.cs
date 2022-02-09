﻿using System.Threading.Tasks;

namespace Diga.NativeControls.WebBrowser.Scripting.DOM
{
    public class DOMScreen : DOMObject
    {
        public DOMScreen(NativeWebBrowser control) : base(control)
        {
            this._InstanceName = "screen";
        }
        public DOMScreen(NativeWebBrowser control, DOMVar domVar) : base(control,domVar)
        {
            
        }

        public int availHeight => Get<int>();
        public Task<int> availHeightAsync=>GetAsync<int>(nameof(this.availHeight));
        public int availWidth => Get<int>();
        public Task<int> availWidthAsync=>GetAsync<int>(nameof(this.availWidth));
        public int colorDepth => Get<int>();
        public Task<int> colorDepthAsync=>GetAsync<int>(nameof(this.colorDepth));
        public int height => Get<int>();
        public Task<int> heightAsync=>GetAsync<int>(nameof(this.height));
        public int pixelDepth => Get<int>();
        public Task<int> pixelDepthAsync=>GetAsync<int>(nameof(this.pixelDepth));
        public int width => Get<int>();
        public Task<int> widthAsync=>GetAsync<int>(nameof(this.width));


    }
}