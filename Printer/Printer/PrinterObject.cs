using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Xsl;

namespace Printer
{
    /// <summary>
    /// Printer class
    /// </summary>
    [Serializable]
    public class PrinterObject : ICloneable
    {
        #region Fields

        /// <summary>
        /// Data variable
        /// </summary>
        private Dictionary<string, PrinterVariable> variables;

        /// <summary>
        /// List of data to prints
        /// </summary>
        private List<string> datas;

        /// <summary>
        /// Generates unique strings
        /// </summary>
        private UniqueStrings unique;

        /// <summary>
        /// Size indent space char
        /// </summary>
        public static readonly int IndentSize = 2;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public PrinterObject()
        {
            this.variables = new Dictionary<string, PrinterVariable>();
            this.datas = new List<string>();
            this.unique = new UniqueStrings();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets all values
        /// </summary>
        public IEnumerable<PrinterVariable> Values
        {
            get { return this.variables.Values; }
        }

        /// <summary>
        /// Gets all data sequence
        /// </summary>
        public IEnumerable<string> Data
        {
            get { return this.datas; }
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
            return this.variables.ContainsKey(key);
        }

        /// <summary>
        /// Edit a variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="val">string value</param>
        public void EditVariable(string key, string val)
        {
            if (this.variables.ContainsKey(key))
            {
                this.variables[key].Value = val;
            }
            else
            {
                PrinterVariable p = new PrinterVariable();
                p.Name = key;
                p.Value = val;
                this.variables.Add(key, p);
            }
        }

        /// <summary>
        /// Edit a variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="obj">object value</param>
        public void EditVariable(string key, PrinterVariable obj)
        {
            if (this.variables.ContainsKey(key))
            {
                this.variables[key] = obj.Clone() as PrinterVariable;
            }
            else
            {
                this.variables.Add(key, obj.Clone() as PrinterVariable);
            }
        }

        /// <summary>
        /// Add a variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="obj">object value</param>
        public void AddVariable(string key, PrinterVariable obj)
        {
            if (this.variables.ContainsKey(key))
            {
                this.variables[key] = obj.Clone() as PrinterVariable;
            }
            else
            {
                this.variables.Add(key, obj.Clone() as PrinterVariable);
            }
        }

        /// <summary>
        /// Add a variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="val">string value</param>
        public void AddVariable(string key, string val)
        {
            if (this.variables.ContainsKey(key))
            {
                this.variables[key].Value = val;
            }
            else
            {
                PrinterVariable p = new PrinterVariable();
                p.Name = key;
                p.Value = val;
                this.variables.Add(key, p);
            }
        }

        /// <summary>
        /// Delete a variable
        /// </summary>
        /// <param name="key">key name</param>
        public void DeleteVariable(string key)
        {
            if (this.variables.ContainsKey(key))
            {
                this.variables.Remove(key);
            }
        }

        /// <summary>
        /// Use a variable
        /// </summary>
        /// <param name="name">variable name</param>
        public void UseVariable(string name)
        {
            this.datas.Add("[" + name +"]");
        }

        /// <summary>
        /// Change the use of a variable
        /// </summary>
        /// <param name="index">position</param>
        /// <param name="name">variable name</param>
        public void UseChangeVariable(int index, string name)
        {
            this.datas[index] = "[" + name + "]";
        }

        /// <summary>
        /// Insert the use of a variable before
        /// </summary>
        /// <param name="index">position</param>
        /// <param name="name">variable name</param>
        public void InsertUseVariableBefore(int index, string name)
        {
            this.datas.Insert(index, "[" + name + "]");
        }

        /// <summary>
        /// Insert the use of a variable after
        /// </summary>
        /// <param name="index">position</param>
        /// <param name="name">variable name</param>
        public void InsertUseVariableAfter(int index, string name)
        {
            if (index + 1 < this.datas.Count)
                this.InsertUseVariableBefore(index + 1, name);
            else
                this.UseVariable(name);
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
                this.AddVariable(name, p);
                this.datas.Add("[" + name + "]");
            }
            else
            {
                this.datas.Add(s);
            }
        }

