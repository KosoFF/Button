using System;
using System.Collections.Generic;
using System.Linq;
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
   public class AccountActivationViewModel : ViewModel
    {
        public AccountActivationViewModel()
        {

            SearchCommand = new RelayCommand(SearchCommandExecute, () => !IsLoading);


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
            var imageService = ServiceLocator.Locator.Get<IImageService>();
            if (!Helpers.IsInternetConnected())
            {
                await messageService.ShowAsync("Connection failed", "Please check your internet connection.");
                return;
            }
            IsLoading = true;
            UsersDictionary = await linkService.GetFreeUsersDictionary();
            FilteredDictionary = UsersDictionary;
            OnQuerySubmitted += (str) =>
            {
                messageService.ShowAsync("Heello", str);
            };
            IsLoading = false;
        }
        private void SearchCommandExecute()
        {
            Search(SearchText);
        }
        #region Base members

        protected override void OnIsLoadingChanged()
        {


            SearchCommand.RaiseCanExecuteChanged();

            GoBackCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Fields

        private string _selectedUserid;
        private ImageSource _userImage;
        private string _searchText;
        private Dictionary<string, string> _usersDictionary;
        private Dictionary<string, string> _filteredDictionary;

        #endregion

        #region Properties
        public string SelectedUserid
        {
            get { return _selectedUserid; }
            set
            {
                Set(ref _selectedUserid, value);
                var accountService = ServiceLocator.Locator.Get<IAccountService>();
                accountService.Account = new Account.Account
                {
                    UserId = SelectedUserid,
                    WatchUserId = SelectedUserid
                };
                OnSelectionChanged();
            }
        }

        private void OnSelectionChanged()
        {

            NavigationService.Navigate(ViewLocator.SignUp);


        }

        public Dictionary<string, string> UsersDictionary
        {
            get { return _usersDictionary; }
            set { Set(ref _usersDictionary, value); }
        }
        public Dictionary<string, string> FilteredDictionary
        {
            get { return _filteredDictionary; }
            set { Set(ref _filteredDictionary, value); }
        }
        public ImageSource UserImage
        {
            get { return _userImage; }
            set { Set(ref _userImage, value); }
        }
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (Set(ref _searchText, value))
                {
                    SearchTextChanged?.Invoke(this, value);
                    Search(_searchText);
                }

            }
        }

        public event EventHandler<string> SearchTextChanged;
        public Action<string> OnQuerySubmitted { get; set; }
        public RelayCommand SearchCommand { get; set; }




        #endregion

        #region Private methods
        private async void Search(string searchQuery)
        {

            FilteredDictionary = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(searchQuery))
            {
                foreach (var user in UsersDictionary)
                {
                    FilteredDictionary.Add(user.Key, user.Value);
                }
            }
            else
            {
                foreach (var user in UsersDictionary.Where(c => !string.IsNullOrEmpty(c.Value) && c.Value.IndexOf(searchQuery, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    FilteredDictionary.Add(user.Key, user.Value);
                }
            }
            var t = FilteredDictionary;
            FilteredDictionary = new Dictionary<string, string>();
            FilteredDictionary = t;

        }
        #endregion
    }
}
