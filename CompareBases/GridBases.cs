using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.IO;
using System.Diagnostics;

namespace CompareBases
{
    public partial class GridBases : UserControl
    {
        public string PathTemp = "Temp";
        private static string RoamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
        private static string SQLCompareEXEFileName = null;
        public static event Action OnChangeListBases;

        public static void ChangeListBases()
        {
            OnChangeListBases();
        }

        private static string GetSQLCompareEXEFileName()
        { 
            if (SQLCompareEXEFileName == null)
            {
                SQLCompareEXEFileName = Path.Combine(RoamingPath, @"Red Gate\SQL Compare 14\SQLCompare.exe");
                if (!File.Exists(SQLCompareEXEFileName))
                {
                    SQLCompareEXEFileName = Path.Combine(RoamingPath, @"Red Gate\SQL Compare 13\SQLCompare.exe");
                    if (!File.Exists(SQLCompareEXEFileName))
                    {
                        SQLCompareEXEFileName = Path.Combine(RoamingPath, @"Red Gate\SQL Compare 12\SQLCompare.exe");
                        if (!File.Exists(SQLCompareEXEFileName))
                        {
                            SQLCompareEXEFileName = Path.Combine(RoamingPath, @"Red Gate\SQL Compare 11\SQLCompare.exe");
                            if (!File.Exists(SQLCompareEXEFileName))
                            {
                                SQLCompareEXEFileName = Path.Combine(RoamingPath, @"Red Gate\SQL Compare 10\SQLCompare.exe");
                            }
                        }
                    }
                }
            }
            return SQLCompareEXEFileName;
        }

        public GridBases()
        {
            InitializeComponent();
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            FillBases();
            OnChangeListBases += () => { FillBases(); };
        }

        public void FillBases(List<string> selected = null)
        {
            if (selected == null) selected = new List<string>();

            //подготовка столбцов
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();

            dataGridView.Columns[dataGridView.Columns.Add(new DataGridViewCheckBoxColumn())].Name = "Select";
            dataGridView.Columns["Select"].HeaderText = "";
            dataGridView.Columns["Select"].Width = 20;
            dataGridView.Columns["Select"].ValueType = typeof(bool);
            //задаем высоту, чтобы влезла мини меню "Доп"
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView.ColumnHeadersHeight = 22; 

            dataGridView.Columns.Add("DB", "База");
            dataGridView.Columns["DB"].Width = 305;// 325;
            dataGridView.Columns["DB"].ReadOnly = true;

            if (Settings.Param.ConnectionStrings == null)
            {
                return;
            }
            //подготовка строк
            foreach (var dbName in Settings.Param.ConnectionStrings.Keys.OrderBy(r => r).ToList())
            {
                object[] row = new object[] 
                {   
                    selected.Any(s => dbName.EndsWith(s)), 
                    dbName ?? ""
                };
                var newRow = dataGridView.Rows[dataGridView.Rows.Add(row)];
            }
            UpdateCountSelect();
        }

        public Dictionary<string, string> GetBases()
        {
            var res = new Dictionary<string, string>();
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                var dbName = row.Cells["DB"].Value as string;
                if (dbName != null && (bool)row.Cells["Select"].Value)
                {
                    res.Add(Settings.Param.ConvertBDNameToMini(dbName), Settings.Param.ConnectionStrings[dbName]);
                }
            }
            return res;
        }

        private bool updateCountSelect_in = false;
        private void UpdateCountSelect()
        {
            if (updateCountSelect_in || dataGridView.Columns.Count < 2) return;
            updateCountSelect_in = true;
            //var bases = GetBases();

            var curCell = dataGridView.CurrentCell;
            var curRow = curCell.OwningRow;
            //var curName = curRow.Cells["DB"].Value as string;
            var curValue = (bool)curRow.Cells["Select"].Value;

            if (OldSelectedRows.Any(r => r == curRow))
                foreach (DataGridViewRow row in OldSelectedRows)
                {
                    row.Cells["Select"].Value = curValue;
                    row.Selected = true;
                }

            int cnt = 0;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                var dbName = row.Cells["DB"].Value as string;
                if (dbName != null && (bool)row.Cells["Select"].Value)
                    cnt++;
            }

