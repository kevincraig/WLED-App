using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using WLED.Models;
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

        public ICommand CreateDevice => new Command((obj) => OnCreateDevice(obj));

        private void OnCreateDevice(object obj)
        {
            WLEDDevice toAdd = (WLEDDevice)obj;

            if (toAdd != null)
            {
                foreach (WLEDDevice d in DeviceList)
                {
                    //ensure there is only one device entry per IP
                    if (toAdd.NetworkAddress.Equals(d.NetworkAddress))
                    {
                        if (toAdd.NameIsCustom)
                        {
                            d.Name = toAdd.Name;
                            d.NameIsCustom = true;
                            ReinsertDeviceSorted(d);
                        }
                        return;
                    }
                }
                InsertDeviceSorted(toAdd);
            }
        }

        
        private void ReinsertDeviceSorted(WLEDDevice device)
        {
            
            try
            {
                
                if (DeviceList.Remove(device)) InsertDeviceSorted(device);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            
        }

        private void InsertDeviceSorted(WLEDDevice d)
        {
            int index = 0;
            while (index < DeviceList.Count && d.CompareTo(DeviceList[index]) > 0) index++;

            DeviceList.Add(d);
        }

        public DeviceViewModel()
        {
            DeviceList = new ObservableCollection<WLEDDevice>();
        }


    }
}
