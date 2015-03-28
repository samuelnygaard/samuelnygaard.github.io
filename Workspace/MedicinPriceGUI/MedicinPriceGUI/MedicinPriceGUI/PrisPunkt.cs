using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinPriceGUI
{
    public class PrisPunkt
    {
        public int Varenummer;
        public double AIP;
        public double AUP;
        public double ESP;
        public double TSP;
        public double LTH;

        public string Dato;

        public PrisPunkt(int varenummer, string dato, double aip, double aup, double esp, double tsp, double lth)
        {
            this.Varenummer = varenummer;
            this.AIP = aip;
            this.AUP = aup;
            this.ESP = esp;
            this.TSP = tsp;
            this.LTH = lth;

            this.Dato = dato;
        }
    }
}
