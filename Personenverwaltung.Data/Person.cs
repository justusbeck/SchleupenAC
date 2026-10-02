using System;
using System.Collections.Generic;

namespace Personenverwaltung.Data
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Vorname { get; set; }
        public DateTime Geburtsdatum { get; set; }

        public virtual ICollection<Anschrift> Anschriften { get; set; } = new List<Anschrift>();
        public virtual ICollection<Telefonverbindung> Telefonverbindungen { get; set; } = new List<Telefonverbindung>();
    }
}