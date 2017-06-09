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
using System.IO;

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
            ofd.InitialDirectory = sfd.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            this.appNewItem_Click(this, new EventArgs());
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
            ofd.FileName = sfd.FileName = "code.prt";
            this.Text = "Editor - " + ofd.SafeFileName;
            vars.Items.Clear();
            datas.Items.Clear();
            FunLab.FillVars(vars, po);
            FunLab.FillData(datas, po);
            txtSource.Text = po.ToString();
        }

        /// <summary>
        /// Load a file
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
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
            try
            {
                if (FunLab.Load(ofd, ref po))
                {
                    this.Text = "Editor - " + ofd.SafeFileName;
                    sfd.FileName = ofd.SafeFileName;
                    vars.Items.Clear();
                    datas.Items.Clear();
                    vars.BeginUpdate();
                    FunLab.FillVars(vars, po);
                    vars.EndUpdate();
                    datas.BeginUpdate();
                    FunLab.FillData(datas, po);
                    datas.EndUpdate();
                    txtSource.Text = po.ToString();
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Not loaded", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Save into a file
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void appSaveItem_Click(object sender, EventArgs e)
        {
            if (FunLab.Save(sfd, po))
            {
                this.Text = "Editor - " + Path.GetFileName(sfd.FileName);
                ofd.FileName = Path.GetFileName(sfd.FileName);
            }
        }

        /// <summary>
        /// Save into an another file
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void appSaveAsItem_Click(object sender, EventArgs e)
        {
            if (FunLab.SaveAs(sfd, po))
            {
                this.Text = "Editor - " + Path.GetFileName(sfd.FileName);
                ofd.FileName = Path.GetFileName(sfd.FileName);
            }
        }

        /// <summary>
        /// Quit
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void appQuitItem_Click(object sender, EventArgs e)
        {
            if (FunLab.IsDirty)
            {
                DialogResult dr = MessageBox.Show("Save current ?", "Not saved", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    FunLab.Save(sfd, po);
                }
            }
            this.Close();
        }

        /// <summary>
        /// Add variable
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void varsAddItem_Click(object sender, EventArgs e)
        {
            if (FunLab.AddNewVariable(vars, po))
            {
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + Path.GetFileName(sfd.FileName) + " *";
            }
        }

        /// <summary>
        /// Add data
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void datasAddItem_Click(object sender, EventArgs e)
        {
            if (FunLab.AddNewData(datas, po))
            {
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + Path.GetFileName(sfd.FileName) + " *";
            }
        }

        private void varsModifyItem_Click(object sender, EventArgs e)
        {
            if (FunLab.EditVariable(vars, po))
            {
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + Path.GetFileName(sfd.FileName) + " *";
            }
        }

        private void varsRemoveItem_Click(object sender, EventArgs e)
        {
            if (FunLab.DeleteVariables(vars, po))
            {
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + Path.GetFileName(sfd.FileName) + " *";
            }
        }

        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FunLab.IsDirty)
            {
                DialogResult dr = MessageBox.Show("Save current ?", "Not saved", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    FunLab.Save(sfd, po);
                }
            }
        }

        private void datasModifyItem_Click(object sender, EventArgs e)
        {
            if (FunLab.EditData(datas, po))
            {
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + Path.GetFileName(sfd.FileName) + " *";
            }
        }

        private void datasRemoveItem_Click(object sender, EventArgs e)
        {
            if (FunLab.DeleteData(datas, po))
            {
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + Path.GetFileName(sfd.FileName) + " *";
            }
        }

        private void vars_DoubleClick(object sender, EventArgs e)
        {
            this.varsModifyItem_Click(sender, e);
        }

        private void datas_DoubleClick(object sender, EventArgs e)
        {
            this.datasModifyItem_Click(sender, e);
        }
    }
}
