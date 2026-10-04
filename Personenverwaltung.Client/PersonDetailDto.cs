using System;
using System.Collections.Generic;

namespace Personenverwaltung.Client
{
    public class AnschriftDto
    {
        public int Id { get; set; }
        public string Postleitzahl { get; set; }
        public string Ort { get; set; }
        public string Straße { get; set; }
        public string Hausnummer  { get; set; }
    }

    public class TelefonverbindungDto
    {
        public int Id { get; set; }
        public string Nummer { get; set; }
    }
    
    public class PersonDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Vorname { get; set; }
        public DateTime Geburtsdatum { get; set; }
        public List<AnschriftDto> Anschriften { get; set; } = new List<AnschriftDto>();
        public List<TelefonverbindungDto>  Telefonverbindungen { get; set; } = new List<TelefonverbindungDto>();
    }
}