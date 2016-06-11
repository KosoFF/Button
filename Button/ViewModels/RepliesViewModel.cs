using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using Button.Account;
using Button.Core.DependencyInjection;
using Button.SqlLinkService;

namespace Button.ViewModels
{
    public class RepliesViewModel : ViewModel
    {
        public RepliesViewModel()
        {
            
        }
        public async override void OnNavigatedTo(NavigationEventArgs e)
        {
            await Init();
        }

        public async Task Init()
        {
            IsLoading = true;
            var linkservice = ServiceLocator.Locator.Get<ISqlLinkService>();
            var accountService = ServiceLocator.Locator.Get<IAccountService>();
            var t = await linkservice.GetRepliesCount(accountService.Account.WatchUserId);
            RepliesDictionary = t;
            IsLoading = false;

        }
        protected override void OnIsLoadingChanged()
        {
            GoBackCommand.RaiseCanExecuteChanged();
        }
        private Dictionary<string, string> _repliesDictionary;

        public Dictionary<string, string> RepliesDictionary
        {
            get { return _repliesDictionary;}
            set { Set(ref _repliesDictionary, value); }
        } 
    }
}
