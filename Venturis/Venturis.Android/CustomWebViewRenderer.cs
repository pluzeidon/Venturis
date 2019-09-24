[assembly: Xamarin.Forms.ExportRenderer(typeof(Venturis.ViewModels.customWebView), typeof(Venturis.Droid.CustomWebViewRenderer))]

namespace Venturis.Droid
{
    using Android.App;
    using Android.Content;
    using Android.Provider;
    using Android.Webkit;
    using Java.Interop;
    using Java.IO;
    using System;
    using Venturis.ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Platform.Android;
    using Venturis.Interfaces;
    public class CustomWebViewRenderer : WebViewRenderer
    {
        private static int FILECHOOSER_RESULTCODE = 1;

        public CustomWebViewRenderer(Context context) : base(context)
        {

        }

        protected override Android.Webkit.WebView CreateNativeControl()
        {
            Android.Webkit.WebView webView = base.CreateNativeControl();
            webView.Settings.JavaScriptEnabled = true;
            webView.AddJavascriptInterface(new JavaScriptInterface(Element as customWebView), "Test");
            return webView;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.WebView> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                ((customWebView)e.NewElement).SetTextScaned = SetTextScaned;
            }

            if (e.OldElement != null)
            {
                ((customWebView)e.OldElement).SetTextScaned = null;
                ((customWebView)e.OldElement).DoSearch = null;
                ((customWebView)e.OldElement).Observe = null;
            }        

            var chromeClient = new FileChooserWebChromeClient((uploadMsg, acceptType, capture) => {
                MainActivity.UploadMessage = uploadMsg;                
                File imageStorageDir = new File(global::Android.OS.Environment.GetExternalStoragePublicDirectory(global::Android.OS.Environment.DirectoryPictures), "");
                if (!imageStorageDir.Exists())
                {
                    imageStorageDir.Mkdir();
                }
                File file = new File(imageStorageDir + File.Separator + "VC" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg");
                MainActivity.mCapturedImageURI = Android.Net.Uri.FromFile(file);

                Intent captureIntent = new Intent(Android.Provider.MediaStore.ActionImageCapture);
                captureIntent.PutExtra(MediaStore.ExtraOutput, MainActivity.mCapturedImageURI);

                var i = new Intent(Intent.ActionGetContent);
                i.AddCategory(Intent.CategoryOpenable);
                i.SetType("image/*");
                var chooserIntent = Intent.CreateChooser(i, "Choose image");
                chooserIntent.PutExtra(Intent.ExtraInitialIntents, new Intent[] { captureIntent });
                ((MainActivity)this.Context).StartActivityForResult(chooserIntent, FILECHOOSER_RESULTCODE);
            });
            Control.SetWebChromeClient(chromeClient);
        }

        private void SetTextScaned(string text)
        {
            string javascript = $"javascript: document.getElementById('P130_RESULTADO').value = '{text}'; beep(); apex.submit('SCAN');";
            Control.EvaluateJavascript(javascript, null);
        }

    }

    public class JavaScriptInterface : Java.Lang.Object
    {
        private customWebView customwebView;
        public JavaScriptInterface(customWebView customwebView) : base()
        {
            this.customwebView = customwebView;
        }

        [JavascriptInterface()]
        [Export("ScanStarted")]
        public void ScanStarted()
        {
            customwebView.OnStartScanning();
        }

        [JavascriptInterface()]
        [Export("SendUserId")]
        public void SendUserId(string userId)
        {
            customwebView.OnSendUserId(userId);
            MyFirebaseMessagingService myFirebaseMessagingService = new MyFirebaseMessagingService();
            MainActivity a = new MainActivity();
            a.RegisterDevice(userId);
        }

    }
}