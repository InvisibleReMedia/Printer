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
    public partial class Data : Form
    {
        public Data()
        {
            InitializeComponent();
        }

        private void rbVariable_CheckedChanged(object sender, EventArgs e)
        {
            this.vars.Enabled = true;
            this.txtConst.Enabled = false;
        }

        private void rbConst_CheckedChanged(object sender, EventArgs e)
        {
            this.vars.Enabled = false;
            this.txtConst.Enabled = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Data_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
                e.Cancel = !((this.rbVariable.Checked && !String.IsNullOrEmpty(this.vars.Text)) || (this.rbConst.Checked && !String.IsNullOrEmpty(this.txtConst.Text)));
            else
                e.Cancel = false;
        }
    }
}
