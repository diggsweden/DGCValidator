using DGCValidator.Resources;
using Xamarin.Forms;

namespace DGCValidator.Views
{

    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            if (!MainPageStore.HasKeyErrorSubscription)
                MessagingCenter.Subscribe<Xamarin.Forms.Application>(Xamarin.Forms.Application.Current,
                    "DisplayPublicKeysError", ShowKeyErrorAlert);

            MainPageStore.HasKeyErrorSubscription = true;
        }

        private async void ShowKeyErrorAlert(Application sender)
        {
            if (MainPageStore.ModalVisible) return;

            MainPageStore.ModalVisible = true;
            await DisplayAlert(labelValidKeysText.Text, AppResources.KeyModalErrorMessage, "OK");
            MainPageStore.ModalVisible = false;
        }
    }

    public static class MainPageStore
    {
        public static bool HasKeyErrorSubscription { get; set; }
        public static bool ModalVisible { get; set; }
    }
}
