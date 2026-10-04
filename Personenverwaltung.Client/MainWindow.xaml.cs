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

        private async void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            LoadPerson.IsEnabled = false;

            try
            {
                string url = "api/personen";

                if (!string.IsNullOrWhiteSpace(SuchTextBox.Text))
                    url += "?name=" + Uri.EscapeDataString(SuchTextBox.Text.Trim());

                if (_detailWindow != null) _detailWindow.Close();

                string json = await HttpClient.GetStringAsync(url);
                var person  = JsonConvert.DeserializeObject<List<PersonDto>>(json);
                
                PersonenGrid.ItemsSource = person;
                KeepOriginal(person);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            finally { LoadPerson.IsEnabled = true; }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            PersonenGrid.CommitEdit(DataGridEditingUnit.Row, true);
            
            var person = PersonenGrid.ItemsSource as List<PersonDto>;

            if (person == null)
                MessageBox.Show("Es wurden noch keine Personen geladen.",
                    "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
          
            var changed = person.Where(IsChanged).ToList();
            
            if(changed.Count == 0)
                MessageBox.Show("Es gibt keine Änderungen zu speichern.",
                    "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
            
            if(changed.Any(p => string.IsNullOrWhiteSpace(p.Name) || string.IsNullOrWhiteSpace(p.Vorname)))
                MessageBox.Show("Name und Vorname dürfen nicht leer sein.",
                    "Hinweis", MessageBoxButton.OK, MessageBoxImage.Warning);

            SavePerson.IsEnabled = false;

            try
            {
                foreach (var personToChange in changed)
                {
                    string json = JsonConvert.SerializeObject(new { personToChange.Id, personToChange.Name, personToChange.Vorname });
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await HttpClient.PutAsync($"api/personen/{personToChange.Id}", content);

                    if (!response.IsSuccessStatusCode)
                    {
                        string error = await response.Content.ReadAsStringAsync();
                        MessageBox.Show("Speichern von " + personToChange.Vorname + " " + personToChange.Name + " " + "fehlgeschlagen\n"
                                        + (int)response.StatusCode + " " + error, "Fehler", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }

                    _original[personToChange.Id] = Copy(personToChange);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Die Änderungen konnten nicht gespeichert werden:\n" + exception.Message,
                    "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                SavePerson.IsEnabled = true;
            }
        }
        
        private void KeepOriginal(List<PersonDto> person)
        {
            _original = person.ToDictionary(p => p.Id, Copy);
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