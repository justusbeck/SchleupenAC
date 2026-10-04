using System;
using System.Linq;
using Microsoft.Owin.Hosting;
using Personenverwaltung.Data;

namespace Personenverwaltung.Api
{
    internal static class Program
    {
        private static void Main()
        {
            const string url = "http://localhost:5050/";

            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("API läuft auf " + url);
                Console.ReadLine();
            }
        }
    }
}
