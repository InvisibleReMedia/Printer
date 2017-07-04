using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Linq;
using System.Xml;
using System.Xml.Xsl;
using System.Text.RegularExpressions;

namespace Luigi
{
    /// <summary>
    /// Printer class
    /// </summary>
    [Serializable]
    public class LuigiObject : ICloneable
    {

        #region Fields

        /// <summary>
        /// Current directory where objects resides
        /// </summary>
        protected string currentDirectory;
        /// <summary>
        /// Data variable
        /// </summary>
        protected Dictionary<string, LuigiVariable> variables;

        /// <summary>
        /// List of data to prints
        /// </summary>
        protected List<string> datas;

        /// <summary>
        /// Generates unique strings
        /// </summary>
        private Printer.UniqueStrings unique;

        /// <summary>
        /// Configuration object
        /// </summary>
        protected Dictionary<string, Accumulate.Accu> types;

        /// <summary>
        /// Size indent space char
        /// </summary>
        public static readonly string IndentString = "  ";

        /// <summary>
        /// Current user directory
        /// stores languages, sources, compiled and temp
        /// </summary>
        public static readonly string PersonalDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\";

        /// <summary>
        /// Current program document directory
        /// </summary>
        public static readonly string LuigiDirectory = Path.Combine(PersonalDirectory, "Luigi");

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public LuigiObject()
            : this(Path.Combine(LuigiDirectory, "sources"))
        {
        }

        /// <summary>
        /// Constructor with currentDirectory setted
        /// </summary>
        /// <param name="cd">current directory</param>
        public LuigiObject(string cd)
        {
            LuigiObject.InitializePersonalDirectory();
            this.variables = new Dictionary<string, LuigiVariable>();
            this.datas = new List<string>();
            this.unique = new Printer.UniqueStrings();
            this.types = new Dictionary<string, Accumulate.Accu>();
            this.currentDirectory = cd;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets all values
        /// </summary>
        public IEnumerable<LuigiVariable> Values
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

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        public Dictionary<string, Accumulate.Accu> Types
        {
            get
            {
                return this.types;
            }
            set
            {
                this.types = value;
            }
        }

        /// <summary>
        /// Gets or sets the current directory
        /// </summary>
        public string CurrentDirectory
        {
            get
            {
                return this.currentDirectory;
            }
            set
            {
                this.currentDirectory = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Indentation before each line
        /// </summary>
        /// <param name="sw">writer</param>
        /// <param name="n">number of indentation</param>
        /// <param name="source">string to write indented (can contains \r\n)</param>
        /// <param name="add">add chars</param>
        public static void IndentSource(TextWriter sw, int n, ref string source, string add)
        {
            string indent = string.Empty;
            for (int index = 0; index < n; ++index) indent += IndentString;
            int pos = add.IndexOf(Environment.NewLine);
            while (pos != -1)
            {
                source += add.Substring(0, pos);
                sw.WriteLine(indent + source);
                source = string.Empty;
                add = add.Substring(pos + 2);
                pos = add.IndexOf(Environment.NewLine);
            }
            source += add;
        }

        /// <summary>
        /// Copy an entire directory to an existing destination directory
        /// </summary>
        /// <param name="di">directory to copy</param>
        /// <param name="destination">destination path</param>
        private static void CopyDirectory(DirectoryInfo di, string destination)
        {
            foreach (FileInfo fi in di.GetFiles())
            {
                fi.CopyTo(Path.Combine(destination, fi.Name), true);
            }
            foreach (DirectoryInfo subDi in di.GetDirectories())
            {
                DirectoryInfo newdi = new DirectoryInfo(Path.Combine(destination, subDi.Name));
                if (!newdi.Exists)
                {
                    newdi.Create();
                }
                CopyDirectory(subDi, Path.Combine(destination, subDi.Name));
            }
        }

        /// <summary>
        /// Initialize personal directory
        /// </summary>
        private static void InitializePersonalDirectory()
        {
            DirectoryInfo di = new DirectoryInfo(LuigiObject.PersonalDirectory);
            if (di.Exists)
            {
                DirectoryInfo diRootDir = new DirectoryInfo(Path.Combine(LuigiObject.PersonalDirectory, "Luigi"));
                if (!diRootDir.Exists)
                {
                    diRootDir.Create();
                    DirectoryInfo diLanguages = new DirectoryInfo(Path.Combine(diRootDir.FullName, "languages"));
                    if (!diLanguages.Exists)
                    {
                        diLanguages.Create();
                        DirectoryInfo sourceLanguages = new DirectoryInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "languages"));
                        if (sourceLanguages.Exists)
                        {
                            CopyDirectory(sourceLanguages, diLanguages.FullName);
                        }
                        else
                        {
                            throw new DirectoryNotFoundException(String.Format("The directory {0} doesn't exist", sourceLanguages.FullName));
                        }
                    }
                    DirectoryInfo diSources = new DirectoryInfo(Path.Combine(diRootDir.FullName, "sources"));
                    if (!diSources.Exists)
                    {
                        diSources.Create();
                    }
                    DirectoryInfo diCompiled = new DirectoryInfo(Path.Combine(diRootDir.FullName, "compiled"));
                    if (!diCompiled.Exists)
                    {
                        diCompiled.Create();
                    }
                    DirectoryInfo diTemp = new DirectoryInfo(Path.Combine(diRootDir.FullName, "temp"));
                    if (!diTemp.Exists)
                    {
                        diTemp.Create();
                    }
                }

            }
            else
            {
                throw new NotSupportedException("Your personal directory doesn't exist. Assumes to have one");
            }
        }

        /// <summary>
        /// Test if existing type
        /// </summary>
        /// <param name="key">key name</param>
        /// <returns>true if exist</returns>
        public bool ExistTestType(string key)
        {
            return this.types.ContainsKey(key);
        }

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
                LuigiVariable p = new LuigiVariable();
                p.Name = key;
                p.Value = val;
                this.variables.Add(key, p);
            }
        }

