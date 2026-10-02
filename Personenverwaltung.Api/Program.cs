using System;
using System.Linq;
using Personenverwaltung.Data;

namespace Personenverwaltung.Api
{
    internal static class Program
    {
        private static void Main()
        {
            try
            {
                using (var db = new PersonenDbContext())
                {
                    Console.WriteLine("Anzahl Personen: " + db.Personen.Count());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadLine();
        }
    }
}
