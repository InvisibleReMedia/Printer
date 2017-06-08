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
    public partial class Editor : Form
    {

        /// <summary>
        /// Dialog to save
        /// </summary>
        private SaveFileDialog sfd;
        /// <summary>
        /// Dialog to load
        /// </summary>
        private OpenFileDialog ofd;

        /// <summary>
        /// Printer object
        /// </summary>
        private PrinterObject po;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Editor()
        {
            InitializeComponent();
            this.sfd = new SaveFileDialog();
            this.ofd = new OpenFileDialog();
        }

        /// <summary>
        /// New object
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void appNewItem_Click(object sender, EventArgs e)
        {
            if (FunLab.IsDirty)
            {
                DialogResult dr = MessageBox.Show("Save current ?", "Not saved", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    FunLab.Save(sfd, po);
                }
            }
            FunLab.New(ref po);
            vars.Items.Clear();
            FunLab.FillVars(vars, po);
        }

        private void appLoadItem_Click(object sender, EventArgs e)
        {
            if (FunLab.IsDirty)
            {
                DialogResult dr = MessageBox.Show("Save current ?", "Not saved", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    FunLab.Save(sfd, po);
                }
            }
            if (FunLab.Load(ofd, ref po))
            {
                vars.Items.Clear();
                datas.Items.Clear();
                FunLab.FillVars(vars, po);
                FunLab.FillData(datas, po);
                txtSource.Text = po.ToString();
            }
        }

        private void appSaveItem_Click(object sender, EventArgs e)
        {
            FunLab.Save(sfd, po);
        }

        private void appSaveAsItem_Click(object sender, EventArgs e)
        {
            FunLab.SaveAs(sfd, po);
        }
    }
}
