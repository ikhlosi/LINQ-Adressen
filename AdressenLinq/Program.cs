using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdressenLinq {
    internal class Program {
        private static List<Adres> Data = new List<Adres>();

        static void Main(string[] args) {
            Console.WriteLine("Hello World!");

            // List<Adres> data = new List<Adres>();

            // FileInfo fi = new FileInfo(@"C:\Users\windoos\Documents\hogent\sem2\Programmeren_gevorderd_(GRAD-PRG)_-_Documenten\GroepB\oefeningen\adresInfo.txt");
            string bestandsPad = @"F:\abucks\windoos\documents\hogent\sem2\programmeren_gevorderd\GroepB\oefeningen\adresInfo.txt";
            // string[] adresInfoRegels = File.ReadAllLines(bestandsPad);

            // using (FileStream fs = File.Open())
            foreach (string regel in File.ReadAllLines(bestandsPad)) {
                string[] adresInfo = regel.Split(',');
                Data.Add(new Adres(adresInfo[0], adresInfo[1], adresInfo[2]));
            }

            //using (StreamReader sr = new StreamReader(bestandsPad)) {
            //    string regelx;
            //    while ((regelx = sr.ReadLine()) != null) {
            //        string[] adresInfox = regelx.Split(',');
            //        Data.Add(new Adres(adresInfox[0], adresInfox[1], adresInfox[2]));
            //    }
            //}


            // string[] adresInfo = adresInfoLines[0].Split(',');

            // Console.WriteLine(adresInfo[1]);
            /* for (int i = 0; i < 5; i++) {
                Console.WriteLine(adresInfo[i]);
            }
            */


            //var provincienamenAlfabetisch = adresInfo.OrderBy(x => x[1]);


            //SelecteerProvinciesAlfabetisch();
            //Console.WriteLine(new String('-', 30));

            //ToonStraatnamenVanGemeente("Nazareth");
            //Console.WriteLine(new String('-', 30));


            //StraatnaamDieMeesteVoorkomt();
            //Console.WriteLine(new String('-', 30));


            //MeestVoorkomendeStraatnamen(4);

            //var resultaat =
                //Gemeenschappelijk("Gent", "Merelbeke");
                //EnkelInX("Gent", new List<string> { "Deinze", "Merelbeke" });
                //uniekeStraatnamen();
                //uniekeStraatnamenInGemeente("Gent");

            //foreach (var x in resultaat) {
            //    Console.WriteLine(x);
            //}

            //Console.WriteLine(gemeenteMetMaxAantalStraatnamen());
            //Console.WriteLine(LangsteStraatnaam());
            //Console.WriteLine(AdresMetLangsteStraatnaam());
        }

        public static void SelecteerProvinciesAlfabetisch() {
            var gesorteerd = Data.Select(a => a.Provincie).Distinct().OrderBy(a => a);
            Console.WriteLine("De provincies gesorteerd:");
            foreach (var x in gesorteerd) {
                Console.WriteLine(x);
            }
        }

        public static void ToonStraatnamenVanGemeente(string gemeente) {
            var gefilterd = Data.Where(a => a.Gemeente == gemeente);
            Console.WriteLine($"Straten in gemeente {gemeente}:");
            foreach (var x in gefilterd) {
                Console.WriteLine(x.Straat);
            }
        }

        public static void StraatnaamDieMeesteVoorkomt() {
            // dictonary maken met straatnaam als key en list van gemeentes als value
            // data toevoegen aan dictionary:
            // met linq (Where of Select)
            // checken of straat er al in zit
            // indien ja: voeg gemeente toe aan de lijst
            // indien nee: maak nieuwe lijst met die gemeente erin
            // daarna de straat met meeste gemeentes halen dmv .Count()
            // Dictionary<string, List<Adres>> gementesPerStraat = 
            // List.ToDictionary(lambda 1: keys, lambda2: values)
            // idee: Select.Distinct straatnamen: lambda 1; en 
            // Dictionary<string, List<Adres>> adressenPerStraat = Data.Select(a => a.Straat).Distinct().ToDictionary(x=>x, x=> Data.Where(a => a.Straat == x).ToList());
            
            var straatGegroepeerd = Data.ToLookup(a => a.Straat);
            var resultaat = straatGegroepeerd.OrderByDescending(x => x.Count());

            var eersteGroep = resultaat.First();
            //Console.WriteLine(eersteGroep.GetType());

            var eersteGroepGesorteerd = eersteGroep.OrderBy(a => a.Provincie).ThenBy(a => a.Gemeente);

            foreach (Adres a in eersteGroepGesorteerd) {
                Console.WriteLine(a);
            }
        }

        public static void MeestVoorkomendeStraatnamen(int n) {
            var straatGegroepeerd = Data.ToLookup(a => a.Straat);
            var resultaat = straatGegroepeerd.OrderByDescending(x => x.Count());

            var eerste3Groepen = resultaat.Take(n);

            //modeloplossing, met SelectMany():
            var r = eerste3Groepen.SelectMany(x => x.ToList());
            var z = r.OrderBy(x => x.Straat).ThenBy(x => x.Provincie).ThenBy(x => x.Gemeente).ToList();
            foreach (var a in z) {
                Console.WriteLine(a);
            }

            //foreach (var groep in eerste3Groepen) {
            //    var groepGesorteerd = groep.OrderBy(a => a.Provincie).ThenBy(a => a.Gemeente);
            //    Console.WriteLine(groep.Count());
            //    foreach (Adres a in groepGesorteerd) {
            //        Console.WriteLine(a);
            //    }
            //}
        }

        public static List<string> Gemeenschappelijk(string g1, string g2) {
            return getStraatnaam(g1).Intersect(getStraatnaam(g2)).ToList();
        }

        public static List<string> getStraatnaam(string g) {
            return Data.Where(x => x.Gemeente == g).Select(x => x.Straat).ToList();
        }

        public static List<string> EnkelInX(string x, List<string> nietInY) {
            var stratenInX = getStraatnaam(x);
            foreach (string gemeente in nietInY) {
                stratenInX = stratenInX.Except(getStraatnaam(gemeente)).ToList();
                //stratenInX = stratenInX.Except(gemeente).ToList();
            }
            return stratenInX;
        }

        public static string gemeenteMetMaxAantalStraatnamen() {
            var r = Data.GroupBy(x => x.Gemeente).Select(x => new { x.Key, n = x.Count() }).OrderByDescending(x => x.n).First();
            return r.Key;
        }

        static string LangsteStraatnaam() {
            var s = Data.GroupBy(x => x.Straat.Length).OrderByDescending(x => x.Key).First().First().Straat;
            return s;
        }
        static Adres AdresMetLangsteStraatnaam() {
            var s = Data.GroupBy(x => x.Straat.Length).OrderByDescending(x => x.Key).First().First();
            return s;
        }

        //// adressen.Select(x=>x.Straat)

        ////unieke straatnamen:
        static List<Adres> uniekeStraatnamen() {
            var r = Data.GroupBy(x => x.Straat).Where(g => g.Count() == 1).Select(g => g.First());
            return r.ToList();
        }

        static List<Adres> uniekeStraatnamenInGemeente(string gemeente) {
            var r = Data.GroupBy(x => x.Straat).Where(g => g.Count() == 1).Select(g => g.First()).Where(a => a.Gemeente == gemeente);
            return r.ToList();
        }



    }
}
