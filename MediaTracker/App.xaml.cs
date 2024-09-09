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
                MinimumWidth = 790,
                MaximumWidth = 790,
                MinimumHeight = 1000,
                MaximumHeight = 1000,
                X = 100,
                Y = 0
            };
        }
    }
}
