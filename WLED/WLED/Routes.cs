using System;
using Xamarin.Forms;

namespace WLED
{
    public class Routes
    {
        public static void RegisterRoutes()
        {
            Routing.RegisterRoute("addDevice", typeof(DeviceAddPage));
        }
    }
}
