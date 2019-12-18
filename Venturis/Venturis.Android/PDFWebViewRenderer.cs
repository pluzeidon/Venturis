using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Venturis.Droid;
using Venturis.PDF;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(PDFWebView), typeof(PDFWebViewRenderer))]
namespace Venturis.Droid
{
    public class PDFWebViewRenderer : WebViewRenderer
    {
        public PDFWebViewRenderer(Context context) : base(context)
        {

        }
        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var customWebView = Element as PDFWebView;
                Control.Settings.AllowUniversalAccessFromFileURLs = true;

                var fileName = $"file:///{customWebView.Uri}";
                Control.LoadUrl($"file:///android_asset/pdfjs/web/viewer.html?file={fileName}");
                //Control.LoadUrl(string.Format("file:///android_asset/pdfjs/web/viewer.html?file={0}", string.Format("file:///android_asset/Content/{0}", WebUtility.UrlEncode(customWebView.Uri))));
            }
        }
    }
}