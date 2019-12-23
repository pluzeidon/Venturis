using Android.App;
using Android.Content.PM;
using Android.Content;
using Android.OS;
using Android.Webkit;
using Xamarin.Forms;
using Android.Util;
using Android.Gms.Common;
using Venturis.Interfaces;
using System;
using System.IO;

namespace Venturis.Droid
{
    [Activity(Label = "Venturis",
              Icon = "@drawable/Icon",
              Theme = "@style/MainTheme",
              MainLauncher = false,
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = @"application/pdf")]
    [IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = @"image/*")]
    //[IntentFilter(new[] { Intent.ActionSend, Intent.ActionSendMultiple }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = @"image/*")]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        App _mainForms;
        #region Singleton

        private static MainActivity instance;
        public static MainActivity GetInstance()
        {
            if (instance == null)
            {
                instance = new MainActivity();
            }
            return instance;
        }

        #endregion

        public static IValueCallback UploadMessage;
        private static int FILECHOOSER_RESULTCODE = 1;
        public static Android.Net.Uri mCapturedImageURI;
        public const string TAG = "MainActivity";
        internal static readonly string CHANNEL_ID = "my_notification_channel";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            instance = this;
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            //Plugin.CurrentActivity.CrossCurrentActivity.Current.Init(this, savedInstanceState);
            Plugin.CurrentActivity.CrossCurrentActivity.Current.Activity = this;
            base.OnCreate(savedInstanceState);
            if (Intent.Extras != null)
            {
                foreach (var key in Intent.Extras.KeySet())
                {
                    if (key != null)
                    {
                        var value = Intent.Extras.GetString(key);
                        Log.Debug(TAG, "Key: {0} Value: {1}", key, value);
                    }
                }
            }

            IsPlayServicesAvailable();
            CreateNotificationChannel();
            Forms.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            _mainForms = new App();

            LoadApplication(_mainForms);
            Intent intent = Intent;
            string type = intent.Type;
            if (Intent.Action == Intent.ActionSend)
            {
                var pdf = Intent.ClipData.GetItemAt(0);
                var uriFromExtras = Intent.GetParcelableExtra(Intent.ExtraStream) as Android.Net.Uri;
                var subject = Intent.GetStringExtra(Intent.ExtraSubject);
                var pdfStream = ContentResolver.OpenInputStream(pdf.Uri);
                string filename = pdf.Uri.LastPathSegment;
                FileInfo fi = new FileInfo(pdf.Uri.EncodedPath);
                var memOfPdf = new System.IO.MemoryStream();
                pdfStream.CopyTo(memOfPdf);
                var docsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                var filePath = System.IO.Path.Combine(docsPath, filename);
                System.IO.File.WriteAllBytes(filePath, memOfPdf.ToArray());
                _mainForms.PresentFileInfo(filePath);
            }
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

        public bool IsPlayServicesAvailable()
        {
            int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                    Log.Debug(TAG, GoogleApiAvailability.Instance.GetErrorString(resultCode));
                else
                {
                    Log.Debug(TAG, "This device is not supported");
                    Finish();
                }
                return false;
            }

            Log.Debug(TAG, "Google Play Services is available.");
            return true;
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification
                // channel on older versions of Android.
                return;
            }

            var channelName = CHANNEL_ID;
            var channelDescription = string.Empty;
            var channel = new NotificationChannel(CHANNEL_ID, channelName, NotificationImportance.Default)
            {
                Description = channelDescription
            };

            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        public void RegisterDevice(string userId)
        {
            var register = DependencyService.Get<IRegisterDevice>();
            register.RegisterDevice(userId);

            var docsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var filePath = System.IO.Path.Combine(docsPath, "userid.txt");
            System.IO.File.Delete(filePath);
            if (!File.Exists(filePath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(userId);
                }
            }
        }

    }
}