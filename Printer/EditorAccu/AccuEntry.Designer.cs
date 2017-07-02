namespace EditorAccu
{
    partial class AccuEntry
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
            this.pNode = new System.Windows.Forms.TabPage();
            this.rbNodes = new System.Windows.Forms.RadioButton();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.nodes = new System.Windows.Forms.ListBox();
            this.ckIndented = new System.Windows.Forms.CheckBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.pRef = new System.Windows.Forms.TabPage();
            this.refs = new System.Windows.Forms.ListBox();
            this.rbRef = new System.Windows.Forms.RadioButton();
            this.pMethod = new System.Windows.Forms.TabPage();
            this.rbMethod = new System.Windows.Forms.RadioButton();
            this.txtMethodName = new System.Windows.Forms.TextBox();
            this.lblMethodName = new System.Windows.Forms.Label();
            this.tc.SuspendLayout();
            this.pValue.SuspendLayout();
            this.pNode.SuspendLayout();
            this.pRef.SuspendLayout();
            this.pMethod.SuspendLayout();
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
            this.tc.Controls.Add(this.pNode);
            this.tc.Controls.Add(this.pRef);
            this.tc.Controls.Add(this.pMethod);
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
            this.rbValue.Location = new System.Drawing.Point(11, 225);
            this.rbValue.Name = "rbValue";
            this.rbValue.Size = new System.Drawing.Size(82, 17);
            this.rbValue.TabIndex = 8;
            this.rbValue.TabStop = true;
            this.rbValue.Text = "select value";
            this.rbValue.UseVisualStyleBackColor = true;
            this.rbValue.CheckedChanged += new System.EventHandler(this.rbValue_CheckedChanged);
            // 
            // pNode
            // 
            this.pNode.BackColor = System.Drawing.Color.Black;
            this.pNode.Controls.Add(this.rbNodes);
            this.pNode.Controls.Add(this.txtSource);
            this.pNode.Controls.Add(this.btnDelete);
            this.pNode.Controls.Add(this.btnEdit);
            this.pNode.Controls.Add(this.btnAdd);
            this.pNode.Controls.Add(this.nodes);
            this.pNode.Location = new System.Drawing.Point(4, 22);
            this.pNode.Name = "pNode";
            this.pNode.Padding = new System.Windows.Forms.Padding(3);
            this.pNode.Size = new System.Drawing.Size(392, 259);
            this.pNode.TabIndex = 1;
            this.pNode.Text = "Node";
            // 
            // rbNodes
            // 
            this.rbNodes.AutoSize = true;
            this.rbNodes.Location = new System.Drawing.Point(11, 225);
            this.rbNodes.Name = "rbNodes";
            this.rbNodes.Size = new System.Drawing.Size(85, 17);
            this.rbNodes.TabIndex = 7;
            this.rbNodes.Text = "select nodes";
            this.rbNodes.UseVisualStyleBackColor = true;
            this.rbNodes.CheckedChanged += new System.EventHandler(this.rbNodes_CheckedChanged);
            // 
            // txtSource
            // 
            this.txtSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSource.Location = new System.Drawing.Point(160, 6);
            this.txtSource.Multiline = true;
            this.txtSource.Name = "txtSource";
            this.txtSource.ReadOnly = true;
            this.txtSource.Size = new System.Drawing.Size(226, 204);
            this.txtSource.TabIndex = 7;
            this.txtSource.TabStop = false;
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
            // nodes
            // 
            this.nodes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.nodes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.nodes.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nodes.ForeColor = System.Drawing.Color.White;
            this.nodes.FormattingEnabled = true;
            this.nodes.IntegralHeight = false;
            this.nodes.Location = new System.Drawing.Point(6, 35);
            this.nodes.Name = "nodes";
            this.nodes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.nodes.Size = new System.Drawing.Size(148, 175);
            this.nodes.TabIndex = 1;
            this.nodes.TabStop = false;
            this.nodes.DoubleClick += new System.EventHandler(this.vars_DoubleClick);
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
            // pRef
            // 
            this.pRef.BackColor = System.Drawing.Color.Black;
            this.pRef.Controls.Add(this.rbRef);
            this.pRef.Controls.Add(this.refs);
            this.pRef.ForeColor = System.Drawing.Color.White;
            this.pRef.Location = new System.Drawing.Point(4, 22);
            this.pRef.Name = "pRef";
            this.pRef.Padding = new System.Windows.Forms.Padding(3);
            this.pRef.Size = new System.Drawing.Size(392, 259);
            this.pRef.TabIndex = 2;
            this.pRef.Text = "Reference";
            // 
            // refs
            // 
            this.refs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.refs.ForeColor = System.Drawing.Color.White;
            this.refs.FormattingEnabled = true;
            this.refs.Location = new System.Drawing.Point(7, 7);
            this.refs.Name = "refs";
            this.refs.Size = new System.Drawing.Size(379, 199);
            this.refs.TabIndex = 0;
            // 
            // rbRef
            // 
            this.rbRef.AutoSize = true;
            this.rbRef.Location = new System.Drawing.Point(11, 225);
            this.rbRef.Name = "rbRef";
            this.rbRef.Size = new System.Drawing.Size(101, 17);
            this.rbRef.TabIndex = 8;
            this.rbRef.Text = "select reference";
            this.rbRef.UseVisualStyleBackColor = true;
            this.rbRef.CheckedChanged += new System.EventHandler(this.rbRef_CheckedChanged);
            // 
            // pMethod
            // 
            this.pMethod.BackColor = System.Drawing.Color.Black;
            this.pMethod.Controls.Add(this.rbMethod);
            this.pMethod.Controls.Add(this.txtMethodName);
            this.pMethod.Controls.Add(this.lblMethodName);
            this.pMethod.Location = new System.Drawing.Point(4, 22);
            this.pMethod.Name = "pMethod";
            this.pMethod.Padding = new System.Windows.Forms.Padding(3);
            this.pMethod.Size = new System.Drawing.Size(392, 259);
            this.pMethod.TabIndex = 3;
            this.pMethod.Text = "Method";
            // 
            // rbMethod
            // 
            this.rbMethod.AutoSize = true;
            this.rbMethod.Location = new System.Drawing.Point(11, 225);
            this.rbMethod.Name = "rbMethod";
            this.rbMethod.Size = new System.Drawing.Size(91, 17);
            this.rbMethod.TabIndex = 11;
            this.rbMethod.Text = "select method";
            this.rbMethod.UseVisualStyleBackColor = true;
            this.rbMethod.CheckedChanged += new System.EventHandler(this.rbMethod_CheckedChanged);
            // 
            // txtMethodName
            // 
            this.txtMethodName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMethodName.Location = new System.Drawing.Point(85, 13);
            this.txtMethodName.Multiline = true;
            this.txtMethodName.Name = "txtMethodName";
            this.txtMethodName.Size = new System.Drawing.Size(301, 203);
            this.txtMethodName.TabIndex = 10;
            // 
            // lblMethodName
            // 
            this.lblMethodName.AutoSize = true;
            this.lblMethodName.Location = new System.Drawing.Point(7, 16);
            this.lblMethodName.Name = "lblMethodName";
            this.lblMethodName.Size = new System.Drawing.Size(72, 13);
            this.lblMethodName.TabIndex = 9;
            this.lblMethodName.Text = "Method name";
            // 
            // AccuEntry
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
            this.Name = "AccuEntry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Accu";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AccuEntry_FormClosing);
            this.Load += new System.EventHandler(this.AccuEntry_Load);
            this.tc.ResumeLayout(false);
            this.pValue.ResumeLayout(false);
            this.pValue.PerformLayout();
            this.pNode.ResumeLayout(false);
            this.pNode.PerformLayout();
            this.pRef.ResumeLayout(false);
            this.pRef.PerformLayout();
            this.pMethod.ResumeLayout(false);
            this.pMethod.PerformLayout();
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
        private System.Windows.Forms.TabPage pNode;
        private System.Windows.Forms.ListBox nodes;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.CheckBox ckIndented;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.RadioButton rbValue;
        private System.Windows.Forms.RadioButton rbNodes;
        private System.Windows.Forms.TabPage pRef;
        private System.Windows.Forms.ListBox refs;
        private System.Windows.Forms.RadioButton rbRef;
        private System.Windows.Forms.TabPage pMethod;
        private System.Windows.Forms.RadioButton rbMethod;
        private System.Windows.Forms.TextBox txtMethodName;
        private System.Windows.Forms.Label lblMethodName;
    }
}