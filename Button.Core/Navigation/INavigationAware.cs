using Windows.UI.Xaml.Navigation;

namespace Button.Core.Navigation
{
    public interface INavigationAware
    {
        void OnNavigatedTo(NavigationEventArgs e);

        void OnNavigatedFrom(NavigationEventArgs e);
    }
}