using System;
using Xamarin.Forms;
using WLED.Views;

namespace WLED
{
    public class Routes
    {
        public static void RegisterRoutes()
        {
            Routing.RegisterRoute("addDevice", typeof(DeviceAddPage));
            Routing.RegisterRoute("embeddedControl", typeof(DeviceControlPage));
        }
    }
}
