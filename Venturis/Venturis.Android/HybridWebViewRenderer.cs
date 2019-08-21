using Android.Content;
using Venturis.Droid;
using Venturis.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(HybridWebView), typeof(HybridWebViewRenderer))]

namespace Venturis.Droid
{
    public class HybridWebViewRenderer : ViewRenderer<HybridWebView, Android.Webkit.WebView>
    {
        const string JavascriptFunction = "function invokeCSharpAction(data){jsBridge.invokeAction(data);}";
        Context _context;

        public HybridWebViewRenderer(Context context) : base(context)
        {
            _context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<HybridWebView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                Control.RemoveJavascriptInterface("jsBridge");
                var hybridWebView = e.OldElement as HybridWebView;
                hybridWebView.Cleanup();
            }
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    var webView = new Android.Webkit.WebView(_context);
                    webView.Settings.JavaScriptEnabled = true;
                    webView.SetWebViewClient(new JavascriptWebViewClient($"javascript: {JavascriptFunction}"));
                    SetNativeControl(webView);
                }
                Control.AddJavascriptInterface(new JSBridge(this), "jsBridge");
                Control.LoadUrl($"{Element.Uri}");
            }
        }
    }
}