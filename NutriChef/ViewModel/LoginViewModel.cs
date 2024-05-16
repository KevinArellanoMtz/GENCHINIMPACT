using Firebase.Auth;
using NutriChef.Conexion;
using NutriChef.Models;
using NutriChef.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NutriChef.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        #region Atributos
        private string email;
        private string clave;
        #endregion

        #region Propiedades
        public string UsernameEntry
        {
            get { return email; }
            set { SetValue(ref email, value); }
        }
        public string PasswordEntry
        {
            get { return clave; }
            set { SetValue(ref clave, value); }
        }

        #endregion

        #region Command
        public Command LoginCommand { get; }
        #endregion

        #region Metodo
        public async Task LoginUsuario()
        {
            var objusuario = new UserModel()
            {
                EmailField = email,
                PasswordField = clave,
            };
            try
            {

                var autenticacion = new FirebaseAuthProvider(new FirebaseConfig(DBconn.WepApyAuthentication));
                var authuser = await autenticacion.SignInWithEmailAndPasswordAsync(objusuario.EmailField.ToString(), objusuario.PasswordField.ToString());
                string obternertoken = authuser.FirebaseToken;

                var Propiedades_NavigationPage = new NavigationPage(new VistaRecetas());

                Propiedades_NavigationPage.BarBackgroundColor = Color.RoyalBlue;
                App.Current.MainPage = Propiedades_NavigationPage;


            }
            catch (Exception)
            {

                await App.Current.MainPage.DisplayAlert("Advertencia", "Los datos introducidos son incorrectos o el usuario se encuentra inactivo.", "Aceptar");
                //await App.Current.MainPage.DisplayAlert("Advertencia", ex.Message, "Aceptar");
            }
        }
        #endregion
        public async Task RegisterUsuario()
        {
            var objusuario = new UserModel()
            {
                EmailField = email,
                PasswordField = clave,
            };

            try
            {
                var autenticacion = new FirebaseAuthProvider(new FirebaseConfig(DBconn.WepApyAuthentication));
                var authuser = await autenticacion.CreateUserWithEmailAndPasswordAsync(objusuario.EmailField, objusuario.PasswordField);

                // Registro exitoso, puedes realizar acciones adicionales si es necesario

                await App.Current.MainPage.DisplayAlert("Registro exitoso", "Usuario registrado correctamente", "Aceptar");
            }
            catch (FirebaseAuthException ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        #region Constructor
        public LoginViewModel(INavigation navegar)
        {
            Navigation = navegar;
            LoginCommand = new Command(async () => await LoginUsuario());

        }
        #endregion
    }
}
