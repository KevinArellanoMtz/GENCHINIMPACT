using NutriChef.Views;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NutriChef
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            Progress.IsVisible = true;

            await Task.Delay(5000); // Simular un tiempo de carga (opcional)

            // Verificar el estado de inicio de sesión
            var isLoggedIn = Application.Current.Properties.ContainsKey("IsLoggedIn") && (bool)Application.Current.Properties["IsLoggedIn"];
            if (isLoggedIn)
            {
                // El usuario está autenticado
                Application.Current.MainPage = new NavigationPage(new VistaRecetas());
                //Progress.IsVisible = false;
            }
            else
            {
                // El usuario no está autenticado
                Application.Current.MainPage = new NavigationPage(new LoginPage());
                //Progress.IsVisible = false;
            }

        }

        private async void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                Connectivity.ConnectivityChanged -= OnConnectivityChanged;
                await Task.Delay(2000); // Simular un retraso de 2 segundos para mostrar la pantalla de carga

                Application.Current.MainPage = new NavigationPage(new VistaRecetas());
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(Connectivity.NetworkAccess))
            {
                UpdateProgressBar();
            }
        }

        private void UpdateProgressBar()
        {
            var strength = GetConnectionStrength();
            Progress.Progress = strength;
        }



        private double GetConnectionStrength()
        {
            var networkAccess = Connectivity.NetworkAccess;

            switch (networkAccess)
            {
                case NetworkAccess.Internet:
                    return 1.0; // 100% strength
                case NetworkAccess.ConstrainedInternet:
                    return 0.5; // 50% strength
                default:
                    return 0.0; // 0% strength
            }
        }
    }
}
