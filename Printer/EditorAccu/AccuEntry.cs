using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EditorAccu
{
    /// <summary>
    /// Variable form
    /// </summary>
    public partial class AccuEntry : Form
    {

        private Accu.Accu a;

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccuEntry()
        {
            InitializeComponent();
            this.a = new Accu.Accu(false, false, false, "name", "empty");
        }

        /// <summary>
        /// Gets the underlying data
        /// </summary>
        public Accu.Accu Entry
        {
            get
            {
                return this.a;
            }
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
        /// When add button clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">arg</param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.Hide();
            FunLab.AddNewAccu(nodes, a);
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
            FunLab.EditAccu(nodes, a);
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
            FunLab.DeleteAccu(nodes, a);
            this.Show();
        }

        /// <summary>
        /// When radio button value clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">arg</param>
        private void rbValue_CheckedChanged(object sender, EventArgs e)
        {
            this.BindingContext[this.rbNodes].SuspendBinding();
            this.BindingContext[this.rbRef].SuspendBinding();
            this.BindingContext[this.rbMethod].SuspendBinding();
            if (rbValue.Checked)
            {
                rbNodes.Checked = false;
                rbRef.Checked = false;
                rbMethod.Checked = false;
            }
            this.BindingContext[this.rbNodes].ResumeBinding();
            this.BindingContext[this.rbRef].ResumeBinding();
            this.BindingContext[this.rbMethod].ResumeBinding();
        }

        /// <summary>
        /// When list box double clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">arg</param>
        private void vars_DoubleClick(object sender, EventArgs e)
        {
            btnEdit_Click(sender, e);
        }

        /// <summary>
        /// When asking to close window
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">arg</param>
        private void AccuEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
                if (this.rbNodes.Checked)
                {
                    e.Cancel = String.IsNullOrEmpty(this.txtName.Text) || String.IsNullOrEmpty(this.txtFile.Text);
                }
                else
                {
                    e.Cancel = String.IsNullOrEmpty(this.txtName.Text) || String.IsNullOrEmpty(this.txtValue.Text);
                }
            else
                e.Cancel = false;
        }

        /// <summary>
        /// When radio button nodes clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">arg</param>
        private void rbNodes_CheckedChanged(object sender, EventArgs e)
        {
            this.BindingContext[this.rbValue].SuspendBinding();
            this.BindingContext[this.rbRef].SuspendBinding();
            this.BindingContext[this.rbMethod].SuspendBinding();
            if (rbNodes.Checked)
            {
                rbValue.Checked = false;
                rbRef.Checked = false;
                rbMethod.Checked = false;
            }
            this.BindingContext[this.rbValue].ResumeBinding();
            this.BindingContext[this.rbRef].ResumeBinding();
            this.BindingContext[this.rbMethod].ResumeBinding();
        }

        /// <summary>
        /// When forms loaded
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">arg</param>
        private void AccuEntry_Load(object sender, EventArgs e)
        {
            FunLab.FillAccu(this.nodes, this.a);
        }

        /// <summary>
        /// When radio button ref clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">arg</param>
        private void rbRef_CheckedChanged(object sender, EventArgs e)
        {
            this.BindingContext[this.rbValue].SuspendBinding();
            this.BindingContext[this.rbNodes].SuspendBinding();
            this.BindingContext[this.rbMethod].SuspendBinding();
            if (rbRef.Checked)
            {
                rbValue.Checked = false;
                rbNodes.Checked = false;
                rbMethod.Checked = false;
            }
            this.BindingContext[this.rbValue].ResumeBinding();
            this.BindingContext[this.rbNodes].ResumeBinding();
            this.BindingContext[this.rbMethod].ResumeBinding();
        }

        /// <summary>
        /// When radio button method clicked
        /// </summary>
        /// <param name="sender">object</param>
        /// <param name="e">arg</param>
        private void rbMethod_CheckedChanged(object sender, EventArgs e)
        {
            this.BindingContext[this.rbValue].SuspendBinding();
            this.BindingContext[this.rbNodes].SuspendBinding();
            this.BindingContext[this.rbRef].SuspendBinding();
            if (rbMethod.Checked)
            {
                rbValue.Checked = false;
                rbNodes.Checked = false;
                rbRef.Checked = false;
            }
            this.BindingContext[this.rbValue].ResumeBinding();
            this.BindingContext[this.rbNodes].ResumeBinding();
            this.BindingContext[this.rbRef].ResumeBinding();
        }
    }
}
