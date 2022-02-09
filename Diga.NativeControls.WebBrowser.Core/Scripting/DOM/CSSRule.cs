using System.Threading.Tasks;

namespace Diga.NativeControls.WebBrowser.Scripting.DOM
{
    public class CSSRule : DOMObject
    {
        public CSSRule(NativeWebBrowser control, DOMVar domVar) : base(control, domVar)
        {
            
        }
        public string cssText => Get<string>();
        public Task<string> cssTextAsync => GetAsync<string>(nameof(this.cssText));

        public CSSRule parentRule => GetTypedVar<CSSRule>();
        public Task<CSSRule> parentRuleAsync => GetTypedVarAsync<CSSRule>(nameof(this.parentRule));

        
    }
}