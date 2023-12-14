using MauiApp5.Models;

namespace MauiApp5;

public partial class MenuPage : ContentPage
{
    private Enlace miEnlace;

    public MenuPage(Enlace enlace)
    {
        InitializeComponent();
        miEnlace = enlace;

        if (!string.IsNullOrEmpty(miEnlace.Liga))
        {
            ImagenMostrada.Source = ImageSource.FromUri(new Uri(miEnlace.Liga));
        }

        // Utiliza miEnlace.NombreTienda como desees, por ejemplo, asignándolo a una etiqueta Label.
        MiLabel.Text = miEnlace.NombreTienda;
    }
    private async void OnButtonClickedClientes(object sender, EventArgs e)
    {
        // Navigate to the SuccessPage
        await Navigation.PushAsync(new SuccessPage());
    }
    private async void OnButtonClickedProductos(object sender, EventArgs e)
    {
        // Navigate to the SuccessPage
        await Navigation.PushAsync(new Productos());
    }
}
