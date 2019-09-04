using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
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
            Solicitar_Permisos();
            cwv.Source = Url = GetUrl();
        }
        private void Cwv_InitScan(object sender, EventArgs e)
        {
            Scanner();
        }

        private async void Solicitar_Permisos()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                        await DisplayAlert("Need Camera", "Gunna need", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Camera))
                        status = results[Permission.Camera];
                }

                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Camera Denied", "Can not continue, try again.", "OK");
                }
            }
            catch (Exception ex)
            {
                return;
            }
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
            return "https://www.venturisapp.net/ords/pdb1/f?p=111:1:13489738520152::NO:1::";

            //return  "http://lascalzadashotelsuites.alsondelossantos.com/click.html";
        }
    }
}
