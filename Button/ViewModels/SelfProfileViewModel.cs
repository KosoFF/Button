﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Graphics.Imaging;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Button.Account;
using Button.Core.Commands;
using Button.Core.DependencyInjection;
using Button.Core.Helpers;
using Button.Core.MessageServices;
using Button.Core.Navigation;
using Button.Services;
using Button.SqlLinkService;


namespace Button.ViewModels
{
    public class SelfProfileViewModel : ViewModel
    {
        #region Ctor

        public SelfProfileViewModel()
        {
            var accountService = ServiceLocator.Locator.Get<IAccountService>();
            accountService.Account.WatchUserId = accountService.Account.UserId;

            SettingsCommand = new RelayCommand(SettingsCommandExecute, () =>!IsLoading);
            MyRepliesCommand = new RelayCommand(MyRepliesCommandExecute, () => !IsLoading);
            ChangeImageCommand = new RelayCommand(ChangeImageCommandExecute, () => !IsLoading);
           

        }

        
        private async void  ChangeImageCommandExecute()
        {
            var accountService = ServiceLocator.Locator.Get<IAccountService>();
            var imageService = ServiceLocator.Locator.Get<IImageService>();
            StorageFile file = await imageService.PickPhoto();
            IsLoading = true;
            UserImage = await imageService.SetImage(accountService.Account.UserId, file);
            IsLoading = false;
        }

        public async override void OnNavigatedTo(NavigationEventArgs e)
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
            Title = await linkService.GetTitle(accountService.Account.UserId);
            UserImage = await imageService.GetPrivateImage(accountService.Account.UserId);
            IsLoading = false;
            
        }

        private void SettingsCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.Settings);
        }

        private void MyRepliesCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.Replies);
        }


        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {
            SettingsCommand.RaiseCanExecuteChanged();

            MyRepliesCommand.RaiseCanExecuteChanged();

            GoBackCommand.RaiseCanExecuteChanged();
            ChangeImageCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Fields

        private byte[] _selfImage;
        private string _title;

        private byte[] userImageBytes;
        private ImageSource userImage;


        #endregion

        #region Properties

        public byte[] SelfImage
        {
            get { return _selfImage; }
            set { Set(ref _selfImage, value); }
        }

        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value);  }
        }
        public ImageSource UserImage
        {
            get { return userImage; }
            set { Set(ref userImage, value); }
        }


        public RelayCommand SettingsCommand { get; set; }

        public RelayCommand MyRepliesCommand { get; set; }
        public RelayCommand ChangeImageCommand { get; set; }
        

        #endregion

        #region Private methods

        

       

        #endregion
    }
}

