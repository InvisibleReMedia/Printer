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
    /// <summary>
    /// A mapper into Luigi language
    /// </summary>
    [Serializable]
    public class LuigiMapper : LuigiElement
    {

        #region Fields

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
        /// Mapper object
        /// </summary>
        /// <param name="n">type name of the object</param>
        /// <param name="imm">immediate switch</param>
        /// <param name="v">dictionary of string</param>
        /// <param name="p">parent</param>
        public LuigiMapper(string n, bool imm, LuigiDictionary v, LuigiElement p)
            : base(n, v, p)
        {
            this.automatic = false;
            this.immediate = imm;
        }

        /// <summary>
        /// Mapper object
        /// </summary>
        /// <param name="v">dictionary of string</param>
        /// <param name="p">parent</param>
        public LuigiMapper(LuigiDictionary v, LuigiElement p)
            : base("", v, p)
        {
            this.immediate = false;
            this.automatic = true;
        }

        /// <summary>
        /// Mapper object
        /// </summary>
        /// <param name="n">type name of the object</param>
        /// <param name="imm">immediate switch</param>
        /// <param name="p">parent</param>
        public LuigiMapper(string n, bool imm, LuigiElement p)
            : base(n, null, p)
        {
            this.automatic = false;
            this.immediate = imm;
            this.Value = new LuigiDictionary("map", "LuigiLiteral", this);
        }

        /// <summary>
        /// Mapper object
        /// </summary>
        /// <param name="p">parent</param>
        public LuigiMapper(LuigiElement p)
            : base("", null, p)
        {
            this.automatic = true;
            this.immediate = false;
            this.Value = new LuigiDictionary("map", "LuigiLiteral", this);
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
        /// Gets the keys of mapper
        /// </summary>
        public LuigiDictionary Keys
        {
            get
            {
                return this.Value;
            }
        }

        /// <summary>
        /// Gets the exec function which route selected
        /// key to its literal
        /// </summary>
        public Func<string, LuigiLiteral> ExecuteCall
        {
            get
            {
                return c =>
                {
                    return this.Keys.Elements[c] as LuigiLiteral;
                };
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a new element
        /// </summary>
        /// <param name="r">reference to add</param>
        public void AddReference(LuigiReference r)
        {
            if (r.ReferencedObject is LuigiLiteral)
            {
                LuigiLiteral lit = r.ReferencedObject as LuigiLiteral;
                lit.IsImmediate = true;
                this.Keys.AddElement(lit);
            }
        }

        /// <summary>
        /// Add a new element
        /// </summary>
        /// <param name="lit">literal to add</param>
        public void AddElement(LuigiLiteral lit)
        {
            lit.IsImmediate = true;
            this.Keys.AddElement(lit);
        }

        /// <summary>
        /// Edit an element
        /// </summary>
        /// <param name="r">reference to add</param>
        public void EditReference(LuigiReference r)
        {
            if (r.ReferencedObject is LuigiLiteral)
            {
                LuigiLiteral lit = r.ReferencedObject as LuigiLiteral;
                lit.IsImmediate = true;
                this.Keys.EditElement(lit);
            }
        }

        /// <summary>
        /// Edit an element
        /// </summary>
        /// <param name="lit">literal to add</param>
        public void EditElement(LuigiLiteral lit)
        {
            lit.IsImmediate = true;
            this.Keys.EditElement(lit);
        }

        /// <summary>
        /// Change the name of an element
        /// </summary>
        /// <param name="oldName">old name</param>
        /// <param name="newName">new name</param>
        public void ChangeName(string oldName, string newName)
        {
            this.Keys.ChangeName(oldName, newName);
        }

        /// <summary>
        /// Remove an existing element
        /// </summary>
        /// <param name="nameToRemove">name to remove</param>
        public void RemoveElement(string nameToRemove)
        {
            this.Keys.RemoveElement(nameToRemove);
        }

        /// <summary>
        /// Execute the process of a list
        /// </summary>
        /// <param name="po">printer</param>
        /// <param name="indentValue">indent</param>
        public override void Execute(PrinterObject po, ref int indentValue)
        {
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
                po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "mapper-au.prt"));
            }
            else
            {
                if (this.immediate)
                {
                    po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "mapper-im.prt"));
                }
                else
                {
                    po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "mapper-src.prt"));
                }
            }
            po.Configuration.Add("typeName", this.Name);
            string objects = string.Empty;
            foreach (KeyValuePair<string, LuigiElement> l in this.Keys.Elements)
            {
                objects += Environment.NewLine + l.Key + " => ";
                objects += l.Value.ToString();
            }
            po.Configuration.Add("items", objects);
            return po.Execute();
        }

        /// <summary>
        /// Copy this into a new element
        /// </summary>
        /// <param name="parent">parent</param>
        /// <returns>a new element</returns>
        public override LuigiElement CopyInto(LuigiElement parent)
        {
            LuigiMapper m;
            if (this.IsAutomatic)
            {
                m = new LuigiMapper(parent);
            }
            else
            {
                m = new LuigiMapper(this.Name, this.IsImmediate, parent);
            }
            foreach (KeyValuePair<string, LuigiElement> kv in this.Keys.Elements)
            {
                m.Keys.AddElement(kv.Value.CopyInto(m));
            }
            return m;
        }

        #endregion
    }
}
