using Venturis.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Venturis
{
    public partial class App : Application
    {
        NavigationPage _navigationRoot;
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new VenturisNavigator()); 


            //MainPage = new NavigationPage(new Views.VenturisOriginal("https://www.venturisapp.net/ords/pdb1/f?p=111"));
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        //public void DisplayThePDF(string url)
        //{
        //    //var openFilePage = new OpenFilesPage(url); ;
        //    MainPage = new OpenFilesPage(url);
            
        //}

        //public void DisplayTheImage(string url)
        //{
        //    //var openImageFiles = new OpenImageFiles(url); ;
        //    MainPage = new OpenImageFiles(url);
        //}

        public void PresentFileInfo(string url)
        {
            //MainPage = new PresentFileInfo(url);
            MainPage = new NavigationPage(new PresentFileInfo(url));
        }
    }
}
