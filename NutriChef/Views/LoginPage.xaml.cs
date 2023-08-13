using System;
using System.IO;
using SQLite;
using SQLite.Net.Attributes;
using Xamarin.Essentials;
using Xamarin.Forms;
using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;


namespace NutriChef.Views
{
    public partial class LoginPage : ContentPage
    {
        private SQLiteAsyncConnection _database;

        public LoginPage()
        {
            InitializeComponent();

            // Crear la conexión a la base de datos SQLite
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "users.db");
            _database = new SQLiteAsyncConnection(databasePath);
            _database.CreateTableAsync<UserCredentials>().Wait();

            // Verificar si el usuario ha iniciado sesión previamente
            var isLoggedIn = Preferences.Get("IsLoggedIn", false);
            if (isLoggedIn)
            {
                // Navegar directamente a la vista de recetas
                Application.Current.MainPage = new NavigationPage(new VistaRecetas());
            }
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            var username = UsernameEntry.Text;
            var password = PasswordEntry.Text;

            // Verificar la conexión a Internet
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("Error", "No hay conexión a Internet", "OK");
                return;
            }

            // Verificar la conexión a datos móviles
            if (Connectivity.NetworkAccess != NetworkAccess.Internet && Connectivity.NetworkAccess != NetworkAccess.ConstrainedInternet)
            {
                await DisplayAlert("Error", "No hay conexión a Internet o datos móviles", "OK");
                return;
            }

            // Realizar la autenticación en la base de datos SQLite
            var user = await _database.Table<UserCredentials>().FirstOrDefaultAsync(u => u.Usuario == username && u.Contrasenia == password);

            if (user != null)
            {
                // Autenticación exitosa

                // Guardar el estado de inicio de sesión en las propiedades de la aplicación
                Application.Current.Properties["IsLoggedIn"] = true;
                await Application.Current.SavePropertiesAsync();

                await DisplayAlert("NutriChef", "Bienvenido :D", "Aceptar");

                // Navegar a la siguiente página
                Application.Current.MainPage = new NavigationPage(new VistaRecetas());
            }
            else
            {
                // Autenticación fallida
                await DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
            }
        }

        private async void FingerprintButton_Clicked(object sender, EventArgs e)
        {
            bool supported = await CrossFingerprint.Current.IsAvailableAsync(true);

            if (supported)
            {
                AuthenticationRequestConfiguration conf = new AuthenticationRequestConfiguration("Lector de biometría para NutriChef", "Ingresa a la app NutriChef");

                var result = await CrossFingerprint.Current.AuthenticateAsync(conf);

                if (result.Authenticated)
                {
                    // Guardar el estado de inicio de sesión en las propiedades de la aplicación
                    Application.Current.Properties["IsLoggedIn"] = true;
                    await Application.Current.SavePropertiesAsync();

                    // Autenticación exitosa
                    await DisplayAlert("Éxito", "Autenticación con huella dactilar exitosa", "Aceptar");

                    // Navegar a la siguiente página
                    Application.Current.MainPage = new NavigationPage(new VistaRecetas());
                }
                else
                {
                    // Autenticación fallida
                    await DisplayAlert("Error", "Autenticación con huella dactilar fallida", "Aceptar");
                }
            }
            else
            {
                await DisplayAlert("Error", "Los biométricos no están disponibles en tu dispositivo", "Aceptar");
            }
        }

        private void ShowPasswordButton_Clicked(object sender, EventArgs e)
        {
            PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        }

        private void RegisterButton_Clicked(object sender, EventArgs e)
        {
            // Lógica para el botón de registro
            Application.Current.MainPage = new NavigationPage(new RegisterPage());
        }
    }

    // Clase de modelo para el usuario
    [SQLite.Table("UserCredentials")]
    public class UserCredentials
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Contrasenia { get; set; }
    }
}
