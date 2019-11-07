using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Xml;

namespace CompareBases
{
    public partial class UtilsView : UserControl
    {
        private static string RoamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string FilePathVer1 = Path.Combine(RoamingPath, @"Red Gate\SQL Multi Script 1");
        private static string FilePathVer2 = Path.Combine(RoamingPath, @"Red Gate\SQL Multi Script");
        private static string FileNameDat = "Application.dat";
        private static string FileToName;

        private static Dictionary<string, string> HashMSPass = new Dictionary<string, string>();

        public UtilsView()
        {
            InitializeComponent();
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
            Settings.Param.OnChange += param_Change;
#if DEBUG
            butTest.Visible = true;
#endif 
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
            param_Change_run = false;
        }

        private void сompare_CheckedChanged(object sender, EventArgs e)
        {
            if (param_Change_run) return;
            param_Change_run = true;
            //Применение настроек и синхронизация
            Settings.Param.CompareWithTable = checkBoxCompareWithTable.Checked;
            Settings.Param.CompareTableWithTrigger = checkBoxCompareTableWithTrigger.Checked;
            Settings.Param.Prefix = textBoxPrefix.Text.Trim();
            Settings.Param.IgnoreByPrefix = new List<string>(textBoxIPref.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
            Settings.Param.IgnoreByPostfix = new List<string>(textBoxIPost.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));

            Settings.Param.Commite();
            param_Change_run = false;
        }

        private void butDBsFromMS_Click(object sender, EventArgs e)
        {
            FileToName = Path.Combine(FilePathVer1, FileNameDat);
            if (!File.Exists(FileToName))
            {
                FileToName = Path.Combine(FilePathVer2, FileNameDat);
            }

            if (!File.Exists(FileToName))
            {
                MessageBox.Show("Нет файла источника " + FileToName);
                return;
            }

            Settings.Param.ConnectionStrings = LoadMSFile(FileToName);
            Settings.SaveProgramParameters();

            GridBases.ChangeListBases();
            MessageBox.Show("Успешно завершено.");
        }

        public SDic<string, string> LoadMSFile(string msFileName)
        {
            var newConnectionStrings = new SDic<string, string>();

            if (Settings.Param.ConnectionStrings != null)
            {
                foreach (var item in Settings.Param.ConnectionStrings)
                {
                    if (item.Key.EndsWith("`")) newConnectionStrings.Add(item.Key, item.Value);
                }
            }

            string datSource = File.ReadAllText(msFileName);
            var datXml = new XmlDocument();
            datXml.LoadXml(datSource);

            var elements = datXml.GetElementsByTagName("databaseLists");
            foreach (XmlNode value in elements)
            {
                //здесь только элементы value содержащие списки баз
                string nameList = null;
                var bases = new Dictionary<string, string>();
                foreach (XmlNode valCilds in value.ChildNodes)
                {
                    nameList = valCilds["name"].InnerText;
                    var databases = valCilds["databases"];
                    foreach (XmlNode valDatabases in databases.ChildNodes)
                    {
                        string baseName = valDatabases["name"].InnerText;
                        string server = valDatabases["server"].InnerText;
                        string username = valDatabases["username"].InnerText;
                        string passHash = valDatabases["password"].InnerText;

                        string name = GetBaseName(nameList, baseName, server);
                        if (name == null) continue;
                        string connectionString = GetConnectionString(server, baseName, username, passHash);
                        if (connectionString == null) continue; 
                        newConnectionStrings.Add(name, connectionString);
                    }
                }
            }

            return newConnectionStrings;
        }

        public string GetBaseName(string nameList, string baseName, string server)
        {
            if (nameList.StartsWith("Default")
                || nameList.Contains("все-все")) return null;

            nameList = nameList
                .Trim()
                .Replace("область", "обл")
                .Replace(".", "");

            if (checkBoxDBsFromMSSokr.Checked)
            {
                nameList = nameList
                    .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                    //сокращаем все длинные слова до 3х букв
                    .Select(w => w.Trim().Length > 6 ? w.Trim().Substring(0, 3) : w.Trim())
                    //первая буква большая
                    .Select(w => char.ToUpper(w[0]).ToString() + (w.Length > 1 ? w.Substring(1).ToLower() : ""))
                    //объеденям
                    .Aggregate("", (s, w) => s + w);
            }
            
            return nameList + "  " + (checkBoxDBsFromMSWithBase.Checked ? server + "." : "") + baseName;
        }

        public string GetConnectionString(string dataSource, string initialCatalog, string login, string hashMS)
        {
            string pass = "";
            if (HashMSPass.ContainsKey(hashMS)) pass = HashMSPass[hashMS];
            if (pass == "")
            {
                var ok = InputBox.Query("Сохранение списка баз"
                    , "Пароль для " + login + " на сервере " + dataSource + " база " + initialCatalog
                    , ref pass);
                if (!ok) return null;
                HashMSPass.Add(hashMS, pass);
            }
            return new ConnectionString(dataSource, initialCatalog, login, pass).ConnString;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Settings.SaveProgramParameters();
            MessageBox.Show("Сохранено.");
        }

        private void butAddConStr_Click(object sender, EventArgs e)
        {
            new ConStrForm().Show();
        }

        private void butTest_Click(object sender, EventArgs e)
        {
            var template = @"
		<base>
			<name>{0}</name>
			<con>{1}</con>
			<login>{2}</login>
			<pass>{3}</pass>
			<use>USE {4}</use>
		</base>";

            var exists = new Dictionary<string, string>();

            var res = new StringBuilder();
            foreach(var bas in Settings.Param.ConnectionStrings)
            {
                if (bas.Key.StartsWith("АВся")) continue;
                int spI = bas.Key.IndexOf(" ");
                if (spI <= 0) continue;
                var basename = bas.Key.Substring(0, spI);

                var connect = new ConnectionString(bas.Value);

                basename += " " + connect.Server;

                if (exists.ContainsKey(basename)) continue;
                exists.Add(basename, null);

                var line = string.Format(template
                    , basename
                    , connect.Server
                    , connect.Username
                    , connect.Password
                    , connect.Database);

                res.Append(line);
            }

            Clipboard.SetData(DataFormats.Text, res.ToString());
            MessageBox.Show("Сформированно в буфер");
        }
        


    }
}
