using Printer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accu
{
    /// <summary>
    /// Programme de traitement des
    /// données Accu
    /// </summary>
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
            if (child.IsMethodCall) return;
            foreach (Accu subChild in child.Children)
            {
                if (subChild.IsMethodCall) continue;
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
                po.AddVariable(subChild.Name, pv);
                po.UseVariable(subChild.Name);
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

        /// <summary>
        /// Execute commands
        /// </summary>
        /// <param name="root">root of Accu</param>
        /// <param name="workingFun">a set of functions that work on value</param>
        /// <returns>string result</returns>
        public static string Execute(Accu root, Func<dynamic, IEnumerable<Accu>, string> workingFun)
        {
            string output = string.Empty;
            foreach (Accu e in root.Children)
            {
                output += AccuWorker.Execute(e, workingFun);
                if (!e.HasResult)
                {
                    e.Execute(workingFun);
                }
                output += e.Result;
            }
            return output;
        }

        #endregion

    }
}
