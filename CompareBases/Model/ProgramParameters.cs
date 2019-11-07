using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CompareBases.Model
{
    [Serializable]
    public class ProgramParameters
    {
        public SDic<string, string> ConnectionStrings;
        public List<string> SchemeIndividual; //не используется
        public List<string> IgnoreByPrefix;
        public List<string> IgnoreByPostfix;
        //[XmlIgnore]
        public string Prefix;
        public string SVNPath;
        public string SnapshotPath;
        public string SVNCommandLog;
        public bool CompareWithTable = true;
        public bool CompareTableWithTrigger = true;
        public bool ShowTableFirst = true;
        public bool ShowSortType = true;

        /// <summary>
        /// При вызове метода Commite
        /// </summary>
        public event Action OnChange;

        public void Commite()
        {
            if (OnChange != null) OnChange();
        }

        /// <summary>
        /// Выдает сокращенное название базы для удобства работы.
        /// </summary>
        /// <param name="bdName">Полное имя базы из списка ConnectionStrings</param>
        /// <returns>Часть имени после последней точки (само имя базы)</returns>
        public string ConvertBDNameToMini(string bdName)
        { 
            var index = bdName.LastIndexOf('.');
            if (index < 0) return bdName;
            var mini = bdName.Substring(index + 1);
            if (ConnectionStrings.Keys.Count(s => s.EndsWith(mini)) > 1) 
                return bdName;
            else 
                return mini;
        }
    }
}
