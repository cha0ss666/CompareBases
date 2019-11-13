using CompareBases.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompareBases
{
    public partial class Form1 : Form
    {
        public Comparator Comp;
        public string PathResult = "Results";
        public bool ExecIsBusy = false;
        private bool ModeCompared = false;
        private string TempGO = Environment.NewLine + "GO" + Environment.NewLine;

        public Form1()
        {
            if (!Settings.ExistFileParam())
            {
                var pfn = Settings.CreateDefaultFileParam();
                MessageBox.Show("При первом запуске заполните файл настроек.", "Первый запуск");
                Process.Start("notepad.exe", pfn);
                Environment.Exit(0);
            }
            if (Settings.Param.ConnectionStrings == null)
            {
                MessageBox.Show("Заполнить список баз данных можно на вкладке Утилиты.", "Первый запуск");
            }
            InitializeComponent();
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            groupBoxComp2.Top = groupBoxComp1.Top;
            Settings.Param.OnChange += param_Change;
            Settings.Param.Commite();
            checkBoxSortType.Checked = Settings.Param.ShowSortType;
            checkBoxShowTableTop.Checked = Settings.Param.ShowTableFirst;
            checkBoxShowTableTop.Enabled = Settings.Param.ShowSortType;

            dataGridView.ContextMenuStrip = dataGridViewMenuStrip;
        }

        private bool param_Change_run = false;
        private void param_Change()
        {
            if (param_Change_run) return;
            param_Change_run = true;
            checkBoxCompareWithTable.Checked = Settings.Param.CompareWithTable;
            checkBoxCompareTableWithTrigger.Checked = Settings.Param.CompareTableWithTrigger;
            textBoxPrefix.Text = Settings.Param.Prefix;
            textBoxIPref.Text = string.Join(Environment.NewLine, Settings.Param.IgnoreByPrefix);
            textBoxIPost.Text = string.Join(Environment.NewLine, Settings.Param.IgnoreByPostfix);
            //Коррекция (только Form1)
            checkBoxCompareTableWithTrigger.Enabled = checkBoxCompareWithTable.Checked;
            param_Change_run = false;
        }

        private void сompare_CheckedChanged(object sender, EventArgs e)
        {
            if (param_Change_run) return;
            param_Change_run = true;
            //Коррекция ввода (только Form1)
            if (sender is TextBox && ((TextBox)sender).Name == "textBoxPrefix") textBoxPrefix_Correct();
            checkBoxCompareTableWithTrigger.Enabled = checkBoxCompareWithTable.Checked;
            //Применение настроек и синхронизация
            Settings.Param.CompareWithTable = checkBoxCompareWithTable.Checked;
            Settings.Param.CompareTableWithTrigger = checkBoxCompareTableWithTrigger.Checked;
            Settings.Param.Prefix = textBoxPrefix.Text.Trim();
            Settings.Param.IgnoreByPrefix = new List<string>(textBoxIPref.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            Settings.Param.IgnoreByPostfix = new List<string>(textBoxIPost.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));

            Settings.Param.Commite();
            param_Change_run = false;
        }

        private void textBoxPrefix_Correct()
        {
            var text = textBoxPrefix.Text;
            var st = textBoxPrefix.SelectionStart;
            if (text.Length == 0) return;
            if (text[0] == '%') return;
            var indexDot = text.IndexOf('.');
            if (indexDot == 0) return;

            if (text[0] != '[')
            {
                text = "[" + text;
                st++;
            }
            indexDot = text.IndexOf('.');
            if (indexDot > 0)
            {
                if (text[indexDot - 1] != ']')
                {
                    text = text.Insert(indexDot, "]");
                    indexDot++;
                    st++;
                }
                if (indexDot + 1 < text.Length && text[indexDot + 1] != '[')
                {
                    text = text.Insert(indexDot + 1, "[");
                    st++;
                }
            }
            textBoxPrefix.Text = text;
            textBoxPrefix.SelectionStart = st;
        }

        private void butStart_Click(object sender, EventArgs e)
        {
            if (ModeCompared)
            {
                ModeCompared = false;
                groupBoxComp1.Visible = true;
                groupBoxComp2.Visible = false;
                butStart.Text = "Сравнить выбранные";
                try
                {
                    if (splitContainer.SplitterDistance <= 2 && 350 <= splitContainer.Width - splitContainer.Panel2MinSize)
                        splitContainer.SplitterDistance = 350;
                }
                catch
                { }
            }
            else
            {
                StartCompare();
            }
        }
        

        private int CountEqualsChars(string str1, string str2)
        {
            int i = 0;
            for (; i < str1.Length && i < str2.Length; i++)
                if (str1[i] != str2[i]) return i;
            return i;
        }

        private void StartCompare()
        { 
            var listBD = compareBases.GetBases();
            if (listBD.Count == 0)
            {
                MessageBox.Show("Выбирите базы данных для чтения");
                return;
            }

            //упрощаем имена баз убирая одинаковые части из каждой имени базы (одинаковые только из выбранных)
            var k0 = listBD.Keys.First();
            var cntBeginName = listBD.Keys.Min(k => CountEqualsChars(k, k0));
            if (cntBeginName > 0)
            {
                //var spaceBeginName = k0.IndexOf(' ');     //вариант когда сокращаем только сокращенное название группы
                //вариант когда сокращаем в т.ч. и одинаковые части IP адреса, оставляем после поледней точки (всю различающуюся часть IP адреса, либо саму базу)
                var spaceBeginName = k0.Substring(0, cntBeginName).LastIndexOfAny(new char[] { '.', ' ' });
                if (spaceBeginName > 0)
                {
                    if (cntBeginName > spaceBeginName) cntBeginName = spaceBeginName + 1;

                    listBD = listBD.ToDictionary(p => p.Key.Substring(cntBeginName).TrimStart(' '), p => p.Value);
                }
            }


            Comp = new Comparator();
            Comp.OnProgress += (msg) =>
                {
                    this.Invoke((Action)(() =>
                        {
                            labelProgress.Text = "Прогресс: " + msg;
                        }));
                };
            Comp.OnError += (msg) =>
            {
                this.Invoke((Action)(() =>
                {
                    MessageBox.Show(msg, "Ошибка при получени данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
            };
            Comp.OnComplete += () =>
            {
                this.Invoke((Action<bool>)dataGridView_FillCompare, true);
            };

            var th = new Thread(() =>
                {
                    Comp.Compare(listBD);
                });
            /*
            comboBoxCompBaseFirst.Items.Clear();
            comboBoxCompBaseFirst.Items.AddRange(listBD.Keys.ToArray());
            comboBoxCompBaseFirst.SelectedIndex = 0;
            */
            th.IsBackground = true;
            th.Start();
            butStart.Enabled = false;
            groupBoxComp1.Enabled = false;
            groupBoxComp2.Enabled = false;
        }

        public void dataGridView_FillCompare(bool firstAfterCompare)
        {
            ModeCompared = true;
            groupBoxComp1.Visible = false;
            groupBoxComp2.Visible = true;
            groupBoxComp1.Enabled = true;
            groupBoxComp2.Enabled = true;
            butStart.Text = "Новое сравнение";

            groupBox2.Enabled = true;
            butStart.Enabled = true;
            checkBoxFilter.Enabled = true;
            checkBoxFilterText.Enabled = true;
            textBoxFilterText.Enabled = true;
            butFilter.Enabled = true;
            butSelExecBases.Enabled = true;

            if (Comp.CompareResult == null) return;

            labelProgress.Text =
                "Выберите несколько ячеек для сравнения по кнопке справа (или контекстным меню). " + Environment.NewLine
                + "Двойной клик по ячейкам открывает скрипт или помещает в вкладку Применить.";
            try
            {
                splitContainer.SplitterDistance = splitContainer.Panel1MinSize;
            }
            catch
            { }

            if (firstAfterCompare)
            {
                //выставляем галочку, если баз больше 1
                checkBoxFilter.Checked = Comp.Objects.Count > 1;
                butFilter.Font = new Font(butFilter.Font, FontStyle.Regular);
            }

            Text = Comp.Objects.Keys.Aggregate("", (s, i) => (s == "" ? "" : s + ", ") + i);
            Comp.PathToSave = Path.Combine(PathResult, Comp.UniqueName);
            Directory.CreateDirectory(Comp.PathToSave);

            var maxCnt = Comp.CompareResult.Count == 0 ? 0 : Comp.CompareResult.Values.Max(c => c.Count);

            //подготовка столбцов
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            dataGridView.Columns.Add("SQLName", "Объект (нет ни одного)");
            dataGridView.Columns["SQLName"].Width = 350;

            if (maxCnt == 0) return;

            for (int i = 0; i < maxCnt; i++)
            {
                int col = dataGridView.Columns.Add("Bases" + i.ToString(), "Вариант " + (i + 1).ToString());
                dataGridView.Columns[col].Width = 200;
            }

            //подготовка строк
            var rowsDataT = Comp.CompareResult.Keys
                .Where(r => Comp.CompareResult[r].Count(co => co.Found) > (checkBoxFilter.Checked ? 1 : 0))
                .Select(r => new {
                    SQLName = r,
                    OutName = Comp.CompareResult[r][0].FirstObject.TypeMark + " " + r,
                    CompareResultList = Comp.CompareResult[r]
                    });
            var rowsData =
                Settings.Param.ShowSortType
                ? Settings.Param.ShowTableFirst
                    ? rowsDataT.OrderBy(r => r.OutName.Replace("U ", "")).ToList()
                    : rowsDataT.OrderBy(r => r.OutName).ToList()
                : rowsDataT.OrderBy(r => r.SQLName).ToList();

            dataGridView.Columns["SQLName"].HeaderText = "Объект (всего " + rowsData.Count.ToString() + " из " + Comp.DistinctNameTotal.ToString() + ")";
            foreach (var item in rowsData)
            {
                var colsData = item.CompareResultList
                        .Where(co => co.Found)
                        .OrderBy(co => co.BasesString)
                        .ToList();
                var newRow = dataGridView.Rows[dataGridView.Rows.Add()];
                newRow.Cells[0].Value = item.OutName;
                for (int i = 0; i < colsData.Count; i++)
                {
                    newRow.Cells[i + 1].Value = colsData[i].BasesString;
                    newRow.Cells[i + 1].Tag = colsData[i];
                    if (colsData[i].Hash == null)
                        newRow.Cells[i + 1].Style.BackColor = System.Drawing.ColorTranslator.FromHtml("#BBB");
                }
                newRow.Tag = item.SQLName;
            }
        }

        private void dataGridView_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e)
        {
            //для кнопки сравнения добавляем выбор в комбобокс
            if (dataGridView.SelectedCells.Count < 3 || dataGridView.SelectedCells.Count > 25)
            {
                labelSelComp3.Visible = false;
                comboBoxSelComp3.Visible = false;
                if (comboBoxSelComp3.Items.Count > 0) comboBoxSelComp3.Items.Clear();
                return;
            }
            string selIndex = null;
            //var selIndex = comboBoxSelComp3.SelectedIndex;
            comboBoxSelComp3.Items.Clear();

            var newList = new List<string>();
            foreach (DataGridViewCell cell in dataGridView.SelectedCells)
            {
                string SQLName = cell.OwningRow.Tag as string;
                if (SQLName == null) continue;

                var val = cell.Value as string;
                if (val == null) continue;

                newList.Add(val + " - " + SQLName);
                if (cell.ColumnIndex == 0) selIndex = val + " - " + SQLName;
            }
            comboBoxSelComp3.Items.AddRange(newList.ToArray());
            if (selIndex == null) comboBoxSelComp3.SelectedIndex = 0;
            else comboBoxSelComp3.SelectedIndex = comboBoxSelComp3.Items.IndexOf(selIndex);
            /*
            if (selIndex >= 0)
            {
                if (selIndex >= newList.Count) selIndex = newList.Count - 1;
                comboBoxSelComp3.SelectedIndex = selIndex;
            }
            if (newList.Count > 0 && comboBoxSelComp3.SelectedIndex < 0) comboBoxSelComp3.SelectedIndex = 0;
            */

            if (newList.Count > 2)
            {
                labelSelComp3.Visible = true;
                comboBoxSelComp3.Visible = true;
            }
        }
        
        private void butSelComp_Click(object sender, EventArgs e)
        {
            //Номер колонки с которой начинается варианты (номер колонки Вариант 1)
            int variant1ColumnIndex = 1;

            if (dataGridView.SelectedCells.Count == 0) return;
            if (dataGridView.SelectedCells.Count < 2)
            {
                MessageBox.Show("Для сравнения выделите минимум 2 ячейки. " + Environment.NewLine + "Ячейка в 1 колонке отвечают за версию из папки.");
                return;
            }
            if (dataGridView.SelectedCells.Count > 21)
            {
                MessageBox.Show("Слишком много для сравнения.");
                return;
            }
            //словари - имя файла, имя ячейки
            var fileNamesSVN = new Dictionary<string, string>();
            var fileNames = new Dictionary<string, string>();
            foreach (DataGridViewCell cell in dataGridView.SelectedCells)
            {
                string SQLName = cell.OwningRow.Tag as string;
                if (SQLName == null) continue;

                var co = Comp.CompareResult[SQLName];

                if (cell.ColumnIndex - variant1ColumnIndex >= co.Count) continue;
                if (cell.ColumnIndex - variant1ColumnIndex < 0)
                { 
                    //версия репозитория
                    //var fnsvn = FindSVNFileBySQLObject(co[0].FirstObject);
                    var fnsvn = SaveCompareCopySVNFileBySQLObject(co[0].FirstObject);
                    if (!string.IsNullOrWhiteSpace(fnsvn))
                        fileNamesSVN.Add(fnsvn, (cell.Value as string) + " - " + SQLName);
                    continue;
                }
                //получаем версию из ячейки
                var fn = Comp.SaveSQLObject(co[cell.ColumnIndex - variant1ColumnIndex].Bases.Values.First()
                    , co[cell.ColumnIndex - variant1ColumnIndex].BasesString);
                fileNames.Add(fn, (cell.Value as string) + " - " + SQLName);
            }
            //объединяем 2 списка, чтобы SVN шел в конце
            var dfns = fileNames.Concat(fileNamesSVN);

            //находим элимент с которым нужно все сравнивать
            int indexSel = -1;
            foreach (var fn in dfns)
            {
                indexSel++;
                if (comboBoxSelComp3.Text == fn.Value) break;
            }
            if (indexSel < 0 || indexSel >= dfns.Count()) indexSel = 0;

            var fns = dfns.Select(k => k.Key).ToList();

            try
            {
                for (int i = 0; i < fns.Count - 1; i++)
                {
                    int ii = i < indexSel ? i : i + 1;
                    var prc = Process.Start("TortoiseMerge.exe", "\"" + fns[indexSel] + "\" \"" + fns[ii] + "\"");
                }
            }
            catch
            {
                MessageBox.Show("Ошибка. Проверьте установлен ли SVN.");
            }
        }

        private string SaveCompareCopySVNFileBySQLObject(SQLObject obj)
        {
            var origFN = FindSVNFileBySQLObject(obj);
            if (string.IsNullOrWhiteSpace(origFN)) return origFN;
            if (obj.TypeMark != "U ") return origFN;
            //для таблиц сортируем колонки
            var pathToSave = Path.Combine(PathResult, Comp.UniqueName);
            var newFN = Path.GetFileNameWithoutExtension(origFN);
            newFN = Path.Combine(pathToSave, newFN + " sortColumns.sql");

            var origSource = File.ReadAllText(origFN, Encoding.UTF8);

            var newSource = Comp.TextSVNTableToCompare(origSource);

            using (var fileS = File.Create(newFN))
            using (var file = new StreamWriter(fileS, Encoding.UTF8))
            {
                file.Write(newSource);
            }

            return newFN;
        }

        private string FindSVNFileBySQLObject(SQLObject obj)
        {
            if (!string.IsNullOrWhiteSpace(Settings.Param.SVNPath))
            {
                if (!Directory.Exists(Settings.Param.SVNPath))
                {
                    MessageBox.Show("Не найдена папка SVN: " + Settings.Param.SVNPath
                        + Environment.NewLine + "Отредактируйте поле SVNPath в файле настроек.");
                    return null;
                }
                var coName = obj.ShemaName + "." + obj.ObjectName;
                foreach (char c in Path.GetInvalidFileNameChars()) coName = coName.Replace(c, '_');

                var files = Directory.EnumerateFiles(Settings.Param.SVNPath
                    , coName + "*.sql"
                    , SearchOption.AllDirectories);
                int tempI;
                var file = files
                    //это не внутри папки svn
                    .Where(f => f.Substring(f.ToLower().StartsWith(Settings.Param.SVNPath.ToLower()) ? Settings.Param.SVNPath.Length : 0)
                            .IndexOf(@"svn\", StringComparison.CurrentCultureIgnoreCase) < 0)
                    //имя должно точно соответствовать или после него идут только цифры (особенность csv)
                    .Where(f => Path.GetFileNameWithoutExtension(f).ToLower() == coName.ToLower()
                            || int.TryParse(f.Substring(coName.Length), out tempI))
                    //берем не пустой файл и, на всякий случай, больше похожий на наш по имени (с самым коротким имененм)
                    .OrderByDescending(f => (new FileInfo(f).Length > 0 ? 1000 : 0) + 1000 - f.Length)
                    .FirstOrDefault();

                return file;
            }
            return null;
        }


        private void SetTbExecText(string text, SQLObject SQLObj, bool isAlter = false)
        {
            /*
            //т.к. не учитываются комментарии строк, то поиск и разбитие по GO происходит только в первых 5 строках
            var top5line = text
                .Split(new string[] { Environment.NewLine }, 7, StringSplitOptions.None)
                .ToList();
            top5line.RemoveAt(top5line.Count - 1);
            var pos5line = top5line.Sum(s => s.Length + 2);

            ///////
            // вставляем удаление объекта
            if (text.IndexOf("create function", 0, pos5line, StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                text = string.Format("if object_id('{0}') is not null drop function {0}" + TempGO, SQLObj.SQLName) + text;
            }
            if (text.IndexOf("create procedure", 0, pos5line, StringComparison.InvariantCultureIgnoreCase) >= 0)
            {
                text = string.Format("if object_id('{0}') is not null drop procedure {0}" + TempGO, SQLObj.SQLName) + text;
            }
            */

            var typeObj = SQLObj.TypeMark == "P " ? "procedure"
                    : SQLObj.TypeMark == "TF" ? "function"
                    : SQLObj.TypeMark == "FN" ? "function"
                    : SQLObj.TypeMark == "IF" ? "function"
                    : SQLObj.TypeMark == "V " ? "view"
                    : null;
            if (typeObj != null)
            {
                if (isAlter)
                {
                    var iCreate = text.IndexOf("CREATE", StringComparison.InvariantCultureIgnoreCase);
                    if (iCreate >= 0 && iCreate < 200)
                    {
                        text = text.Substring(0, iCreate) + "ALTER" + text.Substring(iCreate + 6);
                    }
                }
                else
                {
                    text = string.Format("if object_id('{0}') is not null drop {1} {0}" + TempGO
                        , SQLObj.SQLName
                        , typeObj)
                        + text;
                }
            }
                
            tbExec.Text = text;
        }

        private void dataGridView_DoubleClick(object sender, EventArgs e)
        {
            ActionDoubleClick(radioButton1.Checked);
        }

        private void ActionDoubleClick(bool openFile, bool openFolder = false, bool copyBuffer = false, bool isAlter = false)
        {
            var cell = dataGridView.CurrentCell;
            if (cell == null) return;
            string SQLName = cell.OwningRow.Tag as string;
            if (SQLName == null) return;
            CompareObject cellCO = cell.Tag as CompareObject;

            if (cellCO == null)
            {
                var obj = Comp.CompareResult[SQLName][0].FirstObject;
                var fn = FindSVNFileBySQLObject(obj);
                if (string.IsNullOrWhiteSpace(fn)) return;

                if (copyBuffer)
                {
                    Clipboard.SetDataObject(File.ReadAllText(fn));
                }
                else
                if (openFolder)
                {
                    Process.Start(new ProcessStartInfo("explorer.exe", " /select, \"" + fn + "\""));
                }
                else
                    if (openFile)
                    {
                        Process.Start(fn);
                    }
                    else
                    {
                        if (ExecIsBusy)
                        {
                            labelProgress.Text = "Подождите окончания выполнения запроса.";
                        }
                        else
                        {
                            SetTbExecText(File.ReadAllText(fn), obj, isAlter);
                            labelProgress.Text = "Текст во вкладке Применить: " + obj.SQLName + "  (с репозитория)";
                        }
                    }
                /*
                //выделение для строки
                dataGridView.ClearSelection();
                foreach (DataGridViewCell c in cell.OwningRow.Cells) c.Selected = true;
                */
            }
            else
            { 
                //для ячейки
                var obj = cellCO.Bases.Values.First();


                if (copyBuffer)
                {
                    Clipboard.SetDataObject(obj.Source);
                }
                else
                if (openFolder)
                {
                    var fn = Comp.SaveSQLObject(obj, cellCO.BasesString);

                    Process.Start(new ProcessStartInfo("explorer.exe", " /select, \"" + fn + "\""));
                }
                else
                    if (openFile)
                    {
                        var fn = Comp.SaveSQLObject(obj, cellCO.BasesString);

                        Process.Start(fn);
                    }
                    else
                    {
                        if (ExecIsBusy)
                        {
                            labelProgress.Text = "Подождите окончания выполнения запроса.";
                        }
                        else
                        {
                            SetTbExecText(obj.Source, obj, isAlter);
                            labelProgress.Text = "Текст во вкладке Применить: " + obj.SQLName + "  (вариант с" + cellCO.BasesString + ")";
                        }
                    }
            }
        }

        private void buttonExec_Click(object sender, EventArgs e)
        {
            if (ExecIsBusy) return;
            var bases = gridBasesExec.GetBases();
            if (bases.Count == 0) return;
            var scriptRun = tbExec.Text;
            if (string.IsNullOrEmpty(scriptRun)) return;
            ExecIsBusy = true;
            tbExec.ReadOnly = true;
            buttonExec.Enabled = false;
            buttonExec2.Enabled = false;
            butTextClear.Enabled = false;
            gridBasesExec.Enabled = false;
            labelExecStat.Text = "Выполнение...";
            tabPage2.Text = "Применение (0/" + bases.Count + ")";
            tabPage2.ImageIndex = -1;
            var statusErrors = 0;
            var statusApps = 0;

            //////////////////////////////////////////////////////////////
            /// Всякие манипуляции со скриптом!

            //GO - отключено, т.к. в кавычках GO не должно разбивать скрипт
            /*
            var scrRuns = (Environment.NewLine + scriptRun.Trim() + Environment.NewLine)
                .Split(new string[] { Environment.NewLine + "GO" + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            */
            //var scrRuns = new List<string>() { scriptRun };

            //т.к. не учитываются комментарии строк, то поиск и разбитие по GO происходит только в первых topCount строках
            int topCount = 10;
            var topline = scriptRun
                .Split(new string[] { Environment.NewLine }, topCount + 1, StringSplitOptions.None)
                .ToList();
            topline.RemoveAt(topline.Count - 1);
            var posline = topline.Sum(s => s.Length + 2);

            List<string> scrRuns = new List<string>();
            int sr = 0;
            int srs = 0; //позиция до которой все в scrRuns

            while (true)
            {
                var srl = sr; //позиция прошлого найденного GO
                sr = scriptRun.IndexOf(TempGO, srl, StringComparison.InvariantCultureIgnoreCase);

                if (sr > posline) sr = -1; //на всякий случай отлавливаем только в первых 1000 символах

                if (sr < 0) break;
                int count = new Regex("'").Matches(scriptRun.Substring(srs, sr - srs)).Count;
                sr += TempGO.Length;
                if (count % 2 != 0)
                {
                    continue;
                }
                scrRuns.Add(scriptRun.Substring(srs, sr - srs - TempGO.Length));
                srs = sr;
            }
            scrRuns.Add(scriptRun.Substring(srs, scriptRun.Length - srs));

            /////////////
            // убираем последний GO, после которого ничего нет

            var lsr = scrRuns[scrRuns.Count - 1];
            sr = lsr.LastIndexOf(Environment.NewLine + "GO", StringComparison.InvariantCultureIgnoreCase);
            if (sr > 0 && lsr.Substring(sr + 4).Trim().Length == 0)
            {
                scrRuns[scrRuns.Count - 1] = lsr.Substring(0, sr);
            }

            //////////////////////////////////////////////////////////////

            string textBoxExecResText = "";
            foreach (var scr in scrRuns)
            {
                textBoxExecResText += "Выполнение: "
                    + scr.Substring(0, scr.Length > 200 ? 200 : scr.Length).Replace(Environment.NewLine, " ")
                    + Environment.NewLine;
            }

            textBoxExecRes.Text = textBoxExecResText;

            Action<string> actGetMessage = (msg) =>
                {
                    this.Invoke((Action)(() =>
                        {
                            textBoxExecRes.Text += msg + Environment.NewLine;
                        }));
                };

            var basesReady = new List<string>();
            var th = new Thread(() =>
                {
                    foreach (var db in bases)
                    {

                        this.Invoke((Action)(() =>
                        {
                            textBoxExecRes.Text += Environment.NewLine
                                + "-----------------------------------------" + Environment.NewLine
                                + "Запрос к базе: " + db.Key + Environment.NewLine;
                        }));

                        Action<Exception> error = (ext) =>
                            {
                                textBoxExecRes.Text += Environment.NewLine
                                    + "Возникла ошибка: " + ext.Message;// +Environment.NewLine;
                            };
                        bool existError = false;
                        try
                        {
                            DALSql.SetConnectionString(db.Value);
                            DALSql.AddEventText(actGetMessage);

                            //открываем транзакцию 
                            DALSql.ExecuteDataSet(
                                "SET TRANSACTION ISOLATION LEVEL SERIALIZABLE" + Environment.NewLine
                                + "BEGIN TRANSACTION"
                                , null);
                        }
                        catch (Exception ext)
                        {
                            this.Invoke(error, ext);
                            existError = true;
                        }
                        foreach (var scrRun in scrRuns)
                        {
                            if (existError) break;
                            DataSet sqlResult = null;
                            try
                            {
                                sqlResult = DALSql.ExecuteDataSet(scrRun, null);
                            }
                            catch (Exception ext)
                            {
                                this.Invoke(error, ext);
                                existError = true;
                            }

                            if (sqlResult != null && sqlResult.Tables.Count > 0)
                            {
                                this.Invoke((Action)(() =>
                                {
                                    textBoxExecRes.Text += Environment.NewLine
                                        + "Запрос вернул результат: " + Environment.NewLine;

                                    foreach (DataTable tab in sqlResult.Tables)
                                    {
                                        foreach (DataColumn c in tab.Columns)
                                        {
                                            textBoxExecRes.Text += c.ColumnName + ";";
                                        }
                                        textBoxExecRes.Text += Environment.NewLine;
                                        foreach (DataRow r in tab.Rows)
                                        {
                                            foreach (var c in r.ItemArray)
                                                textBoxExecRes.Text += c.ToString() + ";";
                                            textBoxExecRes.Text += Environment.NewLine;
                                        }
                                        textBoxExecRes.Text += Environment.NewLine;
                                    }
                                }));
                            }
                            else
                            {
                                this.Invoke((Action)(() =>
                                {
                                    textBoxExecRes.Text += Environment.NewLine
                                        + "Запрос завершился" + Environment.NewLine;
                                }));
                            }
                        }
                        if (existError)
                        {
                            statusErrors++;
                            
                            //отменяем транзакцию 
                            try
                            {
                                DALSql.ExecuteDataSet("ROLLBACK TRANSACTION", null);
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            //применяем транзакцию 

                            try
                            {
                                DALSql.ExecuteDataSet("COMMIT TRANSACTION", null);
                            }
                            catch (Exception ext)
                            {
                                this.Invoke(error, ext);
                                existError = true;
                                statusErrors++;
                            }
                        }

                        this.Invoke((Action)(() =>
                        {
                            basesReady.Add(db.Key);
                            var basesString = basesReady.OrderBy(i => i).Aggregate("", (s, i) => (s == "" ? "" : s + ", ") + i);
                            labelExecStat.Text = "Завершено на базах: " + basesString + ". (" + basesReady.Count + " из " + bases.Count + ")";

                            tabPage2.Text = "Применение (" + basesReady.Count + "/" + bases.Count + ")"
                                + (statusErrors > 0 ? " ошибки " + statusErrors + "!" : "");
                            if (statusErrors > 0) tabPage2.ImageIndex = 0;
                            else tabPage2.ImageIndex = -1;
                        }));
                        try
                        {
                            DALSql.CloseConnection();
                        }
                        catch (Exception ext)
                        {
                            if (!existError)
                                this.Invoke(error, ext);
                        }
                    }
                    ExecIsBusy = false;
                    this.Invoke((Action)(() =>
                    {
                        tbExec.ReadOnly = false;
                        buttonExec.Enabled = true;
                        buttonExec2.Enabled = true;
                        butTextClear.Enabled = true;
                        gridBasesExec.Enabled = true;
                        labelExecStat.Text = "Завершено!";
                        tabPage2.Text = "Применение"
                            + (statusErrors > 0 ? " ошибки " + statusErrors + "!" : "");
                    }));
                });
            th.IsBackground = true;
            th.Start();
        }

        private void butSelExecBases_Click(object sender, EventArgs e)
        {
            if (ExecIsBusy)
            {
                labelProgress.Text = "Подождите окончания выполнения запроса.";
                return;
            }
            var bases = new List<string>();
            labelProgress.Text = "";
            foreach (DataGridViewCell cell in dataGridView.SelectedCells)
            {
                CompareObject cellCO = cell.Tag as CompareObject;
                if (cellCO == null) continue;
                bases.AddRange(cellCO.Bases.Keys.Where(b => !bases.Any(bs => bs == b)));
            }
            
            if (bases.Count == 0) return;
            var basesString = bases.OrderBy(i => i).Aggregate("", (s, i) => (s == "" ? "" : s + ", ") + i);

            labelProgress.Text = "На вкладке Применить установлена цель: " + basesString;
            //labelExecStat.Text = "Отмечены базы: " + basesString;
            gridBasesExec.FillBases(bases);
        }

        private void checkBoxFilter_CheckedChanged(object sender, EventArgs e)
        {
            butFilter.Font = new Font(butFilter.Font, FontStyle.Bold);
            Settings.Param.ShowSortType = checkBoxSortType.Checked;
            Settings.Param.ShowTableFirst = checkBoxShowTableTop.Checked;
            checkBoxShowTableTop.Enabled = Settings.Param.ShowSortType;
        }

        private void butFilter_Click(object sender, EventArgs e)
        {
            butFilter.Font = new Font(butFilter.Font, FontStyle.Regular);
            Comp.SetCheckFound(checkBoxFilterText.Checked ? textBoxFilterText.Text : null);
            dataGridView_FillCompare(false);
        }

        private void textBoxFilterText_Enter(object sender, EventArgs e)
        {
            checkBoxFilterText.Checked = true;
        }

        private void toolStripMenuToApp_Click(object sender, EventArgs e)
        {
            ActionDoubleClick(false);
        }

        private void toolStripMenuMSSQL_Click(object sender, EventArgs e)
        {
            ActionDoubleClick(true);
        }

        private void перейтиКФайлуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActionDoubleClick(false, true);
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

            ActionDoubleClick(false, false, true);
        }

        private void toolStripMenuToAppAlter_Click(object sender, EventArgs e)
        {
            ActionDoubleClick(false, false, false, true);
        }

        private void butTextClear_Click(object sender, EventArgs e)
        {
            tbExec.Text = string.Empty;
            tbExec.Focus();
        }

        private void butTextCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(tbExec.Text);
        }

        private void utilsView1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItemSVNLog_Click(object sender, EventArgs e)
        {
            var cell = dataGridView.CurrentCell;
            if (cell == null) return;
            string SQLName = cell.OwningRow.Tag as string;
            if (SQLName == null) return;

            var obj = Comp.CompareResult[SQLName][0].FirstObject;
            var fn = FindSVNFileBySQLObject(obj);
            if (string.IsNullOrWhiteSpace(fn)) return;

            if (string.IsNullOrWhiteSpace(Settings.Param.SVNCommandLog))
            {
                MessageBox.Show("Ошибка. Проверьте задано ли значение тэка SVNCommandLog в файле настрок. Например: " + Environment.NewLine
                    + "<SVNCommandLog>TortoiseProc.exe /command:log /path:\"{0}\"</SVNCommandLog>");
                return;
            }

            try
            {
                var proc = string.Format(Settings.Param.SVNCommandLog, fn).Split(new string[] { " " }, 2, StringSplitOptions.None);
                Process.Start(proc[0], proc[1]);
            }
            catch
            {
                MessageBox.Show("Ошибка. Проверьте установлен ли SVN, и вход произведен. ");
            }
        }
        
    }
}
