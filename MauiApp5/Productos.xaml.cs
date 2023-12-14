using SQLite;

namespace MauiApp5
{
    public partial class Productos : ContentPage
    {
        public class Producto

        {

            [PrimaryKey, AutoIncrement]

            public int Id { get; set; }

            public string Nombre { get; set; }

            public string Descripcion { get; set; }

            public string Cantidad { get; set; }

            public string Costo { get; set; }
            public string Venta { get; set; }

            public string Url { get; set; }
        }



        public class DatabaseManager

        {

            readonly SQLiteConnection database;



            public DatabaseManager(string dbPath)

            {

                database = new SQLiteConnection(dbPath);

                database.CreateTable<Producto>();

            }



            public List<Producto> ObtenerProductos()

            {

                return database.Table<Producto>().ToList();

            }



            public Producto ObtenerProductoPorNombre(string nombre)

            {

                return database.Table<Producto>().FirstOrDefault(c => c.Nombre == nombre);

            }



            public int GuardarProducto(Producto producto)

            {

                if (producto.Id != 0)

                {

                    return database.Update(producto);

                }

                else

                {

                    return database.Insert(producto);

                }

            }



            public int EliminarProducto(Producto producto)

            {

                return database.Delete(producto);

            }

        }



        DatabaseManager dbManager;

        Producto productoEncontrado;

        public Productos()

        {

            InitializeComponent();

            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "products.db");

            dbManager = new DatabaseManager(dbPath);
            contactListView.ItemsSource = dbManager.ObtenerProductos();
        }
        private void ActualizarListView()
        {
            contactListView.ItemsSource = dbManager.ObtenerProductos();
        }
        private void Guardar_Clicked(object sender, EventArgs e)
        {
            if (productoEncontrado != null)
            {
                // Modificar el producto encontrado
                productoEncontrado.Descripcion = descripcionEntry.Text;
                productoEncontrado.Cantidad = cantidadEntry.Text;
                productoEncontrado.Costo = costoEntry.Text;
                productoEncontrado.Venta = ventaEntry.Text;
                productoEncontrado.Url = urlEntry.Text;

                dbManager.GuardarProducto(productoEncontrado);
                DisplayAlert("Modificar", "Registro Modificado!", "OK");

                LimpiarCampos(); // Limpia los campos después de la modificación
                ActualizarListView();
            }
            else
            {
                // Agregar un nuevo producto
                var producto = new Producto
                {
                    Nombre = nombreEntry.Text,
                    Descripcion = descripcionEntry.Text,
                    Cantidad = cantidadEntry.Text,
                    Costo = costoEntry.Text,
                    Venta = ventaEntry.Text,
                    Url = urlEntry.Text
                };

                dbManager.GuardarProducto(producto);
                DisplayAlert("Agregar", "Registro Agregado!", "OK");

                LimpiarCampos(); // Limpia los campos después de agregar un nuevo producto
                ActualizarListView();
            }
        }




        private void Buscar_Clicked(object sender, EventArgs e)

        {

            string nombre = nombreEntry.Text;

            productoEncontrado = dbManager.ObtenerProductoPorNombre(nombre);



            if (productoEncontrado != null)

            {

                descripcionEntry.Text = productoEncontrado.Descripcion;

                cantidadEntry.Text = productoEncontrado.Cantidad;

                costoEntry.Text = productoEncontrado.Costo;

                ventaEntry.Text = productoEncontrado.Venta;

                urlEntry.Text = productoEncontrado.Url;

                ModificarButton.IsEnabled = true;
                ActualizarListView();
            }

            else

            {



                // Mostrar un mensaje indicando que el producto no fue encontrado.

                ModificarButton.IsEnabled = false;

                DisplayAlert("Buscar", "Registro no encontrado!", "OK");
                ActualizarListView();
            }

        }



        private void Eliminar_Clicked(object sender, EventArgs e)

        {

            if (productoEncontrado != null)

            {

                dbManager.EliminarProducto(productoEncontrado);

                LimpiarCampos();

                DisplayAlert("Eliminar", "Registro eliminado!", "OK");
                ActualizarListView();
            }

            else

            {

                // Mostrar un mensaje indicando que el producto no fue encontrado.

                DisplayAlert("Eliminar", "Registro no encontrado!", "OK");
                ActualizarListView();
            }

        }



        private void Modificar_Clicked(object sender, EventArgs e)
        {
            if (productoEncontrado != null)
            {
                // Habilitar campos para permitir modificaciones
                descripcionEntry.IsEnabled = true;
                cantidadEntry.IsEnabled = true;
                costoEntry.IsEnabled = true;
                ventaEntry.IsEnabled = true;
                GuardarButton.IsEnabled = true;
                ActualizarListView();
            }
            else
            {
                // Mostrar un mensaje indicando que el producto no fue encontrado.
                DisplayAlert("Modificar", "Seleccione un producto para modificar primero.", "OK");
                ActualizarListView();
            }
        }




        private void LimpiarCampos()

        {

            nombreEntry.Text = string.Empty;

            descripcionEntry.Text = string.Empty;

            cantidadEntry.Text = string.Empty;

            costoEntry.Text = string.Empty;

            ventaEntry.Text = string.Empty;

            urlEntry.Text = string.Empty;

            productoEncontrado = null;

            GuardarButton.IsEnabled = true;
            ActualizarListView();
        }
    }
}
