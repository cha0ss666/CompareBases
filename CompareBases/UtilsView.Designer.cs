﻿namespace CompareBases
{
    partial class UtilsView
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.butDBsFromMS = new System.Windows.Forms.Button();
            this.checkBoxDBsFromMSSokr = new System.Windows.Forms.CheckBox();
            this.checkBoxDBsFromMSWithBase = new System.Windows.Forms.CheckBox();
            this.groupBoxComp1 = new System.Windows.Forms.GroupBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.checkBoxCompareTableWithTrigger = new System.Windows.Forms.CheckBox();
            this.checkBoxCompareWithTable = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxIPost = new System.Windows.Forms.TextBox();
            this.textBoxIPref = new System.Windows.Forms.TextBox();
            this.textBoxPrefix = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.butAddConStr = new System.Windows.Forms.Button();
            this.butTest = new System.Windows.Forms.Button();
            this.cbPostGreSql = new System.Windows.Forms.CheckBox();
            this.groupBoxComp1.SuspendLayout();
            this.SuspendLayout();
            // 
            // butDBsFromMS
            // 
            this.butDBsFromMS.Location = new System.Drawing.Point(19, 17);
            this.butDBsFromMS.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.butDBsFromMS.Name = "butDBsFromMS";
            this.butDBsFromMS.Size = new System.Drawing.Size(301, 28);
            this.butDBsFromMS.TabIndex = 0;
            this.butDBsFromMS.Text = "Получить базы из SQL Multy Script";
            this.butDBsFromMS.UseVisualStyleBackColor = true;
            this.butDBsFromMS.Click += new System.EventHandler(this.butDBsFromMS_Click);
            // 
            // checkBoxDBsFromMSSokr
            // 
            this.checkBoxDBsFromMSSokr.AutoSize = true;
            this.checkBoxDBsFromMSSokr.Checked = true;
            this.checkBoxDBsFromMSSokr.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDBsFromMSSokr.Location = new System.Drawing.Point(328, 22);
            this.checkBoxDBsFromMSSokr.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxDBsFromMSSokr.Name = "checkBoxDBsFromMSSokr";
            this.checkBoxDBsFromMSSokr.Size = new System.Drawing.Size(145, 21);
            this.checkBoxDBsFromMSSokr.TabIndex = 1;
            this.checkBoxDBsFromMSSokr.Text = "Сокращать слова";
            this.checkBoxDBsFromMSSokr.UseVisualStyleBackColor = true;
            // 
            // checkBoxDBsFromMSWithBase
            // 
            this.checkBoxDBsFromMSWithBase.AutoSize = true;
            this.checkBoxDBsFromMSWithBase.Checked = true;
            this.checkBoxDBsFromMSWithBase.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDBsFromMSWithBase.Location = new System.Drawing.Point(491, 22);
            this.checkBoxDBsFromMSWithBase.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxDBsFromMSWithBase.Name = "checkBoxDBsFromMSWithBase";
            this.checkBoxDBsFromMSWithBase.Size = new System.Drawing.Size(134, 21);
            this.checkBoxDBsFromMSWithBase.TabIndex = 2;
            this.checkBoxDBsFromMSWithBase.Text = "Указывать базу";
            this.checkBoxDBsFromMSWithBase.UseVisualStyleBackColor = true;
            // 
            // groupBoxComp1
            // 
            this.groupBoxComp1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxComp1.Controls.Add(this.cbPostGreSql);
            this.groupBoxComp1.Controls.Add(this.buttonSave);
            this.groupBoxComp1.Controls.Add(this.checkBoxCompareTableWithTrigger);
            this.groupBoxComp1.Controls.Add(this.checkBoxCompareWithTable);
            this.groupBoxComp1.Controls.Add(this.label3);
            this.groupBoxComp1.Controls.Add(this.label2);
            this.groupBoxComp1.Controls.Add(this.textBoxIPost);
            this.groupBoxComp1.Controls.Add(this.textBoxIPref);
            this.groupBoxComp1.Controls.Add(this.textBoxPrefix);
            this.groupBoxComp1.Controls.Add(this.label1);
            this.groupBoxComp1.Location = new System.Drawing.Point(4, 53);
            this.groupBoxComp1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxComp1.Name = "groupBoxComp1";
            this.groupBoxComp1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBoxComp1.Size = new System.Drawing.Size(1325, 423);
            this.groupBoxComp1.TabIndex = 18;
            this.groupBoxComp1.TabStop = false;
            this.groupBoxComp1.Text = "Настройки сравнения";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(1037, 382);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(201, 34);
            this.buttonSave.TabIndex = 19;
            this.buttonSave.Text = "Сохранить по умолчанию";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // checkBoxCompareTableWithTrigger
            // 
            this.checkBoxCompareTableWithTrigger.AutoSize = true;
            this.checkBoxCompareTableWithTrigger.Location = new System.Drawing.Point(279, 23);
            this.checkBoxCompareTableWithTrigger.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxCompareTableWithTrigger.Name = "checkBoxCompareTableWithTrigger";
            this.checkBoxCompareTableWithTrigger.Size = new System.Drawing.Size(168, 21);
            this.checkBoxCompareTableWithTrigger.TabIndex = 3;
            this.checkBoxCompareTableWithTrigger.Text = "Сравнивать триггера";
            this.checkBoxCompareTableWithTrigger.UseVisualStyleBackColor = true;
            this.checkBoxCompareTableWithTrigger.CheckedChanged += new System.EventHandler(this.сompare_CheckedChanged);
            // 
            // checkBoxCompareWithTable
            // 
            this.checkBoxCompareWithTable.AutoSize = true;
            this.checkBoxCompareWithTable.Checked = true;
            this.checkBoxCompareWithTable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCompareWithTable.Location = new System.Drawing.Point(8, 23);
            this.checkBoxCompareWithTable.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.checkBoxCompareWithTable.Name = "checkBoxCompareWithTable";
            this.checkBoxCompareWithTable.Size = new System.Drawing.Size(253, 21);
            this.checkBoxCompareWithTable.TabIndex = 3;
            this.checkBoxCompareWithTable.Text = "Также сравнивать таблицы (beta)";
            this.checkBoxCompareWithTable.UseVisualStyleBackColor = true;
            this.checkBoxCompareWithTable.CheckedChanged += new System.EventHandler(this.сompare_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(519, 113);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(334, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Список игнорируемых окончаний, в т.ч. триггера:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 113);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(335, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Список игнорируемых префиксов, в т.ч. триггера:";
            // 
            // textBoxIPost
            // 
            this.textBoxIPost.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.textBoxIPost.Location = new System.Drawing.Point(523, 133);
            this.textBoxIPost.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxIPost.Multiline = true;
            this.textBoxIPost.Name = "textBoxIPost";
            this.textBoxIPost.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxIPost.Size = new System.Drawing.Size(505, 282);
            this.textBoxIPost.TabIndex = 1;
            this.textBoxIPost.WordWrap = false;
            this.textBoxIPost.TextChanged += new System.EventHandler(this.сompare_CheckedChanged);
            // 
            // textBoxIPref
            // 
            this.textBoxIPref.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.textBoxIPref.Location = new System.Drawing.Point(8, 133);
            this.textBoxIPref.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxIPref.Multiline = true;
            this.textBoxIPref.Name = "textBoxIPref";
            this.textBoxIPref.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxIPref.Size = new System.Drawing.Size(505, 282);
            this.textBoxIPref.TabIndex = 1;
            this.textBoxIPref.WordWrap = false;
            this.textBoxIPref.TextChanged += new System.EventHandler(this.сompare_CheckedChanged);
            // 
            // textBoxPrefix
            // 
            this.textBoxPrefix.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.textBoxPrefix.Location = new System.Drawing.Point(11, 79);
            this.textBoxPrefix.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.textBoxPrefix.Name = "textBoxPrefix";
            this.textBoxPrefix.Size = new System.Drawing.Size(1017, 26);
            this.textBoxPrefix.TabIndex = 1;
            this.textBoxPrefix.TextChanged += new System.EventHandler(this.сompare_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 59);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(664, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Объект начинается с:  (здесь и ниже скобки [ ] обязательно должны быть, можно исп" +
    "ользовать %)";
            // 
            // butAddConStr
            // 
            this.butAddConStr.Location = new System.Drawing.Point(732, 17);
            this.butAddConStr.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.butAddConStr.Name = "butAddConStr";
            this.butAddConStr.Size = new System.Drawing.Size(301, 28);
            this.butAddConStr.TabIndex = 0;
            this.butAddConStr.Text = "Добавить базу вручную";
            this.butAddConStr.UseVisualStyleBackColor = true;
            this.butAddConStr.Click += new System.EventHandler(this.butAddConStr_Click);
            // 
            // butTest
            // 
            this.butTest.Location = new System.Drawing.Point(644, 17);
            this.butTest.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.butTest.Name = "butTest";
            this.butTest.Size = new System.Drawing.Size(80, 28);
            this.butTest.TabIndex = 0;
            this.butTest.Text = "Тест";
            this.butTest.UseVisualStyleBackColor = true;
            this.butTest.Visible = false;
            this.butTest.Click += new System.EventHandler(this.butTest_Click);
            // 
            // cbPostGreSql
            // 
            this.cbPostGreSql.AutoSize = true;
            this.cbPostGreSql.Location = new System.Drawing.Point(455, 23);
            this.cbPostGreSql.Margin = new System.Windows.Forms.Padding(4);
            this.cbPostGreSql.Name = "cbPostGreSql";
            this.cbPostGreSql.Size = new System.Drawing.Size(102, 21);
            this.cbPostGreSql.TabIndex = 20;
            this.cbPostGreSql.Text = "PostGreSql";
            this.cbPostGreSql.UseVisualStyleBackColor = true;
            // 
            // UtilsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxComp1);
            this.Controls.Add(this.checkBoxDBsFromMSWithBase);
            this.Controls.Add(this.checkBoxDBsFromMSSokr);
            this.Controls.Add(this.butTest);
            this.Controls.Add(this.butAddConStr);
            this.Controls.Add(this.butDBsFromMS);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "UtilsView";
            this.Size = new System.Drawing.Size(1333, 677);
            this.groupBoxComp1.ResumeLayout(false);
            this.groupBoxComp1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butDBsFromMS;
        private System.Windows.Forms.CheckBox checkBoxDBsFromMSSokr;
        private System.Windows.Forms.CheckBox checkBoxDBsFromMSWithBase;
        private System.Windows.Forms.GroupBox groupBoxComp1;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.TextBox textBoxPrefix;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxIPost;
        private System.Windows.Forms.TextBox textBoxIPref;
        private System.Windows.Forms.CheckBox checkBoxCompareWithTable;
        private System.Windows.Forms.CheckBox checkBoxCompareTableWithTrigger;
        private System.Windows.Forms.Button butAddConStr;
        private System.Windows.Forms.Button butTest;
        private System.Windows.Forms.CheckBox cbPostGreSql;
    }
}
