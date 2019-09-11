﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Webkit;
using Xamarin.Forms;

namespace Venturis.Droid
{
    [Activity(Label = "Venturis", 
              Icon = "@drawable/Icon",
              Theme = "@style/MainTheme", 
              MainLauncher = false, 
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static IValueCallback UploadMessage;
        private static int FILECHOOSER_RESULTCODE = 1;
        public static Android.Net.Uri mCapturedImageURI;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            //Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;
            base.OnCreate(savedInstanceState);
            //SetContentView(Resource.Layout.SplashScreen);
            Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            //global::ZXing.Net.Mobile.Forms.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            if (requestCode == FILECHOOSER_RESULTCODE)
            {
                if (null == UploadMessage)
                    return;
                Java.Lang.Object result = data == null ? mCapturedImageURI : data.Data;

                UploadMessage.OnReceiveValue(new Android.Net.Uri[] { (Android.Net.Uri)result });
                UploadMessage = null;
            }
            else
                base.OnActivityResult(requestCode, resultCode, data);
        }

    }
}