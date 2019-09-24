using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Venturis.Interfaces;
using Xamarin.Forms;

[assembly: Dependency (typeof(Venturis.Droid.Implementations.RegistrationDevice))]

namespace Venturis.Droid.Implementations
{
    public class RegistrationDevice : IRegisterDevice
    {
        
        public void RegisterDevice(string userId)
        {
            var mainActivity = MainActivity.GetInstance();
            MyFirebaseMessagingService myFirebaseMessagingService = new MyFirebaseMessagingService();
            myFirebaseMessagingService.RegisterDevice(userId, mainActivity);
        }
    }
}