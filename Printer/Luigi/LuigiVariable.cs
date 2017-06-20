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
    /// A variable into Luigi language
    /// </summary>
    [Serializable]
    public class LuigiVariable : LuigiElement
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n">name of variable</param>
        /// <param name="v">luigi element</param>
        /// <param name="p">parent</param>
        public LuigiVariable(string n, LuigiElement v, LuigiElement p)
            : base(n, v, p)
        {
            if (this.Content != null && (this.Content is LuigiLiteral || this.Content is LuigiMapper || this.Content is LuigiSet))
                this.Value.IsAutomatic = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the content of this variable
        /// </summary>
        public LuigiElement Content
        {
            get
            {
                return this.Value;
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
            this.Content.Execute(po, ref indentValue);
        }

        /// <summary>
        /// Converts this object into a string representation (source code)
        /// </summary>
        /// <returns>string representation as the source code</returns>
        public override string ToString()
        {
            PrinterObject po;
            po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "var.prt"));
            po.Configuration.Add("varName", this.Name);
            po.Configuration.Add("value", this.Content.ToString());
            return po.Execute();
        }

        /// <summary>
        /// Copy this into a new element
        /// </summary>
        /// <param name="parent">parent</param>
        /// <returns>a new element</returns>
        public override LuigiElement CopyInto(LuigiElement parent)
        {
            return new LuigiVariable(this.Name, this.Value, parent);
        }

        #endregion
    }
}
