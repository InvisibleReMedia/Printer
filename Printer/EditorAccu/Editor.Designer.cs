namespace EditorAccu
{
    partial class Editor
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ToolStripSeparator editSeparatorItem;
            this.menu = new System.Windows.Forms.MenuStrip();
            this.appItem = new System.Windows.Forms.ToolStripMenuItem();
            this.appNewItem = new System.Windows.Forms.ToolStripMenuItem();
            this.appLoadItem = new System.Windows.Forms.ToolStripMenuItem();
            this.appSaveItem = new System.Windows.Forms.ToolStripMenuItem();
            this.appSaveAsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.appQuitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editUndoItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editRedoItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editCopyVarItem = new System.Windows.Forms.ToolStripMenuItem();
            this.varsItem = new System.Windows.Forms.ToolStripMenuItem();
            this.varsAddItem = new System.Windows.Forms.ToolStripMenuItem();
            this.varsModifyItem = new System.Windows.Forms.ToolStripMenuItem();
            this.varsRemoveItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vars = new System.Windows.Forms.ListBox();
            this.splitterLeft = new System.Windows.Forms.Splitter();
            this.txtSource = new System.Windows.Forms.TextBox();
            editSeparatorItem = new System.Windows.Forms.ToolStripSeparator();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // editSeparatorItem
            // 
            editSeparatorItem.Name = "editSeparatorItem";
            editSeparatorItem.Size = new System.Drawing.Size(149, 6);
            // 
            // menu
            // 
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.appItem,
            this.editItem,
            this.varsItem,
            this.executeItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(628, 24);
            this.menu.TabIndex = 3;
            this.menu.Text = "menuStrip1";
            // 
            // appItem
            // 
            this.appItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.appNewItem,
            this.appLoadItem,
            this.appSaveItem,
            this.appSaveAsItem,
            this.appQuitItem});
            this.appItem.Name = "appItem";
            this.appItem.Size = new System.Drawing.Size(80, 20);
            this.appItem.Text = "Application";
            // 
            // appNewItem
            // 
            this.appNewItem.Name = "appNewItem";
            this.appNewItem.Size = new System.Drawing.Size(152, 22);
            this.appNewItem.Text = "New";
            this.appNewItem.Click += new System.EventHandler(this.appNewItem_Click);
            // 
            // appLoadItem
            // 
            this.appLoadItem.Name = "appLoadItem";
            this.appLoadItem.Size = new System.Drawing.Size(152, 22);
            this.appLoadItem.Text = "Load";
            this.appLoadItem.Click += new System.EventHandler(this.appLoadItem_Click);
            // 
            // appSaveItem
            // 
            this.appSaveItem.Name = "appSaveItem";
            this.appSaveItem.Size = new System.Drawing.Size(152, 22);
            this.appSaveItem.Text = "Save";
            this.appSaveItem.Click += new System.EventHandler(this.appSaveItem_Click);
            // 
            // appSaveAsItem
            // 
            this.appSaveAsItem.Name = "appSaveAsItem";
            this.appSaveAsItem.Size = new System.Drawing.Size(152, 22);
            this.appSaveAsItem.Text = "Save As";
            this.appSaveAsItem.Click += new System.EventHandler(this.appSaveAsItem_Click);
            // 
            // appQuitItem
            // 
            this.appQuitItem.Name = "appQuitItem";
            this.appQuitItem.Size = new System.Drawing.Size(152, 22);
            this.appQuitItem.Text = "Quit";
            this.appQuitItem.Click += new System.EventHandler(this.appQuitItem_Click);
            // 
            // editItem
            // 
            this.editItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.editUndoItem,
            this.editRedoItem,
            editSeparatorItem,
            this.editCopyVarItem});
            this.editItem.Name = "editItem";
            this.editItem.Size = new System.Drawing.Size(39, 20);
            this.editItem.Text = "Edit";
            // 
            // editUndoItem
            // 
            this.editUndoItem.Name = "editUndoItem";
            this.editUndoItem.Size = new System.Drawing.Size(152, 22);
            this.editUndoItem.Text = "Undo";
            this.editUndoItem.Click += new System.EventHandler(this.editUndoItem_Click);
            // 
            // editRedoItem
            // 
            this.editRedoItem.Name = "editRedoItem";
            this.editRedoItem.Size = new System.Drawing.Size(152, 22);
            this.editRedoItem.Text = "Redo";
            this.editRedoItem.Click += new System.EventHandler(this.editRedoItem_Click);
            // 
            // editCopyVarItem
            // 
            this.editCopyVarItem.Name = "editCopyVarItem";
            this.editCopyVarItem.Size = new System.Drawing.Size(152, 22);
            this.editCopyVarItem.Text = "Copy Variable";
            this.editCopyVarItem.Click += new System.EventHandler(this.editCopyVarItem_Click);
            // 
            // varsItem
            // 
            this.varsItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.varsAddItem,
            this.varsModifyItem,
            this.varsRemoveItem});
            this.varsItem.Name = "varsItem";
            this.varsItem.Size = new System.Drawing.Size(46, 20);
            this.varsItem.Text = "Accu";
            // 
            // varsAddItem
            // 
            this.varsAddItem.Name = "varsAddItem";
            this.varsAddItem.Size = new System.Drawing.Size(152, 22);
            this.varsAddItem.Text = "Add";
            this.varsAddItem.Click += new System.EventHandler(this.varsAddItem_Click);
            // 
            // varsModifyItem
            // 
            this.varsModifyItem.Name = "varsModifyItem";
            this.varsModifyItem.Size = new System.Drawing.Size(152, 22);
            this.varsModifyItem.Text = "Modify";
            this.varsModifyItem.Click += new System.EventHandler(this.varsModifyItem_Click);
            // 
            // varsRemoveItem
            // 
            this.varsRemoveItem.Name = "varsRemoveItem";
            this.varsRemoveItem.Size = new System.Drawing.Size(152, 22);
            this.varsRemoveItem.Text = "Remove";
            this.varsRemoveItem.Click += new System.EventHandler(this.varsRemoveItem_Click);
            // 
            // executeItem
            // 
            this.executeItem.Name = "executeItem";
            this.executeItem.Size = new System.Drawing.Size(59, 20);
            this.executeItem.Text = "Execute";
            this.executeItem.Click += new System.EventHandler(this.executeItem_Click);
            // 
            // vars
            // 
            this.vars.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.vars.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.vars.Dock = System.Windows.Forms.DockStyle.Left;
            this.vars.ForeColor = System.Drawing.Color.White;
            this.vars.FormattingEnabled = true;
            this.vars.Location = new System.Drawing.Point(0, 24);
            this.vars.Name = "vars";
            this.vars.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.vars.Size = new System.Drawing.Size(120, 381);
            this.vars.TabIndex = 4;
            this.vars.TabStop = false;
            this.vars.DoubleClick += new System.EventHandler(this.vars_DoubleClick);
            // 
            // splitterLeft
            // 
            this.splitterLeft.Location = new System.Drawing.Point(120, 24);
            this.splitterLeft.Name = "splitterLeft";
            this.splitterLeft.Size = new System.Drawing.Size(3, 381);
            this.splitterLeft.TabIndex = 6;
            this.splitterLeft.TabStop = false;
            // 
            // txtSource
            // 
            this.txtSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSource.Location = new System.Drawing.Point(123, 24);
            this.txtSource.Multiline = true;
            this.txtSource.Name = "txtSource";
            this.txtSource.ReadOnly = true;
            this.txtSource.Size = new System.Drawing.Size(505, 381);
            this.txtSource.TabIndex = 8;
            this.txtSource.TabStop = false;
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(628, 405);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.splitterLeft);
            this.Controls.Add(this.vars);
            this.Controls.Add(this.menu);
            this.MainMenuStrip = this.menu;
            this.Name = "Editor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Editor Accu";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Editor_FormClosing);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem appItem;
        private System.Windows.Forms.ToolStripMenuItem appNewItem;
        private System.Windows.Forms.ToolStripMenuItem appLoadItem;
        private System.Windows.Forms.ToolStripMenuItem appSaveItem;
        private System.Windows.Forms.ToolStripMenuItem appSaveAsItem;
        private System.Windows.Forms.ToolStripMenuItem appQuitItem;
        private System.Windows.Forms.ToolStripMenuItem editItem;
        private System.Windows.Forms.ToolStripMenuItem editCopyVarItem;
        private System.Windows.Forms.ToolStripMenuItem varsItem;
        private System.Windows.Forms.ToolStripMenuItem varsAddItem;
        private System.Windows.Forms.ToolStripMenuItem varsModifyItem;
        private System.Windows.Forms.ToolStripMenuItem varsRemoveItem;
        private System.Windows.Forms.ListBox vars;
        private System.Windows.Forms.Splitter splitterLeft;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.ToolStripMenuItem editUndoItem;
        private System.Windows.Forms.ToolStripMenuItem editRedoItem;
        private System.Windows.Forms.ToolStripMenuItem executeItem;
    }
}

