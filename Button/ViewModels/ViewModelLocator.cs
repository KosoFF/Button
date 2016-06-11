using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Button.ViewModels
{
    public class ViewModelLocator
    {
        public LoginViewModel LoginViewModel => new LoginViewModel();
        public SelfProfileViewModel SelfProfileViewModel => new SelfProfileViewModel();
        public MainSearchViewModel MainSearchViewModel => new MainSearchViewModel();
        public MainViewModel MainViewModel => new MainViewModel();
        
        public ProfileViewModel ProfileViewModel => new ProfileViewModel();
        
        public DrawbacksViewModel DrawbacksViewModel => new DrawbacksViewModel();

        public BenefitsViewModel BenefitsViewModel => new BenefitsViewModel();

        public RepliesViewModel RepliesViewModel => new RepliesViewModel();
        public SettingsViewModel SettingsViewModel => new SettingsViewModel();
        public AccountActivationViewModel AccountActivationViewModel => new AccountActivationViewModel();
        public SignUpViewModel SignUpViewModel => new SignUpViewModel();

        //public ControlViewModel ControlViewModel => new ControlViewModel();
        //public CheckInViewModel CheckInViewModel => new CheckInViewModel();
        //public PassengerRegisterViewModel PassengerRegisterViewModel => new PassengerRegisterViewModel();
        //public AddFlightViewModel AddFlightViewModel => new AddFlightViewModel();
        //public AddRouteViewModel AddRouteViewModel => new AddRouteViewModel();
        //public AddAircraftViewModel AddAircraftViewModel => new AddAircraftViewModel();

        //public BoardingPassViewModel BoardingPassViewModel => new BoardingPassViewModel();
        //public AboutViewModel AboutViewModel => new AboutViewModel();


    }
}
