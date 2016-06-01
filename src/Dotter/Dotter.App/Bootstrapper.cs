using System.Windows;
using Dotter.App.ViewModels;
using Dotter.Core;
using Dotter.Core.Views;
using Microsoft.Practices.ServiceLocation;
using Prism.Events;
using Prism.Regions;
using Prism.Unity;

namespace Dotter.App
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return ServiceLocator.Current.GetInstance<ShellView>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window) Shell;
            Application.Current.MainWindow.Show();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            RegisterShellObjects();
            RegisterViews();
        }

        private void RegisterShellObjects()
        {
            RegisterTypeIfMissing(typeof (IRegionManager), typeof (RegionManager), true);

            var regionManager = Container.TryResolve<RegionManager>();
            regionManager.RegisterViewWithRegion(Regions.TagStatusbarRegion, typeof (StatusbarView));
            regionManager.RegisterViewWithRegion(Regions.TagMenuRegion, typeof(MenuView));
            regionManager.RegisterViewWithRegion(Regions.TagEditorRegion, typeof(TextEditorView));
            regionManager.RegisterViewWithRegion(Regions.TagPreviewRegion, typeof(PreviewView));
            regionManager.RegisterViewWithRegion(Regions.TagGrammarRegion, typeof(GrammarView));
        }

        private void RegisterViews()
        {
            RegisterTypeIfMissing(typeof (ShellViewModel), typeof (ShellViewModel), true);
        }
    }
}