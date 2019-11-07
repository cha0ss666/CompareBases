using System;       
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CompareBases
{
    public class SQLText
    {
        public static string GetTextNormNewLine(string source)
        {
            return source.Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n");
        }

        public static string GetBodySQLProcedure(string source)
        {
            const string GO = "\r\nGO\r\n";
            const string CREATE = "CREATE";
            const string CREATETABLE = "CREATE TABLE";
            string s = GetTextNormNewLine(source) + "\r\n";
            int i1, i2, i3;
            //  -- убираются три первых /ngo/n если не встретили /ncreate
            i2 = s.IndexOf(GO, 0, StringComparison.InvariantCultureIgnoreCase);
            i1 = s.IndexOf(CREATE, 0, StringComparison.InvariantCultureIgnoreCase);
            if (i1 >= 0 && s.IndexOf(CREATETABLE, 0, StringComparison.InvariantCultureIgnoreCase) == i1)
            { 
                //если первый CREATE это создание таблицы, то выходим - таблицы не обрезаем
                s = s.Trim();
                return s;
            }
            if (i2 >= 0 && i2 < i1)
            {
                i3 = s.IndexOf(GO, i2 + 1, StringComparison.InvariantCultureIgnoreCase);
                if (i3 > i2 && i3 < i1)
                {
                    i2 = i3;
                    i3 = s.IndexOf(GO, i2 + 1, StringComparison.InvariantCultureIgnoreCase);
                    if (i3 > i2 && i3 < i1) i2 = i3;
                }
                s = s.Substring(i2 + GO.Length);
            }
            
	        //  -- убирается два последних /ngo/n если не встретили end или )
            i2 = s.LastIndexOf(GO, StringComparison.InvariantCultureIgnoreCase);
            i1 = s.LastIndexOf("END", StringComparison.InvariantCultureIgnoreCase);
            if (i1 < 0) i1 = s.LastIndexOf(")", StringComparison.InvariantCultureIgnoreCase);
            if (i2 >= 0 && i2 > i1)
            {
                i3 = s.LastIndexOf(GO, i2, StringComparison.InvariantCultureIgnoreCase);
                if (i3 >= 0 && i3 < i2 && i3 > i1) i2 = i3;
                s = s.Substring(0, i2);
            }

            s = s.Trim();
            return s;
        }

        public static string GetSQLForHash(string s)
        {
            string r = GetTextNormNewLine(s);
            //если первые символы это CREATE TABLE, то игнорируем регистр всего файла
            const string CREATETABLE = "CREATE TABLE";
            if (s.Substring(0, s.Length > 200 ? 200 : s.Length).Trim().ToUpper().StartsWith(CREATETABLE))
            {
                r = r.ToUpper();
            }
            //обрабатываем Trim каждую строку отдельно! //(исключение - строки в которых символ ' встречается не четное кол-во раз)
            string[] lines = r.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            //if (!lines.Any(line => line.Count(c => c == '\'') % 2 == 1))
            {
                for (int i = 0; i < lines.Length; i++)
                    lines[i] = lines[i].Trim();
                string.Join("\r\n", lines);
                
                /*r = lines.Aggregate((string)null, 
                    (res, line) =>
                    {
                        return (res == null ? string.Empty : res + "\r\n") +line.Trim();
                    });
                */
            }
            const string CREATE = "CREATE";
            //замена строки с CREATE на заглавными "CREATE" и + обработанную функцией GetSQLForHash_HeadLine
            int i1 = 0;
            while ((i1 = r.IndexOf(CREATE, i1, StringComparison.InvariantCultureIgnoreCase)) >= 0)
            {
                i1 += CREATE.Length;
                int cnt = r.IndexOf("\r\n", i1) - i1;
                string s2 = cnt < 0 ? r.Substring(i1) : r.Substring(i1, cnt);
                if (GetSQLForHash_HeadLine(ref s2))
                {
                    r = (i1 - CREATE.Length > 0 ? r.Substring(0, i1 - CREATE.Length) : string.Empty) + CREATE + s2 + (cnt >= 0 ? r.Substring(i1 + cnt) : string.Empty);
                    break;
                }
            }
            return r;
        }

        public static bool GetSQLForHash_HeadLine(ref string s)
        {
            try
            {
                int maxi = s.IndexOf("."); // maxi > i - если указана схема, то определяем, что после схемы явно не будет названия "PROCEDURE"/"FUNCTION"                
                if (maxi < 0)
                { 
                    //определяем первый непробельный символ
                    var st = s.Trim();
                    if (st.Length > 1)
                    {
                        var si = s.IndexOf(st[0]);
                        //теперь определяем первый пробел или табуляцию после первого слова
                        maxi = s.IndexOfAny(new char[] { ' ', '\t' }, si);
                        // т.е. для строки " TABLE MyTab " maxi будет указывать на пробел между словами
                    }
                }
                if (maxi < 0) maxi = int.MaxValue;
                int i = s.IndexOf("TABLE", StringComparison.InvariantCultureIgnoreCase);
                if (i >= 0 && i < maxi) i += "TABLE".Length;
                else
                {
                    i = s.IndexOf("PROCEDURE", StringComparison.InvariantCultureIgnoreCase);
                    if (i >= 0 && i < maxi) i += "PROCEDURE".Length;
                    else
                    {
                        i = s.IndexOf("FUNCTION", StringComparison.InvariantCultureIgnoreCase);
                        if (i >= 0 && i < maxi) i += "FUNCTION".Length;
                        else
                        {
                            i = s.IndexOf("VIEW", StringComparison.InvariantCultureIgnoreCase);
                            if (i >= 0 && i < maxi) i += "VIEW".Length;
                            else return false;
                        }
                    }
                }
                //добавляем "dbo." если его нету (таким образом либо добавляем так где он отсутствует, но должен быть, либо одинаково добавляем во все версии там где не нужен)
                if (s.IndexOf("dbo.") == -1 && s.IndexOf("[dbo].") == -1)
                {
                    s = s.Substring(0, i) + " dbo." + s.Substring(i).TrimStart();
                }
                //иначе убираем все пробелы после найденых PROCEDURE или FUNCTION
                else
                {
                    s = s.Substring(0, i) + " " + s.Substring(i).TrimStart();
                }
                //убираем из строки все символы [ и ]
                s = s.Replace("[", "").Replace("]", "");
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Выдает получает хеш SHA256.
        /// </summary>
        /// <param name="str">Исходные данные</param>
        /// <returns>Хеш в виде строки из 64 символа</returns>
        public static string GetHash(string str)
        {
            var sha = SHA256.Create();
            var result = sha.ComputeHash(Encoding.Unicode.GetBytes(str));
            return BitConverter.ToString(result).Replace("-", string.Empty);
        }
    }
}
