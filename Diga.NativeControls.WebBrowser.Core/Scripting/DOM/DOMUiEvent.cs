using System.Threading.Tasks;

namespace Diga.NativeControls.WebBrowser.Scripting.DOM
{
    public class DOMUiEvent : DOMEvent
    {
        public DOMUiEvent(NativeWebBrowser control,DOMVar var):base(control,var)
        {
            
        }

        public int detail => Get<int>();
        public Task<int> detailAsync => GetAsync<int>(nameof(this.detail));

        public DOMWindow view => GetTypedVar<DOMWindow>();
        public Task<DOMWindow> viewAsync => GetTypedVarAsync<DOMWindow>(nameof(this.view));


    }
}