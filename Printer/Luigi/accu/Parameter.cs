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
    /// Is an element parameter of a set
    /// </summary>
    [Serializable]
    public class Parameter : Accu.Accu
    {

        #region Fields

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
        /// <param name="n">name</param>
        /// <param name="v">value</param>
        /// <param name="p">parent</param>
        public Parameter(string n, dynamic v, dynamic p)
            : base(false, false, false, n, null)
        {
            this.parent = p;
            this.root = p.Root;
            this.AddElement(new Accu.Accu(false, true, false, "type", this.GetType().Name));
            this.AddElement(new Accu.Accu(false, true, false, "print", "result"));
            this.AddElement(new Accu.Accu(false, true, false, "innerType", v.GetType().Name));
            this.AddElement(new Accu.Accu(false, true, false, "value", v));
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
        /// Gets the type name
        /// </summary>
        public string TypeName
        {
            get
            {
                return this.FindByName("type").Value;
            }
        }

        /// <summary>
        /// Gets or sets the inner type of inner object
        /// </summary>
        public string InnerTypeName
        {
            get
            {
                return this.FindByName("innerType").Value;
            }
            set
            {
                this.FindByName("innerType").Value = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts this object into a string representation (source code)
        /// </summary>
        /// <returns>string representation as the source code</returns>
        public override string ToString()
        {
            PrinterObject po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "param.prt"));
            string paramSwitch = string.Empty;
            switch (this.InnerTypeName)
            {
                case "Literal":
                    paramSwitch = "-";
                    break;
                case "Mapper":
                    paramSwitch = "%";
                    break;
                case "Set":
                    paramSwitch = "@";
                    break;
            }
            po.Configuration.Add("paramSwitch", paramSwitch);
            po.Configuration.Add("paramName", this.Name);
            po.Configuration.Add("paramValue", this.Value.ToString());
            return po.Execute();
        }

        #endregion

    }
}
