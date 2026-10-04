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

                if (_detailWindow != null) _detailWindow.Close();

                string json = await HttpClient.GetStringAsync(url);
                PersonenGrid.ItemsSource = JsonConvert.DeserializeObject<List<PersonDto>>(json);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            finally { LadePersonen.IsEnabled = true; }
        }

        private async void PersonenGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var person = PersonenGrid.SelectedItem as PersonDto;

            if (person == null) return;

            try
            {
                string json = await HttpClient.GetStringAsync($"api/personen/{person.Id}");
                var detail = JsonConvert.DeserializeObject<PersonDetailDto>(json);
                ShowDetailWindow(detail);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        

        private void ShowDetailWindow(PersonDetailDto personDetail)
        {
            if (_detailWindow == null)
            {
                _detailWindow = new DetailWindow { Owner = this };
                
                _detailWindow.Closed += (s, args) =>
                {
                    _detailWindow = null;
                    PersonenGrid.SelectedItem = null;
                };
            }
            
            _detailWindow.ShowPersonDetails(personDetail);
            
            if (!_detailWindow.IsVisible) _detailWindow.Show();
        }
    }
}