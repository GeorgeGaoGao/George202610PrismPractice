using Common.Events;
using Prism.Commands;
using Prism.Dialogs;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shell.ViewModels
{
    public class MyMessageBoxViewModel : BindableBase,IDialogAware
    {
        public IEventAggregator EventAggregator { get; set; }
        private DelegateCommand _closeCommand;
        public DelegateCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new DelegateCommand(ExecuteCloseCommand));

        void ExecuteCloseCommand()
        {
            DialogParameters dialogParameters = new DialogParameters();
            dialogParameters.Add("dataOut", "this is the data brought out.");
            dialogParameters.Add("res", "this is the data result.");
            RequestClose.Invoke(dialogParameters,ButtonResult.OK);
        }
        public MyMessageBoxViewModel(IEventAggregator eventAggregator)
        {
            EventAggregator = eventAggregator;
            EventAggregator.GetEvent<CloseDialogEvent>().Subscribe(OnCloseDialogEvent);
        }

        private void OnCloseDialogEvent()
        {
            ExecuteCloseCommand();
        }

        public DialogCloseListener RequestClose{ get; }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
           EventAggregator.GetEvent<CloseDialogEvent>().Unsubscribe(ExecuteCloseCommand);
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            string dataIn = parameters.GetValue<string>("dataIn");
            string dataIn2 = parameters.GetValue<string>("dataIn2");
        }
    }
}
