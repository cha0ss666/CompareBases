using CompareBases.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CompareBases
{
    public class Comparator
    {
        public const int MaxCountConnect = 5;
        public Dictionary<string, string> BaseConnectionString;
        public event Action<string> OnProgress;
        public event Action<string> OnError;
        public event Action OnComplete;
        /// <summary>
        /// Результат сравнения, в виде списка по базам данных
        /// </summary>
        public Dictionary<string, List<SQLObject>> Objects;
        public int DistinctNameTotal;
        /// <summary>
        /// Результат сравнения, в виде списка по полю SQLName
        /// </summary>
        public Dictionary<string, List<CompareObject>> CompareResult;
        /// <summary>
        /// Уникальное имя для сравнения, подходит для файловой системы
        /// </summary>
        public string UniqueName;
        /// <summary>
        /// Путь для сохранения файлов
        /// </summary>
        public string PathToSave;

        public void Compare(Dictionary<string, string> baseConnectionString)
        {
            BaseConnectionString = baseConnectionString;
            Objects = new Dictionary<string,List<SQLObject>>();
            var listGetWork = new List<string>();
            var timeFormatString = "yyyy'-'MM'-'dd' 'HH'.'mm'.'ss'.'fffffff";
            UniqueName = "Comp " + DateTime.Now.ToString(timeFormatString, CultureInfo.InvariantCulture);
            ThreadGetListBaseObjectsStat = new Dictionary<string, string>();
            
            //предварительная проверка подключений
            lock (ThreadGetListBaseObjectsStat)
            {
                var prog = "Проверка подключений...";
                if (OnProgress != null) OnProgress(prog);
            }
            var testFail = "";
            foreach (var cs in baseConnectionString)
            {
                try
                {
                    DALSql.SetConnectionString(cs.Value);
                    DataTable sqlResultCount = DALSql.ExecuteDataTable(
                        "select '123'", null);
                    if ((string)sqlResultCount.Rows[0][0] != "123") testFail += cs.Key + ", ";
                }
                catch (Exception e)
                {
                    if (OnError != null) OnError("При подключении к базе " + cs.Key + " произошла ошибка!"
                        + Environment.NewLine + Environment.NewLine + e.Message);
                    testFail += cs.Key + ", ";
                }
            }
            if (!string.IsNullOrEmpty(testFail))
            {
                testFail = testFail.Remove(testFail.Length - 2);
                lock (ThreadGetListBaseObjectsStat)
                {
                    var prog = "Ошибка при подключении к базам: " + testFail + " :(";
                    if (OnProgress != null) OnProgress(prog);
                }

                if (OnComplete != null) OnComplete();
                return;
            }
            //заполняем Objects
            while(true)
            {
                lock (Objects)
                {
                    //условие выхода
                    if (BaseConnectionString.Count == Objects.Count) break;
                    //добавляем поток к работе
                    int nowConnecting = listGetWork.Count - Objects.Count;
                    if (BaseConnectionString.Count > listGetWork.Count && nowConnecting < MaxCountConnect)
                    {
                        var db = BaseConnectionString.Keys.First(bcs => !listGetWork.Any(lgw => lgw == bcs));
                        listGetWork.Add(db);
                        var th = new Thread(CompareDB);
                        th.IsBackground = true;
                        th.Start(db);
                    }
                    //обновляем статус в интерфейсе
                    lock (ThreadGetListBaseObjectsStat)
                    {
                        var prog = "Получение данных: " + (Objects.Count * 100 / BaseConnectionString.Count).ToString() + "%..."
                            + " (сделано " + Objects.Count.ToString() + " из " + BaseConnectionString.Count.ToString() + ") "
                            + Environment.NewLine
                            + " в работе: "
                                + listGetWork
                                    .Where(w => !Objects.Keys.Any(o => o == w))
                                    .Select(w => w 
                                        + (ThreadGetListBaseObjectsStat.ContainsKey(BaseConnectionString[w]) 
                                            ? "[" + ThreadGetListBaseObjectsStat[BaseConnectionString[w]] + "]"
                                            : ""))
                                    .Aggregate("", (s, w) => (s == "" ? "" : s + ", ") + w) + ".";
                        if (OnProgress != null) OnProgress(prog);
                    }
                }
                Thread.Sleep(100);
            }

            //вставлям отсутствующие объекты, которые есть на других базах
            var distinctSQLName = Objects.SelectMany(b => b.Value.Select(o => o.SQLName)).Distinct().ToList();
            DistinctNameTotal = distinctSQLName.Count;
            foreach (var db in Objects.Values)
            {
                db.AddRange(distinctSQLName
                    .Where(don => !db.Any(o => don == o.SQLName))
                    //создаем пустой SQLObject, для обозначения, что объекта нет
                    .Select(don => new SQLObject(don))
                    );
            }

            //сравниваем Objects
            CompareResult = Objects
                //общий список с указанием базы
                .SelectMany(ob => ob.Value.Select(obj => new { Base = ob.Key, SQLO = obj }))
                //группируем по имени
                .GroupBy(a => a.SQLO.SQLName)
                .ToDictionary(gn => gn.Key
                    , gn => gn
                        //группируем по уникальности текста, и сохраняем список баз
                        .GroupBy(g => g.SQLO.Hash)
                        .Select(gh => new CompareObject()
                            {
                                Hash = gh.Key,
                                SQLName = gn.Key,
                                Bases = gh.ToDictionary(go => go.Base, go => go.SQLO),
                                FirstObject = gn.First(a => !string.IsNullOrEmpty(a.SQLO.TypeMark)).SQLO
                            })
                        .OrderBy(co => co.BasesString)
                        .ToList());

            if (OnComplete != null) OnComplete();
        }

        private void CompareDB(object dbParam)
        {
            var db = (string)dbParam;
            List<SQLObject> listO;
            try
            {
                listO = GetListBaseObjects(BaseConnectionString[db]);
            }
            catch
            {
                try
                {
                    lock (ThreadGetListBaseObjectsStat)
                    {
                        ThreadGetListBaseObjectsStat[BaseConnectionString[db]] = "Ошибка!";
                    }
                    Thread.Sleep(3000);
                    lock (ThreadGetListBaseObjectsStat)
                    {
                        ThreadGetListBaseObjectsStat.Remove(BaseConnectionString[db]);
                    }
                    listO = GetListBaseObjects(BaseConnectionString[db]);
                }
                catch(Exception e)
                {
                    if (OnError != null) OnError("При получении данных базы " + db + " произошла ошибка!"
                        + Environment.NewLine + Environment.NewLine + e.Message
                        + Environment.NewLine + Environment.NewLine + e.ToString());
                    listO = new List<SQLObject>();
                }
            }

            lock (Objects)
            {
                Objects.Add(db, listO);
            }
        }

        /// <summary>
        /// Cтатусы потоков работающих над GetListBaseObjects. Ключ - connectionString
        /// </summary>
        private Dictionary<string, string> ThreadGetListBaseObjectsStat;

        public List<SQLObject> GetListBaseObjects(string connectionString)
        {
            lock (ThreadGetListBaseObjectsStat)
            {
                ThreadGetListBaseObjectsStat.Add(connectionString, "?");
            }
            //запрос
            //if (OnStatusChange != null) OnStatusChange("Запрос к BD...", null);
            DALSql.SetConnectionString(connectionString);
            var srcs = new SQLScriptDB();
            srcs.WithTable = Settings.Param.CompareWithTable;
            srcs.TableWithTrigger = Settings.Param.CompareTableWithTrigger;
            srcs.FilterPrefix = Settings.Param.Prefix;
            srcs.FilterIgnoreByPrefix = Settings.Param.IgnoreByPrefix;
            srcs.FilterIgnoreByPostfix = Settings.Param.IgnoreByPostfix;
            
            //выясняем количество
            DataTable sqlResultCount = DALSql.ExecuteDataTable(
                srcs.GetScriptCountObjects(), null);
            int sqlCountRow = (int)sqlResultCount.Rows[0][0];
            int sqlCountRowReading = 0;

            DataTable sqlResult = new DataTable();

            //стартуем поток проверяющий кол-во считанных строк
            var thSqlReading = new Thread(() => {
                while (sqlCountRow > 0)
                {
                    sqlCountRowReading = sqlResult.Rows.Count;
                    Thread.Sleep(100);
                    lock(ThreadGetListBaseObjectsStat)
                    {
                        if (sqlCountRow > 0)
                            ThreadGetListBaseObjectsStat[connectionString] = (sqlCountRowReading * 100 / sqlCountRow).ToString() + "%";
                    }
                }
            });
            thSqlReading.IsBackground = true;
            thSqlReading.Start();
            
            //запускаем запрос
            DALSql.ExecuteDataTable(
                srcs.GetScriptDefinitionObjects()
                , null, sqlResult);

            //завершение считывания
            lock (ThreadGetListBaseObjectsStat)
            {
                sqlCountRow = 0;
                ThreadGetListBaseObjectsStat[connectionString] = "...";
            }
            DALSql.CloseConnection();

            //маппинг
            List<SQLObject> listSQL = new List<SQLObject>();
            foreach (DataRow item in sqlResult.Rows)
            {
                var no = new SQLObject()
                    {
                        ShemaName = (string)item["schemaName"] ?? "",
                        ObjectName = (string)item["name"] ?? "",
                        Source = (string)item["definition"] ?? "",
                        CreateDate = item.IsNull("create_date") ? null : (DateTime?)(DateTime)item["create_date"],
                        ModifyDate = item.IsNull("modify_date") ? null : (DateTime?)(DateTime)item["modify_date"],
                        TypeMark = (string)item["type"] ?? "",
                    };

                //фильтры перенесены в sql
                //if (!Settings.Param.IgnoreByPrefix.Any(ip => no.SQLName.StartsWith(ip))
                //    && !Settings.Param.IgnoreByPostfix.Any(ip => no.SQLName.EndsWith(ip))) 
                listSQL.Add(no);

                no.Source = SQLText.GetBodySQLProcedure(no.Source);
                no.Hash = SQLText.GetHash(SQLText.GetSQLForHash(no.Source));
            }

            //if (OnStatusChange != null) OnStatusChange("Получено объектов: " + listSQL.Count, null);
            return listSQL;
        }

        /// <summary>
        /// Сохраняет объект.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="suffix">Суфикс файла с базами через запятую</param>
        /// <returns></returns>
        public string SaveSQLObject(SQLObject obj, string suffix)
        {
            if (suffix.Length > 70)
            {
                suffix = suffix.Substring(0, 50) + "... " + (suffix.Count(cs => cs == ',') + 1) + "баз";
            }
            var fn = obj.ShemaName + "." + obj.ObjectName;
            foreach (char c in Path.GetInvalidFileNameChars()) fn = fn.Replace(c, '_');
            fn = Path.Combine(PathToSave,  fn
                + (string.IsNullOrEmpty(suffix) ? "" : " - " + suffix)
                + ".sql");

            using(var fileS = File.Create(fn))
            using(var file = new StreamWriter(fileS, Encoding.UTF8))
            {
                if (obj.Hash == null)
                    file.WriteLine();
                else
                    file.Write(obj.Source + Environment.NewLine);
            }
            return fn;
        }

        /// <summary>
        /// Проставляет Found в CompareResult для тех у кого в ресурсах есть subString
        /// </summary>
        /// <param name="subString">Подстрока для поиска, если пусто, то Found выставляется у всех</param>
        public void SetCheckFound(string subString)
        { 
            foreach(var list in CompareResult.Values)
                foreach (var co in list)
                { 
                    if (string.IsNullOrEmpty(subString))
                        co.Found = true;
                    else if (co.Bases.Count > 0)
                    {
                        var obj = co.Bases.Values.First();
                        co.Found = !string.IsNullOrEmpty(obj.Source) 
                            && obj.Source.IndexOf(subString, StringComparison.CurrentCultureIgnoreCase) >= 0;
                    }
                    else
                        co.Found = false;
                }
        }

        public string TextSVNTableToCompare(string source)
        {
            //находим первый символ внутри скобок с колонками
            var indexBegin = source.IndexOf("CREATE TABLE", StringComparison.InvariantCultureIgnoreCase);
            if (indexBegin < 0) return source;
            var t1 = source.IndexOf("(", indexBegin, StringComparison.InvariantCultureIgnoreCase);
            if (t1 < 0) return source;
            if (t1 - indexBegin > 200) return source;
            indexBegin = t1 + 1;

            //читаем построчно
            var lineBegin = indexBegin;
            var addZpt = false; // для контроля добавления только 1 запятой, а также нужно ли её убирать вконце
            var dicCols = new Dictionary<string, string>();
            do
            {
                var lineNext = source.IndexOf(Environment.NewLine, lineBegin);
                if (lineNext < 0) return source; //неожиданный конец
                var line = source.Substring(lineBegin, lineNext - lineBegin);
                lineNext += 2;
                if (string.IsNullOrWhiteSpace(line))
                {
                    lineBegin = lineNext;
                    continue;
                }

                line = line.TrimEnd();

                //проверка, что эта строка уже не колонка
                var lt = line.Trim().ToUpper();
                if (lt.StartsWith("CONSTRAINT")
                    || lt.StartsWith(")"))
                {
                    break;
                }

                //каждая строка кроме послеедней должна кончаться запятой
                if (!line.EndsWith(",")) 
                {
                    line += ",";
                    if (addZpt) return source;
                    addZpt = true;
                }

                //выделения названия колонки для сортировки
                var key = (lt.Contains("IDENTITY") ? "0" : "1") + lt.Replace("[", "").Replace("]", "");

                dicCols.Add(key, line);

                lineBegin = lineNext;
            } while(true);
            //после цикла в lineBegin первый символ после колонок

            if (dicCols.Count < 2) return source;

            var newColsText = dicCols
                .OrderBy(r => r.Key)
                .Select(r => r.Value)
                .Aggregate("", (s, l) => s + Environment.NewLine + l);

            //если добавляли запятую, то удаляем её (удаление посл символа т.к. вконце каждой строки обязательна запятая)
            if (addZpt) newColsText = newColsText.Remove(newColsText.Length - 1);

            var result = source.Substring(0, indexBegin)
                + newColsText + Environment.NewLine
                + source.Substring(lineBegin);

            return result;
        }
    }
}
