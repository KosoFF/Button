using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Button.Core.Commands;

namespace Button.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            EnterCommand = new RelayCommand(OnEnterCommadExecute, ()=>!IsLoading);
            SignUpCommand = new RelayCommand(OnSignUpCommandExecute, () => !IsLoading);
        }

        private void OnSignUpCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.AccountActivation);
        }

        private void OnEnterCommadExecute()
        {
            NavigationService.Navigate(ViewLocator.Login);
        }

        protected override void OnIsLoadingChanged()
        {


            EnterCommand.RaiseCanExecuteChanged();

            GoBackCommand.RaiseCanExecuteChanged();
        }

        public RelayCommand EnterCommand { get; set; }
        public RelayCommand SignUpCommand { get; set; }
    }
}
