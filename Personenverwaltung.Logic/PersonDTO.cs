using System;

namespace Personenverwaltung.Logic
{
    public class PersonDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Vorname { get; set; }
        public DateTime Geburtsdatum { get; set; }
        public string Großbuchstaben  { get; set; }
    }
}