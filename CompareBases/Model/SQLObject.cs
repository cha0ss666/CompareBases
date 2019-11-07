using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CompareBases
{
    public interface ISource
    {
        string Source { get; set; }

        string Hash { get; set; }

        bool SourceNotNull { get; set; }
    }

    public class SQLObject : ISource
    {
        public string ShemaName;

        public string ObjectName;

        public string SQLName
        {
            get
            {
                return "[" + ShemaName + "]" + (string.IsNullOrEmpty(ObjectName) ? "" : ".[" + ObjectName + "]");
            }
        }

        public string TypeMark;

        public string Source { get; set; }

        public string Hash { get; set; }

        public bool SourceNotNull { get; set; }

        public DateTime? CreateDate;

        public DateTime? ModifyDate;

        public SQLObject()
        { }
        
        /// <summary>
        /// Создать объект с заполнением имени в формате [схема].[имя]
        /// </summary>
        /// <param name="SQLName"></param>
        public SQLObject(string SQLName)
        {
            int index = SQLName.IndexOf('.');
            if (index > 0)
            {
                ShemaName = SQLName.Substring(1, index - 2);
                ObjectName = SQLName.Substring(index + 2, SQLName.Length - index - 3);
            }
            else
            {
                ShemaName = SQLName.Substring(1, SQLName.Length - 2);
                ObjectName = "";
            }
        }

    }
}
