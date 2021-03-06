﻿using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Xsl;

namespace Luigi
{
    /// <summary>
    /// A printer variable
    /// </summary>
    [Serializable]
    public class LuigiVariable : ICloneable
    {

        #region Fields

        /// <summary>
        /// Name of the variable
        /// </summary>
        protected string name;
        /// <summary>
        /// Value of the variable
        /// </summary>
        protected string value;
        /// <summary>
        /// Left-value type
        /// </summary>
        protected LuigiVariableType leftValueType;
        /// <summary>
        /// Right-value type
        /// </summary>
        protected LuigiVariableType rightValueType;

        #endregion

        #region Inner Class

        /// <summary>
        /// Type of variable
        /// </summary>
        public enum LuigiVariableType
        {
            /// <summary>
            /// Indicates a $ expression
            /// </summary>
            VAR,
            /// <summary>
            /// Indicates a reference to an accu
            /// </summary>
            REF,
            /// <summary>
            /// Indicates a constant (only right-value allowed)
            /// </summary>
            CONST,
            /// <summary>
            /// Indicates a new instance of an accu
            /// </summary>
            NEW
        }

#endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public LuigiVariable()
        {
            this.leftValueType = LuigiVariableType.VAR;
            this.rightValueType = LuigiVariableType.REF;
        }

        #endregion

        #region Properties

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
        /// Gets the left value type
        /// </summary>
        public LuigiVariableType LeftValueType
        {
            get
            {
                return this.leftValueType;
            }
            set
            {
                this.leftValueType = value;
            }
        }

        /// <summary>
        /// Gets the right value type
        /// </summary>
        public LuigiVariableType RightValueType
        {
            get
            {
                return this.rightValueType;
            }
            set
            {
                this.rightValueType = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Recursive find by name starting by a reference
        /// </summary>
        /// <param name="vars">all variables</param>
        /// <param name="types">all types</param>
        /// <param name="name">name to find</param>
        /// <returns>an accu child</returns>
        public static Accumulate.AccuChild RecursiveFindByName(Dictionary<string, LuigiVariable> vars, Dictionary<string, Accumulate.Accu> types, string name)
        {
            string[] seq = name.Split('.');
            if (seq.Length > 1)
            {
                if (vars.ContainsKey(seq[0].Substring(1)))
                {
                    LuigiVariable v = vars[seq[0].Substring(1)];
                    if (v.LeftValueType == LuigiVariableType.VAR && v.RightValueType == LuigiVariableType.NEW)
                    {
                        // value est une reference sur un accu
                        Accumulate.Accu a = Accumulate.Accu.FindByName(types, v.Value);
                        Accumulate.AccuChild c = Accumulate.AccuChild.RecursiveFindByName(a, 1, seq);
                    }
                    else
                    {
                        throw new FormatException(String.Format("{0} is not a reference accu", seq[0]));
                    }
                }
                else
                {
                    throw new KeyNotFoundException(String.Format("Variable name {0} not found", seq[0]));
                }
            }
            else
                throw new ArgumentException("bad name", "name");
        }

        /// <summary>
        /// Execute the variable
        /// </summary>
        /// <param name="w">writer</param>
        /// <param name="indentValue">space size</param>
        /// <param name="currentLine">in-progress line add</param>
        /// <param name="config">configuration</param>
        /// <param name="dir">directory</param>
        public void Execute(TextWriter w, ref int indentValue, ref string currentLine, Dictionary<string, LuigiVariable> vars, Dictionary<string,Accumulate.Accu> types, string dir)
        {
            if (LeftValueType == LuigiVariableType.VAR)
            {
            }
            else if (LeftValueType == LuigiVariableType.REF)
            {
                Accumulate.AccuChild a = Accumulate.Accu.RecursiveFindByName(types, this.name);
                if (RightValueType == LuigiVariableType.VAR)
                {
                    if (vars.ContainsKey(this.value))
                    {
                        a.Value = vars[this.value].Value;
                    }
                }
                else if (RightValueType == LuigiVariableType.CONST)
                {
                    a.Value = this.Value;
                }
                else if (RightValueType == LuigiVariableType.NEW)
                {
                    Accumulate.Accu n = Accumulate.Accu.FindByName(types, this.Value);
                    a.SetReference(n.TypeName);
                }
                else
                {
                    // gets the variable element
                    // set the accuchild with an another accuchild
                    Accumulate.AccuChild b = Accumulate.Accu.RecursiveFindByName(types, this.name);
                }
            }

        }

        /// <summary>
        /// Write output as interpretation result
        /// </summary>
        /// <param name="types">types</param>
        /// <param name="dir">directory</param>
        /// <returns>output</returns>
        public string Execute(Dictionary<string, LuigiVariable> vars, Dictionary<string, Accumulate.Accu> types, string dir)
        {
            int indentValue = 0;
            string currentLine = string.Empty;
            StringBuilder sb = new StringBuilder();
            using (TextWriter tw = new StringWriter(sb))
            {
                this.Execute(tw, ref indentValue, ref currentLine, vars, types, dir);
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
            xml.WriteStartElement("affect");
            xml.WriteAttributeString("leftValue", this.leftValueType.ToString());
            xml.WriteAttributeString("name", this.name);
            xml.WriteAttributeString("rightValue", this.rightValueType.ToString());
            xml.WriteAttributeString("value", this.value);
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
            LuigiVariable pv = new LuigiVariable();
            pv.Name = this.Name.Clone() as string;
            if (!String.IsNullOrEmpty(this.Value))
                pv.Value = this.Value.Clone() as string;
            pv.leftValueType = this.leftValueType;
            pv.rightValueType = this.rightValueType;
            return pv;
        }

        #endregion

    }
}
