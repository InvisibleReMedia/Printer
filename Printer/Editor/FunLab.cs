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
        public static void AddNewVariable(ListBox list, PrinterObject po)
        {
            PrinterVariable pv = new PrinterVariable();
            list.Items.Add(pv);
            Variable var = new Variable();
            var.Controls["txtName"].DataBindings.Add("Text", pv, "Name");
            var.Controls["txtValue"].DataBindings.Add("Text", pv, "Value");
            DialogResult dr = var.ShowDialog();
            if (dr == DialogResult.OK)
            {
                po.AddVariable(pv.Name, pv.Value);
            }

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
                    list.Items.Add(s.Substring(1, s.Length - 2));
                }
                else
                {
                    if (s.Length > 10)
                        list.Items.Add(s.Substring(0, 10) + " ...");
                    else
                        list.Items.Add(s);
                }
            }
        }

    }
}
