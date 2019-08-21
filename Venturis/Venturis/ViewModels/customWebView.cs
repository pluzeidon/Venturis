using System;
using Xamarin.Forms;

namespace Venturis.ViewModels
{
    public class customWebView : WebView
    {
        public Action<string> SetTextScaned;
        public Action DoSearch;
        public Action Observe;

        public event EventHandler InitScan;
        public event EventHandler SearchPreseed;

        //public void OnSearchBtnPressed()
        //{
        //    SearchPreseed?.Invoke(this, null);
        //}

        public void OnStartScanning()
        {
            InitScan.Invoke(this, null);
        }
    }
}
