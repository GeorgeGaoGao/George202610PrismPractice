using Common.Attributes;
using Login.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Login
{
    [GeorgeModule(MenuTitle = "Login", ModuleName = "Login", ViewName = "LoginView")]
    public class LoginModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<LoginView>();
        }
    }
}