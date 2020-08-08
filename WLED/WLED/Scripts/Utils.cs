using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WLED
{
    public static class Utils
    {
        private static bool projectDebug = true;

        public static void Log(string message, [CallerFilePathAttribute] string file = null, [CallerMemberName] string method = null, [CallerLineNumber] int line = 0)
        {
            if (projectDebug)
            {
                string output = file.Split('/').LastOrDefault();
                file = output;
                DateTime dateTime = DateTime.Now;
                Debug.WriteLine("[LOG][{4}][{0}][{1}][{2}]: {3}", file, method, line, message, dateTime);
            }
        }
    }
}

