﻿using System.Threading.Tasks;

namespace Diga.NativeControls.WebBrowser.Scripting.DOM
{
    public class CSSRuleList : DOMObject
    {
        public CSSRuleList(NativeWebBrowser control, DOMVar domVar) : base(control, domVar)
        {
            
        }
        public int length => Get<int>();
        public Task<int> lengthAsync => GetAsync<int>(nameof(this.length));

        public CSSRule item(int index)
        {
            DOMVar var = ExecGetVar(new object[] { index });
            return new CSSRule(this._View2Control, var);
        }

        public async Task<CSSRule> itemAsync(int index)
        {
            DOMVar var = await ExecGetVarAsync(new object[] { index },nameof(item));
            return new CSSRule(this._View2Control, var);
        }
    }
}