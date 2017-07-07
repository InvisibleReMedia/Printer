using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Printer;
using System.IO;
using System.Xml;

namespace Accumulate
{
    /// <summary>
    /// Handles a variable as an accu child
    /// </summary>
    [Serializable]
    public class AccuChild : PrinterVariable
    {

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccuChild(PrinterVariable pv)
        {
            this.name = pv.Name;
            this.include = pv.Include;
            this.shouldIndent = pv.Indent;
            this.value = pv.Value;
            this.Cast(pv);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Cast a printer object (base class)
        /// to a Accu object
        /// </summary>
        /// <param name="pv">printer variable</param>
        private void Cast(PrinterVariable pv)
        {
            for (int index = 0; index < pv.Values.Count(); ++index)
            {
                PrinterVariable sub = pv.Values.ElementAt(index);
                this.includedVars.Add(sub.Name, new AccuChild(sub));
            }
        }

        /// <summary>
        /// Write output as interpretation result
        /// </summary>
        /// <param name="dict">gives all dictionaries of accu</param>
        /// <param name="conf">configuration</param>
        /// <param name="dir">directory</param>
        /// <returns>output</returns>
        public string Execute(Dictionary<string, Accu> dict, Configuration conf, string dir)
        {
            int indentValue = 0;
            string currentLine = string.Empty;
            StringBuilder sb = new StringBuilder();
            using (TextWriter tw = new StringWriter(sb))
            {
                this.Execute(tw, ref indentValue, ref currentLine, conf, dir);
                tw.Close();
            }
            if (!String.IsNullOrEmpty(currentLine))
                sb.Append(currentLine);
            return sb.ToString();
        }

        /// <summary>
        /// Find a child by its name
        /// </summary>
        /// <param name="child">child accu</param>
        /// <param name="name">name to find</param>
        /// <returns>element found</returns>
        public static AccuChild FindByName(AccuChild child, string name)
        {
            if (child.includedVars.ContainsKey(name))
            {
                return child.includedVars[name] as AccuChild;
            }
            else
            {
                throw new KeyNotFoundException(String.Format("{0} not found", name));
            }
        }

        /// <summary>
        /// Find a child by its name
        /// </summary>
        /// <param name="child">root accu</param>
        /// <param name="index">index position</param>
        /// <param name="seq">sequence name</param>
        /// <returns>element found</returns>
        public static AccuChild RecursiveFindByName(AccuChild child, int index, string[] seq)
        {
            if (index < seq.Length)
            {
                AccuChild a = child.includedVars[seq[index]] as AccuChild;
                return AccuChild.RecursiveFindByName(a, index + 1, seq);
            }
            else
            {
                return child;
            }
        }

        /// <summary>
        /// Find a child by its name
        /// </summary>
        /// <param name="root">root accu</param>
        /// <param name="index">index position</param>
        /// <param name="seq">sequence name</param>
        /// <returns>element found</returns>
        public static AccuChild RecursiveFindByName(Accu root, int index, string[] seq)
        {
            if (index < seq.Length)
            {
                AccuChild a = root.Values.Last(x => x.Name == seq[index]) as AccuChild;
                return AccuChild.RecursiveFindByName(a, index + 1, seq);
            }
            else
            {
                throw new FormatException();
            }
        }

        #endregion

    }
}
