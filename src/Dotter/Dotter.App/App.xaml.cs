using System.Windows;

namespace Dotter.App
{
    public partial class App : Application
    {
        void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
        }
    }
}
