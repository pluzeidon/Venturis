using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Venturis
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PresentFileInfo : ContentPage
    {
        string _fileLocation;
        NavigationPage _navigationRoot;
        public PresentFileInfo(string fileLocation)
        {
            InitializeComponent();
            _fileLocation = fileLocation;
            FileInfo fi = new FileInfo(fileLocation);
            txtArchivo.Text = fi.Name;
            if (fi.FullName.Contains(".pdf"))
            {
                txtTipo.Text = "PDF";
                Vizualizar.IsEnabled = true;
            }
            else if (fi.FullName.Contains(".xml"))
            {
                txtTipo.Text = "XML";
                Vizualizar.IsEnabled = false;
            }
            else 
            {
                txtTipo.Text = "Imagen";
                Vizualizar.IsEnabled = true;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {

        }

        private void Vizualizr_Clicked(object sender, EventArgs e)
        {

        }

        async void Enviar_Clicked(object sender, EventArgs e)
        {
            await bEnvio();
        }

        async void Vizualizar_Clicked(object sender, EventArgs e)
        {
            if (txtTipo.Text == "PDF")
            {
                await Navigation.PushAsync(new OpenFilesPage(_fileLocation) );
            }
            else
            {
                await Navigation.PushAsync(new OpenImageFiles(_fileLocation));
            }
        }

        public async Task bEnvio()
        {
            bool answer = await DisplayAlert("Confirmación?", "Está seguro de enviar este Archivo como comprobante?", "Si", "No");
            if (answer)
            {
                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)
                {
                    await DisplayAlert("Información", "El Archivo fue enviado exitosamente", "OK");
                }
                else
                {
                    await DisplayAlert("Información", "El Archivo será enviado, cuando exista conexión de internet", "OK");
                }
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }

        }
    }
}