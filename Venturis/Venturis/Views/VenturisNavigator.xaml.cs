using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
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
            GetPermissionsAll();
            cwv.Source = Url = GetUrl();
            //cwv.Source = (cwv.Source as UrlWebViewSource).Url;
            //cwv.HeightRequest = 500;
            //cwv.WidthRequest = 500;
            //trubio@quadrantix.com
        }

        

        //protected async override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    await activity_indicator.ProgressTo(0.9, 900, Easing.SpringIn);

        //}
        private void Cwv_InitScan(object sender, EventArgs e)
        {
            Scanner();
        }

        private async void Solicitar_Permisos_Camara()
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

        private async void Solicitar_Permisos_MediaLibrary()
        {
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Storage))
                    {
                        await DisplayAlert("Need Camera", "Gunna need", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Storage);
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(Permission.Storage))
                        status = results[Permission.Storage];
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
            //activityIndicator.IsRunning = true;
            //activityIndicator.IsVisible = true;
            //cwv.IsEnabled = false;
            //NavigationPage.SetHasNavigationBar(this, args.Url != Url);
        }

        void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            //if (e.Result == WebNavigationResult.Success)
            //{
            //    activityIndicator.IsRunning = false;
            //    activityIndicator.IsVisible = false;
            //    cwv.IsEnabled = true;
            //}
        }

        string GetUrl()
        {
            //return "https://www.venturisapp.net/ords/pdb1/f?p=111:1:13489738520152::NO:1::";

            //return "http://129.213.116.148:8180/aplpr01/f?p=111";

            //return "https://qdxcloud01.net/aplpr01/f?p=111:1:10107162832254::NO:1::";  // CAT 31-10-2019

            return "https://venturisapp.net/aplpr01/f?p=111";  // CAT 31-10-2019

            //return  "http://venturisexpenses-001-site1.etempurl.com/login.aspx";
        }

        private async void GetPermissionsAll()
        {
            await GetPermissions();
        }

        public static async Task<bool> GetPermissions()
        {
            bool permissionsGranted = true;

            var permissionsStartList = new List<Permission>()
        {
            Permission.Location,
            Permission.LocationAlways,
            Permission.LocationWhenInUse,
            Permission.Storage,
            Permission.Camera
        };

            var permissionsNeededList = new List<Permission>();
            try
            {
                foreach (var permission in permissionsStartList)
                {
                    var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);
                    if (status != PermissionStatus.Granted)
                    {
                        permissionsNeededList.Add(permission);
                    }
                }
            }
            catch (Exception ex)
            {
                string messge = ex.Message;
            }

            var results = await CrossPermissions.Current.RequestPermissionsAsync(permissionsNeededList.ToArray());

            try
            {
                foreach (var permission in permissionsNeededList)
                {
                    var status = PermissionStatus.Unknown;
                    //Best practice to always check that the key exists
                    if (results.ContainsKey(permission))
                        status = results[permission];
                    if (status == PermissionStatus.Granted || status == PermissionStatus.Unknown)
                    {
                        permissionsGranted = true;
                    }
                    else
                    {
                        permissionsGranted = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return permissionsGranted;
        }

        private void Cwv_InitSendUserId(object sender, EventArgs e)
        {

        }
        

    }
}
