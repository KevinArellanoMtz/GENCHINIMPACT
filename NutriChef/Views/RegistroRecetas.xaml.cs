using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace NutriChef.Views
{
    public partial class RegistroRecetas : ContentPage
    {
        private RecetaService _recetaService;
        private List<Recetas> _recetas;
        private byte[] _imagenReceta;

        public RegistroRecetas()
        {
            InitializeComponent();

            var databaseFolderPath = FileSystem.AppDataDirectory;
            var databasePath = Path.Combine(databaseFolderPath, "recetasDB.db");
            // Crear la carpeta si no existe
            Directory.CreateDirectory(databaseFolderPath);

            _recetaService = new RecetaService(databasePath);
            _recetas = new List<Recetas>();
        }

        /*
        private async void TomarFotoButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Verificar si el permiso de la cámara está garantizado
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    // Solicitar permiso de la cámara si no está garantizado
                    status = await Permissions.RequestAsync<Permissions.Camera>();
                    if (status != PermissionStatus.Granted)
                    {
                        // El usuario no otorgó permiso, mostrar mensaje de error o solicitar nuevamente
                        return;
                    }
                }

                // Abrir la cámara y capturar una foto
                var photo = await MediaPicker.CapturePhotoAsync();
                
                if (photo != null)
                {

                    // Leer los datos de la foto capturada en un arreglo de bytes
                    using (var stream = await photo.OpenReadAsync())
                    {
                        var memoryStream = new MemoryStream();
                        await stream.CopyToAsync(memoryStream);
                        _imagenReceta = memoryStream.ToArray();
                    }

                    // Mostrar la imagen en el Image
                    ImagenReceta.Source = ImageSource.FromStream(() => new MemoryStream(_imagenReceta));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "Aceptar");
                
            }
        }
        */
        private async void SeleccionarFotoButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Verificar si el permiso de acceso a la galería está garantizado
                var status = await Permissions.CheckStatusAsync<Permissions.Photos>();
                if (status != PermissionStatus.Granted)
                {
                    // Solicitar permiso de acceso a la galería si no está garantizado
                    status = await Permissions.RequestAsync<Permissions.Photos>();
                    if (status != PermissionStatus.Granted)
                    {
                        // El usuario no otorgó permiso, mostrar mensaje de error o solicitar nuevamente
                        return;
                    }
                }

                // Abrir la galería y permitir al usuario seleccionar una imagen
                var photo = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Seleccionar foto"
                });

                if (photo != null)
                {

                    // Leer los datos de la foto seleccionada en un arreglo de bytes
                    using (var stream = await photo.OpenReadAsync())
                    {
                        var memoryStream = new MemoryStream();
                        await stream.CopyToAsync(memoryStream);
                        _imagenReceta = memoryStream.ToArray();
                    }

                    // Mostrar la imagen en el Image
                    ImagenReceta.Source = ImageSource.FromStream(() => new MemoryStream(_imagenReceta));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error: {ex.Message}", "Aceptar");
            }
        }

        private async void GuardarRecetaButton_Clicked(object sender, EventArgs e)
        {
            var nombre = NombreEntry.Text;
            var categoria = CategoriaEntry.Text;
            var calorias = CaloriaEntry.Text;
            var ingredientes = IngredientesEditor.Text;
            var pasos = PasosEditor.Text;

            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(categoria) ||
                string.IsNullOrWhiteSpace(ingredientes) || string.IsNullOrWhiteSpace(pasos) ||
                string.IsNullOrWhiteSpace(calorias))
            {
                await DisplayAlert("Error", "Todos los campos deben ser completados", "Aceptar");
                return;
            }

            if (_imagenReceta == null)
            {
                await DisplayAlert("Error", "Debes de Subir una Imagen de tu noticia", "Aceptar");
                return;
            }

            var recetas = new Recetas
            {
                Nombre = nombre,
                Categoria = categoria,
                Calorias = calorias,
                Imagen = _imagenReceta, // Asignar el arreglo de bytes de la imagen
                Ingredientes = ingredientes,
                Pasos = pasos
            };

            await _recetaService.SaveRecetaAsync(recetas);

            await DisplayAlert("Registro Exitoso", "La publicacion se Registró Correctamente", "Aceptar");

            // Limpiar los campos después de guardar la receta
            NombreEntry.Text = string.Empty;
            CategoriaEntry.Text = string.Empty;
            CaloriaEntry.Text = string.Empty;
            IngredientesEditor.Text = string.Empty;
            PasosEditor.Text = string.Empty;

            await Navigation.PopAsync(); // Regresar a la vista anterior
        }


    }
}
