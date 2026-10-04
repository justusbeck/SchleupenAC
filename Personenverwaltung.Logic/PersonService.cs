using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Personenverwaltung.Data;

namespace Personenverwaltung.Logic
{
    public class PersonService
    {
        public async Task<List<PersonDto>> SuchenAsync(string name)
        {
            using (var db = new PersonenDbContext())
            {
                IQueryable<Person> query = db.Personen;

                if (!string.IsNullOrWhiteSpace(name))
                {
                    string suchbegriff = name.Trim();
                    query = query.Where(p => p.Name.Contains(suchbegriff)
                                                || p.Vorname.Contains(suchbegriff));
                }
                
                return await query
                    .OrderBy(p => p.Name)
                    .Select(p => new PersonDto
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