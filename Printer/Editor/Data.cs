using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Editor
{
    /// <summary>
    /// Data form
    /// </summary>
    public partial class Data : Form
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public Data()
        {
            InitializeComponent();
        }

        /// <summary>
        /// rb var changed
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void rbVariable_CheckedChanged(object sender, EventArgs e)
        {
            this.vars.Enabled = true;
            this.txtConst.Enabled = false;
        }

        /// <summary>
        /// rb const changed
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void rbConst_CheckedChanged(object sender, EventArgs e)
        {
            this.vars.Enabled = false;
            this.txtConst.Enabled = true;
        }

        /// <summary>
        /// When ok clicked
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// When cancel clicked
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// When closing
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void Data_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
                e.Cancel = !((this.rbVariable.Checked && !String.IsNullOrEmpty(this.vars.Text)) || (this.rbConst.Checked && !String.IsNullOrEmpty(this.txtConst.Text)));
            else
                e.Cancel = false;
        }
    }
}
