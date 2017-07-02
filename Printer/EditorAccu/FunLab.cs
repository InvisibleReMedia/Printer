using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Printer;
using System.IO;

namespace EditorAccu
{
    /// <summary>
    /// Laboratory of functions
    /// </summary>
    static class FunLab
    {
        /// <summary>
        /// Has modified switch
        /// </summary>
        private static bool hasModified;
        /// <summary>
        /// When a file has been loaded
        /// </summary>
        private static bool alreadyOpen;

        /// <summary>
        /// Get the file name without his extension or multidotted extension
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>file name without extension</returns>
        public static string GetFileNameWithoutExtension(string fileName)
        {
            try
            {
                int pos = fileName.IndexOf('.');
                return fileName.Substring(0, pos);
            } catch
            {
                return fileName;
            }
        }

        /// <summary>
        /// Load from file
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="fileName">file name</param>
        /// <param name="accu">accu object</param>
        /// <returns>true if loaded</returns>
        public static bool Load(ref string path, ref string fileName, ref Accu.Accu accu)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = false;
            ofd.Filter = "Accu (*.acc)|*.acc";
            ofd.DefaultExt = "acc";
            ofd.InitialDirectory = path;
            ofd.FileName = fileName;
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                path = Path.GetDirectoryName(ofd.FileName);
                fileName = Path.GetFileName(ofd.FileName);
                accu = Accu.Accu.Load(Path.Combine(path, fileName));
                alreadyOpen = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Save to file
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="fileName">file name</param>
        /// <param name="accu">accu object</param>
        /// <returns>true if saved</returns>
        public static bool Save(ref string path, ref string fileName, Accu.Accu accu)
        {
            if (alreadyOpen)
            {
                Accu.Accu.Save(accu, Path.Combine(path, fileName));
                hasModified = false;
                return true;
            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.AddExtension = false;
                sfd.Filter = "Accu (*.acc)|*.acc";
                sfd.DefaultExt = "acc";
                sfd.InitialDirectory = path;
                sfd.FileName = fileName;
                DialogResult dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    path = Path.GetDirectoryName(sfd.FileName);
                    fileName = Path.GetFileName(sfd.FileName);
                    Accu.Accu.Save(accu, Path.Combine(path, fileName));
                    alreadyOpen = true;
                    hasModified = false;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Save to an another file
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="fileName">file name</param>
        /// <param name="accu">accu object</param>
        /// <returns>true if saved</returns>
        public static bool SaveAs(ref string path, ref string fileName, Accu.Accu accu)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = false;
            sfd.Filter = "Accu (*.acc)|*.acc";
            sfd.DefaultExt = "acc";
            sfd.InitialDirectory = path;
            sfd.FileName = fileName;
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                path = Path.GetDirectoryName(sfd.FileName);
                fileName = Path.GetFileName(sfd.FileName);
                Accu.Accu.Save(accu, Path.Combine(path, fileName));
                alreadyOpen = true;
                hasModified = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Create a new printer object
        /// </summary>
        /// <param name="accu">accu object</param>
        public static void New(ref Accu.Accu accu)
        {
            accu = new Accu.Accu(false, false, false, "root", null);
            alreadyOpen = false;
        }


        /// <summary>
        /// Returns true of false if data has modified
        /// </summary>
        public static bool IsDirty
        {
            get
            {
                return hasModified;
            }
            set
            {
                hasModified = value;
            }
        }

        /// <summary>
        /// Add a new variable
        /// </summary>
        /// <param name="list">accu list</param>
        /// <param name="root">top-level accu object</param>
        /// <returns>added variable</returns>
        public static bool AddNewAccu(ListBox list, Accu.Accu root)
        {
            AccuEntry f = new AccuEntry();
            f.Controls["txtName"].Text = f.Entry.Name;
            if (f.Entry)
            {
                (var.IncludePage.Controls["rbInclude"] as RadioButton).Checked = true;
                var.IncludePage.Controls["txtFile"].Text = pv.Value;
                FillVars(var.IncludePage.Controls["vars"] as ListBox, pv);
                var.IncludePage.Controls["txtSource"].Text = pv.ToString();
            }
            else
            {
                (var.ValuePage.Controls["rbValue"] as RadioButton).Checked = true;
                var.ValuePage.Controls["txtValue"].Text = pv.Value;
            }
            DialogResult dr = var.ShowDialog();
            if (dr == DialogResult.OK)
            {
                if (!String.IsNullOrEmpty(pv.Name) && pv.Name != var.Controls["txtName"].Text)
                {
                    po.DeleteVariable(pv.Name);
                    list.Items.Remove(pv.Name);
                }
                pv.Indent = var.IsIndented;
                pv.Include = (var.IncludePage.Controls["rbInclude"] as RadioButton).Checked;
                if (pv.Include)
                {
                    pv.Value = var.IncludePage.Controls["txtFile"].Text;
                    foreach (PrinterVariable subpv in (var.IncludePage.Controls["vars"] as ListBox).Items)
                    {
                        pv.AddVariable(subpv.Name, subpv);
                    }
                }
                else
                {
                    pv.Value = var.ValuePage.Controls["txtValue"].Text;
                }
                pv.Name = var.Controls["txtName"].Text;
                if (po.ExistTestVariable(pv.Name))
                {
                    po.EditVariable(pv.Name, pv);
                    for (int index = 0; index < list.Items.Count; ++index)
                    {
                        if ((list.Items[index] as PrinterVariable).Name == pv.Name)
                        {
                            list.Items.RemoveAt(index);
                            list.Items.Insert(index, pv);
                            break;
                        }
                    }
                }
                else
                {
                    po.AddVariable(pv.Name, pv);
                    list.Items.Add(pv);
                }
                list.Refresh();
                hasModified = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Edit a variable
        /// </summary>
        /// <param name="list">variable list</param>
        /// <param name="po">variable object</param>
        /// <returns>added variable</returns>
        public static bool EditVariable(ListBox list, PrinterVariable po)
        {
            if (list.SelectedIndices.Count == 1)
            {
                PrinterVariable pv = list.SelectedItems[0] as PrinterVariable;
                Variable var = new Variable();
                var.IsIndented = pv.Indent;
                var.Controls["txtName"].Text = pv.Name;
                if (pv.Include)
                {
                    (var.IncludePage.Controls["rbInclude"] as RadioButton).Checked = true;
                    var.IncludePage.Controls["txtFile"].Text = pv.Value;
                    FillVars(var.IncludePage.Controls["vars"] as ListBox, pv);
                    var.IncludePage.Controls["txtSource"].Text = pv.ToString();
                }
                else
                {
                    (var.ValuePage.Controls["rbValue"] as RadioButton).Checked = true;
                    var.ValuePage.Controls["txtValue"].Text = pv.Value;
                }
                DialogResult dr = var.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    if (!String.IsNullOrEmpty(pv.Name) && pv.Name != var.Controls["txtName"].Text)
                    {
                        po.DeleteVariable(pv.Name);
                        list.Items.RemoveAt(list.SelectedIndices[0]);
                    }
                    pv.Indent = var.IsIndented;
                    pv.Include = (var.IncludePage.Controls["rbInclude"] as RadioButton).Checked;
                    if (pv.Include)
                    {
                        pv.Value = var.IncludePage.Controls["txtFile"].Text;
                        foreach (PrinterVariable subpv in (var.IncludePage.Controls["vars"] as ListBox).Items)
                        {
                            pv.AddVariable(subpv.Name, subpv);
                        }
                    }
                    else
                    {
                        pv.Value = var.ValuePage.Controls["txtValue"].Text;
                    }
                    pv.Name = var.Controls["txtName"].Text;
                    if (po.ExistTestVariable(pv.Name))
                    {
                        po.EditVariable(pv.Name, pv);
                        for (int index = 0; index < list.Items.Count; ++index)
                        {
                            if ((list.Items[index] as PrinterVariable).Name == pv.Name)
                            {
                                list.Items.RemoveAt(index);
                                list.Items.Insert(index, pv);
                                list.SelectedIndices.Add(index);
                                break;
                            }
                        }
                    }
                    else
                    {
                        po.AddVariable(pv.Name, pv);
                        int pos = list.Items.Add(pv);
                        list.SelectedIndices.Add(pos);
                    }
                    list.Refresh();
                    hasModified = true;
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// Edit a variable
        /// </summary>
        /// <param name="list">variable list</param>
        /// <param name="po">printer object</param>
        /// <returns>added variable</returns>
        public static bool EditVariable(ListBox list, PrinterObject po)
        {
            if (list.SelectedIndices.Count == 1)
            {
                PrinterVariable pv = list.SelectedItems[0] as PrinterVariable;
                Variable var = new Variable();
                var.IsIndented = pv.Indent;
                var.Controls["txtName"].Text = pv.Name;
                if (pv.Include)
                {
                    (var.IncludePage.Controls["rbInclude"] as RadioButton).Checked = true;
                    var.IncludePage.Controls["txtFile"].Text = pv.Value;
                    FillVars(var.IncludePage.Controls["vars"] as ListBox, pv);
                    var.IncludePage.Controls["txtSource"].Text = pv.ToString();
                }
                else
                {
                    (var.ValuePage.Controls["rbValue"] as RadioButton).Checked = true;
                    var.ValuePage.Controls["txtValue"].Text = pv.Value;
                }
                DialogResult dr = var.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    if (!String.IsNullOrEmpty(pv.Name) && pv.Name != var.Controls["txtName"].Text)
                    {
                        po.DeleteVariable(pv.Name);
                        list.Items.RemoveAt(list.SelectedIndices[0]);
                    }
                    pv.Indent = var.IsIndented;
                    pv.Include = (var.IncludePage.Controls["rbInclude"] as RadioButton).Checked;
                    if (pv.Include)
                    {
                        pv.Value = var.IncludePage.Controls["txtFile"].Text;
                        foreach (PrinterVariable subpv in (var.IncludePage.Controls["vars"] as ListBox).Items)
                        {
                            pv.AddVariable(subpv.Name, subpv);
                        }
                    }
                    else
                    {
                        pv.Value = var.ValuePage.Controls["txtValue"].Text;
                    }
                    pv.Name = var.Controls["txtName"].Text;
                    if (po.ExistTestVariable(pv.Name))
                    {
                        po.EditVariable(pv.Name, pv);
                        for (int index = 0; index < list.Items.Count; ++index)
                        {
                            if ((list.Items[index] as PrinterVariable).Name == pv.Name)
                            {
                                list.Items.RemoveAt(index);
                                list.Items.Insert(index, pv);
                                list.SelectedIndices.Add(index);
                                break;
                            }
                        }
                    }
                    else
                    {
                        po.AddVariable(pv.Name, pv);
                        int pos = list.Items.Add(pv);
                        list.SelectedIndices.Add(pos);
                    }
                    list.Refresh();
                    hasModified = true;
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// Copy variables
        /// </summary>
        /// <param name="list">variable list</param>
        /// <param name="po">printer object</param>
        /// <returns>if at least one item removed</returns>
        public static bool CopyVariables(ListBox list, PrinterObject po)
        {
            bool atLeastOne = false;
            for(int index = 0; index < list.SelectedIndices.Count; ++index)
            {
                PrinterVariable pv = list.SelectedItems[index] as PrinterVariable;
                PrinterVariable copied = new PrinterVariable();
                copied.Name = "Copy of " + pv.Name;
                copied.Value = pv.Value;
                if (po.ExistTestVariable(copied.Name))
                {
                    po.EditVariable(copied.Name, copied.Value);
                    for (int counter = 0; counter < list.Items.Count; ++counter)
                    {
                        if ((list.Items[counter] as PrinterVariable).Name == copied.Name)
                        {
                            list.Items.RemoveAt(counter);
                            list.Items.Insert(counter, copied);
                            break;
                        }
                    }
                }
                else
                {
                    po.AddVariable(copied.Name, copied.Value);
                    list.Items.Add(copied);
                }
                hasModified = true;
                atLeastOne = true;
            }
            list.Refresh();
            return atLeastOne;
        }

        /// <summary>
        /// Delete variables
        /// </summary>
        /// <param name="list">variable list</param>
        /// <param name="po">printer object</param>
        /// <returns>if at least one item removed</returns>
        public static bool DeleteVariables(ListBox list, PrinterObject po)
        {
            bool atLeastOne = false;
            for (int index = list.SelectedIndices.Count - 1; index >= 0; --index)
            {
                PrinterVariable pv = list.SelectedItems[index] as PrinterVariable;
                int pos = list.SelectedIndices[index];
                po.DeleteVariable(pv.Name);
                list.Items.RemoveAt(pos);
                hasModified = true;
                atLeastOne = true;
            }
            list.Refresh();
            return atLeastOne;
        }

        /// <summary>
        /// Delete variables
        /// </summary>
        /// <param name="list">variable list</param>
        /// <param name="po">variable object</param>
        /// <returns>if at least one item removed</returns>
        public static bool DeleteVariables(ListBox list, PrinterVariable po)
        {
            bool atLeastOne = false;
            for (int index = list.SelectedIndices.Count - 1; index >= 0; --index)
            {
                PrinterVariable pv = list.SelectedItems[index] as PrinterVariable;
                int pos = list.SelectedIndices[index];
                po.DeleteVariable(pv.Name);
                list.Items.RemoveAt(pos);
                hasModified = true;
                atLeastOne = true;
            }
            list.Refresh();
            return atLeastOne;
        }

        /// <summary>
        /// Fill all variable in list box
        /// </summary>
        /// <param name="list">variable list</param>
        /// <param name="po">printer object</param>
        public static void FillVars(ListBox list, PrinterObject po)
        {
            foreach(PrinterVariable pv in po.Values)
            {
                list.Items.Add(pv);
            }
            list.DisplayMember = "Name";
            list.ValueMember = "Value";
            if (po.Values.Count() > 0)
            {
                list.SetSelected(0, true);
            }
        }

        /// <summary>
        /// Fill all variable in list box
        /// </summary>
        /// <param name="list">variable list</param>
        /// <param name="pv">printer variable</param>
        public static void FillAccu(ListBox list, Accu.Accu a)
        {
        }

    }
}
