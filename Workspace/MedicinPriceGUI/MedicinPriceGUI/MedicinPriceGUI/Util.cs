using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

namespace MedicinPriceGUI
{
    class Util
    {
        static Dictionary<long, Lægemiddel> drugs = new Dictionary<long, Lægemiddel>();

        // Firms
        static Dictionary<int, string> firms = new Dictionary<int, string>();

        // Varenummer to DrugId dictionary
        static Dictionary<int, long> wares = new Dictionary<int, long>();

        // Price data
        static PrisContainer prices = new PrisContainer();

        // Lægemiddelform
        static Dictionary<string, string> forms = new Dictionary<string, string>();

        public static void readData(string zipFilePath)
        {
            using (ZipArchive archive = System.IO.Compression.ZipFile.OpenRead(zipFilePath))
            {
                using (StreamReader sr = new StreamReader(archive.GetEntry("lms22.txt").Open(), Encoding.GetEncoding(850)))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                        forms.Add(line.Substring(0, 7).TrimEnd(), line.Substring(7, 100).TrimEnd());
                }
            }

            // Datafiles (all zip files)
            int i = 0;
            string[] files = Directory.GetFiles(zipFilePath.Remove(zipFilePath.Length - 15), "lms.zip", SearchOption.AllDirectories);
            int t = files.Length;

            Console.Write("Processing files: 0 %");

            foreach (string file in files)
            {
                using (ZipArchive archive = System.IO.Compression.ZipFile.OpenRead(file))
                {
                    string dato = "UNKNOWN";
                    using (StreamReader sr = new StreamReader(archive.GetEntry("system.txt").Open(), Encoding.GetEncoding(850)))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (line.StartsWith("00"))              // The system.txt line containing the date
                            {
                                dato = line.Substring(47, 8);       // The date
                                break;
                            }
                        }
                    }

                    i++;

                    if (dato == "UNKNOWN")
                    {
                        continue;
                    }

                    Form1.updateProgressBar(((i * 100) / t));

                    // Update firms
                    using (StreamReader sr = new StreamReader(archive.GetEntry("lms09.txt").Open(), Encoding.GetEncoding(850)))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            int firmID = Int32.Parse(line.Substring(0, 6).Trim());

