using Common.Attributes;
using George202610PrismPractice.Share;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Automation;

namespace George202610PrismPractice.ViewModels
{

    public class Menu
    {
        public string MenuTitle { get; set; }
        public string ModuleName { get; set; }
        public string ViewName { get; set; }
    }
    public class MainWindowViewModel : BindableBase
    {
        private List<Menu> _menus = new List<Menu>();
    //    {
    //        new Menu() { MenuTitle = "Home", ModuleName = "HomeModule", ViewName = "HomeView" },
    //        new Menu() { MenuTitle = "Login", ModuleName = "LoginModule", ViewName = "LoginView" }
    //};
    public List<Menu> Menus
        {
            get { return _menus; }
            set { SetProperty(ref _menus, value); }
        }
        public IRegionManager RegionManager { get; set; }

        private DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> NavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(ExecuteNavigateCommand));

        void ExecuteNavigateCommand(string parameter)
        {
            RegionManager.RequestNavigate(RegionNames.ContentRegion, parameter);
        }



        public MainWindowViewModel(IRegionManager regionManager)
        {
            RegionManager = regionManager;
            
            
           


        }

        private DelegateCommand _loadedCommand;
        public DelegateCommand LoadedCommand =>
            _loadedCommand ?? (_loadedCommand = new DelegateCommand(ExecuteLoadedCommand));

        void ExecuteLoadedCommand()
        {
            string modulePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Modules");
            //AttributeDiagnostics.Dump(modulePath);

            List<Menu> tempMenus = new List<Menu>();
            List<GeorgeModuleAttribute> moduleAttributes = ModuleScanner.GetGeorgeModuleAttributes(modulePath);
            foreach (var item in moduleAttributes)
            {
                tempMenus.Add(new Menu()
                {
                    MenuTitle = item.MenuTitle,
                    ModuleName = item.ModuleName,
                    ViewName = item.ViewName
                });
            }
            Menus = tempMenus;
        }
       
    }
}
