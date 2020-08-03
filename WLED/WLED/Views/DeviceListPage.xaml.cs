using System;
using System.Collections.Generic;


using Xamarin.Forms;

namespace WLED.Views
{
    public partial class DeviceListPage : ContentPage
    {
        public DeviceListPage()
        {
            InitializeComponent();
            BindingContext = App.DeviceViewModel;
            
        }

        private async void Handle_DeviceTapped(object sender, EventArgs e)
        {
            //Deselect Item immediately
            ((ListView)sender).SelectedItem = null;

            if (!(sender is WLEDDevice targetDevice)) return;

            string url = "http://" + targetDevice.NetworkAddress;

            //Open web UI control page
            var page = new DeviceControlPage(url, targetDevice);
            await Navigation.PushModalAsync(page, false);
        }
    }
}
