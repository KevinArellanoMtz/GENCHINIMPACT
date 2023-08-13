using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NutriChef.Views
{ 
	public partial class DetalleRecetas : ContentPage
	{
        public DetalleRecetas(Recetas receta)
        {
            InitializeComponent();
            BindingContext = receta;
        }

        private async void VolverButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}