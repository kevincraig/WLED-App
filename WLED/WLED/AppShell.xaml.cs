using Xamarin.Forms;

namespace WLED
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routes.RegisterRoutes();
        }
    }
}