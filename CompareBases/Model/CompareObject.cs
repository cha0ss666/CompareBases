using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareBases
{
    public class CompareObject
    {
        public string SQLName;

        public string Hash;

        /// <summary>
        /// Базы, где процедуры идентичны
        /// </summary>
        public Dictionary<string, SQLObject> Bases;

        public SQLObject FirstObject;

        public bool Found = true;

        public string BasesString
        {
            get 
            {
                if (Bases == null || Bases.Count == 0) return "";
                
                return Bases.OrderBy(i => i.Key).Aggregate("", (s, i) => (s == "" ? "" : s + ", ") + i.Key);
            }
        }
    }
}
