using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Button.Account;
using Button.Core.Commands;
using Button.Core.DependencyInjection;
using Button.Core.Helpers;
using Button.Core.MessageServices;
using Button.Services;
using Button.SqlLinkService;

namespace Button.ViewModels
{
    public class SettingsViewModel : ViewModel
    {
        #region Ctor

        

        public SettingsViewModel()
        {

            LogoutCommand = new RelayCommand(LogoutCommandExecute, () => !IsLoading);
            SaveCommand = new RelayCommand(SaveCommandExecute, () => !IsLoading);
           
        }

        

        private async void SaveCommandExecute()
        {
            var messageService = ServiceLocator.Locator.Get<IMessageService>();
            var accountService = ServiceLocator.Locator.Get<IAccountService>();
            var linkService = ServiceLocator.Locator.Get<ISqlLinkService>();
            
            if (!Helpers.IsInternetConnected())
            {
                await messageService.ShowAsync("Connection failed", "Please check your internet connection.");
                return;
            }
            IsLoading = true;
            await linkService.SetEmail(accountService.Account.UserId, Email);
            await linkService.SetMobilePhone(accountService.Account.UserId, PhoneNumber);
            await messageService.ShowAsync("Успех!", "Телефон и имейл успешно сохранены.");
            IsLoading = false;
        }


        public override async void OnNavigatedTo(NavigationEventArgs e)
        {

            await Init();
        }
        private async Task Init()
        {

            var messageService = ServiceLocator.Locator.Get<IMessageService>();
            var accountService = ServiceLocator.Locator.Get<IAccountService>();
            var linkService = ServiceLocator.Locator.Get<ISqlLinkService>();
           
            if (!Helpers.IsInternetConnected())
            {
                await messageService.ShowAsync("Connection failed", "Please check your internet connection.");
                return;
            }
            IsLoading = true;
            Title = await linkService.GetTitle(accountService.Account.UserId);
            PhoneNumber = await linkService.GetMobilePhone(accountService.Account.UserId);
            Email = await linkService.GetEmail(accountService.Account.UserId);
            
            IsLoading = false;

        }

        private void LogoutCommandExecute()
        {
            var accountService = ServiceLocator.Locator.Get<IAccountService>();
            accountService.Account = new Account.Account();
            accountService.IsLoggedIn = false;
            NavigationService.Navigate(ViewLocator.Main);
        }

      


        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {
            LogoutCommand.RaiseCanExecuteChanged();

            SaveCommand.RaiseCanExecuteChanged();
        
            GoBackCommand.RaiseCanExecuteChanged();
           
        }

        #endregion

        #region Fields

      
        private string _title;
        private string _phoneNumber;
        private string _email;
     

        #endregion

        #region Properties

      

        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }

        public string Email
        {
            get { return _email;}
            set { Set(ref _email, value); }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber;}
            set { Set(ref _phoneNumber, value); }
        }
   

        public RelayCommand LogoutCommand { get; set; }

        public RelayCommand SaveCommand { get; set; }
 

        #endregion


    }
}
