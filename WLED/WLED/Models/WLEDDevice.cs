using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Xamarin.Forms;
using WLED.ViewModels;

namespace WLED.Models
{
    enum DeviceStatus { Default, Unreachable, Error };

    //Data Model. Represents a WLED light with a network address, name, and some current light values.
    [XmlType("dev")]
    public class WLEDDevice : ViewModelBase, IComparable
    {
        private string networkAddress = "192.168.4.1";                          //device IP (can also be hostname if applicable)
        private string name = "";                                               //device display name ("Server Description")
        private DeviceStatus status = DeviceStatus.Default;                     //Current connection status
        private bool isEnabled = true;                                          //Disabled devices don't get polled or show up in the list
        private double brightnessReceived = 0.9, brightnessCurrent = 0.9;       //There are two vars for brightness to discern API responses from slider updates

        [XmlElement("url")]
        public string NetworkAddress
        {
            set
            {
                if (value == null || value.Length < 3) return; //More elaborate checking for URL syntax could be added here
                SetPropertyValue(ref networkAddress, value);

            }
            get => networkAddress;
        }

        [XmlElement("name")]
        public string Name
        {
            set
            {
                if (value == null || name.Equals(value)) return; //Make sure name is not set to null
                SetPropertyValue(ref name, value);
            }
                
            
            get => name;
        }

        internal DeviceStatus CurrentStatus
        {
            set => SetPropertyValue(ref status, value);

            get => status;
        }

        [XmlElement("ncustom")]
        public bool NameIsCustom { get; set; } = true; //If the light name is custom, the name returned by the API response will be ignored

        [XmlElement("en")]
        public bool IsEnabled
        {
            set
            {
                SetPropertyValue(ref isEnabled, value);
                RaisePropertyChanged("Status");
                RaisePropertyChanged("ListHeight");
                RaisePropertyChanged("TextColor");
                RaisePropertyChanged("IsEnabled");
            }
            get => isEnabled;
        }

        [XmlIgnore]
        public double BrightnessCurrent
        {
            set
            {
                brightnessCurrent = value;
                if (brightnessCurrent != brightnessReceived) //only send if value was changed by slider
                {
                    byte toSend = (byte)Math.Round(brightnessCurrent);
                    RateLimitedSender.SendAPICall(this, "A=" + toSend);
                }
            }
            get { return brightnessCurrent; }
        }

        [XmlIgnore]
        private Color _colorCurrent;
        public Color ColorCurrent
        {
            get => _colorCurrent;
            set => SetPropertyValue(ref _colorCurrent, value);
        }

        [XmlIgnore]
        private bool stateCurrent; //Is the light on?
        public bool StateCurrent
        {
            get => stateCurrent;
            set
            {
                SetPropertyValue(ref stateCurrent, value);
                RaisePropertyChanged("StateColor");
            }
                    
        }
            

        //helper properties for updating view dynamically via data binding
        [XmlIgnore]
        public Color StateColor { get => StateCurrent ? Color.FromHex("#5294e2") : Color.FromHex("#222"); } //button background color


        [XmlIgnore]
        public string TextColor { get { return isEnabled ? "#FFF" : "#999"; } } //text color for modification page

        [XmlIgnore]
        public string Status //string containing IP and current status, second label in list viewcell
        {
            get
            {
                string statusText = "";
                if (IsEnabled)
                {
                    switch (status)
                    {
                        case DeviceStatus.Default: statusText = ""; break;
                        case DeviceStatus.Unreachable: statusText = " (Offline)"; break;
                        case DeviceStatus.Error: statusText = " (Error)"; break;
                    }
                }
                else
                {
                    statusText = " (Hidden)";
                }
                return string.Format("{0}{1}", networkAddress, statusText);
            }
        }

        //constructors
        public WLEDDevice() { }

        public WLEDDevice(string nA, string name)
        {
            NetworkAddress = nA;
            Name = name;
        }

        //member functions

        //send a call to this device's WLED HTTP API
        //API reference: http://apiref.wled.me
        public async Task<bool> SendAPICall(string call)
        {
            string url = "http://" + networkAddress;
            if (networkAddress.StartsWith("https://"))
            {
                url = networkAddress;
            }

            string response = await DeviceHttpConnection.GetInstance().Send_WLED_API_Call(url, call);
            if (response == null)
            {
                CurrentStatus = DeviceStatus.Unreachable;
                return false;
            }

            if (response.Equals("err")) //404 or other non-success http status codes, indicates that target is not a WLED device
            {
                CurrentStatus = DeviceStatus.Error;
                return false;
            }

            XmlApiResponse deviceResponse = XmlApiResponseParser.ParseApiResponse(response);
            if (deviceResponse == null) //could not parse XML API response
            {
                CurrentStatus = DeviceStatus.Error;
                return false;
            }

            CurrentStatus = DeviceStatus.Default; //the received response was valid

            if (!NameIsCustom) Name = deviceResponse.Name;

            //only consider brightness if light is on and if it wasn't modified in the same call (prevents brightness slider "jumps")
            if (deviceResponse.Brightness > 0 && !call.Contains("A="))
            {
                brightnessReceived = deviceResponse.Brightness;
                BrightnessCurrent = brightnessReceived;
                RaisePropertyChanged("BrightnessCurrent"); //update slider binding
            }

            ColorCurrent = deviceResponse.LightColor;
            

            StateCurrent = deviceResponse.IsOn;
            return true;
        }

        public async Task<bool> Refresh() //fetches updated values from WLED device
        {
            if (!IsEnabled) return false;
            return await SendAPICall("");
        }

        public int CompareTo(object comp) //compares devices in alphabetic order based on name
        {
            WLEDDevice c = comp as WLEDDevice;
            if (c == null || c.Name == null) return 1;
            int result = (name.CompareTo(c.name));
            if (result != 0) return result;
            return (networkAddress.CompareTo(c.networkAddress));
        }

        //protected virtual void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
