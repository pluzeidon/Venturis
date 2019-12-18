
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Venturis
{
    public partial class OpenFilesPage : ContentPage
    {
        public OpenFilesPage(string pdfLocation)
        {
            InitializeComponent();
            theWebView.Uri = pdfLocation;
            
        }

        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PopModalAsync(true);
        }


    }
}