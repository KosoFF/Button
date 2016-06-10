using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Graphics.Imaging;
using Windows.Security.Credentials;
using Windows.UI.Xaml.Media.Imaging;

using Button.Account;
using Button.Core.Commands;
using Button.Core.DependencyInjection;
using Button.Core.Helpers;
using Button.Core.MessageServices;
using Button.Core.Navigation;
using Button.SqlLinkService;


namespace Button.ViewModels
{
    public class SelfProfileViewModel : ViewModel
    {
        #region Ctor

        public SelfProfileViewModel()
        {

            SettingsCommand = new RelayCommand(SettingsCommandExecute, () =>!IsLoading);
            MyRepliesCommand = new RelayCommand(MyRepliesCommandExecute, () => !IsLoading);
           
            Init();
          
            




        }


        private async void Init()
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
            SelfImage = await linkService.GetPhoto(accountService.Account.UserId);
            Title = await linkService.GetTitle(accountService.Account.UserId);
            IsLoading = false;
        }



        public async Task ReadAll()
        {
            
            string directory = @"D:\Source\Local\Button\Button\20140225_215327000_iOS.png";
            await Task.Run(() =>
            {
                Task.Yield();
                //File.SetAttributes(directory, FileAttributes.Normal);

            }); 
        }




        private void SettingsCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.Settings);
        }

        private void MyRepliesCommandExecute()
        {
            throw new NotImplementedException();
        }


        #endregion

        #region Base members

        protected override void OnIsLoadingChanged()
        {
            SettingsCommand.RaiseCanExecuteChanged();

            MyRepliesCommand.RaiseCanExecuteChanged();

            GoBackCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Fields

        private byte[] _selfImage;
        private string _title;
        

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
        
  
        public RelayCommand SettingsCommand { get; set; }

        public RelayCommand MyRepliesCommand { get; set; }
        

        #endregion

        #region Private methods

        

       

        #endregion
    }
}

