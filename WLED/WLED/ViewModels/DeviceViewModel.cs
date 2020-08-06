using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using WLED.Models;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WLED.ViewModels
{
    public class DeviceViewModel : ViewModelBase
    {
            
        private ObservableCollection<WLEDDevice> _deviceList;
        public ObservableCollection<WLEDDevice> DeviceList
        {
            get => _deviceList;
            set => SetPropertyValue(ref _deviceList, value);
        }

        private WLEDDevice _deviceToCreate;
        public WLEDDevice DeviceToCreate
        {
            get => _deviceToCreate;
            set => SetPropertyValue(ref _deviceToCreate, value);
        }

        public ICommand CreateDevice => new Command((obj) => OnCreateDevice());
        public ICommand CacheDevices => new Command((obj) => OnCacheDevices());
        public ICommand GetCachedDevices => new Command((obj) => OnGetCachedDevices());
        public ICommand ResortDevices => new Command((obj) => OnResortDevices());
           
        private void OnCreateDevice()
        {
            
            if (DeviceToCreate != null)
            {
                foreach (WLEDDevice d in DeviceList)
                {
                    //ensure there is only one device entry per IP
                    if (DeviceToCreate.NetworkAddress.Equals(d.NetworkAddress))
                    {
                        if (DeviceToCreate.NameIsCustom)
                        {
                            d.Name = DeviceToCreate.Name;
                            d.NameIsCustom = true;
                            var index = DeviceList.IndexOf(d);
                            DeviceList.RemoveAt(index);
                            DeviceList.Insert(index, d);
                        }
                        return;
                    }
                }
                DeviceList.Add(DeviceToCreate);
                DeviceList.OrderBy(x => x.Name);
            }
            OnCacheDevices();
        }

        private void OnCacheDevices()
        {
            string devices = Serialization.SerializeObject(DeviceList);
            Preferences.Set("wleddevices", devices);
        }

        private void OnGetCachedDevices()
        {
            string devices = Preferences.Get("wleddevices", "");
            if (!devices.Equals(""))
            {
                ObservableCollection<WLEDDevice> fromPreferences = Serialization.Deserialize(devices);
                if (fromPreferences != null) DeviceList = new ObservableCollection<WLEDDevice>(fromPreferences);
                Console.WriteLine($"Device Count: {DeviceList.Count}");
            }
            RefreshAll();
        }

        private void OnResortDevices()
        {
            if (DeviceList.Count > 0)
            {
                DeviceList.OrderBy(x => x.Name);
            }
        }

        public void RefreshAll()
        {
            foreach (WLEDDevice d in DeviceList) _ = d.Refresh();
        }

        public DeviceViewModel()
        {
            //Initialize Device List
            DeviceList = new ObservableCollection<WLEDDevice>();
        }


    }
}
