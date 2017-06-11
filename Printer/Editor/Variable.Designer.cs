namespace Editor
{
    partial class Variable
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblValue = new System.Windows.Forms.Label();
            this.txtValue = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tc = new System.Windows.Forms.TabControl();
            this.pValue = new System.Windows.Forms.TabPage();
            this.rbValue = new System.Windows.Forms.RadioButton();
            this.pInclude = new System.Windows.Forms.TabPage();
            this.rbInclude = new System.Windows.Forms.RadioButton();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.vars = new System.Windows.Forms.ListBox();
            this.ckIndented = new System.Windows.Forms.CheckBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.tc.SuspendLayout();
            this.pValue.SuspendLayout();
            this.pInclude.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblValue
            // 
            this.lblValue.AutoSize = true;
            this.lblValue.Location = new System.Drawing.Point(7, 9);
            this.lblValue.Name = "lblValue";
            this.lblValue.Size = new System.Drawing.Size(34, 13);
            this.lblValue.TabIndex = 2;
            this.lblValue.Text = "Value";
            // 
            // txtValue
            // 
            this.txtValue.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtValue.Location = new System.Drawing.Point(58, 6);
            this.txtValue.Multiline = true;
            this.txtValue.Name = "txtValue";
            this.txtValue.Size = new System.Drawing.Size(328, 203);
            this.txtValue.TabIndex = 7;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.BackColor = System.Drawing.Color.Black;
            this.btnOK.Location = new System.Drawing.Point(377, 329);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(35, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.Black;
            this.btnCancel.Location = new System.Drawing.Point(321, 329);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(50, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tc
            // 
            this.tc.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tc.Controls.Add(this.pValue);
            this.tc.Controls.Add(this.pInclude);
            this.tc.Location = new System.Drawing.Point(12, 38);
            this.tc.Name = "tc";
            this.tc.SelectedIndex = 0;
            this.tc.Size = new System.Drawing.Size(400, 285);
            this.tc.TabIndex = 6;
            this.tc.TabStop = false;
            // 
            // pValue
            // 
            this.pValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pValue.Controls.Add(this.rbValue);
            this.pValue.Controls.Add(this.txtValue);
            this.pValue.Controls.Add(this.lblValue);
            this.pValue.Location = new System.Drawing.Point(4, 22);
            this.pValue.Name = "pValue";
            this.pValue.Padding = new System.Windows.Forms.Padding(3);
            this.pValue.Size = new System.Drawing.Size(392, 259);
            this.pValue.TabIndex = 0;
            this.pValue.Text = "Value";
            // 
            // rbValue
            // 
            this.rbValue.AutoSize = true;
            this.rbValue.Checked = true;
            this.rbValue.Location = new System.Drawing.Point(10, 222);
            this.rbValue.Name = "rbValue";
            this.rbValue.Size = new System.Drawing.Size(82, 17);
            this.rbValue.TabIndex = 8;
            this.rbValue.TabStop = true;
            this.rbValue.Text = "select value";
            this.rbValue.UseVisualStyleBackColor = true;
            this.rbValue.CheckedChanged += new System.EventHandler(this.rbValue_CheckedChanged);
            // 
            // pInclude
            // 
            this.pInclude.BackColor = System.Drawing.Color.Black;
            this.pInclude.Controls.Add(this.rbInclude);
            this.pInclude.Controls.Add(this.txtSource);
            this.pInclude.Controls.Add(this.btnBrowse);
            this.pInclude.Controls.Add(this.txtFile);
            this.pInclude.Controls.Add(this.btnDelete);
            this.pInclude.Controls.Add(this.btnEdit);
            this.pInclude.Controls.Add(this.btnAdd);
            this.pInclude.Controls.Add(this.vars);
            this.pInclude.Location = new System.Drawing.Point(4, 22);
            this.pInclude.Name = "pInclude";
            this.pInclude.Padding = new System.Windows.Forms.Padding(3);
            this.pInclude.Size = new System.Drawing.Size(392, 259);
            this.pInclude.TabIndex = 1;
            this.pInclude.Text = "Include";
            // 
            // rbInclude
            // 
            this.rbInclude.AutoSize = true;
            this.rbInclude.Location = new System.Drawing.Point(9, 225);
            this.rbInclude.Name = "rbInclude";
            this.rbInclude.Size = new System.Drawing.Size(90, 17);
            this.rbInclude.TabIndex = 7;
            this.rbInclude.Text = "select include";
            this.rbInclude.UseVisualStyleBackColor = true;
            this.rbInclude.CheckedChanged += new System.EventHandler(this.rbInclude_CheckedChanged);
            // 
            // txtSource
            // 
            this.txtSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSource.Location = new System.Drawing.Point(160, 35);
            this.txtSource.Multiline = true;
            this.txtSource.Name = "txtSource";
            this.txtSource.ReadOnly = true;
            this.txtSource.Size = new System.Drawing.Size(226, 175);
            this.txtSource.TabIndex = 7;
            this.txtSource.TabStop = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnBrowse.Location = new System.Drawing.Point(320, 6);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(66, 23);
            this.btnBrowse.TabIndex = 13;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = false;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtFile
            // 
            this.txtFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFile.Location = new System.Drawing.Point(160, 8);
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(154, 20);
            this.txtFile.TabIndex = 12;
            // 
            // btnDelete
            // 
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Location = new System.Drawing.Point(98, 6);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(56, 23);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.AutoSize = true;
            this.btnEdit.ForeColor = System.Drawing.Color.Black;
            this.btnEdit.Location = new System.Drawing.Point(52, 6);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(40, 23);
            this.btnEdit.TabIndex = 9;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.AutoSize = true;
            this.btnAdd.ForeColor = System.Drawing.Color.Black;
            this.btnAdd.Location = new System.Drawing.Point(6, 6);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(40, 23);
            this.btnAdd.TabIndex = 8;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // vars
            // 
            this.vars.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.vars.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.vars.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.vars.ForeColor = System.Drawing.Color.White;
            this.vars.FormattingEnabled = true;
            this.vars.IntegralHeight = false;
            this.vars.Location = new System.Drawing.Point(6, 35);
            this.vars.Name = "vars";
            this.vars.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.vars.Size = new System.Drawing.Size(148, 175);
            this.vars.TabIndex = 1;
            this.vars.TabStop = false;
            this.vars.DoubleClick += new System.EventHandler(this.vars_DoubleClick);
            // 
            // ckIndented
            // 
            this.ckIndented.AutoSize = true;
            this.ckIndented.Location = new System.Drawing.Point(12, 333);
            this.ckIndented.Name = "ckIndented";
            this.ckIndented.Size = new System.Drawing.Size(68, 17);
            this.ckIndented.TabIndex = 2;
            this.ckIndented.Text = "Indented";
            this.ckIndented.UseVisualStyleBackColor = true;
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(74, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(338, 20);
            this.txtName.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(22, 15);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            // 
            // Variable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(424, 362);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.ckIndented);
            this.Controls.Add(this.tc);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.ForeColor = System.Drawing.Color.White;
            this.Location = new System.Drawing.Point(100, 100);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(440, 400);
            this.Name = "Variable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Variable";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Variable_FormClosing);
            this.Load += new System.EventHandler(this.Variable_Load);
            this.tc.ResumeLayout(false);
            this.pValue.ResumeLayout(false);
            this.pValue.PerformLayout();
            this.pInclude.ResumeLayout(false);
            this.pInclude.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblValue;
        private System.Windows.Forms.TextBox txtValue;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabControl tc;
        private System.Windows.Forms.TabPage pValue;
        private System.Windows.Forms.TabPage pInclude;
        private System.Windows.Forms.ListBox vars;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.CheckBox ckIndented;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.RadioButton rbValue;
        private System.Windows.Forms.RadioButton rbInclude;
    }
}