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
        /// Undo/redo memory
        /// </summary>
        private List<MemoryStream> stored;

        /// <summary>
        /// undo position to redo
        /// </summary>
        private int undoPos;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Editor()
        {
            InitializeComponent();
            this.stored = new List<MemoryStream>();
            this.sfd = new SaveFileDialog();
            this.ofd = new OpenFileDialog();
            ofd.InitialDirectory = sfd.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            this.appNewItem_Click(this, new EventArgs());
        }

        /// <summary>
        /// Add for undo feature
        /// </summary>
        private void AddUndo()
        {
            MemoryStream stream = new MemoryStream();
            PrinterObject.Save(po, stream);
            this.AddUndo(stream);
        }

        /// <summary>
        /// Add to undo feature
        /// </summary>
        /// <param name="stream">stream to keep</param>
        private void AddUndo(MemoryStream stream)
        {
            if (this.undoPos < this.stored.Count)
            {
                this.stored[this.undoPos] = stream;
            }
            else
            {
                this.stored.Add(stream);
            }
            ++this.undoPos;
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
            this.stored.ForEach(x => { x.Close(); x.Dispose(); });
            this.stored.Clear();
            this.undoPos = 0;
            vars.BeginUpdate();
            vars.Items.Clear();
            FunLab.FillVars(vars, po);
            vars.EndUpdate();
            datas.BeginUpdate();
            datas.Items.Clear();
            FunLab.FillData(datas, po);
            datas.EndUpdate();
            txtSource.Text = po.ToString();
            this.AddUndo();
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
                    this.stored.ForEach(x => { x.Close(); x.Dispose(); });
                    this.stored.Clear();
                    this.undoPos = 0;
                    vars.BeginUpdate();
                    vars.Items.Clear();
                    FunLab.FillVars(vars, po);
                    vars.EndUpdate();
                    datas.BeginUpdate();
                    datas.Items.Clear();
                    FunLab.FillData(datas, po);
                    datas.EndUpdate();
                    txtSource.Text = po.ToString();
                    this.AddUndo();
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
                this.AddUndo();
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
                this.AddUndo();
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + Path.GetFileName(sfd.FileName) + " *";
            }
        }

        private void varsModifyItem_Click(object sender, EventArgs e)
        {
            if (FunLab.EditVariable(vars, po))
            {
                this.AddUndo();
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + Path.GetFileName(sfd.FileName) + " *";
            }
        }

        private void varsRemoveItem_Click(object sender, EventArgs e)
        {
            if (FunLab.DeleteVariables(vars, po))
            {
                this.AddUndo();
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
                this.AddUndo();
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + Path.GetFileName(sfd.FileName) + " *";
            }
        }

        private void datasRemoveItem_Click(object sender, EventArgs e)
        {
            if (FunLab.DeleteData(datas, po))
            {
                this.AddUndo();
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

        private void datasInsertBeforeItem_Click(object sender, EventArgs e)
        {
            if (FunLab.InsertBefore(datas, po))
            {
                this.AddUndo();
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + Path.GetFileName(sfd.FileName) + " *";
            }
        }

        private void datasInsertAfterItem_Click(object sender, EventArgs e)
        {
            if (FunLab.InsertAfter(datas, po))
            {
                this.AddUndo();
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + Path.GetFileName(sfd.FileName) + " *";
            }
        }

        private void editCopyVarItem_Click(object sender, EventArgs e)
        {
            if (FunLab.CopyVariables(vars, po))
            {
                this.AddUndo();
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + Path.GetFileName(sfd.FileName) + " *";
            }
        }

        private void editCopyDataItem_Click(object sender, EventArgs e)
        {
            if (FunLab.CopyData(datas, po))
            {
                this.AddUndo();
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + Path.GetFileName(sfd.FileName) + " *";
            }
        }

        private void editUndoItem_Click(object sender, EventArgs e)
        {
            if (this.undoPos > 1)
            {
                --this.undoPos;
                MemoryStream mem = this.stored[this.undoPos - 1];
                mem.Seek(0, SeekOrigin.Begin);

                PrinterObject previous = PrinterObject.Load(mem);

                po = previous as PrinterObject;
                vars.BeginUpdate();
                vars.Items.Clear();
                FunLab.FillVars(vars, po);
                vars.EndUpdate();
                datas.BeginUpdate();
                datas.Items.Clear();
                FunLab.FillData(datas, po);
                datas.EndUpdate();
                txtSource.Text = po.ToString();
                FunLab.IsDirty = true;
                this.Text = "Editor - " + Path.GetFileName(sfd.FileName) + " *";
            }
        }

        private void editRedoItem_Click(object sender, EventArgs e)
        {
            if (this.undoPos < this.stored.Count)
            {
                MemoryStream mem = this.stored[this.undoPos];
                ++this.undoPos;
                mem.Seek(0, SeekOrigin.Begin);
                PrinterObject previous = PrinterObject.Load(mem);

                po = previous as PrinterObject;
                vars.BeginUpdate();
                vars.Items.Clear();
                FunLab.FillVars(vars, po);
                vars.EndUpdate();
                datas.BeginUpdate();
                datas.Items.Clear();
                FunLab.FillData(datas, po);
                datas.EndUpdate();
                txtSource.Text = po.ToString();
                FunLab.IsDirty = true;
                this.Text = "Editor - " + Path.GetFileName(sfd.FileName) + " *";
            }
        }
    }
}
