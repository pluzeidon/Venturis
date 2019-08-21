using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace Venturis.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HybridWebViewPage : ContentPage
    {
        string Url = string.Empty;
        public HybridWebViewPage()
        {
            InitializeComponent();
            browser.Uri = Url = GetUrl();
            browser.RegisterAction(data => Scanner());
        }

        private async void Scanner()
        {
            var scannerPage = new ZXingScannerPage();
            scannerPage.Title = "Presente el codigo de barras";
            scannerPage.OnScanResult += (result) =>
            {
                scannerPage.IsScanning = false;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    browser.Eval("SetValue('" + result.Text + "');");
                });
            };
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(scannerPage);
            });
        }

        string GetUrl()
        {
            return "http://lascalzadashotelsuites.alsondelossantos.com/JS_CSharp.html";
        }

    }
}