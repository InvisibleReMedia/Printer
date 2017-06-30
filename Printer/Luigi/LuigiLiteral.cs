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
            this.data.AddElement(new Accu.Accu("delimiter", d));
            this.data.AddElement(new Accu.Accu("automatic", false));
            this.data.AddElement(new Accu.Accu("immediate", im));
        }

        /// <summary>
        /// Literal object
        /// </summary>
        /// <param name="d">delimiter</param>
        /// <param name="v">value string</param>
        /// <param name="p">parent</param>
        public LuigiLiteral(string d, string v, LuigiElement p)
            : base(p.Root.ComputeNewString(), v, p)
        {
            this.data.AddElement(new Accu.Accu("delimiter", d));
            this.data.AddElement(new Accu.Accu("automatic", true));
            this.data.AddElement(new Accu.Accu("immediate", false));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the source name
        /// unique name to serve a file name
        /// prefix usr if user-defined name
        /// prefix auto if automatic object
        /// </summary>
        public string SourceName
        {
            get
            {
                string name;
                if (this.IsAutomatic)
                {
                    name = "auto_" + this.Name;
                }
                else
                {
                    name = "usr_" + this.Name;
                }
                return name;
            }
        }

        /// <summary>
        /// Gets or sets the no name switch
        /// </summary>
        public bool IsAutomatic
        {
            get
            {
                return this.data.FindByName("automatic").Value;
            }
            set
            {
                this.data.FindByName("automatic").Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the immediate switch
        /// </summary>
        public bool IsImmediate
        {
            get
            {
                return this.data.FindByName("immediate").Value;
            }
            set
            {
                this.data.FindByName("immediate").Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the delimiter
        /// </summary>
        public string Delimiter
        {
            get
            {
                return this.data.FindByName("delimiter").Value;
            }
            set
            {
                this.data.FindByName("delimiter").Value = value;
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
        /// Write output as interpretation result
        /// </summary>
        /// <param name="pars">parameters</param>
        /// <returns>string value</returns>
        public override string Execute(Dictionary<string, string> pars)
        {
            PrinterObject poLiteral = new PrinterObject();
            poLiteral.Configuration.Edit("programmingLanguage", "Luigi");
            poLiteral.Configuration.Add("delimiter", this.Delimiter);
            poLiteral.Configuration.Add("content", this.Content);
            poLiteral.AddVariable("delimiter", "@delimiter");
            poLiteral.AddVariable("value", "@content");
            poLiteral.UseVariable("value");
            PrinterObject.Save(poLiteral, Path.Combine(PrinterObject.PrinterDirectory, "compiled", this.SourceName + ".prt"));

            return poLiteral.Execute();
        }

        /// <summary>
        /// Converts this object into a string representation (source code)
        /// </summary>
        /// <returns>string representation as the source code</returns>
        public override string ToString()
        {
            PrinterObject po = null;
            if (this.IsAutomatic)
            {
                po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "literal-au.prt"));
            }
            else
            {
                if (this.IsImmediate)
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
