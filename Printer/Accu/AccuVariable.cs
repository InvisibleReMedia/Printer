using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace Accu
{
    /// <summary>
    /// A accu variable
    /// </summary>
    [Serializable]
    public class AccuVariable : ICloneable
    {

        #region Fields

        /// <summary>
        /// include switch
        /// </summary>
        private bool include;
        /// <summary>
        /// Indent switch
        /// </summary>
        private bool shouldIndent;
        /// <summary>
        /// Name of the variable
        /// </summary>
        private string name;
        /// <summary>
        /// Value of the variable
        /// </summary>
        private string value;
        /// <summary>
        /// Included variables
        /// </summary>
        private Dictionary<string, PrinterVariable> includedVars;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public PrinterVariable()
        {
            this.includedVars = new Dictionary<string, PrinterVariable>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indent switch
        /// </summary>
        public bool Indent
        {
            get { return this.shouldIndent; }
            set { this.shouldIndent = value; }
        }

        /// <summary>
        /// Include switch
        /// </summary>
        public bool Include
        {
            get { return this.include; }
            set { this.include = value; }
        }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Gets all values
        /// </summary>
        public IEnumerable<PrinterVariable> Values
        {
            get { return this.includedVars.Values; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Test if existing variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <returns>true if exist</returns>
        public bool ExistTestVariable(string key)
        {
            return this.includedVars.ContainsKey(key);
        }

        /// <summary>
        /// Edit a variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="obj">object value</param>
        public void EditVariable(string key, PrinterVariable obj)
        {
            if (this.includedVars.ContainsKey(key))
            {
                this.includedVars[key] = obj;
            }
            else
            {
                this.includedVars.Add(key, obj);
            }
        }

        /// <summary>
        /// Edit a variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="val">string value</param>
        public void EditVariable(string key, string val)
        {
            if (this.includedVars.ContainsKey(key))
            {
                this.includedVars[key].Value = val;
            }
            else
            {
                PrinterVariable p = new PrinterVariable();
                p.Name = key;
                p.Value = val;
                this.includedVars.Add(key, p);
            }
        }

        /// <summary>
        /// Add a variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="obj">object value</param>
        public void AddVariable(string key, PrinterVariable obj)
        {
            if (this.includedVars.ContainsKey(key))
            {
                this.includedVars[key] = obj.Clone() as PrinterVariable;
            }
            else
            {
                this.includedVars.Add(key, obj.Clone() as PrinterVariable);
            }
        }

        /// <summary>
        /// Add a variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="val">string value</param>
        public void AddVariable(string key, string val)
        {
            if (this.includedVars.ContainsKey(key))
            {
                this.includedVars[key].Value = val;
            }
            else
            {
                PrinterVariable p = new PrinterVariable();
                p.Name = key;
                p.Value = val;
                this.includedVars.Add(key, p);
            }
        }

        /// <summary>
        /// Delete a variable
        /// </summary>
        /// <param name="key">key name</param>
        public void DeleteVariable(string key)
        {
            if (this.includedVars.ContainsKey(key))
            {
                this.includedVars.Remove(key);
            }
        }

        /// <summary>
        /// Execute the variable
        /// </summary>
        /// <param name="w">writer</param>
        /// <param name="indentValue">space size</param>
        /// <param name="currentLine">in-progress line add</param>
        /// <param name="config">configuration</param>
        /// <param name="dir">directory</param>
        public void Execute(TextWriter w, ref int indentValue, ref string currentLine, Configuration config, string dir)
        {
            if (shouldIndent) ++indentValue;
            if (include)
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
            else
            {
                if (!String.IsNullOrEmpty(this.value))
                {
                    string val = config.Execute(this.value);
                    PrinterObject.IndentSource(w, indentValue, ref currentLine, val);
                }
            }
            if (shouldIndent) --indentValue;
        }

        /// <summary>
        /// Write output as interpretation result
        /// </summary>
        /// <param name="conf">configuration</param>
        /// <param name="dir">directory</param>
        /// <returns>output</returns>
        public string Execute(Configuration conf, string dir)
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
        /// Converts the source into a string representation
        /// </summary>
        /// <param name="xml">xml document</param>
        public void ToString(XmlWriter xml)
        {
            xml.WriteStartElement("set");
            xml.WriteAttributeString("name", this.name);
            if (this.include)
            {
                xml.WriteAttributeString("include", "true");
                if (this.shouldIndent)
                {
                    xml.WriteAttributeString("indented", "true");
                }
                else
                {
                    xml.WriteAttributeString("non-indented", "true");
                }
                xml.WriteAttributeString("file", this.value);
                xml.WriteStartElement("vars");
                foreach (PrinterVariable pv in this.Values)
                {
                    pv.ToString(xml);
                }
                xml.WriteEndElement();
            }
            else
            {
                xml.WriteAttributeString("include", "false");
                if (this.shouldIndent)
                {
                    xml.WriteAttributeString("indented", "true");
                }
                else
                {
                    xml.WriteAttributeString("non-indented", "true");
                }
                if (!String.IsNullOrEmpty(this.value))
                {
                    string v = this.value.Replace(":", ":dbdot;");
                    v = v.Replace("\"", ":dbquot;");
                    xml.WriteString(v);
                }
            }
            xml.WriteEndElement();
        }

        /// <summary>
        /// Print the string representation of a variable
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            using (MemoryStream stream = new MemoryStream())
            {
                XmlWriter xml = XmlWriter.Create(stream);

                xml.WriteStartDocument();
                xml.WriteStartElement("Program");
                xml.WriteStartElement("vars");
                this.ToString(xml);
                xml.WriteEndElement();
                xml.WriteEndElement();
                xml.WriteEndDocument();
                xml.Flush();

                stream.Seek(0, SeekOrigin.Begin);
#if DEBUG
                using (FileStream fs = new FileStream("printer-output.xml", FileMode.Create))
                {
                    stream.CopyTo(fs);
                    fs.Close();
                }
                stream.Seek(0, SeekOrigin.Begin);
#endif

                XslCompiledTransform xsl = new XslCompiledTransform();
                xsl.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "printer.xsl"));
                using (XmlReader reader = XmlReader.Create(stream))
                using (TextWriter writer = new StringWriter(output))
                {
                    xsl.Transform(reader, new XsltArgumentList(), writer);
                    reader.Close();
                    writer.Close();
                }
                stream.Close();
            }

            return output.ToString();
        }

        /// <summary>
        /// Clone this object
        /// </summary>
        /// <returns>new object</returns>
        public object Clone()
        {
            PrinterVariable pv = new PrinterVariable();
            pv.Name = this.Name.Clone() as string;
            if (!String.IsNullOrEmpty(this.Value))
                pv.Value = this.Value.Clone() as string;
            pv.shouldIndent = this.shouldIndent;
            pv.include = this.include;
            foreach (PrinterVariable subpv in this.Values)
            {
                pv.includedVars.Add(subpv.Name, subpv.Clone() as PrinterVariable);
            }
            return pv;
        }

        #endregion

    }
}
