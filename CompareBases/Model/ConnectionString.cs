using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompareBases
{
    public class ConnectionString
    {
        public string ConnString = string.Empty;
        public string Server = string.Empty;
        public string Database = string.Empty;
        public string Username;
        public string Password;

        public ConnectionString(string server, string database, string login = null, string pass = null)
        {
            var res = string.Format(
                "Data Source={0};Initial Catalog={1};Asynchronous Processing=True;Persist security info=true;"
                , server
                , database
                );
            if (login != null) res += string.Format("User Id={0};", login);
            if (pass != null) res += string.Format("Password={0};", pass);

            ConnString = res;
            Server = server;
            Database = database;
            Username = login;
            Password = pass;
        }

        public ConnectionString(string connString)
        {
            if (string.IsNullOrWhiteSpace(connString)) return;
            ConnString = connString.Trim();
            if (ConnString[ConnString.Length - 1] != ';') ConnString = ConnString + ";";

            Func<string, string> getSub = (name) =>
                {
                    int i = ConnString.IndexOf(name, StringComparison.CurrentCultureIgnoreCase);
                    if (i < 0) return null;
                    int b = ConnString.IndexOf("=", i) + 1;
                    int l = ConnString.IndexOf(";", b) - b;
                    return ConnString.Substring(b, l);
                };

            Server = getSub("Data Source=");
            Database = getSub("Initial Catalog=");
            Username = getSub("User Id=");
            Password = getSub("Password=");
        }

    }
}
