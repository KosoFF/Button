using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
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
    public class ProfileViewModel : ViewModel
    {
        #region Ctor

        public ProfileViewModel()
        {

            LinkCommand = new RelayCommand(SettingsCommandExecute, () => !IsLoading);
            VoteForAdvantagesCommand = new RelayCommand(OnVoteForAdvantagesCommandExecute, () => !IsLoading);
            VoteForDisadvantagesCommand = new RelayCommand(OnVoteForDisadvantagesCommandExecute, () => !IsLoading);
            RepliesCommand = new RelayCommand(OnRepliesCommandExecute, () => !IsLoading);
        }

        private void OnRepliesCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.Replies);
        }

        private void OnVoteForDisadvantagesCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.Drawbacks);
        }

        private void OnVoteForAdvantagesCommandExecute()
        {
            NavigationService.Navigate(ViewLocator.Benefits);
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
            Title = await linkService.GetTitle(accountService.Account.WatchUserId);
            UserImage = await imageService.GetPrivateImage(accountService.Account.WatchUserId);
            IsLoading = false;

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
            LinkCommand.RaiseCanExecuteChanged();

            VoteForAdvantagesCommand.RaiseCanExecuteChanged();
            RepliesCommand.RaiseCanExecuteChanged();
            GoBackCommand.RaiseCanExecuteChanged();
            VoteForDisadvantagesCommand.RaiseCanExecuteChanged();
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
            set { Set(ref _title, value); }
        }
        public ImageSource UserImage
        {
            get { return userImage; }
            set { Set(ref userImage, value); }
        }


        public RelayCommand LinkCommand { get; set; }

        public RelayCommand VoteForAdvantagesCommand { get; set; }
        public RelayCommand VoteForDisadvantagesCommand { get; set; }

        public RelayCommand RepliesCommand { get; set; }
        #endregion

    }
}
