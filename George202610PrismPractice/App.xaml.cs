using George202610PrismPractice.Views;
using Home;
using Login;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.IO;
using System.Windows;

namespace George202610PrismPractice
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            //moduleCatalog.AddModule<HomeModule>();
            //moduleCatalog.AddModule<LoginModule>();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new DirectoryModuleCatalog()
            {
                ModulePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modules")
            };
        }
    }
}
