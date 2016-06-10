using System;
using System.Linq;
using System.Threading.Tasks;
using Button.Core.DependencyInjection;
using Button.Core.Helpers;
using Button.Core.MessageServices;
using Button.Core.Observable;
using Button.SqlLinkService;
using Button.SqlReference;

namespace Button.Account
{
    public class AccountService : ObservableObject, IAccountService
    {
        //private string _userName;
        //public string UserName {
        //    get { return this._userName; }
        //    set { Set(ref this._userName, value); }
        //}

        //private string _email;
        //public string Email
        //{
        //    get { return this._email; }
        //    set { Set(ref this._email, value); }
        //}

        //private string _passwordHash;
        //public string PasswordHash
        //{
        //    get { return this._passwordHash; }
        //    set { Set(ref this._passwordHash, value); }
        //}

        private bool _checkInProgress;
        private bool _isLoggedIn;

        private Account _account;

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                Set(ref _isLoggedIn, value);
                OnAccountStatusChanged();
            }
        }

        public Account Account
        {
            get { return _account; }
            set { Set(ref _account, value); }
        }

        public Action AccountStatusChanged { get; set; }

        public async Task SignUpAsync(Account account)
        {
            //account.PasswordHash = Helpers.ComputeMd5(account.PasswordHash);
            //var sqlService = new ServiceClient();
            //await sqlService.SignUpAsync(new Check_in()
            //{
            //    Login = account.UserName,
            //    PasswordHash = account.PasswordHash,
            //    FirstName = account.FirstName,
            //    LastName = account.LastName
            //});
        }

        public async Task LoginAsync(string login, string pass)
        {
            var password = Helpers.ComputeMd5(pass);
            if (_checkInProgress) return;
            _checkInProgress = true;
            var messageService = ServiceLocator.Locator.Get<IMessageService>();
            var linkService = ServiceLocator.Locator.Get<ISqlLinkService>();

            

            var response = await linkService.LoginAsync(login, pass);
            if (response == null)
            {
                await messageService.ShowAsync("Error", "Invalid login or password");
                _checkInProgress = false;
                return;
            }
            //            if (response.Login == "Connection error")
            //            {
            //#if DEBUG

            //                await
            //                    messageService.ShowAsync("Error",
            //                        "There must be a problem with connection to database. \n Anyway, welcome.");

            //#else
            //                await messageService.ShowAsync("Error", "There must be a problem with connection to database. ");
            //                return;
            //#endif
            //            }

          
            Account = new Account()
                        {
                            Username = response.Username,
                            PasswordHash = response.PasswordHash,
                            AccountId = response.AccountID,
                            Email = response.Email,
                            EmailConfirmed = response.EmailConfirmed,
                            RegistrationTime = response.RegistrationTime,
                            UserId = response.UserId
                        };
            #if DEBUG
                        await messageService.ShowAsync("Success", "Hi, " + Account.Username);
            #endif
            IsLoggedIn = true;
            OnAccountStatusChanged();
            _checkInProgress = false;
        }

        public async Task LogoutAsync()
        {
            IsLoggedIn = false;
            Account = new Account();
        }

        public void OnAccountStatusChanged()
        {
            var handler = AccountStatusChanged;
            handler?.Invoke();
        }
    }
}