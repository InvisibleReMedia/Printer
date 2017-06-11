using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Printer;

namespace Editor
{
    /// <summary>
    /// Variable form
    /// </summary>
    public partial class Variable : Form
    {
        private PrinterVariable pv;
        private OpenFileDialog ofd;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Variable()
        {
            InitializeComponent();
            this.ofd = new OpenFileDialog();
            this.pv = new PrinterVariable();
        }

        /// <summary>
        /// Gets the include tab page
        /// </summary>
        public TabPage IncludePage
        {
            get { return tc.TabPages[1]; }
        }

        /// <summary>
        /// Gets the value tab page
        /// </summary>
        public TabPage ValuePage
        {
            get { return tc.TabPages[0]; }
        }

        /// <summary>
        /// Gets or sets indented switch
        /// </summary>
        public bool IsIndented
        {
            get { return this.ckIndented.Checked; }
            set { this.ckIndented.Checked = value; }
        }

        /// <summary>
        /// When ok button clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">arg</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// When cancel button clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">arg</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// When asking to close window
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">arg</param>
        private void Variable_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
                if (this.rbInclude.Checked)
                {
                    e.Cancel = String.IsNullOrEmpty(this.txtName.Text) || String.IsNullOrEmpty(this.txtFile.Text);
                } else
                {
                    e.Cancel = String.IsNullOrEmpty(this.txtName.Text) || String.IsNullOrEmpty(this.txtValue.Text);
                }
            else
                e.Cancel = false;
        }

        /// <summary>
        /// When add button clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">arg</param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Hide();
            FunLab.AddNewVariable(vars, pv);
            this.Show();
        }

        /// <summary>
        /// When edit button clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">arg</param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            this.Hide();
            FunLab.EditVariable(vars, pv);
            this.Show();
        }

        /// <summary>
        /// When delete button clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">arg</param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.Hide();
            FunLab.DeleteVariables(vars, pv);
            this.Show();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult dr = this.ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.txtFile.Text = this.ofd.SafeFileName;
            }
        }

        private void rbInclude_CheckedChanged(object sender, EventArgs e)
        {
            this.BindingContext[this.rbValue].SuspendBinding();
            if (rbInclude.Checked)
            {
                rbValue.Checked = false;
            }
            else
            {
                rbValue.Checked = true;
            }
            this.BindingContext[this.rbValue].ResumeBinding();
        }

        private void rbValue_CheckedChanged(object sender, EventArgs e)
        {
            this.BindingContext[this.rbValue].SuspendBinding();
            if (rbValue.Checked)
            {
                rbInclude.Checked = false;
            }
            else
            {
                rbInclude.Checked = true;
            }
            this.BindingContext[this.rbValue].ResumeBinding();
        }

        private void Variable_Load(object sender, EventArgs e)
        {
            FunLab.FillVars(vars, pv);
        }

        private void vars_DoubleClick(object sender, EventArgs e)
        {
            btnEdit_Click(sender, e);
        }
    }
}
