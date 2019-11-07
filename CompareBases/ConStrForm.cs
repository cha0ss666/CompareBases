using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompareBases
{
    public partial class ConStrForm : Form
    {
        private bool Changing = false;

        public ConStrForm()
        {
            InitializeComponent();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            if (Changing) return;
            Changing = true;
            var cs = new ConnectionString(textBox6.Text);
            textBox2.Text = cs.Server;
            textBox3.Text = cs.Database;
            textBox4.Text = cs.Username;
            textBox5.Text = cs.Password;
            textBox7.Text = new ConnectionString(textBox2.Text, textBox3.Text).ConnString;
            Changing = false;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (Changing) return;
            Changing = true;
            var cs = new ConnectionString(textBox7.Text);
            Changing = false;
            //и запускаем общее обновление: как будто пользователь изменил сервер и базу
            textBox2.Text = cs.Server;
            textBox3.Text = cs.Database;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (sender == textBox2 || sender == textBox3) textBox1.Text = "Test  " + textBox2.Text.Trim() + "." + textBox3.Text.Trim() + "`";

            if (Changing) return;
            Changing = true;
            var cs = string.IsNullOrEmpty(textBox4.Text)
                ? new ConnectionString(textBox2.Text, textBox3.Text)
                : new ConnectionString(textBox2.Text, textBox3.Text, textBox4.Text, textBox5.Text);
            textBox6.Text = cs.ConnString;
            textBox7.Text = new ConnectionString(textBox2.Text, textBox3.Text).ConnString;
            Changing = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.IndexOf(".") <= 0
                || textBox1.Text.Trim().Length < 3) 
            {
                MessageBox.Show("Введите название: <произвольное>.<databaseName>[`]");
                return;
            }
            Settings.Param.ConnectionStrings.Add(textBox1.Text.Trim(), textBox6.Text.Trim().Replace("\r", "").Replace("\n", ""));
            GridBases.ChangeListBases();
            MessageBox.Show("База добавлена, сохраните настройки.");
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }


    }
}
