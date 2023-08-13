using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NutriChef.Views
{
    public partial class VistaRecetas : ContentPage
    {
        private RecetaService _recetaService;
        private List<Recetas> _recetas;

        public VistaRecetas()
        {
            InitializeComponent();
            var databaseFolderPath = FileSystem.AppDataDirectory;
            var databasePath = Path.Combine(databaseFolderPath, "recetasDB.db");

            // Crear la carpeta si no existe
            Directory.CreateDirectory(databaseFolderPath);

            _recetaService = new RecetaService(databasePath);
            _recetas = new List<Recetas>();
            RefreshRecetas();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            RefreshRecetas();
        }

        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            // Eliminar el estado de inicio de sesión guardado
            if (Application.Current.Properties.ContainsKey("IsLoggedIn"))
            {
                Application.Current.Properties.Remove("IsLoggedIn");
                await Application.Current.SavePropertiesAsync();
            }

            // Redirigir al usuario a la pantalla de inicio de sesión
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }


        private async void RefreshRecetas()
        {
            try
            {
                _recetas = await _recetaService.GetRecetasAsync();
                RecetasListView.ItemsSource = _recetas;
            }
            catch (Exception ex)
            {
                // Manejar el error de forma adecuada, como mostrar un mensaje de error
                await DisplayAlert("Error", $"Error al obtener las recetas: {ex.Message}", "Aceptar");
                Console.WriteLine($"Error al obtener las recetas: {ex.Message}");
            }
        }


        private async void RegistroRecetasButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegistroRecetas());
        }

        private async void RecetasListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var selectedReceta = (Recetas)e.SelectedItem;
            await Navigation.PushAsync(new DetalleRecetas(selectedReceta));

            RecetasListView.SelectedItem = null;
        }

        private async void FiltroButton_Clicked(object sender, EventArgs e)
        {
            var categorias = _recetas.Select(r => CapitalizeFirstLetter(r.Categoria)).Distinct().ToList();
            categorias.Insert(0, "Todas las categorías");

            var selectedCategory = await DisplayActionSheet("Selecciona una categoría", "Cancelar", null, categorias.ToArray());

            if (selectedCategory != null && selectedCategory != "Cancelar")
            {
                if (selectedCategory == "Todas las categorías")
                {
                    RecetasListView.ItemsSource = _recetas;
                }
                else
                {
                    var filteredRecetas = _recetas.Where(r => CapitalizeFirstLetter(r.Categoria) == selectedCategory).ToList();
                    RecetasListView.ItemsSource = filteredRecetas;
                }
            }
        }

        private string CapitalizeFirstLetter(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }


    }
}
