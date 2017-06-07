using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace Printer
{
    /// <summary>
    /// Printer class
    /// </summary>
    [Serializable]
    class PrinterObject
    {
        #region Fields

        /// <summary>
        /// Data variable
        /// </summary>
        private Dictionary<string, string> variables;

        /// <summary>
        /// List of data to prints
        /// </summary>
        private List<string> datas;

        /// <summary>
        /// Generates unique strings
        /// </summary>
        private UniqueStrings unique;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public PrinterObject()
        {
            this.variables = new Dictionary<string, string>();
            this.datas = new List<string>();
            this.unique = new UniqueStrings();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets all values
        /// </summary>
        public IEnumerable<string> Values
        {
            get { return this.variables.Values; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="val">string value</param>
        public void AddVariable(string key, string val)
        {
            if (this.variables.ContainsKey(key))
            {
                this.variables[key] = val;
            }
            else
            {
                this.variables.Add(key, val);
            }
        }

        /// <summary>
        /// Add data into list
        /// </summary>
        /// <param name="s"></param>
        public void AddData(string s)
        {
            if (s.StartsWith("[") && s.EndsWith("]"))
            {
                string name = this.unique.ComputeNewString();
                string p = s.Substring(1, s.Length - 2);
                this.variables.Add(name, p);
                this.datas.Add("[" + name + "]");
            }
            else
            {
                this.datas.Add(s);
            }
        }

        /// <summary>
        /// Write the data into an output
        /// </summary>
        /// <param name="sb">output</param>
        public void Execute(StringBuilder sb)
        {
            foreach(string e in this.datas)
            {
                if (e.StartsWith("[") && e.EndsWith("]"))
                {
                    sb.Append(this.variables[e.Substring(1, e.Length - 2)]);
                } else
                {
                    sb.Append(e);
                }
            }
        }

        /// <summary>
        /// Load a file from disk
        /// </summary>
        /// <param name="fileName">full path of fileName</param>
        /// <returns>object</returns>
        public static PrinterObject Load(string fileName)
        {
            PrinterObject po = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    po = bf.Deserialize(fs) as PrinterObject;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }

            return po;
        }


        /// <summary>
        /// Save a PrinterObject to disk
        /// </summary>
        /// <param name="obj">object to save</param>
        /// <param name="fileName">full path of fileName to save</param>
        public static void Save(PrinterObject obj, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    bf.Serialize(fs, obj);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// Generates the source code
        /// of this PrinterObject
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
                xml.WriteStartElement("vars");
                foreach (KeyValuePair<string, string> kv in this.variables)
                {
                    xml.WriteStartElement("set");
                    xml.WriteAttributeString("name", kv.Key);
                    string v;
                    v = kv.Value.Replace(":", ":dbdot;");
                    v = v.Replace("\"", ":dbquot;");
                    xml.WriteString(v);
                    xml.WriteEndElement();
                }
                xml.WriteEndElement();
                xml.WriteStartElement("text");
                foreach (string s in this.datas)
                {
                    if (s.StartsWith("[") && s.EndsWith("]"))
                    {
                        xml.WriteElementString("var", s.Substring(1, s.Length - 2));
                    }
                    else
                    {
                        string r;
                        r = s.Replace(":", ":dbdot;");
                        r = r.Replace("[", ":sqBracketOpen;");
                        r = r.Replace("]", ":sqBracketClose;");
                        xml.WriteElementString("const", r);
                    }
                }
                xml.WriteEndElement();
                xml.WriteEndElement();
                xml.WriteEndDocument();
                xml.Flush();

                stream.Seek(0, SeekOrigin.Begin);

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

        #endregion
    }
}