        /// <summary>
        /// Edit a variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="fi">file name value</param>
        public void EditType(string key, string fi)
        {
            if (this.types.ContainsKey(key))
            {
                this.types[key] = new Accumulate.Accu(key, fi);
            }
            else
            {
                this.types.Add(key, new Accumulate.Accu(key, fi));
            }
        }

        /// <summary>
        /// Edit a variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="obj">object value</param>
        public void EditVariable(string key, LuigiVariable obj)
        {
            if (this.variables.ContainsKey(key))
            {
                this.variables[key] = obj.Clone() as LuigiVariable;
            }
            else
            {
                this.variables.Add(key, obj.Clone() as LuigiVariable);
            }
        }

        /// <summary>
        /// Add a type
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="obj">object value</param>
        public void AddType(string key, Accumulate.Accu obj)
        {
            if (this.types.ContainsKey(key))
            {
                this.types[key] = obj.Clone() as Accumulate.Accu;
            }
            else
            {
                this.types.Add(key, obj.Clone() as Accumulate.Accu);
            }
        }

        /// <summary>
        /// Add a type
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="obj">file name</param>
        public void AddType(string key, string fi)
        {
            if (this.types.ContainsKey(key))
            {
                this.types[key] = new Accumulate.Accu(key, fi);
            }
            else
            {
                this.types.Add(key, new Accumulate.Accu(key, fi));
            }
        }

        /// <summary>
        /// Add a variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="obj">object value</param>
        public void AddVariable(string key, LuigiVariable obj)
        {
            if (this.variables.ContainsKey(key))
            {
                this.variables[key] = obj.Clone() as LuigiVariable;
            }
            else
            {
                this.variables.Add(key, obj.Clone() as LuigiVariable);
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
                LuigiVariable p = new LuigiVariable();
                p.Name = key;
                p.Value = val;
                this.variables.Add(key, p);
            }
        }

