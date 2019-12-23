using Refit;
using System;
using System.IO;
using System.Threading.Tasks;
using Venturis.Interfaces;
using Venturis.Model;
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
        IMyAPI myAPI;
        string sMIME_TYPE;
        public PresentFileInfo(string fileLocation)
        {
            InitializeComponent();
            _fileLocation = fileLocation;
            FileInfo fi = new FileInfo(fileLocation);
            txtArchivo.Text = fi.Name;
            if (fi.FullName.Contains(".pdf"))
            {
                txtTipo.Text = "PDF";
                sMIME_TYPE = "application/pdf";
                Vizualizar.IsEnabled = true;
            }
            else if (fi.FullName.Contains(".xml"))
            {
                txtTipo.Text = "XML";
                sMIME_TYPE = "application/xml";
                Vizualizar.IsEnabled = false;
            }
            else
            {
                txtTipo.Text = "Imagen";
                sMIME_TYPE = "image/jpeg";
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
                await Navigation.PushAsync(new OpenFilesPage(_fileLocation));
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
                    try
                    {
                        myAPI = RestService.For<IMyAPI>("https://venturisapp.net/aplpr01/xpm/comprobantes");
                        PostContent post = new PostContent();
                        var docsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                        var filePath = System.IO.Path.Combine(docsPath, "userid.txt");
                        string readText = File.ReadAllText(filePath);
                        post.sUSUARIO = readText.Substring(0,readText.Length-1);
                        post.sFILE_NAME = txtArchivo.Text;
                        post.sMIME_TYPE = sMIME_TYPE;
                        Byte[] bytes = File.ReadAllBytes(_fileLocation);
                        post.sCOMPROBANTE = Convert.ToBase64String(bytes);
                        await myAPI.SumitPost(post);
                        await DisplayAlert("Información", "El Archivo fue enviado exitosamente", "OK");
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", "" + ex.Message, "OK");
                    }
                    
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