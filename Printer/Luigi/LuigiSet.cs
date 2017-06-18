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
    /// A set into Luigi language
    /// </summary>
    [Serializable]
    public class LuigiSet : LuigiElement
    {

        #region Fields

        /// <summary>
        /// Immediate switch
        /// </summary>
        private bool immediate;

        /// <summary>
        /// Function to exec
        /// </summary>
        private LuigiFunction fun;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n">type name</param>
        /// <param name="im">immediate switch</param>
        /// <param name="v">list of parameters</param>
        /// <param name="f">expression</param>
        /// <param name="p">parent</param>
        public LuigiSet(string n, bool im, LuigiDictionary v, LuigiFunction f, LuigiElement p) : base(n, v, p)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n">type name</param>
        /// <param name="im">immediate switch</param>
        /// <param name="p">parent</param>
        public LuigiSet(string n, bool im, LuigiElement p)
            : base(n, null, p)
        {
            this.Value = new LuigiDictionary("params", "LuigiParameter", this);
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
        /// Gets the parameters of set
        /// </summary>
        public LuigiDictionary Parameters
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
                    return this.Parameters.Elements[c] as LuigiLiteral;
                };
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a new element
        /// </summary>
        /// <param name="e">literal to add</param>
        public void AddElement(LuigiElement e)
        {
            this.Parameters.AddElement(e);
        }

        /// <summary>
        /// Add a new element
        /// </summary>
        /// <param name="e">literal to add</param>
        public void EditElement(LuigiElement e)
        {
            this.Parameters.EditElement(e);
        }

        /// <summary>
        /// Add a new element
        /// </summary>
        /// <param name="oldName">old name</param>
        /// <param name="newName">new name</param>
        public void ChangeName(string oldName, string newName)
        {
            this.Parameters.ChangeName(oldName, newName);
        }

        /// <summary>
        /// Add a new element
        /// </summary>
        /// <param name="nameToRemove">name to remove</param>
        public void RemoveElement(string nameToRemove)
        {
            this.Parameters.RemoveElement(nameToRemove);
        }

        /// <summary>
        /// Execute this object
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
            PrinterObject po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "set-src.prt"));
            po.Configuration.Add("typeName", this.Name);
            string objects = string.Empty;
            foreach (KeyValuePair<string, LuigiElement> l in this.Parameters.Elements)
            {
                objects += Environment.NewLine;
                objects += l.Value.ToString();
            }
            po.Configuration.Add("params", objects);
            return po.Execute();
        }

        #endregion
    }
}
