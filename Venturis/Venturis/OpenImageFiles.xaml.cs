using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace Venturis
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OpenImageFiles : ContentPage
	{
		public OpenImageFiles (string imageLocation)
		{
			InitializeComponent ();
            backgroundImage.Source = imageLocation;
            
        }

        public async Task bEnvio()
        {
            bool answer = await DisplayAlert("Confirmación?", "Esta seguro de enviar esta imagen como comprobante?", "Si", "No");
            if (answer)
            {
                var current = Connectivity.NetworkAccess;

                if (current == NetworkAccess.Internet)
                {
                    await DisplayAlert("Información", "La imagen fue enviada exitosamente", "OK");
                }
                else
                {
                    await DisplayAlert("Información", "La imagen sera enviada, cuando exista conexion de internet", "OK");
                }

            }

        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            bEnvio();
        }
    }
}