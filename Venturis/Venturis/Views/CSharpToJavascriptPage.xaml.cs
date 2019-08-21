using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace Venturis.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CSharpToJavascriptPage : ContentPage
    {
        string Url;

        public CSharpToJavascriptPage()
        {
            InitializeComponent();
            browser.Source = Url = GetUrl();
        }

        void OnBack(object sender, EventArgs args)
        {
            browser.GoBack();
        }

        void OnJavaScript(object sender, EventArgs args)
        {
            Scanner();
        }

        private async void Scanner()
        {
            var scannerPage = new ZXingScannerPage();
            scannerPage.Title = "Presente el codigo de barras CAT";
            scannerPage.OnScanResult += (result) =>
            {
                scannerPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopAsync();
                    browser.Eval("SetValue('" + result.Text + "');");
                });
            };
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(scannerPage);
            });
        }

        protected override bool OnBackButtonPressed()
        {
            if (browser.CanGoBack)
            {
                browser.GoBack();
                return true;
            }
            else return base.OnBackButtonPressed();
        }

        void OnNavigating(object sender, WebNavigatingEventArgs args)
        {
            // Comprobando si estamos en la URL de la página de inicio
            // browser.CanGoBack no parece funcionar (no se actualiza a tiempo)
            NavigationPage.SetHasNavigationBar(this, args.Url != Url);
        }

        string GetUrl()
        {
            return "http://lascalzadashotelsuites.alsondelossantos.com/index.html";
            //return "https://www.venturisapp.net/ords/pdb1/f?p=111";

        }
    }
}