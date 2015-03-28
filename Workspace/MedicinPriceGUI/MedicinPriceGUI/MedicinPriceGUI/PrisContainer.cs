using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicinPriceGUI
{
    public class PrisContainer : IEnumerable<PrisPunkt>
    {
        private Dictionary<string, Dictionary<int, PrisPunkt>> data = new Dictionary<string, Dictionary<int, PrisPunkt>>();

        private SortedSet<string> sortedKeys = new SortedSet<string>();

        public void Add(string dato, int varenummer, PrisPunkt prisPunkt)
        {
            if (data.ContainsKey(dato))
            {
                data[dato].Add(varenummer, prisPunkt);
            }
            else
            {
                var newDict = new Dictionary<int, PrisPunkt>();
                newDict.Add(varenummer, prisPunkt);
                data.Add(dato, newDict);
                sortedKeys.Add(dato);
            }
        }

        public Dictionary<int, PrisPunkt> this[string dato]
        {
            get
            {
                return data[dato];
            }
        }

        public PrisPunkt this[string dato, int varenummer]
        {
            get
            {
                return data[dato][varenummer];
            }
            set
            {
                if (data.ContainsKey(dato))
                {
                    if (data[dato].ContainsKey(varenummer))
                        data[dato][varenummer] = value;
                    else
                    {
                        data[dato].Add(varenummer, value);
                    }
                }
                else
                {
                    var newDict = new Dictionary<int, PrisPunkt>();
                    newDict.Add(varenummer, value);
                    data.Add(dato, newDict);
                    sortedKeys.Add(dato);
                }
            }
        }

        public PrisPunkt GetPrisPunkt(string dato, int varenummer)
        {
            return this[dato, varenummer];
        }

        public string[] GetDatoer()
        {
            var result = new string[sortedKeys.Count];

            int i = 0;
            foreach (string dato in sortedKeys)
            {
                result[i] = dato;

                i++;
            }

            return result;
        }

        public PrisPunkt[] GetPrices(int varenummer)
        {
            var result = new PrisPunkt[sortedKeys.Count];

            int i = 0;
            foreach (string dato in sortedKeys)
            {
                if (data[dato].ContainsKey(varenummer))
                    result[i] = this[dato, varenummer];
                else
                    result[i] = null;

                i++;
            }

            return result;
        }

        /// <summary>
        /// The total number of PrisPunkter contained in this PrisContainer
        /// </summary>
        public int Count
        {
            get
            {
                int result = 0;
                foreach (string dato in sortedKeys)
                    result += data[dato].Count;
                return result;
            }
        }

        public IEnumerator<PrisPunkt> GetEnumerator()
        {
            foreach (var prisDato in data.Values)
            {
                foreach (PrisPunkt pp in prisDato.Values)
                {
                    yield return pp;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
