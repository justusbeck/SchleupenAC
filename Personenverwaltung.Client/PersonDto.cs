using System;

namespace Personenverwaltung.Client
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Vorname { get; set; }
        public DateTime Geburtsdatum { get; set; }
        public string Großbuchstaben  { get; set; }
    }
}