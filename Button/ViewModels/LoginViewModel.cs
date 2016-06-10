using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Credentials;
using Button.Account;
using Button.Core.Commands;
using Button.Core.DependencyInjection;
using Button.Core.Helpers;
using Button.Core.MessageServices;
using Button.Core.Navigation;

namespace Button.ViewModels
{
    public class LoginViewModel : ViewModel
    {
        #region Ctor

        public LoginViewModel()
        {

            LoginCommand = new RelayCommand(LoginCommandExecute, () => !IsLoading);
            LinkCommand = new RelayCommand(LinkCommandExecute, () => !IsLoading);
            SignUpCommand = new RelayCommand(SignupCommandExecute, () => !IsLoading);
            
#if DEBUG
            Username = "abraam";
            Password = "123456";
#endif
        }

        private void LinkCommandExecute()
        {
            throw new NotImplementedException();
        }


        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {
            SignUpCommand.RaiseCanExecuteChanged();
            
            LoginCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Fields

        private string _username;
        private string _password;
        private bool _rememberMe;

        #endregion

        #region Properties

        public string Username
        {
            get { return _username; }
            set { Set(ref _username, value); }
        }

        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        public bool RememberMe
        {
            get { return _rememberMe; }
            set { Set(ref _rememberMe, value); }
        }

        public RelayCommand SignUpCommand { get; set; }

        public RelayCommand LoginCommand { get; set; }
        public RelayCommand LinkCommand { get; set; }


        public RelayCommand BackCommand { get; set; }

        #endregion

        #region Private methods

        private void SigninWithVkCommandExecute()
        {
            //NavigationService.Navigate(ViewLocator.ForgotPassword);
        }
        private void SignupCommandExecute()
        {
           throw new NotImplementedException();
        }

        private async void LoginCommandExecute()
        {
            var messageService = ServiceLocator.Locator.Get<IMessageService>();
            var accountService = ServiceLocator.Locator.Get<IAccountService>();
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                await messageService.ShowAsync("Message", "Please fill all empty input fields");
                return;
            }

            if (!Helpers.IsInternetConnected())
            {
                await messageService.ShowAsync("Connection failed", "Please check your internet connection.");
                return;
            }

            IsLoading = true;

            if (RememberMe)
            {
                var passwordVault = new PasswordVault();
                
            }
            await accountService.LoginAsync(Username, Password);
            if (accountService.IsLoggedIn)
                NavigationService.NavigateWithoutHistory(ViewLocator.MainSearch);
            IsLoading = false;

            
        }

        #endregion
    }
}
