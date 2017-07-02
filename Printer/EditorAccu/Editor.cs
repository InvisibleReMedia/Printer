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

namespace EditorAccu
{
    /// <summary>
    /// Editor form (main window)
    /// </summary>
    public partial class Editor : Form
    {

        /// <summary>
        /// Path
        /// </summary>
        private string path;
        /// <summary>
        /// File name
        /// </summary>
        private string fileName;

        /// <summary>
        /// Configuration file name
        /// </summary>
        private string confFileName;

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
        /// exec switch
        /// </summary>
        private bool exec;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Editor()
        {
            InitializeComponent();
            this.stored = new List<MemoryStream>();
            this.path = PrinterObject.PrinterDirectory;
            this.fileName = "code.prt";
            this.confFileName = "code.confprt";
            this.appNewItem_Click(this, new EventArgs());
            this.exec = true;
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
                    FunLab.Save(ref this.path, ref this.fileName, po);
                }
            }
            FunLab.New(ref po);
            this.fileName = "code.prt";
            this.Text = "Editor - " + this.fileName;
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
                    FunLab.Save(ref this.path, ref this.fileName, po);
                }
            }
            try
            {
                if (FunLab.Load(ref this.path, ref this.fileName, ref po))
                {
                    this.Text = "Editor - " + this.fileName;
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
            }
            catch (Exception ex)
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
            if (FunLab.Save(ref this.path, ref this.fileName, po))
            {
                this.Text = "Editor - " + this.fileName;
            }
        }

        /// <summary>
        /// Save into an another file
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void appSaveAsItem_Click(object sender, EventArgs e)
        {
            if (FunLab.SaveAs(ref this.path, ref this.fileName, po))
            {
                this.Text = "Editor - " + this.fileName;
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
                this.Text = "Editor - " + this.fileName + " *";
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
                this.Text = "Editor - " + this.fileName + " *";
            }
        }

        /// <summary>
        /// Modify variable
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void varsModifyItem_Click(object sender, EventArgs e)
        {
            if (FunLab.EditVariable(vars, po))
            {
                this.AddUndo();
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + this.fileName + " *";
            }
        }

        /// <summary>
        /// Remove variable
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void varsRemoveItem_Click(object sender, EventArgs e)
        {
            if (FunLab.DeleteVariables(vars, po))
            {
                this.AddUndo();
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + this.fileName + " *";
            }
        }

        /// <summary>
        /// When closing (avoid not saved)
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void Editor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FunLab.IsDirty)
            {
                DialogResult dr = MessageBox.Show("Save current ?", "Not saved", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    FunLab.Save(ref this.path, ref this.fileName, po);
                }
            }
        }

        /// <summary>
        /// Modify data
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void datasModifyItem_Click(object sender, EventArgs e)
        {
            if (FunLab.EditData(datas, po))
            {
                this.AddUndo();
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + this.fileName + " *";
            }
        }

        /// <summary>
        /// Suppress data
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void datasRemoveItem_Click(object sender, EventArgs e)
        {
            if (FunLab.DeleteData(datas, po))
            {
                this.AddUndo();
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + this.fileName + " *";
            }
        }

        /// <summary>
        /// Dbl click on vars
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void vars_DoubleClick(object sender, EventArgs e)
        {
            this.varsModifyItem_Click(sender, e);
        }

        /// <summary>
        /// Dbl click on datas
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void datas_DoubleClick(object sender, EventArgs e)
        {
            this.datasModifyItem_Click(sender, e);
        }

        /// <summary>
        /// Insert before
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void datasInsertBeforeItem_Click(object sender, EventArgs e)
        {
            if (FunLab.InsertBefore(datas, po))
            {
                this.AddUndo();
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + this.fileName + " *";
            }
        }

        /// <summary>
        /// Insert after
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void datasInsertAfterItem_Click(object sender, EventArgs e)
        {
            if (FunLab.InsertAfter(datas, po))
            {
                this.AddUndo();
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + this.fileName + " *";
            }
        }

        /// <summary>
        /// Copy variable
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void editCopyVarItem_Click(object sender, EventArgs e)
        {
            if (FunLab.CopyVariables(vars, po))
            {
                this.AddUndo();
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + this.fileName + " *";
            }
        }

        /// <summary>
        /// Copy data
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void editCopyDataItem_Click(object sender, EventArgs e)
        {
            if (FunLab.CopyData(datas, po))
            {
                this.AddUndo();
                txtSource.Text = po.ToString();
                this.Text = "Editor - " + this.fileName + " *";
            }
        }

        /// <summary>
        /// Undo
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
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
                this.Text = "Editor - " + this.fileName + " *";
            }
        }

        /// <summary>
        /// Redo
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
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
                this.Text = "Editor - " + this.fileName + " *";
            }
        }

        /// <summary>
        /// Open config
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void configParamsItem_Click(object sender, EventArgs e)
        {
            Config c = new Config();
            c.Defines = po.Configuration;
            DialogResult dr = c.ShowDialog();
            if (dr == DialogResult.OK)
            {
                if (FunLab.IsDirty)
                {
                    po.Configuration = c.Defines.Clone() as Configuration;
                    this.Text = "Editor - " + this.fileName + " *";
                    txtSource.Text = po.ToString();
                    this.AddUndo();
                }
            }
            else
            {
                FunLab.IsDirty = false;
            }
        }

        /// <summary>
        /// When execute clicked
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void executeItem_Click(object sender, EventArgs e)
        {
            Control c = sender as Control;
            if (this.exec)
            {
                this.txtSource.Text = po.Execute();
                this.exec = false;
                this.executeItem.Text = "Source";
            }
            else
            {
                this.txtSource.Text = po.ToString();
                this.exec = true;
                this.executeItem.Text = "Execute";
            }
        }

        /// <summary>
        /// When config imports
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void configImportItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = false;
            ofd.Filter = "Configuration Printer (*.confprt)|*.confprt";
            ofd.DefaultExt = "confprt";
            ofd.InitialDirectory = this.path;
            ofd.FileName = this.confFileName;
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.path = Path.GetDirectoryName(ofd.FileName);
                this.confFileName = Path.GetFileName(ofd.FileName);
                Configuration conf = Configuration.Load(Path.Combine(this.path, this.confFileName));
                po.ImportConfiguration(conf);
                FunLab.IsDirty = true;
                this.AddUndo();
                this.Text = "Editor - " + this.fileName + " *";
                txtSource.Text = po.ToString();
            }
        }

        /// <summary>
        /// When config save
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">arg</param>
        private void configSaveItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = false;
            sfd.Filter = "Configuration Printer (*.confprt)|*.confprt";
            sfd.DefaultExt = "confprt";
            sfd.InitialDirectory = this.path;
            sfd.FileName = this.confFileName;
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.path = Path.GetDirectoryName(sfd.FileName);
                this.confFileName = Path.GetFileName(sfd.FileName);
                Configuration.Save(po.Configuration, Path.Combine(this.path, this.confFileName));
            }
        }
    }
}
