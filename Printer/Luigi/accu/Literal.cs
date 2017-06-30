﻿using Printer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luigi.accu
{
    /// <summary>
    /// State a literal
    /// </summary>
    [Serializable]
    public class Literal
    {

        #region Fields

        /// <summary>
        /// Accumulator
        /// </summary>
        private Accu.Accu accu;

        /// <summary>
        /// Top level class (use for a get reference object)
        /// </summary>
        private TopLevel root;

        /// <summary>
        /// Parent object
        /// </summary>
        private dynamic parent;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="n"></param>
        /// <param name="p">parent</param>
        public Literal(string n, dynamic p)
        {
            this.parent = p;
            this.root = p.Root;
            this.accu = new Accu.Accu(false, false, n, this);
            this.accu.AddElement(new Accu.Accu(true, false, "type", this.GetType().Name));
            this.accu.AddElement(new Accu.Accu(false, true, "delimiter", "."));
            this.accu.AddElement(new Accu.Accu(false, true, "text", "text"));
            this.accu.AddElement(new Accu.Accu(false, true, "print", "result"));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the root parent
        /// </summary>
        public TopLevel Root
        {
            get
            {
                return this.root;
            }
        }

        /// <summary>
        /// Gets the parent
        /// </summary>
        public dynamic Parent
        {
            get
            {
                return this.parent;
            }
        }

        /// <summary>
        /// Gets the name
        /// </summary>
        public string Name
        {
            get
            {
                return this.accu.Name;
            }
        }

        /// <summary>
        /// Gets or sets the delimiter
        /// </summary>
        public string Delimiter
        {
            get
            {
                return this.accu.FindByName("delimiter").Value;
            }
            set
            {
                this.accu.FindByName("delimiter").Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the content
        /// </summary>
        public string Text
        {
            get
            {
                return this.accu.FindByName("text").Value;
            }
            set
            {
                this.accu.FindByName("text").Value = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Apply
        /// </summary>
        /// <param name="pars">parameters</param>
        /// <returns>string result</returns>
        public string Apply(Dictionary<string, string> pars)
        {
            string output = string.Empty;
            switch (pars.ElementAt(0).Key)
            {
                case "delimiter":
                    output = this.Delimiter;
                    break;
                case "text":
                    output = this.Text;
                    break;
                case "print":
                    // TODO : purpose parameters
                    output = this.Text;
                    break;
            }
            return output;
        }

        /// <summary>
        /// Converts this object into a string representation (source code)
        /// </summary>
        /// <returns>string representation as the source code</returns>
        public override string ToString()
        {
            PrinterObject po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "literal.prt"));
            po.Configuration.Add("delimiter", this.Delimiter);
            po.Configuration.Add("value", this.Text);
            return po.Execute();
        }

        #endregion

    }
}
