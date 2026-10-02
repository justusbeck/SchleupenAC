namespace Personenverwaltung.Data
{
    public class Telefonverbindung
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string Nummer { get; set; }

        public virtual Person Person { get; set; }
    }
}