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
    /// Operator of printing element
    /// </summary>
    [Serializable]
    public class LuigiPrint : LuigiElement
    {

        #region Constructor

        /// <summary>
        /// Print object
        /// </summary>
        /// <param name="n">type name of the object</param>
        /// <param name="v">value</param>
        /// <param name="p">parent</param>
        public LuigiPrint(string n, LuigiElement v, LuigiElement p)
            : base(n, null, p)
        {
            this.Name = v.Name;
            this.Value = new LuigiPolymorph("poly", v, this);
        }

        /// <summary>
        /// Print object
        /// </summary>
        /// <param name="n">type name of the object</param>
        /// <param name="s">name of the element</param>
        /// <param name="p">parent</param>
        public LuigiPrint(string n, string s, LuigiElement p)
            : base(n, null, p)
        {
            this.Name = s;
            this.Value = new LuigiPolymorph("poly", this.Root.Find(s), this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a polymorph object
        /// </summary>
        public LuigiPolymorph PolymorphObject
        {
            get
            {
                return this.Value as LuigiPolymorph;
            }
        }

        /// <summary>
        /// Gets the inner object to print
        /// </summary>
        public LuigiElement Object
        {
            get
            {
                return (this.Value as LuigiPolymorph).Value;
            }
        }

        /// <summary>
        /// Gets the name of the inner object to print
        /// </summary>
        public string ObjectName
        {
            get
            {
                return this.Object.Name;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Execute operation
        /// </summary>
        /// <param name="po">printer output</param>
        /// <param name="indentValue">indent size</param>
        public override void Execute(Printer.PrinterObject po, ref int indentValue)
        {
            this.PolymorphObject.Execute(po, ref indentValue);
        }

        /// <summary>
        /// Copy this into a new element
        /// </summary>
        /// <param name="parent">parent for the new element</param>
        /// <returns>new element</returns>
        public override LuigiElement CopyInto(LuigiElement parent)
        {
            return new LuigiPrint(this.Name, this.Object, parent);
        }

        /// <summary>
        /// Converts this object into a string representation (source code)
        /// </summary>
        /// <returns>string representation as the source code</returns>
        public override string ToString()
        {
            PrinterObject po;
            po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "print.prt"));
            po.Configuration.Add("varName", this.Name);
            po.Configuration.Add("expr", this.PolymorphObject.ToString());
            return po.Execute();
        }

        #endregion

    }
}
