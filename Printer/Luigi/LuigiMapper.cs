using Printer;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luigi
{
    public class LuigiMapper : LuigiElement
    {

        #region Fields

        /// <summary>
        /// Immediate switch
        /// </summary>
        private bool immediate;

        #endregion

        #region Constructor

        /// <summary>
        /// Mapper object
        /// </summary>
        /// <param name="n">type name of the object</param>
        /// <param name="v">dictionary of string</param>
        public LuigiMapper(string n, LuigiList v) : base(n, v)
        {
        }

        /// <summary>
        /// Mapper object
        /// </summary>
        /// <param name="n">type name of the object</param>
        public LuigiMapper(string n) : base(n, new LuigiList("map", "LuigiLiteral"))
        {
        }

        #endregion

        #region Properties

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

        public Dictionary<string, LuigiLiteral> Keys
        {
            get
            {
                return (this.Value as LuigiList).Elements.Values.Cast<LuigiLiteral>().ToDictionary(x => x.Name);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Execute this literal statement
        /// </summary>
        /// <param name="w">writer</param>
        /// <param name="indentValue">indent size</param>
        public override void Execute(TextWriter w, ref int indentValue)
        {
            PrinterObject po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "mapper.prt"));
            po.Configuration.Add("typeName", this.Name);
            string objects = string.Empty;
            foreach(LuigiLiteral l in this.Value)
            po.Configuration.Add("items", );
            po.Execute(w, ref indentValue, po.Configuration);
        }

        /// <summary>
        /// Converts this object into a string representation (source code)
        /// </summary>
        /// <returns>string representation as the source code</returns>
        public override string ToString()
        {
            PrinterObject po = null;
            if (this.immediate)
            {
                po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "literal-im.prt"));
            }
            else
            {
                po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "literal-src.prt"));
            }
            po.Configuration.Add("typeName", this.Name);
            po.Configuration.Add("delimiter", this.Delimiter);
            po.Configuration.Add("value", this.Value);
            return po.Execute();
        }

        #endregion
    }
}
