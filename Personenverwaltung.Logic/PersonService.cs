using System;
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

        public async Task<PersonDetailDto> GetDetailAsync(int id)
        {
            using (var db = new PersonenDbContext())
            {
                var person = await db.Personen
                    .Include(p => p.Anschriften)
                    .Include(p => p.Telefonverbindungen)
                    .FirstAsync(p => p.Id == id);
                
                if (person == null) return null;

                return new PersonDetailDto
                {
                    Id = person.Id,
                    Name = person.Name,
                    Vorname = person.Vorname,
                    Geburtsdatum = person.Geburtsdatum,
                    Anschriften = person.Anschriften
                        .Select(a => new AnschriftDto
                        {
                            Id = a.Id,
                            Postleitzahl = a.Postleitzahl,
                            Ort = a.Ort,
                            Straße = a.Straße,
                            Hausnummer = a.Hausnummer
                        })
                        .ToList(),
                    Telefonverbindungen = person.Telefonverbindungen
                        .Select(t => new TelefonverbindungDto
                        {
                            Id = t.Id,
                            Nummer = t.Nummer
                        })
                        .ToList()
                };
            }
        }
        
        public async Task<bool> ChangeNameAsync(int id, string name, string vorname)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(vorname))
                throw new ArgumentException("Name und Vorname dürfen nicht leer sein.");
        
            name = name.Trim();
            vorname = vorname.Trim();

            using (var db = new PersonenDbContext())
            {
                var person = await db.Personen.FindAsync(id);

                if (person == null) return false;
            
                person.Name = name;
                person.Vorname = vorname;

                await db.SaveChangesAsync();
                return true;
            }
        }
    }
}