﻿using Printer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luigi
{
    class LuigiLiteral : LuigiElement
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

        #endregion

        #region Constructor

        /// <summary>
        /// Literal object
        /// </summary>
        /// <param name="n">type name of the object</param>
        /// <param name="d">delimiter</param>
        /// <param name="v">value string</param>
        public LuigiLiteral(string n, string d, string v) : base(n, v)
        {
            this.delimiter = d;
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

        #endregion

        #region Methods

        /// <summary>
        /// Execute this literal statement
        /// </summary>
        /// <param name="w">writer</param>
        /// <param name="indentValue">indent size</param>
        public override void Execute(TextWriter w, ref int indentValue)
        {
            PrinterObject po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "literal.prt"));
            po.Configuration.Add("typeName", this.Name);
            po.Configuration.Add("delimiter", this.Delimiter);
            po.Configuration.Add("value", this.Value);
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