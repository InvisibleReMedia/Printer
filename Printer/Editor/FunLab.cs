using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Printer;
using System.IO;

namespace Editor
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
        /// Load from file
        /// </summary>
        /// <param name="path">path</param>
        /// <param name="fileName">file name</param>
        /// <param name="po">printer object</param>
        /// <returns>true if loaded</returns>
        public static bool Load(ref string path, ref string fileName, ref PrinterObject po)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = path;
            ofd.FileName = fileName;
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                path = Path.GetDirectoryName(ofd.FileName);
                fileName = Path.GetFileName(ofd.FileName);
                po = PrinterObject.Load(Path.Combine(path, fileName));
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
        /// <param name="po">printer object</param>
        /// <returns>true if saved</returns>
        public static bool Save(ref string path, ref string fileName, PrinterObject po)
        {
            if (alreadyOpen)
            {
                PrinterObject.Save(po, Path.Combine(path, fileName));
                hasModified = false;
                return true;
            }
            else
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.InitialDirectory = path;
                sfd.FileName = fileName;
                DialogResult dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    path = Path.GetDirectoryName(sfd.FileName);
                    fileName = Path.GetFileName(sfd.FileName);
                    PrinterObject.Save(po, Path.Combine(path, fileName));
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
        /// <param name="po">printer object</param>
        /// <returns>true if saved</returns>
        public static bool SaveAs(ref string path, ref string fileName, PrinterObject po)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = path;
            sfd.FileName = fileName;
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                path = Path.GetDirectoryName(sfd.FileName);
                fileName = Path.GetFileName(sfd.FileName);
                PrinterObject.Save(po, Path.Combine(path, fileName));
                alreadyOpen = true;
                hasModified = false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Create a new printer object
        /// </summary>
        /// <param name="po">printer object</param>
        public static void New(ref PrinterObject po)
        {
            po = new PrinterObject();
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
        /// <param name="list">variable list</param>
        /// <param name="po">printer object</param>
        /// <returns>added variable</returns>
        public static bool AddNewVariable(ListBox list, PrinterVariable po)
        {
            PrinterVariable pv = new PrinterVariable();
            Variable var = new Variable();
            var.IsIndented = pv.Indent;
            var.Controls["txtName"].DataBindings.Add("Text", pv, "Name");
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
                pv.Indent = var.IsIndented;
                pv.Include = (var.IncludePage.Controls["rbInclude"] as RadioButton).Checked;
                if (pv.Include)
                {
                    pv.Value = var.IncludePage.Controls["txtFile"].Text;
                }
                else
                {
                    pv.Value = var.ValuePage.Controls["txtValue"].Text;
                }
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
        /// Add a new variable
        /// </summary>
        /// <param name="list">variable list</param>
        /// <param name="po">printer object</param>
        /// <returns>added variable</returns>
        public static bool AddNewVariable(ListBox list, PrinterObject po)
        {
            PrinterVariable pv = new PrinterVariable();
            Variable var = new Variable();
            var.IsIndented = pv.Indent;
            var.Controls["txtName"].DataBindings.Add("Text", pv, "Name");
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
                var.Controls["txtName"].DataBindings.Add("Text", pv, "Name");
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
                    if (pv.Name != var.Controls["txtName"].Text)
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
                var.Controls["txtName"].DataBindings.Add("Text", pv, "Name");
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
                    if (pv.Name != var.Controls["txtName"].Text)
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
        /// Add a new data
        /// </summary>
        /// <param name="list">data list</param>
        /// <param name="po">printer object</param>
        /// <returns>added variable</returns>
        public static bool AddNewData(ListBox list, PrinterObject po)
        {
            string s = string.Empty;
            Data d = new Data();
            FillVars(d.Controls["vars"] as ListBox, po);
            DialogResult dr = d.ShowDialog();
            if (dr == DialogResult.OK)
            {
                bool byVar = (d.Controls["rbVariable"] as RadioButton).Checked;
                if (byVar)
                {
                    po.UseVariable(d.Controls["vars"].Text);
                }
                else
                {
                    po.AddData(d.Controls["txtConst"].Text);
                }
                list.Items.Add(po.Data.Last());
                list.Refresh();
                hasModified = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Edit data
        /// </summary>
        /// <param name="list">data list</param>
        /// <param name="po">printer object</param>
        /// <returns>added variable</returns>
        public static bool EditData(ListBox list, PrinterObject po)
        {
            if (list.SelectedIndices.Count == 1)
            {
                int pos = list.SelectedIndices[0];
                string s = po.Data.ElementAt(pos);
                Data d = new Data();
                FillVars(d.Controls["vars"] as ListBox, po);
                if (s.StartsWith("[") && s.EndsWith("]"))
                {
                    d.Controls["vars"].Text = s.Substring(1, s.Length - 2);
                    (d.Controls["rbVariable"] as RadioButton).Checked = true;
                }
                else
                {
                    d.Controls["txtConst"].Text = s;
                    (d.Controls["rbConst"] as RadioButton).Checked = true;
                }
                DialogResult dr = d.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    bool byVar = (d.Controls["rbVariable"] as RadioButton).Checked;
                    if (byVar)
                    {
                        po.UseChangeVariable(pos, d.Controls["vars"].Text);
                    }
                    else
                    {
                        po.EditData(pos, d.Controls["txtConst"].Text);
                    }
                    list.Items[pos] = po.Data.ElementAt(pos);
                    list.Refresh();
                    hasModified = true;
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// Insert a new data before a given position
        /// </summary>
        /// <param name="list">data list</param>
        /// <param name="po">printer object</param>
        /// <returns>added variable</returns>
        public static bool InsertBefore(ListBox list, PrinterObject po)
        {
            if (list.SelectedIndices.Count == 1)
            {
                int pos = list.SelectedIndices[0];
                Data d = new Data();
                FillVars(d.Controls["vars"] as ListBox, po);
                DialogResult dr = d.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    bool byVar = (d.Controls["rbVariable"] as RadioButton).Checked;
                    if (byVar)
                    {
                        po.InsertUseVariableBefore(pos, d.Controls["vars"].Text);
                    }
                    else
                    {
                        po.InsertDataBefore(pos, d.Controls["txtConst"].Text);
                    }
                    list.Items.Insert(pos, po.Data.ElementAt(pos));
                    list.Refresh();
                    hasModified = true;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Insert a new data after a given position
        /// </summary>
        /// <param name="list">data list</param>
        /// <param name="po">printer object</param>
        /// <returns>added variable</returns>
        public static bool InsertAfter(ListBox list, PrinterObject po)
        {
            if (list.SelectedIndices.Count == 1)
            {
                int pos = list.SelectedIndices[0];
                Data d = new Data();
                FillVars(d.Controls["vars"] as ListBox, po);
                DialogResult dr = d.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    bool byVar = (d.Controls["rbVariable"] as RadioButton).Checked;
                    if (byVar)
                    {
                        po.InsertUseVariableAfter(pos, d.Controls["vars"].Text);
                    }
                    else
                    {
                        po.InsertDataAfter(pos, d.Controls["txtConst"].Text);
                    }
                    if (pos + 1 < list.Items.Count)
                        list.Items.Insert(pos + 1, po.Data.ElementAt(pos + 1));
                    else
                        list.Items.Add(po.Data.ElementAt(pos + 1));
                    list.Refresh();
                    hasModified = true;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Delete data
        /// </summary>
        /// <param name="list">data list</param>
        /// <param name="po">printer object</param>
        /// <returns>if at least one item removed</returns>
        public static bool DeleteData(ListBox list, PrinterObject po)
        {
            bool atLeastOne = false;
            for (int index = list.SelectedIndices.Count - 1; index >= 0; --index)
            {
                int pos = list.SelectedIndices[index];
                po.DeleteData(pos);
                list.Items.RemoveAt(pos);
                hasModified = true;
                atLeastOne = true;
            }
            list.Refresh();
            return atLeastOne;
        }

        /// <summary>
        /// Copy data
        /// </summary>
        /// <param name="list">data list</param>
        /// <param name="po">printer object</param>
        /// <returns>if at least one item removed</returns>
        public static bool CopyData(ListBox list, PrinterObject po)
        {
            bool atLeastOne = false;
            for (int index = 0; index < list.SelectedIndices.Count; ++index)
            {
                int pos = list.SelectedIndices[index];
                string s = po.Data.ElementAt(pos);
                if (s.StartsWith("[") && s.EndsWith("]"))
                {
                    po.UseVariable(s.Substring(1, s.Length - 2));
                } else
                {
                    po.AddData(s);
                }
                list.Items.Add(po.Data.Last());
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
        public static void FillVars(ListBox list, PrinterVariable pv)
        {
            foreach (PrinterVariable inpv in pv.Values)
            {
                list.Items.Add(inpv);
            }
            list.DisplayMember = "Name";
            list.ValueMember = "Value";
            if (pv.Values.Count() > 0)
            {
                list.SetSelected(0, true);
            }
        }

        /// <summary>
        /// Fill all data in list box
        /// </summary>
        /// <param name="list">data list</param>
        /// <param name="po">printer object</param>
        public static void FillData(ListBox list, PrinterObject po)
        {
            foreach (string s in po.Data)
            {
                if (s.StartsWith("[") && s.EndsWith("]"))
                {
                    list.Items.Add(s);
                }
                else
                {
                    if (s.Length > 10)
                        list.Items.Add(s.Substring(0, 10) + " ...");
                    else
                        list.Items.Add(s);
                }
            }
            if (po.Data.Count() > 0)
            {
                list.SetSelected(0, true);
            }
        }

        /// <summary>
        /// Fill all config in list box
        /// </summary>
        /// <param name="list">variable list</param>
        /// <param name="conf">configuration object</param>
        public static void FillConfigs(ListBox list, Configuration conf)
        {
            foreach (string c in conf)
            {
                list.Items.Add(c);
            }
            if (conf.Count > 0)
            {
                list.SetSelected(0, true);
            }
        }

    }
}
