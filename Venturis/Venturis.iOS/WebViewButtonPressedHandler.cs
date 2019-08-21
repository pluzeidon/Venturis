
namespace Venturis.iOS
{
    using Foundation;
    using Venturis.ViewModels;
    using WebKit;
    public class WebViewButtonPressedHandler : NSObject, IWKScriptMessageHandler
    {
        private customWebView customWebView;

        public WebViewButtonPressedHandler(customWebView customWebView)
        {
            this.customWebView = customWebView;
        }

        public void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            customWebView.OnStartScanning();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                customWebView = null;
            }
        }
    }
}