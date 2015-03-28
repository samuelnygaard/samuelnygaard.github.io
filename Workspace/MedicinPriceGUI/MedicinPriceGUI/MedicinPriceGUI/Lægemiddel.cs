using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinPriceGUI
{
    public class Lægemiddel
    {
        public readonly long ID;
        public string Navn;
        public string ATC;
        public int Varenummer;
        public string Pakning;
        public string Styrke;
        public string Form;
        public string Firma;
        public double DDD;

        public bool HasLongName = false;

        public Lægemiddel(long ID, string navn, string atc, string styrke, string firma, string form)
        {
            this.ID = ID;
            this.Navn = navn;
            this.ATC = atc;
            this.Styrke = styrke;
            this.Firma = firma;
            this.Form = form;
            this.Varenummer = -1;
        }
    }
}
