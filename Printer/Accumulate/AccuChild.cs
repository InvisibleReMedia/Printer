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

        #region Fields

        /// <summary>
        /// Type of element as a functionnal definition
        /// </summary>
        private AccuType function;

        /// <summary>
        /// Reference by name
        /// </summary>
        private string referenceName;

        #endregion

        #region Inner Class

        public enum AccuType
        {
            /// <summary>
            /// Printer object
            /// </summary>
            NORMAL,
            /// <summary>
            /// Reference to an existing value
            /// </summary>
            REFERENCE
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public AccuChild(PrinterVariable pv)
        {
            this.function = AccuType.NORMAL;
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
        /// Make a reference with an existing accu
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="refName">reference name</param>
        public void SetReference(string refName)
        {
            this.function = AccuType.REFERENCE;
            this.referenceName = refName;
        }

        /// <summary>
        /// Execute the variable
        /// </summary>
        /// <param name="dict">gives all dictionaries of accu</param>
        /// <param name="w">writer</param>
        /// <param name="indentValue">space size</param>
        /// <param name="currentLine">in-progress line add</param>
        /// <param name="config">configuration</param>
        /// <param name="dir">directory</param>
        public void Execute(Dictionary<string, Accu> dict, TextWriter w, ref int indentValue, ref string currentLine, Configuration config, string dir)
        {
            if (shouldIndent) ++indentValue;
            if (include)
            {
                if (this.function == AccuType.REFERENCE)
                {
                    if (dict.ContainsKey(this.referenceName))
                    {
                        PrinterObject.IndentSource(w, indentValue, ref currentLine, dict[this.referenceName].Result);
                    }
                }
                else
                {
                    string fileName = config.Execute(this.value);
                    FileInfo fi = new FileInfo(Path.Combine(dir, fileName));
                    if (fi.Exists)
                    {
                        using (FileStream fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            PrinterObject po = PrinterObject.Load(fs);
                            po.CurrentDirectory = dir;
                            foreach (PrinterVariable pv in this.Values)
                            {
                                po.AddVariable(pv.Name, pv);
                            }
                            po.ImportConfiguration(config);
                            po.Execute(w, ref indentValue, ref currentLine, po.Configuration);
                            fs.Close();
                        }
                    }
                }
            }
            else
            {
                if (this.function == AccuType.REFERENCE)
                {
                    if (dict.ContainsKey(this.referenceName))
                    {
                        PrinterObject.IndentSource(w, indentValue, ref currentLine, dict[this.referenceName].Result);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(this.value))
                    {
                        string val = config.Execute(this.value);
                        PrinterObject.IndentSource(w, indentValue, ref currentLine, val);
                    }
                }
            }
            if (shouldIndent) --indentValue;
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
                this.Execute(dict, tw, ref indentValue, ref currentLine, conf, dir);
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

        #endregion

    }
}
