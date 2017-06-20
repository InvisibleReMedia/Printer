using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace Printer
{
    /// <summary>
    /// Class of configuration
    /// variables definition
    /// </summary>
    [Serializable]
    public class Configuration : ICloneable
    {
        #region Fields

        private static readonly string Indentifier = "@";
        /// <summary>
        /// Values
        /// </summary>
        private Dictionary<string, string> values;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Configuration()
        {
            this.values = new Dictionary<string, string>();
            this.values.Add("author", Environment.GetEnvironmentVariable("USERNAME"));
            this.values.Add("date", DateTime.Now.ToShortDateString());
            this.values.Add("programmingLanguage", "C");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Count the number of conf
        /// </summary>
        public int Count
        {
            get { return this.values.Count; }
        }

        /// <summary>
        /// Gets or sets a value key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        public string this[string key]
        {
            get
            {
                if (this.values.ContainsKey(key))
                    return this.values[key];
                else
                    return string.Empty;
            }
            set
            {
                this.Edit(key, value);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add an item
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public void Add(string key, string value)
        {
            if (this.values.ContainsKey(key))
            {
                this.values[key] = value;
            }
            else
            {
                this.values.Add(key, value);
            }
        }

        /// <summary>
        /// Edit an item
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public void Edit(string key, string value)
        {
            if (this.values.ContainsKey(key))
            {
                this.values[key] = value;
            }
            else
            {
                this.values.Add(key, value);
            }
        }

        /// <summary>
        /// Suppress a key
        /// </summary>
        /// <param name="key">key</param>
        public void Delete(string key)
        {
            if (this.values.ContainsKey(key))
                this.values.Remove(key);
        }

        /// <summary>
        /// Find all occurences
        /// </summary>
        /// <param name="s">string to compare</param>
        /// <returns>found matches keys</returns>
        public IEnumerable<string> Find(string s)
        {
            List<string> find = new List<string>();
            foreach (string key in this.values.Keys)
            {
                if (s.StartsWith(key))
                {
                    find.Add(key);
                }
            }
            return find;
        }

        /// <summary>
        /// Gets the enumerator to iterate
        /// conf
        /// </summary>
        /// <returns></returns>
        public IEnumerator<string> GetEnumerator()
        {
            return this.values.Keys.GetEnumerator();
        }

        /// <summary>
        /// Clone this object
        /// </summary>
        /// <returns>new object</returns>
        public object Clone()
        {
            Configuration newConf = new Configuration();
            newConf.values = new Dictionary<string, string>(this.values);
            return newConf;
        }

        /// <summary>
        /// Execute commands
        /// </summary>
        /// <param name="input">input string</param>
        /// <returns>edited string</returns>
        public string Execute(string input)
        {
            StringBuilder sb = new StringBuilder();
            Regex reg = new Regex("(\\" + Configuration.Indentifier + @"([a-zA-Z][a-zA-Z\-_]+))|(\n|\r|" + Environment.NewLine + ")|([^\r\n" + Configuration.Indentifier + "]*)", RegexOptions.Multiline);
            MatchCollection mCol = reg.Matches(input);
            foreach (Match m in mCol)
            {
                if (m.Groups[2].Success)
                {
                    IEnumerable<string> selected = this.Find(m.Groups[2].Value);
                    int gSize = selected.Max(x => x.Length);
                    string f = selected.First(x => x.Length == gSize);
                    sb.Append(this[f]);
                }
                else
                {
                    sb.Append(m.Groups[0].Value);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Generates the source code
        /// of this Configuration
        /// </summary>
        /// <param name="xml"></param>
        /// <returns>the string representation</returns>
        public void ToString(XmlWriter xml)
        {
            xml.WriteStartElement("conf");
            foreach (string key in this.values.Keys)
            {
                xml.WriteStartElement("itemConf");
                xml.WriteAttributeString("name", key);
                xml.WriteString(this.values[key]);
                xml.WriteEndElement();
            }
            xml.WriteEndElement();
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
                this.ToString(xml);
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
        /// Load a file from disk
        /// </summary>
        /// <param name="fileName">full path of fileName</param>
        /// <returns>object</returns>
        public static Configuration Load(string fileName)
        {
            Configuration c = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    c = bf.Deserialize(fs) as Configuration;
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

            return c;
        }


        /// <summary>
        /// Save a PrinterObject to disk
        /// </summary>
        /// <param name="obj">object to save</param>
        /// <param name="fileName">full path of fileName to save</param>
        public static void Save(Configuration obj, string fileName)
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

        #endregion

    }
}