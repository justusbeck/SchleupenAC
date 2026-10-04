using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using Newtonsoft.Json;

namespace Personenverwaltung.Client
{
    public partial class MainWindow : Window
    {
        private static readonly HttpClient HttpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5050/")
        };
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void LadenButton_Click(object sender, RoutedEventArgs e)
        {
            //LadePersonen.IsEnabled = true;
            LadePersonen.IsEnabled = false;

            try
            {
                string json = await HttpClient.GetStringAsync("api/personen");
                PersonenGrid.ItemsSource = JsonConvert.DeserializeObject<List<PersonDto>>(json);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Die Personen konnten nicht geladen werden:\n" + ex.Message,
                    "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                LadePersonen.IsEnabled = true;
            }
        }
    }
}