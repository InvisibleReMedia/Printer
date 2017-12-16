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
    public class PrinterObject : PersistentDataObject, ICloneable
    {
        #region Fields

        /// <summary>
        /// Index name for version
        /// </summary>
        protected static readonly string versionName = "version";
        /// <summary>
        /// Index name for revision
        /// </summary>
        protected static readonly string revisionName = "revision";
        /// <summary>
        /// Index name for current directory
        /// </summary>
        protected static readonly string currentDirectoryName = "currentDirectory";
        /// <summary>
        /// Index name for variables
        /// </summary>
        protected static readonly string variablesName = "variables";
        /// <summary>
        /// Index name for data
        /// </summary>
        protected static readonly string dataName = "data";
        /// <summary>
        /// Index name for unique strings
        /// </summary>
        protected static readonly string uniqueName = "uniqueStrings";
        /// <summary>
        /// Index name for configuration
        /// </summary>
        protected static readonly string configurationName = "configuration";

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
        public static readonly string PrinterDirectory = Path.Combine(PersonalDirectory, "Printer");

        /// <summary>
        /// Default output language
        /// </summary>
        public static readonly string DefaultOutputLanguage = "C#.NET";

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        protected PrinterObject()
            : this(Path.Combine(PrinterDirectory, "languages", DefaultOutputLanguage), "1-0")
        {
        }

        /// <summary>
        /// Constructor with currentDirectory setted
        /// </summary>
        /// <param name="cd">current directory</param>
        /// <param name="version">version</param>
        protected PrinterObject(string cd, string version)
        {
            PrinterObject.InitializePersonalDirectory();
            this.Set(versionName, version);
            this.Set(revisionName, 0);
            this.Set(currentDirectoryName, cd);
            this.Set(variablesName, new Dictionary<string, PrinterVariable>());
            this.Set(dataName, new List<string>());
            this.Set(uniqueName, new UniqueStrings());
            this.Set(configurationName, new Configuration());
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the version
        /// </summary>
        public string Version
        {
            get
            {
                return this.Get(versionName, "1-0");
            }
            set
            {
                this.Set(versionName, value);
            }
        }

        /// <summary>
        /// Gets or sets the revision number
        /// </summary>
        public int Revision
        {
            get
            {
                return this.Get(revisionName, 0);
            }
            set
            {
                this.Set(revisionName, value);
            }
        }

        /// <summary>
        /// Gets or sets the current directory
        /// </summary>
        public string CurrentDirectory
        {
            get
            {
                return this.Get(currentDirectoryName, "");
            }
            set
            {
                this.Set(currentDirectoryName, value);
            }
        }

        /// <summary>
        /// Gets all variables
        /// </summary>
        protected Dictionary<string, PrinterVariable> Variables
        {
            get
            {
                return this.Get(variablesName, new Dictionary<string, PrinterVariable>());
            }
        }

        /// <summary>
        /// Gets all data
        /// </summary>
        protected List<string> Strings
        {
            get
            {
                return this.Get(dataName, new List<string>());
            }
        }

        /// <summary>
        /// Gets or sets all unique
        /// </summary>
        protected UniqueStrings Unique
        {
            get
            {
                return this.Get(uniqueName, new UniqueStrings());
            }
            set
            {
                this.Set(uniqueName, value);
            }
        }

        /// <summary>
        /// Gets or sets the configuration object
        /// </summary>
        public Configuration Configuration
        {
            get
            {
                return this.Get(configurationName, new Configuration());
            }
            set
            {
                if (value != null)
                    this.Set(configurationName, value);
            }
        }

        /// <summary>
        /// Gets all values
        /// </summary>
        public IEnumerable<PrinterVariable> Values
        {
            get { return this.Variables.Values; }
        }

        /// <summary>
        /// Gets all data sequence
        /// </summary>
        public IEnumerable<string> Datas
        {
            get { return this.Strings; }
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
            DirectoryInfo di = new DirectoryInfo(PrinterObject.PersonalDirectory);
            if (di.Exists)
            {
                DirectoryInfo diRootDir = new DirectoryInfo(PrinterObject.PrinterDirectory);
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
        /// Test if existing variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <returns>true if exist</returns>
        public bool ExistTestVariable(string key)
        {
            return this.Variables.ContainsKey(key);
        }

        /// <summary>
        /// Edit a variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="val">string value</param>
        public void EditVariable(string key, string val)
        {
            if (this.Variables.ContainsKey(key))
            {
                this.Variables[key].Value = val;
            }
            else
            {
                PrinterVariable p = new PrinterVariable();
                p.Name = key;
                p.Value = val;
                this.Variables.Add(key, p);
            }
        }

        /// <summary>
        /// Edit a variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="obj">object value</param>
        public void EditVariable(string key, PrinterVariable obj)
        {
            if (this.Variables.ContainsKey(key))
            {
                this.Variables[key] = obj.Clone() as PrinterVariable;
            }
            else
            {
                this.Variables.Add(key, obj.Clone() as PrinterVariable);
            }
        }

        /// <summary>
        /// Add a variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="obj">object value</param>
        public void AddVariable(string key, PrinterVariable obj)
        {
            if (this.Variables.ContainsKey(key))
            {
                this.Variables[key] = obj.Clone() as PrinterVariable;
            }
            else
            {
                this.Variables.Add(key, obj.Clone() as PrinterVariable);
            }
        }

        /// <summary>
        /// Add a variable
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="val">string value</param>
        public void AddVariable(string key, string val)
        {
            if (this.Variables.ContainsKey(key))
            {
                this.Variables[key].Value = val;
            }
            else
            {
                PrinterVariable p = new PrinterVariable();
                p.Name = key;
                p.Value = val;
                this.Variables.Add(key, p);
            }
        }

        /// <summary>
        /// Delete a variable
        /// </summary>
        /// <param name="key">key name</param>
        public void DeleteVariable(string key)
        {
            if (this.Variables.ContainsKey(key))
            {
                this.Variables.Remove(key);
            }
        }

        /// <summary>
        /// Use a variable
        /// </summary>
        /// <param name="name">variable name</param>
        public void UseVariable(string name)
        {
            this.Strings.Add("[" + name + "]");
        }

        /// <summary>
        /// Change the use of a variable
        /// </summary>
        /// <param name="index">position</param>
        /// <param name="name">variable name</param>
        public void UseChangeVariable(int index, string name)
        {
            this.Strings[index] = "[" + name + "]";
        }

        /// <summary>
        /// Insert the use of a variable before
        /// </summary>
        /// <param name="index">position</param>
        /// <param name="name">variable name</param>
        public void InsertUseVariableBefore(int index, string name)
        {
            this.Strings.Insert(index, "[" + name + "]");
        }

        /// <summary>
        /// Insert the use of a variable after
        /// </summary>
        /// <param name="index">position</param>
        /// <param name="name">variable name</param>
        public void InsertUseVariableAfter(int index, string name)
        {
            if (index + 1 < this.Strings.Count)
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
                string name = this.Unique.ComputeNewString();
                string p = s.Substring(1, s.Length - 2);
                this.AddVariable(name, p);
                this.Strings.Add("[" + name + "]");
            }
            else
            {
                this.Strings.Add(s);
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
                string name = this.Unique.ComputeNewString();
                string p = s.Substring(1, s.Length - 2);
                this.AddVariable(name, p);
                this.Strings[index] = "[" + name + "]";
            }
            else
            {
                this.Strings[index] = s;
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
                string name = this.Unique.ComputeNewString();
                string p = s.Substring(1, s.Length - 2);
                this.AddVariable(name, p);
                this.Strings.Insert(index, "[" + name + "]");
            }
            else
            {
                this.Strings.Insert(index, s);
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
                string name = this.Unique.ComputeNewString();
                string p = s.Substring(1, s.Length - 2);
                this.AddVariable(name, p);
                if (index + 1 < this.Strings.Count)
                    this.Strings.Insert(index + 1, "[" + name + "]");
                else
                    this.Strings.Add("[" + name + "]");
            }
            else
            {
                if (index + 1 < this.Strings.Count)
                    this.Strings.Insert(index + 1, s);
                else
                    this.Strings.Add(s);
            }
        }

        /// <summary>
        /// Delete data at index
        /// </summary>
        /// <param name="index">position</param>
        public void DeleteData(int index)
        {
            this.Strings.RemoveAt(index);
        }

        /// <summary>
        /// Write output as interpretation result
        /// </summary>
        /// <param name="w">writer</param>
        /// <param name="indentValue">space size</param>
        /// <param name="currentLine">in-progress line add</param>
        /// <param name="config">configuration</param>
        public void Execute(TextWriter w, ref int indentValue, ref string currentLine, Configuration config)
        {
            foreach (string e in this.Strings)
            {
                if (e.StartsWith("[") && e.EndsWith("]"))
                {
                    string r = e.Substring(1, e.Length - 2);
                    r = config.Execute(r);
                    if (this.ExistTestVariable(r))
                        this.Variables[r].Execute(w, ref indentValue, ref currentLine, config, this.CurrentDirectory);
                }
                else
                {
                    string val = config.Execute(e);
                    PrinterObject.IndentSource(w, indentValue, ref currentLine, val);
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
                this.Execute(tw, ref indentValue, ref currentLine, this.Configuration);
                tw.Close();
            }
            if (!String.IsNullOrEmpty(currentLine))
                sb.Append(currentLine);
            return sb.ToString();
        }

        /// <summary>
        /// Import a configuration values
        /// </summary>
        /// <param name="from">configuration values</param>
        public void ImportConfiguration(Configuration from)
        {
            foreach (string key in from)
            {
                this.Configuration.Add(key, from[key]);
            }
        }

        /// <summary>
        /// Load a file from disk
        /// </summary>
        /// <param name="fileName">full path of fileName</param>
        /// <returns>object</returns>
        public static PrinterObject Load(string fileName)
        {
            PersistentDataObject po = null;
            FileInfo fi = new FileInfo(fileName);
            if (fi.Exists)
                if (PersistentDataObject.Load(fi, out po))
                    return po as PrinterObject;

            return null;
        }


        /// <summary>
        /// Save a PrinterObject to disk
        /// </summary>
        /// <param name="obj">object to save</param>
        /// <param name="fileName">full path of fileName to save</param>
        public static void Save(PrinterObject obj, string fileName)
        {
            FileInfo fi = new FileInfo(fileName);
            obj.CurrentDirectory = fi.DirectoryName;
            PersistentDataObject.Save(fi, obj);
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
        /// Computes a new name 
        /// </summary>
        /// <returns>a new name</returns>
        public string ComputeNewString()
        {
            return this.Unique.ComputeNewString();
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
                this.Configuration.ToString(xml);
                xml.WriteStartElement("vars");
                foreach (KeyValuePair<string, PrinterVariable> kv in this.Variables)
                {
                    kv.Value.ToString(xml);
                }
                xml.WriteEndElement();
                xml.WriteStartElement("text");
                foreach (string s in this.Strings)
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
            foreach (string s in this.Datas)
            {
                newPo.Strings.Add(s.Clone() as string);
            }
            newPo.Unique = new UniqueStrings(this.Unique.Counter);
            foreach (PrinterVariable pv in this.Values)
            {
                newPo.Variables.Add(pv.Name, pv.Clone() as PrinterVariable);
            }
            return newPo;
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="pv">printer version</param>
        /// <returns>printer object</returns>
        public static PrinterObject Create(PrinterVersion pv)
        {
            PrinterObject po = new PrinterObject(Path.GetDirectoryName(pv.FullName), pv.LatestVersion);
            PrinterObject.Save(po, pv.FullName);
            return po;
        }

        #endregion
    }
}
