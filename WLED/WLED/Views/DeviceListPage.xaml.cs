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
            await Shell.Current.GoToAsync("addDevice");
        }

       
        void OnPowerButtonTapped(System.Object sender, EventArgs e)
        {
            Console.WriteLine("Power Button Tapped");
            Button s = sender as Button;
            WLEDDevice d = new WLEDDevice();
            if (s.Parent.BindingContext is WLEDDevice targetDevice)
            {
                d = targetDevice;
                viewModel.TogglePower?.Execute(targetDevice);

            }
            else
            {
                DisplayAlert($"{d.Name}", "We were unable to toggle power, please try again later.", "OK");
            }

            
        }
    }
}
