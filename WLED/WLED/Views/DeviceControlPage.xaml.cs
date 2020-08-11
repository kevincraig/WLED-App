using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLED.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WLED.Views
{
    //Query properties
    [QueryProperty("PageUrl", "pageUrl")]
    //Viewmodel: Open a web view that loads the mobile UI natively hosted on WLED device
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DeviceControlPage : ContentPage
	{

        private bool _isWorking;
        public bool IsWorking
        {
            get => _isWorking;
            set => _isWorking = value;
        }
        

        private string _pageUrl;
        public string PageUrl
        {
            get => _pageUrl;
            set => _pageUrl = Uri.EscapeUriString(value);
        }

      
        
        public DeviceControlPage()
		//public DeviceControlPage (string pageURL, WLEDDevice device)
		{
			InitializeComponent ();
            //currentDevice = device;
            //if (currentDevice == null) loadingLabel.Text = "Loading... (WLED-AP)"; //If the device is null, we are connected to the WLED light's access point
            UIBrowser.Navigating += OnNavigating;
            UIBrowser.Navigated += OnNavigationCompleted;
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            loadingLabel.IsVisible = true;
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            UIBrowser.Source = $"http://{PageUrl}";
        }

        private void OnNavigating(object sender, WebNavigatingEventArgs e)
        {
           
        }

       

        private void OnNavigationCompleted(object sender, WebNavigatedEventArgs e)
        {
            loadingLabel.IsVisible = false;
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;
            UIBrowser.IsVisible = true;

            //if (e.Result == WebNavigationResult.Success)
            //{
            //    loadingLabel.IsVisible = false;
            //    if (currentDevice != null) currentDevice.CurrentStatus = DeviceStatus.Default;
            //} else
            //{
            //    if (currentDevice != null) currentDevice.CurrentStatus = DeviceStatus.Unreachable;
            //    loadingLabel.IsVisible = true;
            //    loadingLabel.Text = "Device Unreachable";
            //}
        }

        private async void OnBackButtonTapped(object sender, EventArgs e)
        {
            //await Navigation.PopModalAsync(false);
            //currentDevice?.Refresh(); //refresh device list item to apply changes made in the control page
        }
    }
}