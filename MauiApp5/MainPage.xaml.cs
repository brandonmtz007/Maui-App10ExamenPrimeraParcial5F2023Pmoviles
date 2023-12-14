using System.Data.SqlTypes;
using MauiApp5.Models;
using Microsoft.Extensions.Configuration;
using SQLite;
namespace MauiApp5
{

    public partial class MainPage : ContentPage
    {
        private const string DatabaseFilename = "UserData.db3";
        private SQLiteAsyncConnection _database;

        private const string RememberMeKey = "RememberMe";
        private const string UsernameKey = "Username";
        private const string PasswordKey = "Password";

        public MainPage()
        {
            InitializeComponent();
            InitializeDatabaseAsync();
            // Load saved data if available
            if (Preferences.Get(RememberMeKey, false))
            {
                UsernameEntry.Text = Preferences.Get(UsernameKey, string.Empty);
                PasswordEntry.Text = Preferences.Get(PasswordKey, string.Empty);
                miEnlace.Liga = Preferences.Get(PasswordKey, string.Empty);
                miEnlace.NombreTienda = Preferences.Get(UsernameKey, string.Empty);
                RememberMeCheckbox.IsChecked = true;
                Navigation.PushAsync(new MenuPage(miEnlace));
            }
        }
        private async Task InitializeDatabaseAsync()
        {
            string databasePath = Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);
            _database = new SQLiteAsyncConnection(databasePath);
            await _database.CreateTableAsync<UserData>();

            // Insert an example user for testing
            var user2 = new UserData
            {
                Username = "user",
                Password = "password"
            };
            await _database.InsertAsync(user2);
        }

        private Enlace miEnlace = new Enlace();
        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;
            miEnlace.Liga = PasswordEntry.Text;
            miEnlace.NombreTienda = UsernameEntry.Text;
            bool rememberMe = RememberMeCheckbox.IsChecked;

            if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(password))
            {
                miEnlace.NombreTienda = "Tienda";
                await Navigation.PushAsync(new MenuPage(miEnlace));
            }

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                    if (rememberMe)
                    {
                        // Save authentication data to preferences
                        Preferences.Set(UsernameKey, username);
                        Preferences.Set(PasswordKey, password);
                        Preferences.Set(RememberMeKey, true);
                    }
                    else
                    {
                        // If not remembering data, remove from preferences
                        Preferences.Remove(UsernameKey);
                        Preferences.Remove(PasswordKey);
                        Preferences.Set(RememberMeKey, false);
                    }
                await Navigation.PushAsync(new MenuPage(miEnlace));



            }
        }
    }
}
