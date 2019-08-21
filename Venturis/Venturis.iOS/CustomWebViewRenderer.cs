

namespace Venturis.iOS
{
    using Foundation;
    using System.ComponentModel;
    using Venturis.ViewModels;
    using WebKit;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.iOS;
    public class CustomWebViewRenderer : ViewRenderer<customWebView, WKWebView>
    {
        protected WKWebView wkWebView;
        protected WKUserContentController userController;
        protected WebViewButtonPressedHandler buttonPressedScriptHandler;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                userController.RemoveAllUserScripts();
                userController.RemoveScriptMessageHandler("ButtonPressedScriptHandler");
                buttonPressedScriptHandler.Dispose();
                userController.Dispose();
                wkWebView.Dispose();
            }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<customWebView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                e.OldElement.SetTextScaned = null;
                e.OldElement.DoSearch = null;
                e.OldElement.Observe = null;
            }
            if (e.NewElement != null)
            {
                if (Control == null)
                {
                    userController = new WKUserContentController();
                    var config = new WKWebViewConfiguration { UserContentController = userController };
                    wkWebView = new WKWebView(Frame, config);
                    SetNativeControl(wkWebView);

                    buttonPressedScriptHandler = new WebViewButtonPressedHandler(e.NewElement);
                    userController.AddScriptMessageHandler(buttonPressedScriptHandler, "ButtonPressedScriptHandler");
                }

                e.NewElement.SetTextScaned = SetSearchText;
                e.NewElement.DoSearch = DoSearch;
                e.NewElement.Observe = Observe;

                if (e.NewElement.Source != null)
                {
                    Control.LoadRequest(new NSUrlRequest(new NSUrl((Element.Source as UrlWebViewSource)?.Url)));
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == WebView.SourceProperty.PropertyName &&
                Element?.Source != null &&
                (Element.Source as UrlWebViewSource)?.Url != null &&
                (Element.Source as UrlWebViewSource)?.Url != Control?.Url?.AbsoluteString)
            {
                Control.LoadRequest(new NSUrlRequest(new NSUrl((Element.Source as UrlWebViewSource)?.Url)));
            }
        }

        private void SetSearchText(string text)
        {
            string javascript = $"javascript: document.getElementById('sb_form_q').value = '{text}';";
            Control.EvaluateJavaScript(javascript, null);
        }

        private void DoSearch()
        {
            string javascript = "javascript: document.getElementById('sbBtn').click();";
            Control.EvaluateJavaScript(javascript, null);
        }

        private void Observe()
        {
            string javascript = "javascript: document.getElementById('sb_form_go').onclick = function() { window.webkit.messageHandlers.ButtonPressedScriptHandler.postMessage(''); };";
            Control.EvaluateJavaScript(javascript, null);
        }
    }
}