            dataGridView.Columns[1].HeaderText = "База" + (cnt > 0 ? " (выбрано " + cnt.ToString() + ")" : "");
            panelMenu.Visible = cnt > 0;
            updateCountSelect_in = false;
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateCountSelect();
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private List<DataGridViewRow> SelectedRows = new List<DataGridViewRow>();
        private List<DataGridViewRow> OldSelectedRows = new List<DataGridViewRow>();

        private void dataGridView_SelectionChanged(object sender, EventArgs e)
        {
            OldSelectedRows = SelectedRows;
            SelectedRows = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dataGridView.SelectedRows)
            {
                SelectedRows.Add(row);
            };
        }

        private void dataGridView_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
        }

        private void sQLCompareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Взять строку подключения
            var bases = GetBases();
            if (bases.Count == 0) return;

            var txt = "";
            foreach (var k in bases)
            {
                var cs0 = new ConnectionString(k.Value);
                var cs1 = new ConnectionString(cs0.Server, cs0.Database);
                txt += cs1.ConnString + Environment.NewLine + Environment.NewLine;
            }

            Clipboard.SetText(txt.Trim());
        }

        private void создатьСнапшотыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Settings.Param.SVNPath))
            {
                MessageBox.Show("Не найдена папка для назначения: " + Settings.Param.SnapshotPath
                    + Environment.NewLine + "Отредактируйте поле SnapshotPath в файле настроек.");
                return;
            }

            var bases = GetBases();
            if (bases.Count == 0) return;
            var bats = bases.Count >= 5 ? new string[5] : new string[1];
            int batIndex = 0;
            foreach (var k in bases)
            {
                var timeFormatString = "yyyy'-'MM'-'dd'-'HHmm";
                var snap = Path.Combine(Settings.Param.SnapshotPath
                    , k.Key + "-" + DateTime.Now.ToString(timeFormatString, CultureInfo.InvariantCulture) + ".snp");
                var xml = CreateXMLFileSnapshot(k.Value, snap);

                bats[batIndex++ % bats.Length] += "\"" + GetSQLCompareEXEFileName() + "\" /argfile:\"" + xml + "\"" + Environment.NewLine;
            }

            foreach (var bat in bats)
            {
                var text = bat + "pause";
                var batfn = GetTempFileName("cmd");
                File.WriteAllText(batfn, text, Encoding.GetEncoding(866));

                Process.Start(batfn);
            }
        }

        public string CreateXMLFileSnapshot(string connectionString, string snapshotFileName)
        {
            var conn = new ConnectionString(connectionString);

            var text = string.Format(@"<?xml version=""1.0""?>
<commandline>
<server1>{0}</server1>
<database1>{1}</database1>
<username1>{2}</username1>
<password1>{3}</password1>
<MakeSnapshot>{4}</MakeSnapshot>
</commandline>"
                , conn.Server, conn.Database, conn.Username, conn.Password, snapshotFileName);

            var fn = GetTempFileName("xml");
            File.WriteAllText(fn, text);

            return new FileInfo(fn).FullName;
        }

        private static Random Rnd = new Random();
        public string GetTempFileName(string ext = ".txt")
        {
            var timeFormatString = "yyyy'-'MM'-'dd'_'HH'.'mm'.'ss'.'fffffff";
            var uniqueName = "Temp_" + DateTime.Now.ToString(timeFormatString, CultureInfo.InvariantCulture) + Rnd.Next(100, 999).ToString();
            if (!Directory.Exists(PathTemp)) Directory.CreateDirectory(PathTemp);
            return Path.Combine(PathTemp, uniqueName) + "." + ext.Replace(".", "");
        }







    }
}
