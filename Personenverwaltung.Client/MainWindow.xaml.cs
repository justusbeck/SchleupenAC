using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace Personenverwaltung.Client
{
    public partial class MainWindow : Window
    {
        private static readonly HttpClient HttpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5050/") };
        
        private DetailWindow _detailWindow;
        
        private Dictionary<int, PersonDto> _original = new Dictionary<int, PersonDto>();
        
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
                var personen  = JsonConvert.DeserializeObject<List<PersonDto>>(json);
                
                PersonenGrid.ItemsSource = personen;
                MerkeOriginale(personen);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            finally { LadePersonen.IsEnabled = true; }
        }

        private async void SpeichernButton_Click(object sender, RoutedEventArgs e)
        {
            PersonenGrid.CommitEdit(DataGridEditingUnit.Row, true);
            
            var personen = PersonenGrid.ItemsSource as List<PersonDto>;

            if (personen == null)
                MessageBox.Show("Es wurden noch keine Personen geladen.",
                    "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
          
            var changed = personen.Where(IsChanged).ToList();
            
            if(changed.Count == 0)
                MessageBox.Show("Es gibt keine Änderungen zu speichern.",
                    "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
            
            if(changed.Any(p => string.IsNullOrWhiteSpace(p.Name) || string.IsNullOrWhiteSpace(p.Vorname)))
                MessageBox.Show("Name und Vorname dürfen nicht leer sein.",
                    "Hinweis", MessageBoxButton.OK, MessageBoxImage.Warning);

            SpeicherPersonen.IsEnabled = false;

            try
            {
                foreach (var person in changed)
                {
                    string json = JsonConvert.SerializeObject(new { person.Id, person.Name, person.Vorname });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await HttpClient.PutAsync($"api/personen/{person.Id}", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Speichern von " + person.Vorname + " " + person.Name + " " + "fehlgeschlagen\n"
                                        + (int)response.StatusCode + " " + error, "Fehler", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }

                    _original[person.Id] = Copy(person);

                    MessageBox.Show(changed.Count + " Änderung(en) gespeichert.",
                        "Speichern", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Die Änderungen konnten nicht gespeichert werden:\n" + exception.Message,
                    "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                SpeicherPersonen.IsEnabled = true;
            }
        }
        
        private void MerkeOriginale(List<PersonDto> personen)
        {
            _original = personen.ToDictionary(p => p.Id, Copy);
        }
        
        private bool IsChanged(PersonDto person)
        {
            PersonDto original;

            if (!_original.TryGetValue(person.Id, out original))
            {
                return false;
            }

            return person.Name != original.Name || person.Vorname != original.Vorname;
        }
        
        private static PersonDto Copy(PersonDto p)
        {
            return new PersonDto { Id = p.Id, Name = p.Name, Vorname = p.Vorname, Geburtsdatum = p.Geburtsdatum };
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