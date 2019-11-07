namespace CompareBases
{
    partial class GridBases
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
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.допToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sQLCompareToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.создатьСнапшотыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelMenu = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.panelMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(204, 260);
            this.dataGridView.TabIndex = 1;
            this.dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellContentClick);
            this.dataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
            this.dataGridView.RowStateChanged += new System.Windows.Forms.DataGridViewRowStateChangedEventHandler(this.dataGridView_RowStateChanged);
            this.dataGridView.SelectionChanged += new System.EventHandler(this.dataGridView_SelectionChanged);
            // 
            // menuStrip
            // 
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.допToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, -2);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.ShowItemToolTips = true;
            this.menuStrip.Size = new System.Drawing.Size(49, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // допToolStripMenuItem
            // 
            this.допToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sQLCompareToolStripMenuItem,
            this.создатьСнапшотыToolStripMenuItem});
            this.допToolStripMenuItem.Name = "допToolStripMenuItem";
            this.допToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.допToolStripMenuItem.Text = "Доп";
            // 
            // sQLCompareToolStripMenuItem
            // 
            this.sQLCompareToolStripMenuItem.Name = "sQLCompareToolStripMenuItem";
            this.sQLCompareToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.sQLCompareToolStripMenuItem.Text = "Взять строку подключения";
            this.sQLCompareToolStripMenuItem.Click += new System.EventHandler(this.sQLCompareToolStripMenuItem_Click);
            // 
            // создатьСнапшотыToolStripMenuItem
            // 
            this.создатьСнапшотыToolStripMenuItem.Name = "создатьСнапшотыToolStripMenuItem";
            this.создатьСнапшотыToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.создатьСнапшотыToolStripMenuItem.Text = "Создать снапшоты";
            this.создатьСнапшотыToolStripMenuItem.Click += new System.EventHandler(this.создатьСнапшотыToolStripMenuItem_Click);
            // 
            // panelMenu
            // 
            this.panelMenu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMenu.Controls.Add(this.menuStrip);
            this.panelMenu.Location = new System.Drawing.Point(128, 2);
            this.panelMenu.Name = "panelMenu";
            this.panelMenu.Size = new System.Drawing.Size(51, 20);
            this.panelMenu.TabIndex = 3;
            this.panelMenu.Visible = false;
            // 
            // GridBases
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMenu);
            this.Controls.Add(this.dataGridView);
            this.Name = "GridBases";
            this.Size = new System.Drawing.Size(204, 260);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panelMenu.ResumeLayout(false);
            this.panelMenu.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem допToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sQLCompareToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem создатьСнапшотыToolStripMenuItem;
        private System.Windows.Forms.Panel panelMenu;
    }
}