        /// <summary>
        /// Edit data at index
        /// </summary>
        /// <param name="index">position</param>
        /// <param name="s">change</param>
        public void EditData(int index, string s)
        {
            if (s.StartsWith("[") && s.EndsWith("]"))
            {
                string name = this.unique.ComputeNewString();
                string p = s.Substring(1, s.Length - 2);
                this.AddVariable(name, p);
                this.datas[index] = "[" + name + "]";
            }
            else
            {
                this.datas[index] = s;
            }
        }

        /// <summary>
        /// Insert data at index before
        /// </summary>
        /// <param name="index">position</param>
        /// <param name="s">value</param>
        public void InsertDataBefore(int index, string s)
        {
            if (s.StartsWith("[") && s.EndsWith("]"))
            {
                string name = this.unique.ComputeNewString();
                string p = s.Substring(1, s.Length - 2);
                this.AddVariable(name, p);
                this.datas.Insert(index, "[" + name + "]");
            }
            else
            {
                this.datas.Insert(index, s);
            }
        }

        /// <summary>
        /// Insert data at index after
        /// </summary>
        /// <param name="index">position</param>
        /// <param name="s">value</param>
        public void InsertDataAfter(int index, string s)
        {
            if (s.StartsWith("[") && s.EndsWith("]"))
            {
                string name = this.unique.ComputeNewString();
                string p = s.Substring(1, s.Length - 2);
                this.AddVariable(name, p);
                if (index + 1 < this.datas.Count)
                    this.datas.Insert(index + 1, "[" + name + "]");
                else
                    this.datas.Add("[" + name + "]");
            }
            else
            {
                if (index + 1 < this.datas.Count)
                    this.datas.Insert(index + 1, s);
                else
                    this.datas.Add(s);
            }
        }

        /// <summary>
        /// Delete data at index
        /// </summary>
        /// <param name="index">position</param>
        public void DeleteData(int index)
        {
            this.datas.RemoveAt(index);
        }

        /// <summary>
        /// Write output as interpretation result
        /// </summary>
        /// <param name="w">writer</param>
        private void Execute(IndentedTextWriter w)
        {
            foreach (string e in this.datas)
            {
                if (e.StartsWith("[") && e.EndsWith("]"))
                {
                    string r = e.Substring(1, e.Length - 2);
                    this.variables[r].Execute(w);
                }
                else
                {
                    w.Write(e);
                }
            }
        }

        /// <summary>
        /// Write output as interpretation result
        /// </summary>
        /// <returns>output</returns>
        public string Execute()
        {
            StringBuilder sb = new StringBuilder();
            using (TextWriter tw = new StringWriter(sb))
            using (IndentedTextWriter itw = new IndentedTextWriter(tw))
            {
                this.Execute(itw);
                itw.WriteLine();
                itw.Close();
                tw.Close();
            }
            return sb.ToString();
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
        /// Load a file from memory
        /// You must close the stream after this method
        /// </summary>
        /// <param name="stream">stream buffer</param>
        /// <returns>object</returns>
        public static PrinterObject Load(Stream stream)
        {
            PrinterObject po = null;
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                po = bf.Deserialize(stream) as PrinterObject;
            }
            catch (Exception)
            {
                throw;
            }

            return po;
        }


        /// <summary>
        /// Save a PrinterObject to memory
        /// You must close the stream after this method
        /// </summary>
        /// <param name="obj">object to save</param>
        /// <param name="stream">stream buffer</param>
        public static void Save(PrinterObject obj, Stream stream)
        {
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                bf.Serialize(stream, obj);
            }
            catch (Exception)
            {
                throw;
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
                foreach (KeyValuePair<string, PrinterVariable> kv in this.variables)
                {
                    kv.Value.ToString(xml);
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
            PrinterObject newPo = new PrinterObject();
            foreach(string s in this.datas)
            {
                newPo.datas.Add(s.Clone() as string);
            }
            newPo.unique = new UniqueStrings(this.unique.Counter);
            foreach(PrinterVariable pv in this.Values)
            {
                newPo.variables.Add(pv.Name, pv.Clone() as PrinterVariable);
            }
            return newPo;
        }

        #endregion
    }
}
