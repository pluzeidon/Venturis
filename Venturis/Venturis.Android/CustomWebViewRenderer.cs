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
    using Android.Net.Http;

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
                this.Control.SetWebViewClient(new MyFormsWebViewClient(this));

            }

            if (e.OldElement != null)
            {
                ((customWebView)e.OldElement).SetTextScaned = null;
                ((customWebView)e.OldElement).DoSearch = null;
                ((customWebView)e.OldElement).Observe = null;
                this.Control.SetWebViewClient(new MyFormsWebViewClient(this));
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

    internal class MyFormsWebViewClient : FormsWebViewClient
    {
        CustomWebViewRenderer _renderer;

        public MyFormsWebViewClient(CustomWebViewRenderer renderer) : base(renderer)
        {
            _renderer = renderer;
        }

        public override void OnReceivedSslError(Android.Webkit.WebView view, SslErrorHandler handler, SslError error)
        {
            handler.Proceed();
            //AlertDialog.Builder builder = new AlertDialog.Builder((Activity)Forms.Context);
            //AlertDialog ad = builder.Create();
            //string message = "Zertifikat Fehler";
            //switch (error.PrimaryError)
            //{
            //    case SslErrorType.Untrusted:
            //        message = "Zertifikat ist nicht vertrauenswürdig."; break;
            //    case SslErrorType.Expired:
            //        message = "Zertifikat ist abgelaufen."; break;
            //    case SslErrorType.Idmismatch:
            //        message = "Zertifikat ID ist fehlerhaft."; break;
            //    case SslErrorType.Notyetvalid:
            //        message = "Zertifikat ist noch nicht gültig."; break;
            //}
            //message += " Möchten Sie trotzdem fortfahren?";
            //ad.SetTitle("SSL Zertifikat Fehler");
            //ad.SetMessage(message);
            //ad.SetButton("OK", (sender, args) =>
            //{
            //    handler.Proceed();
            //});
            //ad.SetButton2("Cancel", (sender, args) =>
            //{
            //    handler.Cancel();
            //});
            //ad.SetIcon(Android.Resource.Drawable.IcDialogAlert);
            //ad.Show();
        }
    }
}