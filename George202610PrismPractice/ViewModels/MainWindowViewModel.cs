using Common.Attributes;
using Common.Events;
using George202610PrismPractice.Share;
using Prism.Commands;
using Prism.Dialogs;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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
        public IDialogService DialogService { get; set; }
        public IEventAggregator EventAggregator { get; set; }

        private DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> NavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(ExecuteNavigateCommand));

        void ExecuteNavigateCommand(string parameter)
        {
            RegionManager.RequestNavigate(RegionNames.ContentRegion, parameter);
        }



        public MainWindowViewModel(IRegionManager regionManager,IDialogService dialogService,
            IEventAggregator eventAggregator)
        {
            RegionManager = regionManager;
            DialogService = dialogService;
            EventAggregator = eventAggregator;
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

        private DelegateCommand _openDialogCommand;
        public DelegateCommand OpenDialogCommand =>
            _openDialogCommand ?? (_openDialogCommand = new DelegateCommand(ExecuteOpenDialog));

        async void ExecuteOpenDialog()
        {
            IDialogParameters dialogParameters = new DialogParameters();
            dialogParameters.Add("dataIn", "this is the string was brought in.");
            dialogParameters.Add("dataIn2", "this is the string was brought in.2");
            DialogService.Show("MyMessageBox", dialogParameters, result =>
            {
                if (result.Result == ButtonResult.OK)
                {
                    string stringResult = result.Parameters.GetValue<string>("res");
                    // Handle OK result
                }
                else if (result.Result == ButtonResult.Cancel)
                {
                    // Handle Cancel result
                }
            });
            await Task.Delay(5000);


            EventAggregator.GetEvent<CloseDialogEvent>().Publish();
        }
       
    }
}
