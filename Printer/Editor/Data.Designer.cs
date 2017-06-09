namespace Editor
{
    partial class Data
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
            this.vars = new System.Windows.Forms.ListBox();
            this.rbVariable = new System.Windows.Forms.RadioButton();
            this.txtConst = new System.Windows.Forms.TextBox();
            this.rbConst = new System.Windows.Forms.RadioButton();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
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
            this.vars.Location = new System.Drawing.Point(12, 38);
            this.vars.Name = "vars";
            this.vars.Size = new System.Drawing.Size(156, 195);
            this.vars.TabIndex = 0;
            this.vars.TabStop = false;
            // 
            // rbVariable
            // 
            this.rbVariable.Checked = true;
            this.rbVariable.Location = new System.Drawing.Point(12, 9);
            this.rbVariable.Name = "rbVariable";
            this.rbVariable.Size = new System.Drawing.Size(156, 24);
            this.rbVariable.TabIndex = 1;
            this.rbVariable.TabStop = true;
            this.rbVariable.Text = "Variable";
            this.rbVariable.UseVisualStyleBackColor = true;
            this.rbVariable.CheckedChanged += new System.EventHandler(this.rbVariable_CheckedChanged);
            // 
            // txtConst
            // 
            this.txtConst.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConst.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtConst.Enabled = false;
            this.txtConst.Location = new System.Drawing.Point(185, 38);
            this.txtConst.Multiline = true;
            this.txtConst.Name = "txtConst";
            this.txtConst.Size = new System.Drawing.Size(248, 195);
            this.txtConst.TabIndex = 2;
            // 
            // rbConst
            // 
            this.rbConst.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rbConst.Location = new System.Drawing.Point(185, 9);
            this.rbConst.Name = "rbConst";
            this.rbConst.Size = new System.Drawing.Size(248, 24);
            this.rbConst.TabIndex = 3;
            this.rbConst.Text = "Constant string";
            this.rbConst.UseVisualStyleBackColor = true;
            this.rbConst.CheckedChanged += new System.EventHandler(this.rbConst_CheckedChanged);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(393, 240);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(40, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(316, 240);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(61, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // Data
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(445, 272);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.rbConst);
            this.Controls.Add(this.txtConst);
            this.Controls.Add(this.rbVariable);
            this.Controls.Add(this.vars);
            this.ForeColor = System.Drawing.Color.White;
            this.Location = new System.Drawing.Point(100, 100);
            this.MinimumSize = new System.Drawing.Size(340, 206);
            this.Name = "Data";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Data";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Data_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox vars;
        private System.Windows.Forms.RadioButton rbVariable;
        private System.Windows.Forms.TextBox txtConst;
        private System.Windows.Forms.RadioButton rbConst;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}