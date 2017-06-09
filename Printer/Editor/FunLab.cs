using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Printer;

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
        /// <param name="ofd">dialog</param>
        /// <param name="po">printer object</param>
        /// <returns>true if loaded</returns>
        public static bool Load(OpenFileDialog ofd, ref PrinterObject po)
        {
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                po = PrinterObject.Load(ofd.FileName);
                alreadyOpen = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Save to file
        /// </summary>
        /// <param name="sfd">dialog</param>
        /// <param name="po">printer object</param>
        /// <returns>true if saved</returns>
        public static bool Save(SaveFileDialog sfd, PrinterObject po)
        {
            if (alreadyOpen)
            {
                PrinterObject.Save(po, sfd.FileName);
                hasModified = false;
                return true;
            }
            else
            {
                DialogResult dr = sfd.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    PrinterObject.Save(po, sfd.FileName);
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
        /// <param name="sfd">dialog</param>
        /// <param name="po">printer object</param>
        /// <returns>true if saved</returns>
        public static bool SaveAs(SaveFileDialog sfd, PrinterObject po)
        {
            DialogResult dr = sfd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                PrinterObject.Save(po, sfd.FileName);
                hasModified = false;
                alreadyOpen = true;
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
            var.Controls["txtName"].DataBindings.Add("Text", pv, "Name");
            var.Controls["txtValue"].DataBindings.Add("Text", pv, "Value");
            DialogResult dr = var.ShowDialog();
            if (dr == DialogResult.OK)
            {
                if (po.ExistTestVariable(pv.Name))
                {
                    po.EditVariable(pv.Name, pv.Value);
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
                    po.AddVariable(pv.Name, pv.Value);
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
        /// <param name="po">printer object</param>
        /// <returns>added variable</returns>
        public static bool EditVariable(ListBox list, PrinterObject po)
        {
            if (list.SelectedIndices.Count == 1)
            {
                PrinterVariable pv = list.SelectedItems[0] as PrinterVariable;
                Variable var = new Variable();
                var.Controls["txtName"].Text = pv.Name;
                var.Controls["txtValue"].DataBindings.Add("Text", pv, "Value");
                DialogResult dr = var.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    if (pv.Name != var.Controls["txtName"].Text)
                    {
                        po.DeleteVariable(pv.Name);
                        list.Items.RemoveAt(list.SelectedIndices[0]);
                    }
                    pv.Name = var.Controls["txtName"].Text;
                    if (po.ExistTestVariable(pv.Name))
                    {
                        po.EditVariable(pv.Name, pv.Value);
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
                        po.AddVariable(pv.Name, pv.Value);
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
        /// Delete variables
        /// </summary>
        /// <param name="list">variable list</param>
        /// <param name="po">printer object</param>
        /// <returns>if at least one item removed</returns>
        public static bool DeleteVariables(ListBox list, PrinterObject po)
        {
            bool atLeastOne = false;
            for(int index = list.SelectedIndices.Count - 1; index >= 0; --index)
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
                string s = list.SelectedItems[0] as string;
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
                        list.Items[pos] = d.Controls["vars"].Text;
                    }
                    else
                    {
                        po.EditData(pos, d.Controls["txtConst"].Text);
                        list.Items[pos] = d.Controls["txtConst"].Text;
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

    }
}
