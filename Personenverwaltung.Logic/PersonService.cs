using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Personenverwaltung.Data;

namespace Personenverwaltung.Logic
{
    public class PersonService
    {
        public async Task<List<PersonDTO>> GetAllAsync()
        {
            using (var db = new PersonenDbContext())
            {
                return await db.Personen
                    .OrderBy(p => p.Name)
                    .Select(p => new PersonDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Vorname = p.Vorname,
                        Geburtsdatum = p.Geburtsdatum,
                        Großbuchstaben = p.Großbuchstaben
                    })
                    .ToListAsync();
            }
        }
    }
}