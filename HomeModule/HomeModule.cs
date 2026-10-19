using Common.Attributes;
using Home.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Home
{
    [GeorgeModule(MenuTitle = "Home", ModuleName = "HomeModule", ViewName = "HomeView")]
   
    public class HomeModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<HomeView>();
        }
    }
}