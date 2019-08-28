
namespace Venturis.Droid
{
    using Android.App;
    using Android.OS;

    [Activity(Theme = "@style/Theme.Splash",
              MainLauncher =true,
              NoHistory =true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            System.Threading.Thread.Sleep(1000);
            this.StartActivity(typeof(MainActivity));

            // Create your application here
        }
    }
}