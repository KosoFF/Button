using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Button.Account;
using Button.Core.Commands;
using Button.Core.DependencyInjection;
using Button.Core.Helpers;
using Button.Core.MessageServices;
using Button.SqlLinkService;

namespace Button.ViewModels
{
    public class MainSearchViewModel : ViewModel
    {
        #region Ctor

        public MainSearchViewModel()
        {

            MyProfileCommand = new RelayCommand(MyProfileCommandExecute, () => !IsLoading);

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

        private byte[] _selfImage;
        private string _title;


        #endregion

        #region Properties

        public string Title
        {
            get { return _title; }
            set { Set(ref _title, value); }
        }


        public RelayCommand MyProfileCommand { get; set; }

    


        #endregion

        #region Private methods





        #endregion

    }
}
