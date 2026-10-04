using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace Personenverwaltung.Client
{
    public partial class MainWindow : Window
    {
        private static readonly HttpClient HttpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5050/") };
        
        private DetailWindow _detailWindow;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void LadenButton_Click(object sender, RoutedEventArgs e)
        {
            LadePersonen.IsEnabled = false;

            try
            {
                string url = "api/personen";

                if (!string.IsNullOrWhiteSpace(SuchTextBox.Text))
                    url += "?name=" + Uri.EscapeDataString(SuchTextBox.Text.Trim());
                
                string json = await HttpClient.GetStringAsync(url);
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

        private void PersonenGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PersonenGrid.SelectedItem == null) return;

            if (_detailWindow == null)
            {
                _detailWindow = new DetailWindow { Owner = this };
                
                _detailWindow.Closed += (s, args) =>
                {
                    _detailWindow = null;
                    PersonenGrid.SelectedItem =  null;
                };
            }
            
            if (!_detailWindow.IsVisible) _detailWindow.Show();
        }
    }
}