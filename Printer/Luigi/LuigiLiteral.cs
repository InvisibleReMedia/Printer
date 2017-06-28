using Printer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luigi
{
    /// <summary>
    /// A literal into Luigi language
    /// </summary>
    [Serializable]
    public class LuigiLiteral : LuigiElement
    {

        #region Fields

        /// <summary>
        /// Delimiter
        /// </summary>
        private string delimiter;
        /// <summary>
        /// Immediate switch
        /// </summary>
        private bool immediate;
        /// <summary>
        /// no name switch
        /// </summary>
        private bool automatic;

        #endregion

        #region Constructor

        /// <summary>
        /// Literal object
        /// </summary>
        /// <param name="n">type name of the object</param>
        /// <param name="im">immediate switch</param>
        /// <param name="d">delimiter</param>
        /// <param name="v">value string</param>
        /// <param name="p">parent</param>
        public LuigiLiteral(string n, bool im, string d, string v, LuigiElement p)
            : base(n, v, p)
        {
            this.automatic = false;
            this.immediate = im;
            this.delimiter = d;
        }

        /// <summary>
        /// Literal object
        /// </summary>
        /// <param name="d">delimiter</param>
        /// <param name="v">value string</param>
        /// <param name="p">parent</param>
        public LuigiLiteral(string d, string v, LuigiElement p)
            : base("", v, p)
        {
            this.automatic = true;
            this.immediate = false;
            this.delimiter = d;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the no name switch
        /// </summary>
        public bool IsAutomatic
        {
            get
            {
                return this.automatic;
            }
            set
            {
                this.automatic = value;
            }
        }

        /// <summary>
        /// Gets or sets the immediate switch
        /// </summary>
        public bool IsImmediate
        {
            get
            {
                return this.immediate;
            }
            set
            {
                this.immediate = value;
            }
        }

        /// <summary>
        /// Gets or sets the delimiter
        /// </summary>
        public string Delimiter
        {
            get
            {
                return this.delimiter;
            }
            set
            {
                this.delimiter = value;
            }
        }

        /// <summary>
        /// Gets or sets the content
        /// </summary>
        public string Content
        {
            get
            {
                return this.Value;
            }
            set
            {
                this.Value = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Execute the process of a list
        /// </summary>
        /// <param name="po">printer</param>
        /// <param name="indentValue">indent</param>
        public override void Execute(PrinterObject po, ref int indentValue)
        {
            if (this.IsAutomatic)
            {
                po.AddData("[" + this.Content + "]");
            }
            else
            {
                PrinterObject poLiteral = new PrinterObject(po.CurrentDirectory);
                poLiteral.Configuration.Edit("programmingLanguage", po.Configuration["programmingLanguage"]);
                poLiteral.Configuration.Add("delimiter", this.Delimiter);
                poLiteral.Configuration.Add("content", this.Content);
                poLiteral.AddVariable("delimiter", "@delimiter");
                poLiteral.AddVariable("value", "@content");
                poLiteral.UseVariable("value");
                PrinterObject.Save(poLiteral, Path.Combine(PrinterObject.PrinterDirectory, "compiled", this.Name + ".prt"));
            }
        }

        /// <summary>
        /// Converts this object into a string representation (source code)
        /// </summary>
        /// <returns>string representation as the source code</returns>
        public override string ToString()
        {
            PrinterObject po = null;
            if (this.automatic)
            {
                po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "literal-au.prt"));
            }
            else
            {
                if (this.immediate)
                {
                    po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "literal-im.prt"));
                }
                else
                {
                    po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "literal-src.prt"));
                }
            }
            po.Configuration.Add("typeName", this.Name);
            po.Configuration.Add("delimiter", this.Delimiter);
            po.Configuration.Add("value", this.Value);
            return po.Execute();
        }

        /// <summary>
        /// Copy this into a new element
        /// </summary>
        /// <param name="parent">parent</param>
        /// <returns>a new element</returns>
        public override LuigiElement CopyInto(LuigiElement parent)
        {
            LuigiLiteral l;
            if (this.IsAutomatic)
            {
                l = new LuigiLiteral(this.Delimiter, this.Value, parent);
            }
            else
            {
                l = new LuigiLiteral(this.Name, this.IsImmediate, this.Delimiter, this.Value, parent);
            }
            return l;
        }

        #endregion
    }
}
