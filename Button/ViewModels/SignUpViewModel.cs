using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Button.Account;
using Button.Core.Commands;
using Button.Core.DependencyInjection;
using Button.Core.Helpers;
using Button.Core.MessageServices;
using Button.Core.Navigation;
using Button.SqlLinkService;
using Button.SqlReference;

namespace Button.ViewModels
{
    public class SignUpViewModel : ViewModel
    {
        #region Fields

        private string _userName;
        private string _email;
        private string _password;
        private byte[] userImageBytes;
        private ImageSource userImage;
        private readonly IMessageService messageService;

        #endregion

        #region Ctor

        public SignUpViewModel()
        {
            messageService = ServiceLocator.Locator.Get<IMessageService>();

            
            SignUpCommand = new RelayCommand(SignUpCommandExecute, () => !IsLoading);
           
            BackCommand = new RelayCommand(() => NavigationService.GoBack());
        }

        #endregion

        #region Properties

        public string UserName
        {
            get { return _userName; }
            set { Set(ref _userName, value); }
        }

        public string Email
        {
            get { return _email; }
            set { Set(ref _email, value); }
        }

        public string Password
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }


        public RelayCommand SignUpCommand { get; set; }

     

        public RelayCommand BackCommand { get; set; }

        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {
           BackCommand.RaiseCanExecuteChanged();
            SignUpCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Public methods

        

        #endregion

        #region Private methods

       

        private async void SignUpCommandExecute()
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                await messageService.ShowAsync("Message", "Please fill all empty input fields");
                return;
            }

            if (!Helpers.IsInternetConnected())
            {
                await messageService.ShowAsync("Connection failed", "Please check your internet connection.");
                return;
            }
            var sqlService = ServiceLocator.Locator.Get<ISqlLinkService>();
            var accService = ServiceLocator.Locator.Get<IAccountService>();
            IsLoading = true;
            if (!await sqlService.SignUp(new AccountInformation()
            {
                Username = UserName,
                Email = Email,
                PasswordHash = Password,
                UserId = accService.Account.UserId
            }))
                await messageService.ShowAsync("Успех!", "Вы успешно зарегистрированы.");
            else
            {
                await messageService.ShowAsync("Упс!", "Что-то пошло не так...");
            }
            NavigationService.Navigate(ViewLocator.Login);
            IsLoading = false;
        }

      

      
        

        


        #endregion
    }
}