                            if (!firms.ContainsKey(firmID))
                            {
                                string name = line.Substring(26, 32).TrimEnd();
                                firms.Add(firmID, name);
                            }
                        }
                    }

                    // Update drugs
                    using (StreamReader sr = new StreamReader(archive.GetEntry("lms01.txt").Open(), Encoding.GetEncoding(850)))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            long drugId = Int64.Parse(line.Substring(0, 11).Trim());

                            if (!drugs.ContainsKey(drugId))
                            {
                                string name = line.Substring(29, 30).TrimEnd();
                                string atc = removeWhitespace(line.Substring(138, 8));
                                string styrke = line.Substring(93, 20).TrimEnd();
                                string mt_owner = firms[Int32.Parse(line.Substring(126, 6))];
                                string form = forms[line.Substring(79, 7).TrimEnd()];
                                drugs.Add(drugId, new Lægemiddel(drugId, name, atc, styrke, mt_owner, form));
                            }
                        }
                    }

                    // Update names
                    using (StreamReader sr = new StreamReader(archive.GetEntry("lms21.txt").Open(), Encoding.GetEncoding(850)))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            long drugId = Int64.Parse(line.Substring(0, 11).Trim());

                            if (drugs.ContainsKey(drugId) && !drugs[drugId].HasLongName)
                            {
                                drugs[drugId].Navn = line.Substring(11, 60).TrimEnd();
                                drugs[drugId].HasLongName = true;
                            }
                        }
                    }

                    // Update Pakning, Varenummer and DDD
                    using (StreamReader sr = new StreamReader(archive.GetEntry("lms02.txt").Open(), Encoding.GetEncoding(850)))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            long drugId = Int64.Parse(line.Substring(0, 11).Trim());

                            if (drugs.ContainsKey(drugId) && drugs[drugId].Varenummer == -1)
                            {
                                int varenummer = Int32.Parse(line.Substring(11, 6).TrimEnd());
                                drugs[drugId].Varenummer = varenummer;
                                drugs[drugId].Pakning = line.Substring(29, 30).TrimEnd();
                                drugs[drugId].DDD = Double.Parse(line.Substring(90, 6) + "." + line.Substring(96, 3));

                                if (!wares.ContainsKey(varenummer))
                                    wares.Add(varenummer, drugId);
                            }
                        }
                    }

                    // Update pris-punkter
                    using (StreamReader sr = new StreamReader(archive.GetEntry("lms03.txt").Open(), Encoding.GetEncoding(850)))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            int varenummer = Int32.Parse(line.Substring(0, 6));
                            double aip = Double.Parse(line.Substring(6, 7) + "," + line.Substring(13, 2));
                            double aup = Double.Parse(line.Substring(15, 7) + "," + line.Substring(22, 2));
                            double esp = Double.Parse(line.Substring(24, 7) + "," + line.Substring(31, 2));
                            double tsp = Double.Parse(line.Substring(51, 7) + "," + line.Substring(58, 2));
                            double lth = Double.Parse(line.Substring(60, 7) + "," + line.Substring(67, 2));

                            prices[dato, varenummer] = new PrisPunkt(varenummer, dato, aip, aup, esp, tsp, lth);
                        }
                    }
                }
            }
        }

        public static string removeWhitespace(string s)
        {
            string result = string.Empty;

            foreach (char c in s)
                if (!Char.IsWhiteSpace(c))
                    result += c;

            return result;
        }

        public static void printToFile(string path)
        {
            StringBuilder priceHeader = new StringBuilder();

            TextWriter tw = new StreamWriter(path, false);

            foreach (string dato in prices.GetDatoer())
            {
                priceHeader.Append(";" + dato);
            }

            tw.WriteLine("DrugID;ATCKode;Varenummer;Navn;Pakning;Styrke;Form;Firma;PrisType" + priceHeader.ToString());

            int i = 0;
            int j = drugs.Count;

            foreach (KeyValuePair<long, Lægemiddel> kv in drugs)
            {
                StringBuilder drug = new StringBuilder();
                drug.Append(kv.Key);
                drug.Append(";\"" + kv.Value.ATC + "\"");
                drug.Append(";" + kv.Value.Varenummer);
                drug.Append(";\"" + kv.Value.Navn + "\"");
                drug.Append(";\"" + kv.Value.Pakning + "\"");
                drug.Append(";\"" + kv.Value.Styrke + "\"");
                drug.Append(";\"" + kv.Value.Form + "\"");
                drug.Append(";\"" + kv.Value.Firma + "\"");

                StringBuilder aipPrices = new StringBuilder();
                StringBuilder aupPrices = new StringBuilder();
                StringBuilder espPrices = new StringBuilder();
                StringBuilder tspPrices = new StringBuilder();
                StringBuilder lthPrices = new StringBuilder();

                foreach (PrisPunkt pp in prices.GetPrices(kv.Value.Varenummer))
                {
                    aipPrices.Append(";");
                    aupPrices.Append(";");
                    espPrices.Append(";");
                    tspPrices.Append(";");
                    lthPrices.Append(";");

                    if (pp != null)
                    {
                        if (!pp.AIP.Equals(0)) aipPrices.Append(pp.AUP);
                        if (!pp.AUP.Equals(0)) aupPrices.Append(pp.AIP);
                        if (!pp.ESP.Equals(0)) espPrices.Append(pp.ESP);
                        if (!pp.TSP.Equals(0)) tspPrices.Append(pp.TSP);
                        if (!pp.LTH.Equals(0)) lthPrices.Append(pp.LTH);
                    }
                }

                tw.WriteLine(drug.ToString() + ";aip" + aipPrices.ToString());
                tw.WriteLine(drug.ToString() + ";aup" + aupPrices.ToString());
                tw.WriteLine(drug.ToString() + ";esp" + espPrices.ToString());
                tw.WriteLine(drug.ToString() + ";tsp" + tspPrices.ToString());
                tw.WriteLine(drug.ToString() + ";lth" + lthPrices.ToString());

                i++;
                Form1.updateProgressBar(((i * 100) / j));
            }
        }
    }
}
