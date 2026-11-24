
using System;
using System.Collections.Generic;
using System.Linq;

namespace pizzafutar
{
    public class Feladatok
    {
        private List<Rendeles> lista;

        public Feladatok()
        {
            Adatbazis a = new Adatbazis();
            lista = a.Feltolt();

            Console.WriteLine(lista[0].Cim.Nev);
            Console.WriteLine(lista[1].Pizza.Meret);

            var olcso = lista
                .Where(l => l.Pizza.Ar < 1500)
                .Select(l => l.Pizza)
                .Distinct()
                .OrderByDescending(p => p.Meret)
                .ThenBy(p => p.Nev)
                .Select(p => new
                {
                    Nev = p.Nev,
                    Meret = p.Meret,
                    Ar = p.Ar
                })
                .ToList();

            Console.WriteLine("\n3. Olcso pizzak:");
            foreach (var p in olcso)
            {
                Console.WriteLine($"{p.Nev} - {p.Meret} cm - {p.Ar} Ft");
            }

            var nepszeru = lista
                .GroupBy(l => l.Pizza)
                .Select(g => new
                {
                    Nev = g.Key.Nev,
                    Meret = g.Key.Meret,
                    Darab = g.Sum(l => l.Db)
                })
                .OrderByDescending(p => p.Darab)
                .FirstOrDefault();

            Console.WriteLine("\n4. Legnepszerubb pizza:");
            if (nepszeru != null)
            {
                Console.WriteLine($"{nepszeru.Nev} - {nepszeru.Meret} cm - {nepszeru.Darab} db");
            }

            var esti = lista
                .Where(l => l.Ido >= new TimeSpan(18, 0, 0)
                         && l.Ido <= new TimeSpan(19, 0, 0))
                .GroupBy(l => new
                {
                    l.Pizza.Nev,
                    l.Pizza.Meret
                })
                .Select(g => new
                {
                    Nev = g.Key.Nev,
                    Meret = g.Key.Meret,
                    Darab = g.Sum(l => l.Db)
                })
                .ToList();

            Console.WriteLine("\n5. Esti rendelesek:");
            foreach (var p in esti)
            {
                Console.WriteLine($"{p.Nev} - {p.Meret} cm - {p.Darab} db");
            }

            var jolfogy = lista
                .GroupBy(l => l.Pizza.Nev)
                .Where(g => g.Select(l => l.Pizza.Meret)
                             .Distinct()
                             .Count() == 3)
                .Select(g => g.Key)
                .ToList();

            Console.WriteLine("\n6. Mindharom meretben rendelt pizzak:");
            foreach (var p in jolfogy)
            {
                Console.WriteLine(p);
            }

            var reklam = lista
                .GroupBy(l => new
                {
                    l.Ido,
                    l.Cim.Id
                })
                .Where(g => g.Sum(l => l.Db) > 3)
                .Select(g => new
                {
                    Ido = g.Key.Ido,
                    Nev = g.First().Cim.Nev,
                    Utca = g.First().Cim.Utca,
                    Hsz = g.First().Cim.Hsz
                })
                .ToList();

            Console.WriteLine("\n7. Ingyen udito:");
            foreach (var p in reklam)
            {
                Console.WriteLine($"{p.Ido} - {p.Nev} - {p.Utca} {p.Hsz}");
            }

            var osszeg = lista
                .GroupBy(l => new
                {
                    l.Ido,
                    l.Cim.Id
                })
                .Select(g => new
                {
                    Ido = g.Key.Ido,
                    Nev = g.First().Cim.Nev,
                    Utca = g.First().Cim.Utca,
                    Hsz = g.First().Cim.Hsz,
                    Osszeg = g.Sum(l => l.Db * l.Pizza.Ar)
                })
                .OrderBy(l => l.Ido)
                .ToList();

            Console.WriteLine("\n8. Fizetendo osszegek:");
            foreach (var p in osszeg)
            {
                Console.WriteLine($"{p.Ido} - {p.Nev} - {p.Utca} {p.Hsz} - {p.Osszeg} Ft");
            }
        }
    }
}
