using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace Venturis.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VenturisNavigator : ContentPage
    {
        string Url;

        public VenturisNavigator()
        {
            InitializeComponent();
            cwv.Source = Url = GetUrl();
        }
        private void Cwv_InitScan(object sender, EventArgs e)
        {
            Scanner();
        }

        private async void Scanner()
        {
            string _result = string.Empty;
            var scannerPage = new ZXingScannerPage();
            scannerPage.Title = "Presente el codigo de barras";
            scannerPage.OnScanResult += (result) =>
            {
                scannerPage.IsScanning = false;
                Task.Run(() => Device.BeginInvokeOnMainThread(() =>
                {
                    Navigation.PopAsync();
                    //_result = result.Text;
                    cwv.SetTextScaned(result.Text);

                    //string strDireccion = cwv.Source.GetValue(UrlWebViewSource.UrlProperty).ToString();

                    //string strResultado = Base64Encode(result.Text);

                    //if (strResultado != "")
                    //    strDireccion = strDireccion + strResultado;

                    //cwv.Source = strDireccion;
                }));
            };
            Device.BeginInvokeOnMainThread(() =>
            {
                Navigation.PushAsync(scannerPage);
            });
        }

        static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        //void OnBack(object sender, EventArgs args)
        //{
        //    cwv.GoBack();
        //}

        protected override bool OnBackButtonPressed()
        {
            if (cwv.CanGoBack)
            {
                cwv.GoBack();
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
            return "https://qdxcloud01.net/ords/pdb1/f?p=111:LOGIN_DESKTOP:8150988639774::::";
            //return  "http://lascalzadashotelsuites.alsondelossantos.com/click.html";
        }
    }
}
