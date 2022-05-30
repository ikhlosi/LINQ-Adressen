using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdressenLinq {
    public class Adres {
        public Adres(string provincie, string gemeente, string straat) {
            Provincie = provincie;
            Gemeente = gemeente;
            Straat = straat;
        }

        public string Provincie { get; set; }
        public string Gemeente { get; set; }
        public string Straat { get; set; }

        public override string ToString() {
            return $"{Provincie}, {Gemeente}, {Straat}";
        }
    }
}
