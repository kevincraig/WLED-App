using System;
using System.Collections.Generic;
using Xamarin.Forms;
using WLED.ViewModels;

namespace WLED.Views
{
    public partial class ColorPicker : ContentView
    {
        private DeviceViewModel viewModel;

        private Color selectedColor;

        public ColorPicker()
        {
            InitializeComponent();

            viewModel = App.DeviceViewModel;
        }
              
        private void ColorPicker_PickedColorChanged(object sender, Color colorPicked)
        {
            selectedColor = colorPicked;   
        }

        async void OnSetColor(System.Object sender, System.EventArgs e)
        {
            try
            {
                Utils.Log("Setting Color");
                var r = selectedColor.R * 255;//Multiple by 255 to get RGB values
                var g = selectedColor.G * 255;
                var b = selectedColor.B * 255;
                   
                await viewModel.CurrentDevice.SendAPICall($"&R={r}&G={g}&B={b}");
            }
            catch (Exception ex)
            {
                Utils.Log($"Exception setting Color: {ex.Message}");
            }
            
        }

        void OnClose(object sender, EventArgs e)
        {
            this.IsVisible = false;
        }

    }
}
