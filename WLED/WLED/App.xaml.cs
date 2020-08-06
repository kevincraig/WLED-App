using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using System.Linq;
using WLED.Views;
using WLED.ViewModels;
using Xamarin.Forms.Internals;
using System.Diagnostics;
using WLED.Models;

/*
 * WLED App v1.0.2
 * (c) 2019 Christian Schwinne
 * Licensed under the MIT license
 * 
 * This project was build for and tested with Android, iOS and UWP.
 */

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace WLED
{
    public partial class App : Application
    {
        
        //ViewModel Instances
        public static DeviceViewModel DeviceViewModel { get; private set; }

        private bool connectedToLocalLast = false;

        public App()
        {
            //Public ViewModel Instance
            DeviceViewModel = new DeviceViewModel();
            Xamarin.Forms.Internals.Log.Listeners.Add(new DelegateLogListener((arg1, arg2) => Debug.WriteLine(arg2)));
            InitializeComponent();

            //listview = new DeviceListViewPage();
            //MainPage = listview;
            //MainPage.SetValue(NavigationPage.BarTextColorProperty, Color.White);

            MainPage = new AppShell();
            Application.Current.MainPage.SetValue(NavigationPage.BarBackgroundColorProperty, "#0000AA");

            Connectivity.ConnectivityChanged += OnConnectivityChanged;

          
        }

        protected override void OnStart()
        {
            //Todo Rework OnStart()
            //Directly open the device web page if connected to WLED Access Point
            //if (NetUtility.IsConnectedToWledAP()) listview.OpenAPDeviceControlPage();

            if (Preferences.ContainsKey("wleddevices"))
            {
                App.DeviceViewModel.GetCachedDevices?.Execute(null);
            }
        }

        protected override void OnSleep()
        {
            //Todo Rework OnSleep()
            //Handle when app sleeps, save device list to Preferences
            App.DeviceViewModel.CacheDevices?.Execute(null);
        }

        protected override void OnResume()
        {
            //Todo Rework OnResume()
            //Handle when app resumes, directly open the device web page if connected to WLED Access Point
            //if (NetUtility.IsConnectedToWledAP()) listview.OpenAPDeviceControlPage();

            ////Refresh light states
            //listview.RefreshAll();
            App.DeviceViewModel.RefreshAll();
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            //Todo Rework OnConnectivityChanged()
            //Detect if currently connected to local (WiFi) or mobile network
            //var profiles = Connectivity.ConnectionProfiles;
            //bool connectedToLocal = (profiles.Contains(ConnectionProfile.WiFi) || profiles.Contains(ConnectionProfile.Ethernet));

            ////Directly open the device web page if connected to WLED Access Point
            //if (connectedToLocal && NetUtility.IsConnectedToWledAP()) listview.OpenAPDeviceControlPage();

            ////Refresh all devices on connection change
            //if (connectedToLocal && !connectedToLocalLast) listview.RefreshAll();
            //connectedToLocalLast = connectedToLocal;
        }
    }
}