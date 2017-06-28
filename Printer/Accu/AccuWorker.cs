using Printer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accu
{
    public static class AccuWorker
    {

        #region Methods

        /// <summary>
        /// Converts accumulator to a string representation
        /// </summary>
        /// <param name="index">iterator</param>
        /// <param name="child">child</param>
        /// <param name="subPv">printer variable</param>
        private static void ToString(int index, Accu child, PrinterVariable subPv)
        {
            subPv.Include = true;
            Accu e = child.Children.ElementAt(index);
            subPv.Value = Path.Combine("Accu", "child.prt");
            subPv.AddVariable("ref", e.Name);
            if (index + 1 < child.Children.Count())
            {
                PrinterVariable current = new PrinterVariable();
                current.Indent = false;
                current.Name = "next";
                AccuWorker.ToString(index + 1, child, current);
                subPv.AddVariable("next", current);
            }
            else
            {
                subPv.AddVariable("next", string.Empty);
            }
        }

        /// <summary>
        /// Converts Accumulator to string
        /// </summary>
        /// <param name="child">child</param>
        /// <param name="po">printer output</param>
        private static void ToString(Accu child, PrinterObject po)
        {
            foreach (Accu subChild in child.Children)
            {
                AccuWorker.ToString(subChild, po);
                PrinterVariable pv = new PrinterVariable();
                pv.Include = true;
                if (subChild.Children.Count() > 0)
                {
                    pv.Name = subChild.Name;
                    pv.Value = Path.Combine("Accu", "node.prt");
                    pv.AddVariable("name", subChild.Name);
                    PrinterVariable current = new PrinterVariable();
                    current.Name = "node";
                    current.Indent = true;
                    AccuWorker.ToString(0, subChild, current);
                    pv.AddVariable("node", current);
                }
                else
                {
                    pv.Name = subChild.Name;
                    pv.Value = Path.Combine("Accu", "val.prt");
                    pv.AddVariable("name", subChild.Name);
                    pv.AddVariable("value", subChild.Value.ToString());
                }
                string id = po.ComputeNewString();
                po.AddVariable(id, pv);
                po.UseVariable(id);
            }
        }

        /// <summary>
        /// Converts Accumulator to string
        /// </summary>
        /// <param name="root">accumulator</param>
        /// <returns>string result</returns>
        public static string ToString(Accu root)
        {
            PrinterObject po = new PrinterObject();
            AccuWorker.ToString(root, po);
            return po.Execute();
        }

        #endregion

    }
}