        /// <summary>
        /// Delete a type
        /// </summary>
        /// <param name="key">key name</param>
        public void DeleteType(string key)
        {
            if (this.types.ContainsKey(key))
            {
                this.types.Remove(key);
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
            this.datas.Add("[" + name + "]");
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
        /// <param name="indentValue">space size</param>
        /// <param name="currentLine">in-progress line add</param>
        /// <param name="t">types</param>
        public void Execute(TextWriter w, ref int indentValue, ref string currentLine, Dictionary<string, Accumulate.Accu> t)
        {
            foreach (string e in this.datas)
            {
                if (e.StartsWith("[") && e.EndsWith("]"))
                {
                    string r = e.Substring(1, e.Length - 2);
                    this.variables[r].Execute(w, ref indentValue, ref currentLine, t, this.CurrentDirectory);
                }
                else
                {
                    LuigiObject.IndentSource(w, indentValue, ref currentLine, e);
                }
            }
        }

        /// <summary>
        /// Write output as interpretation result
        /// </summary>
        /// <returns>output</returns>
        public string Execute()
        {
            int indentValue = 0;
            string currentLine = string.Empty;
            StringBuilder sb = new StringBuilder();
            using (TextWriter tw = new StringWriter(sb))
            {
                this.Execute(tw, ref indentValue, ref currentLine, this.types);
                tw.Close();
            }
            if (!String.IsNullOrEmpty(currentLine))
                sb.Append(currentLine);
            return sb.ToString();
        }

        /// <summary>
        /// Import types
        /// </summary>
        /// <param name="from">types</param>
        public void ImportTypes(Dictionary<string, Accumulate.Accu> from)
        {
            foreach (string key in from.Keys)
            {
                this.types.Add(key, from[key]);
            }
        }

        /// <summary>
        /// Load a file from disk
        /// </summary>
        /// <param name="fileName">full path of fileName</param>
        /// <returns>object</returns>
        public static LuigiObject Load(string fileName)
        {
            LuigiObject po = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    po = bf.Deserialize(fs) as LuigiObject;
                    if (String.IsNullOrEmpty(po.CurrentDirectory))
                    {
                        po.CurrentDirectory = Path.GetDirectoryName(fileName);
                    }
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
        /// Save a LuigiObject to disk
        /// </summary>
        /// <param name="obj">object to save</param>
        /// <param name="fileName">full path of fileName to save</param>
        public static void Save(LuigiObject obj, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    obj.CurrentDirectory = Path.GetDirectoryName(fileName);
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
        public static LuigiObject Load(Stream stream)
        {
            LuigiObject po = null;
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                po = bf.Deserialize(stream) as LuigiObject;
            }
            catch (Exception)
            {
                throw;
            }

            return po;
        }


        /// <summary>
        /// Save a LuigiObject to memory
        /// You must close the stream after this method
        /// </summary>
        /// <param name="obj">object to save</param>
        /// <param name="stream">stream buffer</param>
        public static void Save(LuigiObject obj, Stream stream)
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
        /// Computes a new name 
        /// </summary>
        /// <returns>a new name</returns>
        public string ComputeNewString()
        {
            return this.unique.ComputeNewString();
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
                xml.WriteStartElement("defs");
                foreach (string key in this.types.Keys)
                {
                    this.types[key].ToString(xml);
                }
                xml.WriteEndElement();
                xml.WriteStartElement("vars");
                foreach (LuigiVariable l in this.Values)
                {
                    l.ToString(xml);
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
        public object Clone()
        {
            LuigiObject newPo = new LuigiObject();
            foreach (string s in this.datas)
            {
                newPo.datas.Add(s.Clone() as string);
            }
            newPo.unique = new Printer.UniqueStrings(this.unique.Counter);
            foreach (LuigiVariable pv in this.Values)
            {
                newPo.variables.Add(pv.Name, pv.Clone() as LuigiVariable);
            }
            return newPo;
        }

        #endregion
    }
}
