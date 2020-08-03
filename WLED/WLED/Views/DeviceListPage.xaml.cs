using System;
using System.Collections.Generic;
using WLED.Models;
using WLED.ViewModels;
using Xamarin.Forms;

namespace WLED.Views
{
    public partial class DeviceListPage : ContentPage
    {
        private DeviceViewModel viewModel;

        public DeviceListPage()
        {
            InitializeComponent();
           
            
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindingContext = App.DeviceViewModel;
            viewModel = App.DeviceViewModel;

        }

        private async void Handle_DeviceTapped(object sender, ItemTappedEventArgs e)
        {
            //Deselect Item immediately
            ((ListView)sender).SelectedItem = null;

            if (!(e.Item is WLEDDevice targetDevice)) return;

            string url = "http://" + targetDevice.NetworkAddress;

            //Open web UI control page
            var page = new DeviceControlPage(url, targetDevice);
            await Navigation.PushModalAsync(page, false);
        }

        async void OnAddButtonTapped(Object sender, EventArgs e)
        {
            DeviceAddPage.DeviceCreated -= OnDeviceCreated;
            DeviceAddPage.DeviceCreated += OnDeviceCreated;
            await Shell.Current.GoToAsync("addDevice");
        }

        private void OnDeviceCreated(object sender, DeviceCreatedEventArgs e)
        {
            if (viewModel == null)
            {
                viewModel = App.DeviceViewModel;
            }
            viewModel.CreateDevice.Execute(e.CreatedDevice);  
        }

        void OnPowerButtonTapped(System.Object sender, EventArgs e)
        {
            Console.WriteLine("Power Button Tapped");
        }
    }
}
