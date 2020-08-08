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
        #region Properties    
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

        //public Color ColorCurrent { get; set; }

        //private bool _stateCurrent;
        //public bool StateCurrent
        //{
        //    get => _stateCurrent;
        //    set => SetPropertyValue(ref _stateCurrent, value);
        //}

        //public Color StateColor
        //{
        //    get => StateCurrent ? Color.FromHex("#4af2a1") : Color.FromHex("#515051");
        //}
        #endregion


        #region Commands
        public ICommand CreateDevice => new Command((obj) => OnCreateDevice());
        public ICommand CacheDevices => new Command((obj) => OnCacheDevices());
        public ICommand DeleteDevice => new Command((obj) => OnDeleteDevice(obj));
        public ICommand GetCachedDevices => new Command((obj) => OnGetCachedDevices());
        public ICommand ResortDevices => new Command((obj) => OnResortDevices());
        public ICommand TogglePower => new Command((obj) => OnTogglePower(obj));
        
        #endregion


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
                Utils.Log($"Device Count: {DeviceList.Count}");
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

        private async void OnTogglePower(object d)
        {
            WLEDDevice device = d as WLEDDevice;            
            await device.SendAPICall("T=2");
        }


        private async void OnDeleteDevice(object d)
        {

            DeviceList.Remove(d as WLEDDevice);
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
