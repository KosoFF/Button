﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Button.Account;
using Button.Core.Commands;
using Button.Core.DependencyInjection;
using Button.Core.Helpers;
using Button.Core.MessageServices;
using Button.Services;
using Button.SqlLinkService;
using Button.Views;

namespace Button.ViewModels
{
    public class MainSearchViewModel : ViewModel
    {
        #region Ctor

        public MainSearchViewModel()
        {

            MyProfileCommand = new RelayCommand(MyProfileCommandExecute, () => !IsLoading);


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
            UserImage = await imageService.GetPrivateImage(accountService.Account.UserId);
            UsersDictionary = await linkService.GetUsersDictionary();
            FilteredDictionary = UsersDictionary;
            OnQuerySubmitted += (str) =>
            {
                messageService.ShowAsync("Heello", str);
            };
            

            IsLoading = false;

        }

       


        
   



        private void MyProfileCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.SelfProfile);
        }

     

        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {
            

            MyProfileCommand.RaiseCanExecuteChanged();

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
                accountService.Account.WatchUserId = SelectedUserid;
                OnSelectionChanged();
            }
        }

        private void OnSelectionChanged()
        {
            NavigationService.Navigate(ViewLocator.Profile);


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
            get {return _userImage;}
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
        public RelayCommand MyProfileCommand { get; set; }




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
