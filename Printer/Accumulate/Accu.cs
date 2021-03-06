﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Printer;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace Accumulate
{
    /// <summary>
    /// Accumulator : read a printer object
    /// as an accumulator that's works as setted values
    /// </summary>
    [Serializable]
    public class Accu : PrinterObject, ICloneable
    {

        #region Fields

        /// <summary>
        /// Name of this accu type
        /// </summary>
        private string typeName;
        /// <summary>
        /// Name of the file
        /// </summary>
        private string fileName;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor from a printer object
        /// </summary>
        /// <param name="tn">type name of this accu</param>
        /// <param name="fi">file name to load</param>
        public Accu(string tn, string fi)
        {
            this.typeName = tn;
            this.fileName = fi;
            PrinterObject po = PrinterObject.Load(Path.Combine(Printer.PrinterObject.PrinterDirectory, "languages", fi));
            this.Cast(po);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gives the result string
        /// after setted values
        /// </summary>
        public string Result
        {
            get
            {
                return this.Execute();
            }
        }

        /// <summary>
        /// Gets or sets the name of the type accu
        /// </summary>
        public string TypeName
        {
            get
            {
                return this.typeName;
            }
            set
            {
                this.typeName = value;
            }
        }

        /// <summary>
        /// Gets or sets the file name or its value
        /// </summary>
        public string ValueOrFileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                this.fileName = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Cast a printer object (base class)
        /// to a Accu object
        /// </summary>
        /// <param name="po">printer object</param>
        private void Cast(PrinterObject po)
        {
            for (int index = 0; index < po.Values.Count(); ++index)
            {
                PrinterVariable pv = po.Values.ElementAt(index);
                this.Variables.Add(pv.Name, new AccuChild(pv));
            }
            this.Strings.AddRange(po.Datas);
        }

        /// <summary>
        /// Edit a variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="obj">object value</param>
        public new void EditVariable(string key, PrinterVariable obj)
        {
            if (this.Variables.ContainsKey(key))
            {
                this.Variables[key] = obj.Clone() as AccuChild;
            }
            else
            {
                this.Variables.Add(key, obj.Clone() as AccuChild);
            }
        }

        /// <summary>
        /// Add a variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="obj">object value</param>
        public new void AddVariable(string key, PrinterVariable obj)
        {
            if (this.Variables.ContainsKey(key))
            {
                this.Variables[key] = obj.Clone() as AccuChild;
            }
            else
            {
                this.Variables.Add(key, obj.Clone() as AccuChild);
            }
        }

        /// <summary>
        /// Write output as interpretation result
        /// </summary>
        /// <param name="w">writer</param>
        /// <param name="indentValue">space size</param>
        /// <param name="currentLine">in-progress line add</param>
        /// <param name="config">configuration</param>
        public new void Execute(TextWriter w, ref int indentValue, ref string currentLine, Configuration config)
        {
            foreach (string e in this.Datas)
            {
                if (e.StartsWith("[") && e.EndsWith("]"))
                {
                    string r = e.Substring(1, e.Length - 2);
                    r = config.Execute(r);
                    (this.Variables[r] as AccuChild).Execute(w, ref indentValue, ref currentLine, config, this.CurrentDirectory);
                }
                else
                {
                    string val = config.Execute(e);
                    PrinterObject.IndentSource(w, indentValue, ref currentLine, val);
                }
            }
        }

        /// <summary>
        /// Find a child by its name
        /// </summary>
        /// <param name="types">accu types</param>
        /// <param name="vars">accu variables</param>
        /// <param name="name">structured multiple names separated by a dot</param>
        /// <returns>element found</returns>
        public static AccuChild RecursiveFindByName(Dictionary<string, Accu> types, Dictionary<string, Accu> vars, string name)
        {
            if (name.StartsWith("$"))
            {
                name = name.Substring(1);
                return Accu.RecursiveFindByName(vars, name);
            }
            else
            {
                return Accu.RecursiveFindByName(types, name);
            }
        }

        /// <summary>
        /// Find a child by its name
        /// </summary>
        /// <param name="dict">accu sets</param>
        /// <param name="name">structured multiple names separated by a dot</param>
        /// <returns>element found</returns>
        public static AccuChild RecursiveFindByName(Dictionary<string, Accu> dict, string name)
        {
            string[] seq = name.Split('.');
            if (seq.Length > 1)
            {
                if (dict.ContainsKey(seq[0]))
                {
                    Accu a = dict[seq[0]];
                    if (a.Variables.ContainsKey(seq[1]))
                    {
                        return AccuChild.RecursiveFindByName(a.Variables[seq[1]] as AccuChild, 2, seq);
                    }
                    else
                    {
                        throw new KeyNotFoundException(String.Format("{0} not found", seq[1]));
                    }
                }
                else
                {
                    throw new KeyNotFoundException(String.Format("{0} not found", seq[0]));
                }
            }
            else
                throw new ArgumentException("bad name", "name");
        }

        /// <summary>
        /// Find accu by its name
        /// </summary>
        /// <param name="dict">accu names</param>
        /// <param name="name">name to find</param>
        /// <returns>element found</returns>
        public static Accu FindByName(Dictionary<string, Accu> dict, string name)
        {
            if (dict.ContainsKey(name))
            {
                return dict[name];
            }
            else
            {
                throw new KeyNotFoundException(String.Format("{0} not found", name));
            }
        }

        /// <summary>
        /// Converts the source into a string representation
        /// </summary>
        /// <param name="xml">xml document</param>
        public void ToString(XmlWriter xml)
        {
            xml.WriteStartElement("accu");
            xml.WriteAttributeString("typeName", this.TypeName);
            xml.WriteAttributeString("fileName", this.fileName);
            xml.WriteEndElement();
        }

        /// <summary>
        /// Generates the source code
        /// of this Accu
        /// </summary>
        /// <returns>the string representation</returns>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            using (MemoryStream stream = new MemoryStream())
            {
                XmlWriter xml = XmlWriter.Create(stream);

                xml.WriteStartDocument();
                xml.WriteStartElement("Program");
                xml.WriteStartElement("defs");
                this.ToString(xml);
                xml.WriteStartElement("vars");
                xml.WriteEndElement();
                xml.WriteStartElement("text");
                xml.WriteEndElement();
                xml.WriteEndElement();
                xml.WriteEndDocument();
                xml.Flush();

                stream.Seek(0, SeekOrigin.Begin);
#if DEBUG
                using (FileStream fs = new FileStream("luigi-output.xml", FileMode.Create))
                {
                    stream.CopyTo(fs);
                    fs.Close();
                }
                stream.Seek(0, SeekOrigin.Begin);
#endif

                XslCompiledTransform xsl = new XslCompiledTransform();
                xsl.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "luigi.xsl"));
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
        public new object Clone()
        {
            return new Accu(this.typeName, this.fileName);
        }

        #endregion

    }
}
