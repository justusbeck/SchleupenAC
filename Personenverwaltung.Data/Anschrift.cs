namespace Personenverwaltung.Data
{
    public class Anschrift
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Postleitzahl { get; set; }
        public string Ort { get; set; }
        public string Straße { get; set; }
        public string Hausnummer { get; set; }

        public virtual Person Person { get; set; }
    }
}