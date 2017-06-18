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

        #endregion

        #region Constructor

        /// <summary>
        /// Mapper object
        /// </summary>
        /// <param name="n">type name of the object</param>
        /// <param name="imm">immediate switch</param>
        /// <param name="v">dictionary of string</param>
        /// <param name="p">parent</param>
        public LuigiMapper(string n, bool imm, LuigiDictionary v, LuigiElement p) : base(n, v, p)
        {
            this.immediate = imm;
        }

        /// <summary>
        /// Mapper object
        /// </summary>
        /// <param name="n">type name of the object</param>
        /// <param name="imm">immediate switch</param>
        /// <param name="p">parent</param>
        public LuigiMapper(string n, bool imm, LuigiElement p) : base(n, null, p)
        {
            this.immediate = imm;
            this.Value = new LuigiDictionary("map", "LuigiLiteral", this);
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
        /// <param name="lit">literal to add</param>
        public void AddElement(LuigiLiteral lit)
        {
            lit.IsImmediate = true;
            this.Keys.AddElement(lit);
        }

        /// <summary>
        /// Add a new element
        /// </summary>
        /// <param name="lit">literal to add</param>
        public void EditElement(LuigiLiteral lit)
        {
            lit.IsImmediate = true;
            this.Keys.EditElement(lit);
        }

        /// <summary>
        /// Add a new element
        /// </summary>
        /// <param name="oldName">old name</param>
        /// <param name="newName">new name</param>
        public void ChangeName(string oldName, string newName)
        {
            this.Keys.ChangeName(oldName, newName);
        }

        /// <summary>
        /// Add a new element
        /// </summary>
        /// <param name="nameToRemove">name to remove</param>
        public void RemoveElement(string nameToRemove)
        {
            this.Keys.RemoveElement(nameToRemove);
        }

        /// <summary>
        /// Execute this literal statement
        /// </summary>
        /// <param name="w">writer</param>
        /// <param name="indentValue">indent size</param>
        public override void Execute(TextWriter w, ref int indentValue)
        {
            throw new NotImplementedException();
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
                po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "mapper-im.prt"));
            }
            else
            {
                po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "mapper-src.prt"));
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

        #endregion
    }
}
