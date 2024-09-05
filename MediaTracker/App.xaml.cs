using Microsoft.Maui.Devices;

namespace MediaTracker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            return new Window(new AppShell())
            {
                MinimumWidth = 600,
                MaximumWidth = 600,
                MinimumHeight = 900,
                MaximumHeight = 900,
                X = 100,
                Y = 100
            };
        }
    }
}
