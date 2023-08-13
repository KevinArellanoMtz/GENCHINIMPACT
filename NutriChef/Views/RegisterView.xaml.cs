using System;
using System.IO;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NutriChef.Views
{
    public partial class RegisterPage : ContentPage
    {
        private SQLiteAsyncConnection _database;

        public RegisterPage()
        {
            InitializeComponent();

            // Crear la conexión a la base de datos SQLite
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "users.db");
            _database = new SQLiteAsyncConnection(databasePath);
            _database.CreateTableAsync<UserRegistration>().Wait();
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }

        private void ShowPasswordButton_Clicked(object sender, EventArgs e)
        {
            PasswordEntry.IsPassword = !PasswordEntry.IsPassword;
        }

        private void ShowConfirmPasswordButton_Clicked(object sender, EventArgs e)
        {
            ConfirmPasswordEntry.IsPassword = !ConfirmPasswordEntry.IsPassword;
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            // Obtener los valores ingresados por el usuario
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;
            string confirmPassword = ConfirmPasswordEntry.Text;

            // Validar que los campos no estén vacíos y que las contraseñas coincidan
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                await DisplayAlert("Error", "Por favor, complete todos los campos", "Aceptar");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("Error", "Las contraseñas no coinciden", "Aceptar");
                return;
            }

            // Verificar si el usuario ya existe en la base de datos
            var existingUser = await _database.Table<UserRegistration>().FirstOrDefaultAsync(u => u.Usuario == username);
            if (existingUser != null)
            {
                await DisplayAlert("Error", "El usuario ya existe", "Aceptar");
                return;
            }

            // Crear un nuevo usuario
            var newUser = new UserRegistration
            {
                Usuario = username,
                Contrasenia = password
            };

            // Insertar el nuevo usuario en la base de datos
            await _database.InsertAsync(newUser);

            // Mostrar un mensaje de registro exitoso
            await DisplayAlert("Éxito", "Registro exitoso", "Aceptar");

            // Redirigir al usuario a la página de inicio de sesión
            await Navigation.PushAsync(new LoginPage());
        }
    }

    // Clase de modelo para el usuario
    [Table("UserCredentials")]
    public class UserRegistration
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Contrasenia { get; set; }
    }
}
