
namespace Venturis.Droid
{
    using Android.App;
    using Android.Content.PM;
    using Android.OS;
    using Android.Widget;
    using System.Threading;
    using System.Threading.Tasks;

    [Activity(Theme = "@style/SplashTheme",
              MainLauncher = true,
              NoHistory = true,
              ConfigurationChanges = ConfigChanges.ScreenSize)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SplashScreen);
            FindViewById<TextView>(Resource.Id.txtAppVersion).Text = $"Version {PackageManager.GetPackageInfo(PackageName, 0).VersionName}";
            // Create your application here
            Task.Run(() =>
            {
                Thread.Sleep(3000); // Simulate a long loading process on app startup.
                RunOnUiThread(() =>
                {
                    StartActivity(typeof(MainActivity));
                });
            });
        }
    }
}