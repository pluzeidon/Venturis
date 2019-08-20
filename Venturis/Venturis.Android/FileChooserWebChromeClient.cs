using Android.Webkit;
using System;


namespace Venturis.Droid
{
    public class FileChooserWebChromeClient : WebChromeClient
    {
        private Action<IValueCallback, Java.Lang.String, Java.Lang.String> callback;

        public FileChooserWebChromeClient(Action<IValueCallback, Java.Lang.String, Java.Lang.String> callback2)
        {
            callback = callback2;
        }

        // For Android < 5.0
        [Java.Interop.Export]
        public void openFileChooser(IValueCallback uploadMsg, Java.Lang.String acceptType, Java.Lang.String capture)
        {
            callback(uploadMsg, acceptType, capture);
        }

        // For Android > 5.0
        public override Boolean OnShowFileChooser(Android.Webkit.WebView webView, IValueCallback uploadMsg,
            WebChromeClient.FileChooserParams fileChooserParams)
        {
            try
            {
                callback(uploadMsg, null, null);
            }
            catch (Exception e)
            {

            }
            return true;
        }
    }
}