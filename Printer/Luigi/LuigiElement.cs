using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Luigi
{
    public abstract class LuigiElement
    {

        #region Fields

        /// <summary>
        /// Name of the object
        /// </summary>
        private string name;
        /// <summary>
        /// Value object
        /// </summary>
        private dynamic value;

        /// <summary>
        /// Size indent space char
        /// </summary>
        public static readonly string IndentString = " ";

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="n">name</param>
        /// <param name="t">type name</param>
        /// <param name="v">value</param>
        protected LuigiElement(string n, dynamic v)
        {
            this.name = n;
            this.value = v;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the type name
        /// </summary>
        public string TypeName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public dynamic Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
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
        public static void IndentSource(TextWriter sw, int n, string source)
        {
            string indent = String.Empty;
            for (int index = 0; index < n; ++index) indent += IndentString;
            Regex reg = new Regex(Environment.NewLine);
            sw.Write(reg.Replace(source, Environment.NewLine + indent));
        }

        /// <summary>
        /// Write output as interpretation result
        /// </summary>
        /// <param name="w">writer</param>
        /// <param name="indentValue">space size</param>
        /// <param name="config">configuration</param>
        public abstract void Execute(TextWriter w, ref int indentValue);

        /// <summary>
        /// Write output as interpretation result
        /// </summary>
        /// <returns>output</returns>
        public string Execute()
        {
            int indentValue = 0;
            StringBuilder sb = new StringBuilder();
            using (TextWriter tw = new StringWriter(sb))
            {
                this.Execute(tw, ref indentValue);
                IndentSource(tw, indentValue, Environment.NewLine);
                tw.Close();
            }
            return sb.ToString();
        }

        /// <summary>
        /// Load a file from disk
        /// </summary>
        /// <param name="fileName">full path of fileName</param>
        /// <returns>object</returns>
        public static LuigiElement Load(string fileName)
        {
            LuigiElement po = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    po = bf.Deserialize(fs) as LuigiElement;
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
        /// Save a LuigiElement to disk
        /// </summary>
        /// <param name="obj">object to save</param>
        /// <param name="fileName">full path of fileName to save</param>
        public static void Save(LuigiElement obj, string fileName)
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
        public static LuigiElement Load(Stream stream)
        {
            LuigiElement po = null;
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                po = bf.Deserialize(stream) as LuigiElement;
            }
            catch (Exception)
            {
                throw;
            }

            return po;
        }


        /// <summary>
        /// Save a LuigiElement to memory
        /// You must close the stream after this method
        /// </summary>
        /// <param name="obj">object to save</param>
        /// <param name="stream">stream buffer</param>
        public static void Save(LuigiElement obj, Stream stream)
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
        /// of this LuigiElement
        /// </summary>
        /// <returns>the string representation</returns>
        public override string ToString()
        {
            StringBuilder output = new StringBuilder();

            return output.ToString();
        }

        #endregion


    }
}
