namespace CompareBases
{
    partial class Form1
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.butStart = new System.Windows.Forms.Button();
            this.labelProgress = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBoxComp2 = new System.Windows.Forms.GroupBox();
            this.checkBoxShowTableTop = new System.Windows.Forms.CheckBox();
            this.checkBoxSortType = new System.Windows.Forms.CheckBox();
            this.checkBoxFilter = new System.Windows.Forms.CheckBox();
            this.textBoxFilterText = new System.Windows.Forms.TextBox();
            this.checkBoxFilterText = new System.Windows.Forms.CheckBox();
            this.butFilter = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxSelComp3 = new System.Windows.Forms.ComboBox();
            this.labelSelComp3 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.butSelComp = new System.Windows.Forms.Button();
            this.butSelExecBases = new System.Windows.Forms.Button();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBoxComp1 = new System.Windows.Forms.GroupBox();
            this.textBoxIPost = new System.Windows.Forms.TextBox();
            this.textBoxIPref = new System.Windows.Forms.TextBox();
            this.textBoxPrefix = new System.Windows.Forms.TextBox();
            this.checkBoxCompareTableWithTrigger = new System.Windows.Forms.CheckBox();
            this.checkBoxCompareWithTable = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.compareBases = new CompareBases.GridBases();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainerExec = new System.Windows.Forms.SplitContainer();
            this.butTextCopy = new System.Windows.Forms.Button();
            this.butTextClear = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridBasesExec = new CompareBases.GridBases();
            this.tbExec = new System.Windows.Forms.TextBox();
            this.labelExecStat = new System.Windows.Forms.Label();
            this.buttonExec = new System.Windows.Forms.Button();
            this.textBoxExecRes = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.utilsView1 = new CompareBases.UtilsView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.dataGridViewMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuComp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuToApp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuBases = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuMSSQL = new System.Windows.Forms.ToolStripMenuItem();
            this.перейтиКФайлуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSVNLog = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonExec2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBoxComp2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxComp1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerExec)).BeginInit();
            this.splitContainerExec.Panel1.SuspendLayout();
            this.splitContainerExec.Panel2.SuspendLayout();
            this.splitContainerExec.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.dataGridViewMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.Size = new System.Drawing.Size(766, 417);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.CellStateChanged += new System.Windows.Forms.DataGridViewCellStateChangedEventHandler(this.dataGridView_CellStateChanged);
            this.dataGridView.DoubleClick += new System.EventHandler(this.dataGridView_DoubleClick);
            // 
            // butStart
            // 
            this.butStart.Location = new System.Drawing.Point(6, 6);
            this.butStart.Name = "butStart";
            this.butStart.Size = new System.Drawing.Size(139, 23);
            this.butStart.TabIndex = 1;
            this.butStart.Text = "Сравнить выбранные";
            this.butStart.UseVisualStyleBackColor = true;
            this.butStart.Click += new System.EventHandler(this.butStart_Click);
            // 
            // labelProgress
            // 
            this.labelProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelProgress.Location = new System.Drawing.Point(151, 3);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(683, 29);
            this.labelProgress.TabIndex = 2;
            this.labelProgress.Text = "Выберите базы";
            this.labelProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1140, 571);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBoxComp2);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBoxComp1);
            this.tabPage1.Controls.Add(this.splitContainer);
            this.tabPage1.Controls.Add(this.labelProgress);
            this.tabPage1.Controls.Add(this.butStart);
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1132, 544);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Сравнение";
            // 
            // groupBoxComp2
            // 
            this.groupBoxComp2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxComp2.Controls.Add(this.checkBoxShowTableTop);
            this.groupBoxComp2.Controls.Add(this.checkBoxSortType);
            this.groupBoxComp2.Controls.Add(this.checkBoxFilter);
            this.groupBoxComp2.Controls.Add(this.textBoxFilterText);
            this.groupBoxComp2.Controls.Add(this.checkBoxFilterText);
            this.groupBoxComp2.Controls.Add(this.butFilter);
            this.groupBoxComp2.Location = new System.Drawing.Point(6, 121);
            this.groupBoxComp2.Name = "groupBoxComp2";
            this.groupBoxComp2.Size = new System.Drawing.Size(814, 80);
            this.groupBoxComp2.TabIndex = 17;
            this.groupBoxComp2.TabStop = false;
            this.groupBoxComp2.Text = "Фильтры результата сравнения";
            this.groupBoxComp2.Visible = false;
            // 
            // checkBoxShowTableTop
            // 
            this.checkBoxShowTableTop.AutoSize = true;
            this.checkBoxShowTableTop.Checked = true;
            this.checkBoxShowTableTop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxShowTableTop.Location = new System.Drawing.Point(144, 46);
            this.checkBoxShowTableTop.Name = "checkBoxShowTableTop";
            this.checkBoxShowTableTop.Size = new System.Drawing.Size(108, 17);
            this.checkBoxShowTableTop.TabIndex = 10;
            this.checkBoxShowTableTop.Text = "Таблицы сверху";
            this.checkBoxShowTableTop.UseVisualStyleBackColor = true;
            this.checkBoxShowTableTop.CheckedChanged += new System.EventHandler(this.checkBoxFilter_CheckedChanged);
            // 
            // checkBoxSortType
            // 
            this.checkBoxSortType.AutoSize = true;
            this.checkBoxSortType.Checked = true;
            this.checkBoxSortType.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSortType.Location = new System.Drawing.Point(7, 46);
            this.checkBoxSortType.Name = "checkBoxSortType";
            this.checkBoxSortType.Size = new System.Drawing.Size(131, 17);
            this.checkBoxSortType.TabIndex = 10;
            this.checkBoxSortType.Text = "Сортировать по типу";
            this.checkBoxSortType.UseVisualStyleBackColor = true;
            this.checkBoxSortType.CheckedChanged += new System.EventHandler(this.checkBoxFilter_CheckedChanged);
            // 
            // checkBoxFilter
            // 
            this.checkBoxFilter.AutoSize = true;
            this.checkBoxFilter.Checked = true;
            this.checkBoxFilter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxFilter.Enabled = false;
            this.checkBoxFilter.Location = new System.Drawing.Point(7, 23);
            this.checkBoxFilter.Name = "checkBoxFilter";
            this.checkBoxFilter.Size = new System.Drawing.Size(104, 17);
            this.checkBoxFilter.TabIndex = 10;
            this.checkBoxFilter.Text = "Только разные";
            this.checkBoxFilter.UseVisualStyleBackColor = true;
            this.checkBoxFilter.CheckedChanged += new System.EventHandler(this.checkBoxFilter_CheckedChanged);
            // 
            // textBoxFilterText
            // 
            this.textBoxFilterText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilterText.Enabled = false;
            this.textBoxFilterText.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxFilterText.Location = new System.Drawing.Point(320, 21);
            this.textBoxFilterText.Multiline = true;
            this.textBoxFilterText.Name = "textBoxFilterText";
            this.textBoxFilterText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxFilterText.Size = new System.Drawing.Size(418, 35);
            this.textBoxFilterText.TabIndex = 8;
            this.textBoxFilterText.TextChanged += new System.EventHandler(this.checkBoxFilter_CheckedChanged);
            this.textBoxFilterText.Enter += new System.EventHandler(this.textBoxFilterText_Enter);
            // 
            // checkBoxFilterText
            // 
            this.checkBoxFilterText.AutoSize = true;
            this.checkBoxFilterText.Enabled = false;
            this.checkBoxFilterText.Location = new System.Drawing.Point(117, 23);
            this.checkBoxFilterText.Name = "checkBoxFilterText";
            this.checkBoxFilterText.Size = new System.Drawing.Size(197, 17);
            this.checkBoxFilterText.TabIndex = 10;
            this.checkBoxFilterText.Text = "Фильтровать содержащие текст:";
            this.checkBoxFilterText.UseVisualStyleBackColor = true;
            this.checkBoxFilterText.CheckedChanged += new System.EventHandler(this.checkBoxFilter_CheckedChanged);
            // 
            // butFilter
            // 
            this.butFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.butFilter.Enabled = false;
            this.butFilter.Location = new System.Drawing.Point(744, 23);
            this.butFilter.Name = "butFilter";
            this.butFilter.Size = new System.Drawing.Size(64, 23);
            this.butFilter.TabIndex = 11;
            this.butFilter.Text = "Обновить";
            this.butFilter.UseVisualStyleBackColor = true;
            this.butFilter.Click += new System.EventHandler(this.butFilter_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.comboBoxSelComp3);
            this.groupBox2.Controls.Add(this.labelSelComp3);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.butSelComp);
            this.groupBox2.Controls.Add(this.butSelExecBases);
            this.groupBox2.Controls.Add(this.radioButton2);
            this.groupBox2.Controls.Add(this.radioButton1);
            this.groupBox2.Enabled = false;
            this.groupBox2.Location = new System.Drawing.Point(826, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(298, 114);
            this.groupBox2.TabIndex = 16;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Действия";
            // 
            // comboBoxSelComp3
            // 
            this.comboBoxSelComp3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSelComp3.FormattingEnabled = true;
            this.comboBoxSelComp3.Location = new System.Drawing.Point(6, 88);
            this.comboBoxSelComp3.Name = "comboBoxSelComp3";
            this.comboBoxSelComp3.Size = new System.Drawing.Size(286, 21);
            this.comboBoxSelComp3.TabIndex = 14;
            this.comboBoxSelComp3.Visible = false;
            // 
            // labelSelComp3
            // 
            this.labelSelComp3.AutoSize = true;
            this.labelSelComp3.Location = new System.Drawing.Point(6, 74);
            this.labelSelComp3.Name = "labelSelComp3";
            this.labelSelComp3.Size = new System.Drawing.Size(161, 13);
            this.labelSelComp3.TabIndex = 13;
            this.labelSelComp3.Text = "Все выбранные сравнивать с:";
            this.labelSelComp3.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Двойной клик:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // butSelComp
            // 
            this.butSelComp.Location = new System.Drawing.Point(152, 19);
            this.butSelComp.Name = "butSelComp";
            this.butSelComp.Size = new System.Drawing.Size(140, 34);
            this.butSelComp.TabIndex = 7;
            this.butSelComp.Text = "Сравнить выделенные ячейки";
            this.butSelComp.UseVisualStyleBackColor = true;
            this.butSelComp.Click += new System.EventHandler(this.butSelComp_Click);
            // 
            // butSelExecBases
            // 
            this.butSelExecBases.Enabled = false;
            this.butSelExecBases.Location = new System.Drawing.Point(6, 19);
            this.butSelExecBases.Name = "butSelExecBases";
            this.butSelExecBases.Size = new System.Drawing.Size(140, 34);
            this.butSelExecBases.TabIndex = 7;
            this.butSelExecBases.Text = "Выделенные базы отметить в Применение";
            this.butSelExecBases.UseVisualStyleBackColor = true;
            this.butSelExecBases.Click += new System.EventHandler(this.butSelExecBases_Click);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(181, 54);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(101, 17);
            this.radioButton2.TabIndex = 6;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "К Применению";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(94, 54);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(81, 17);
            this.radioButton1.TabIndex = 5;
            this.radioButton1.Text = "Открывать";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // groupBoxComp1
            // 
            this.groupBoxComp1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxComp1.Controls.Add(this.textBoxIPost);
            this.groupBoxComp1.Controls.Add(this.textBoxIPref);
            this.groupBoxComp1.Controls.Add(this.textBoxPrefix);
            this.groupBoxComp1.Controls.Add(this.checkBoxCompareTableWithTrigger);
            this.groupBoxComp1.Controls.Add(this.checkBoxCompareWithTable);
            this.groupBoxComp1.Controls.Add(this.label4);
            this.groupBoxComp1.Controls.Add(this.label5);
            this.groupBoxComp1.Controls.Add(this.label6);
            this.groupBoxComp1.Location = new System.Drawing.Point(6, 35);
            this.groupBoxComp1.Name = "groupBoxComp1";
            this.groupBoxComp1.Size = new System.Drawing.Size(814, 80);
            this.groupBoxComp1.TabIndex = 17;
            this.groupBoxComp1.TabStop = false;
            this.groupBoxComp1.Text = "Настройки сравнения (подробнее см. Утилиты)";
            // 
            // textBoxIPost
            // 
            this.textBoxIPost.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.textBoxIPost.Location = new System.Drawing.Point(608, 30);
            this.textBoxIPost.Multiline = true;
            this.textBoxIPost.Name = "textBoxIPost";
            this.textBoxIPost.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxIPost.Size = new System.Drawing.Size(200, 50);
            this.textBoxIPost.TabIndex = 4;
            this.textBoxIPost.WordWrap = false;
            this.textBoxIPost.TextChanged += new System.EventHandler(this.сompare_CheckedChanged);
            // 
            // textBoxIPref
            // 
            this.textBoxIPref.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.textBoxIPref.Location = new System.Drawing.Point(402, 30);
            this.textBoxIPref.Multiline = true;
            this.textBoxIPref.Name = "textBoxIPref";
            this.textBoxIPref.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxIPref.Size = new System.Drawing.Size(200, 50);
            this.textBoxIPref.TabIndex = 5;
            this.textBoxIPref.WordWrap = false;
            this.textBoxIPref.TextChanged += new System.EventHandler(this.сompare_CheckedChanged);
            // 
            // textBoxPrefix
            // 
            this.textBoxPrefix.Font = new System.Drawing.Font("Courier New", 8.25F);
            this.textBoxPrefix.Location = new System.Drawing.Point(123, 19);
            this.textBoxPrefix.Name = "textBoxPrefix";
            this.textBoxPrefix.Size = new System.Drawing.Size(273, 20);
            this.textBoxPrefix.TabIndex = 6;
            this.textBoxPrefix.TextChanged += new System.EventHandler(this.сompare_CheckedChanged);
            // 
            // checkBoxCompareTableWithTrigger
            // 
            this.checkBoxCompareTableWithTrigger.AutoSize = true;
            this.checkBoxCompareTableWithTrigger.Location = new System.Drawing.Point(213, 45);
            this.checkBoxCompareTableWithTrigger.Name = "checkBoxCompareTableWithTrigger";
            this.checkBoxCompareTableWithTrigger.Size = new System.Drawing.Size(134, 17);
            this.checkBoxCompareTableWithTrigger.TabIndex = 9;
            this.checkBoxCompareTableWithTrigger.Text = "Сравнивать триггера";
            this.checkBoxCompareTableWithTrigger.UseVisualStyleBackColor = true;
            this.checkBoxCompareTableWithTrigger.CheckedChanged += new System.EventHandler(this.сompare_CheckedChanged);
            // 
            // checkBoxCompareWithTable
            // 
            this.checkBoxCompareWithTable.AutoSize = true;
            this.checkBoxCompareWithTable.Checked = true;
            this.checkBoxCompareWithTable.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCompareWithTable.Location = new System.Drawing.Point(10, 45);
            this.checkBoxCompareWithTable.Name = "checkBoxCompareWithTable";
            this.checkBoxCompareWithTable.Size = new System.Drawing.Size(197, 17);
            this.checkBoxCompareWithTable.TabIndex = 10;
            this.checkBoxCompareWithTable.Text = "Также сравнивать таблицы (beta)";
            this.checkBoxCompareWithTable.UseVisualStyleBackColor = true;
            this.checkBoxCompareWithTable.CheckedChanged += new System.EventHandler(this.сompare_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(605, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(179, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Список игнорируемых окончаний:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(399, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(182, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Список игнорируемых префиксов:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(118, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Объект начинается с:";
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.Location = new System.Drawing.Point(6, 121);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.compareBases);
            this.splitContainer.Panel1MinSize = 1;
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.dataGridView);
            this.splitContainer.Panel2MinSize = 250;
            this.splitContainer.Size = new System.Drawing.Size(1120, 417);
            this.splitContainer.SplitterDistance = 350;
            this.splitContainer.TabIndex = 3;
            // 
            // compareBases
            // 
            this.compareBases.Dock = System.Windows.Forms.DockStyle.Fill;
            this.compareBases.Location = new System.Drawing.Point(0, 0);
            this.compareBases.Name = "compareBases";
            this.compareBases.Size = new System.Drawing.Size(350, 417);
            this.compareBases.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainerExec);
            this.tabPage2.Location = new System.Drawing.Point(4, 23);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1132, 544);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Применение";
            // 
            // splitContainerExec
            // 
            this.splitContainerExec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerExec.Location = new System.Drawing.Point(3, 3);
            this.splitContainerExec.Name = "splitContainerExec";
            this.splitContainerExec.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerExec.Panel1
            // 
            this.splitContainerExec.Panel1.Controls.Add(this.butTextCopy);
            this.splitContainerExec.Panel1.Controls.Add(this.butTextClear);
            this.splitContainerExec.Panel1.Controls.Add(this.splitContainer1);
            this.splitContainerExec.Panel1.Controls.Add(this.labelExecStat);
            this.splitContainerExec.Panel1.Controls.Add(this.buttonExec);
            // 
            // splitContainerExec.Panel2
            // 
            this.splitContainerExec.Panel2.Controls.Add(this.textBoxExecRes);
            this.splitContainerExec.Size = new System.Drawing.Size(1126, 538);
            this.splitContainerExec.SplitterDistance = 260;
            this.splitContainerExec.TabIndex = 0;
            // 
            // butTextCopy
            // 
            this.butTextCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butTextCopy.Location = new System.Drawing.Point(974, 233);
            this.butTextCopy.Name = "butTextCopy";
            this.butTextCopy.Size = new System.Drawing.Size(79, 23);
            this.butTextCopy.TabIndex = 12;
            this.butTextCopy.Text = "Копировать";
            this.butTextCopy.UseVisualStyleBackColor = true;
            this.butTextCopy.Click += new System.EventHandler(this.butTextCopy_Click);
            // 
            // butTextClear
            // 
            this.butTextClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butTextClear.Location = new System.Drawing.Point(1059, 233);
            this.butTextClear.Name = "butTextClear";
            this.butTextClear.Size = new System.Drawing.Size(64, 23);
            this.butTextClear.TabIndex = 12;
            this.butTextClear.Text = "Очистить";
            this.butTextClear.UseVisualStyleBackColor = true;
            this.butTextClear.Click += new System.EventHandler(this.butTextClear_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridBasesExec);
            this.splitContainer1.Panel1MinSize = 1;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tbExec);
            this.splitContainer1.Size = new System.Drawing.Size(1126, 229);
            this.splitContainer1.SplitterDistance = 350;
            this.splitContainer1.TabIndex = 3;
            // 
            // gridBasesExec
            // 
            this.gridBasesExec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBasesExec.Location = new System.Drawing.Point(0, 0);
            this.gridBasesExec.Name = "gridBasesExec";
            this.gridBasesExec.Size = new System.Drawing.Size(350, 229);
            this.gridBasesExec.TabIndex = 1;
            // 
            // tbExec
            // 
            this.tbExec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbExec.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tbExec.Location = new System.Drawing.Point(0, 0);
            this.tbExec.Multiline = true;
            this.tbExec.Name = "tbExec";
            this.tbExec.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbExec.Size = new System.Drawing.Size(772, 229);
            this.tbExec.TabIndex = 0;
            this.tbExec.WordWrap = false;
            // 
            // labelExecStat
            // 
            this.labelExecStat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelExecStat.AutoSize = true;
            this.labelExecStat.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelExecStat.Location = new System.Drawing.Point(98, 238);
            this.labelExecStat.Name = "labelExecStat";
            this.labelExecStat.Size = new System.Drawing.Size(64, 13);
            this.labelExecStat.TabIndex = 2;
            this.labelExecStat.Text = "Нет задачи";
            // 
            // buttonExec
            // 
            this.buttonExec.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonExec.BackColor = System.Drawing.Color.Bisque;
            this.buttonExec.Image = ((System.Drawing.Image)(resources.GetObject("buttonExec.Image")));
            this.buttonExec.Location = new System.Drawing.Point(3, 230);
            this.buttonExec.Name = "buttonExec";
            this.buttonExec.Size = new System.Drawing.Size(89, 28);
            this.buttonExec.TabIndex = 1;
            this.buttonExec.Text = "Выполнить";
            this.buttonExec.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonExec.UseVisualStyleBackColor = false;
            this.buttonExec.Click += new System.EventHandler(this.buttonExec_Click);
            // 
            // textBoxExecRes
            // 
            this.textBoxExecRes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxExecRes.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxExecRes.Location = new System.Drawing.Point(0, 0);
            this.textBoxExecRes.Multiline = true;
            this.textBoxExecRes.Name = "textBoxExecRes";
            this.textBoxExecRes.ReadOnly = true;
            this.textBoxExecRes.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxExecRes.Size = new System.Drawing.Size(1126, 274);
            this.textBoxExecRes.TabIndex = 1;
            this.textBoxExecRes.WordWrap = false;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.utilsView1);
            this.tabPage4.Location = new System.Drawing.Point(4, 23);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1132, 544);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Утилиты";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // utilsView1
            // 
            this.utilsView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.utilsView1.Location = new System.Drawing.Point(3, 3);
            this.utilsView1.Name = "utilsView1";
            this.utilsView1.Size = new System.Drawing.Size(1126, 538);
            this.utilsView1.TabIndex = 0;
            this.utilsView1.Load += new System.EventHandler(this.utilsView1_Load);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Location = new System.Drawing.Point(4, 23);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1132, 544);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "О программе";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(30, 204);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(1094, 290);
            this.label7.TabIndex = 0;
            this.label7.Text = resources.GetString("label7.Text");
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(29, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(610, 162);
            this.label2.TabIndex = 0;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "RedIcon.ico");
            // 
            // dataGridViewMenuStrip
            // 
            this.dataGridViewMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuComp,
            this.toolStripMenuToApp,
            this.toolStripMenuBases,
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuMSSQL,
            this.перейтиКФайлуToolStripMenuItem,
            this.toolStripMenuItemSVNLog});
            this.dataGridViewMenuStrip.Name = "dataGridViewMenuStrip";
            this.dataGridViewMenuStrip.Size = new System.Drawing.Size(232, 164);
            // 
            // toolStripMenuComp
            // 
            this.toolStripMenuComp.Name = "toolStripMenuComp";
            this.toolStripMenuComp.Size = new System.Drawing.Size(231, 22);
            this.toolStripMenuComp.Text = "Сравнить";
            this.toolStripMenuComp.Click += new System.EventHandler(this.butSelComp_Click);
            // 
            // toolStripMenuToApp
            // 
            this.toolStripMenuToApp.Image = global::CompareBases.Properties.Resources.sqlQuery;
            this.toolStripMenuToApp.Name = "toolStripMenuToApp";
            this.toolStripMenuToApp.Size = new System.Drawing.Size(231, 22);
            this.toolStripMenuToApp.Text = "Содержимое к применению";
            this.toolStripMenuToApp.Click += new System.EventHandler(this.toolStripMenuToApp_Click);
            // 
            // toolStripMenuBases
            // 
            this.toolStripMenuBases.Image = global::CompareBases.Properties.Resources.check;
            this.toolStripMenuBases.Name = "toolStripMenuBases";
            this.toolStripMenuBases.Size = new System.Drawing.Size(231, 22);
            this.toolStripMenuBases.Text = "Отметить базы как цель";
            this.toolStripMenuBases.Click += new System.EventHandler(this.butSelExecBases_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(228, 6);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(231, 22);
            this.toolStripMenuItem2.Text = "Содержимое в буфер";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuMSSQL
            // 
            this.toolStripMenuMSSQL.Name = "toolStripMenuMSSQL";
            this.toolStripMenuMSSQL.Size = new System.Drawing.Size(231, 22);
            this.toolStripMenuMSSQL.Text = "Открыть в MS SQL";
            this.toolStripMenuMSSQL.Click += new System.EventHandler(this.toolStripMenuMSSQL_Click);
            // 
            // перейтиКФайлуToolStripMenuItem
            // 
            this.перейтиКФайлуToolStripMenuItem.Name = "перейтиКФайлуToolStripMenuItem";
            this.перейтиКФайлуToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.перейтиКФайлуToolStripMenuItem.Text = "Перейти к файлу";
            this.перейтиКФайлуToolStripMenuItem.Click += new System.EventHandler(this.перейтиКФайлуToolStripMenuItem_Click);
            // 
            // toolStripMenuItemSVNLog
            // 
            this.toolStripMenuItemSVNLog.Name = "toolStripMenuItemSVNLog";
            this.toolStripMenuItemSVNLog.Size = new System.Drawing.Size(231, 22);
            this.toolStripMenuItemSVNLog.Text = "SVN лог";
            this.toolStripMenuItemSVNLog.Click += new System.EventHandler(this.toolStripMenuItemSVNLog_Click);
            // 
            // buttonExec2
            // 
            this.buttonExec2.BackColor = System.Drawing.Color.Bisque;
            this.buttonExec2.Image = ((System.Drawing.Image)(resources.GetObject("buttonExec2.Image")));
            this.buttonExec2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonExec2.Location = new System.Drawing.Point(360, 0);
            this.buttonExec2.Name = "buttonExec2";
            this.buttonExec2.Size = new System.Drawing.Size(180, 23);
            this.buttonExec2.TabIndex = 5;
            this.buttonExec2.Text = "Выполнить в \"Применение\"";
            this.buttonExec2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonExec2.UseVisualStyleBackColor = false;
            this.buttonExec2.Click += new System.EventHandler(this.buttonExec_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1140, 571);
            this.Controls.Add(this.buttonExec2);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Сравнение баз данных";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBoxComp2.ResumeLayout(false);
            this.groupBoxComp2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBoxComp1.ResumeLayout(false);
            this.groupBoxComp1.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainerExec.Panel1.ResumeLayout(false);
            this.splitContainerExec.Panel1.PerformLayout();
            this.splitContainerExec.Panel2.ResumeLayout(false);
            this.splitContainerExec.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerExec)).EndInit();
            this.splitContainerExec.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.dataGridViewMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UtilsView utilsView1;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button butStart;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer splitContainer;
        private GridBases compareBases;
        private System.Windows.Forms.SplitContainer splitContainerExec;
        private System.Windows.Forms.Label labelExecStat;
        private System.Windows.Forms.Button buttonExec;
        private System.Windows.Forms.TextBox tbExec;
        private System.Windows.Forms.TextBox textBoxExecRes;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private GridBases gridBasesExec;
        private System.Windows.Forms.Button butSelExecBases;
        private System.Windows.Forms.Button butFilter;
        private System.Windows.Forms.CheckBox checkBoxFilter;
        private System.Windows.Forms.TextBox textBoxFilterText;
        private System.Windows.Forms.CheckBox checkBoxFilterText;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBoxComp2;
        private System.Windows.Forms.GroupBox groupBoxComp1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxIPost;
        private System.Windows.Forms.TextBox textBoxIPref;
        private System.Windows.Forms.TextBox textBoxPrefix;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBoxCompareTableWithTrigger;
        private System.Windows.Forms.CheckBox checkBoxCompareWithTable;
        private System.Windows.Forms.CheckBox checkBoxSortType;
        private System.Windows.Forms.CheckBox checkBoxShowTableTop;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button butSelComp;
        private System.Windows.Forms.ComboBox comboBoxSelComp3;
        private System.Windows.Forms.Label labelSelComp3;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip dataGridViewMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuComp;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuToApp;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuBases;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuMSSQL;
        private System.Windows.Forms.ToolStripMenuItem перейтиКФайлуToolStripMenuItem;
        private System.Windows.Forms.Button buttonExec2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button butTextCopy;
        private System.Windows.Forms.Button butTextClear;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSVNLog;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}

