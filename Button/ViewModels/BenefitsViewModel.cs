using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Button.Account;
using Button.Core.Commands;
using Button.Core.DependencyInjection;
using Button.Core.MessageServices;
using Button.SqlLinkService;

namespace Button.ViewModels
{
    public class BenefitsViewModel : ViewModel
    {
        public BenefitsViewModel()
        {
           AcceptCommand = new RelayCommand(OnAcceptCommandExecute, () => !IsLoading);
            DeclineCommand = new RelayCommand(OnDeclineCommandExecute, () => !IsLoading);
        }

        private void OnDeclineCommandExecute()
        {
            if(NavigationService.CanGoBack)
            NavigationService.GoBack();
        }

        private async void OnAcceptCommandExecute()
        {
            var linkService = ServiceLocator.Locator.Get<ISqlLinkService>();
            var accountService = ServiceLocator.Locator.Get<IAccountService>();
            var ms = ServiceLocator.Locator.Get<IMessageService>();
            IsLoading = true;
            if (!await linkService.AddReplies(accountService.Account.UserId, accountService.Account.WatchUserId, false,
                SelectedItemsDictionary))
            {
                await ms.ShowAsync("Успех!", "Ваше мнение было учтено.");
                if (NavigationService.CanGoBack)
                    NavigationService.GoBack();
                IsLoading = false;
            }
            else
            {
                await ms.ShowAsync("Упс...", "Кажется что-то пошло не так");
                IsLoading = false;
            }
        }


        public async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await Init();
        }

        public async Task Init()
        {
            IsLoading = true;
            var linkservice = ServiceLocator.Locator.Get<ISqlLinkService>();
            var t = await linkservice.GetBenefits();
            BenefitsDictionary = t;
            IsLoading = false;
            
        }
        protected override void OnIsLoadingChanged()
        {
          AcceptCommand.RaiseCanExecuteChanged();
          DeclineCommand.RaiseCanExecuteChanged();
        }
        private Dictionary<string, string> _benefitsDictionary;
        private Dictionary<string, string> _selectedItemsDictionary;

        public Dictionary<string, string> SelectedItemsDictionary
        {
            get { return _selectedItemsDictionary;}
            set { Set(ref _selectedItemsDictionary, value); }
        }
        public Dictionary<string, string> BenefitsDictionary
        {
            get { return _benefitsDictionary;}
            set { Set(ref _benefitsDictionary, value); }
        }

        private RelayCommand<IList<object>> _selectionChangedCommand;
        public RelayCommand<IList<object>> SelectionChangedCommand
        {
            get
            {
                if (_selectionChangedCommand == null)
                {
                    _selectionChangedCommand = new RelayCommand<IList<object>>(
                        items =>
                        {
                            SelectedItemsDictionary = new Dictionary<string, string>();
                            foreach (var item in items)
                            {
                                
                                SelectedItemsDictionary.Add((item as KeyValuePair<string, string>?).Value.Key,
                                    (item as KeyValuePair<string, string>?).Value.Value);
                               
                            }
                        }
                       
                    );
                }

                return _selectionChangedCommand;
            }
        }

        public RelayCommand AcceptCommand { get; set; }

        public RelayCommand DeclineCommand{ get; set; }
    }
}
