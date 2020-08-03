using System;

using Xamarin.Forms;

namespace WLED.Utils
{
    public class FontAwesomeIcons : ContentPage
    {
        public FontAwesomeIcons()
        {
            Content = new StackLayout
            {
                Children = {
                    new Label { Text = "Hello ContentPage" }
                }
            };
        }
    }
}

