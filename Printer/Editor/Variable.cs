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
    public partial class Variable : Form
    {
        public Variable()
        {
            InitializeComponent();
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

        private void Variable_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = String.IsNullOrEmpty(this.txtName.Text) || String.IsNullOrEmpty(this.txtValue.Text);
        }
    }
}
