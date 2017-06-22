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
        /// no name switch
        /// </summary>
        private bool automatic;

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
        public LuigiSet(string n, bool im, LuigiDictionary v, LuigiFunction f, LuigiElement p)
            : base(n, v, p)
        {
            this.automatic = false;
            this.immediate = im;
            this.fun = f;
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
            this.automatic = false;
            this.immediate = im;
            this.Value = new LuigiDictionary("params", "LuigiParameter", this);
            this.fun = new LuigiFunction("concat", this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="v">list of parameters</param>
        /// <param name="f">expression</param>
        /// <param name="p">parent</param>
        public LuigiSet(LuigiDictionary v, LuigiFunction f, LuigiElement p)
            : base("", v, p)
        {
            this.automatic = true;
            this.immediate = false;
            this.fun = new LuigiFunction("concat", this);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="p">parent</param>
        public LuigiSet(LuigiElement p)
            : base("", null, p)
        {
            this.automatic = true;
            this.immediate = false;
            this.Value = new LuigiDictionary("params", "LuigiParameter", this);
            this.fun = new LuigiFunction("concat", this);
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
        /// Gets the function element
        /// </summary>
        public LuigiFunction Function
        {
            get
            {
                return this.fun;
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
        /// Print method
        /// </summary>
        /// <param name="po">printer</param>
        /// <param name="indentValue">indent</param>
        public void Print(PrinterObject po, ref int indentValue)
        {
            for (int index = 0; index < this.Function.EffectiveValues.Elements.Count; ++index)
            {
                LuigiElement e = this.Function.EffectiveValues.Elements[index];
                if (e is LuigiValue)
                {
                    if (this.Parameters.Elements.ContainsKey(e.Name))
                    {
                        e.Value = this.Parameters.Elements[e.Name];
                    }
                }
            }
            this.Function.Execute(po, ref indentValue);
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
            PrinterObject po;
            if (this.automatic)
            {
                po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "set-au.prt"));
            }
            else
            {
                if (this.immediate)
                {
                    po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "set-im.prt"));
                }
                else
                {
                    po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "set-src.prt"));
                }
            }
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

        /// <summary>
        /// Copy this into a new element
        /// </summary>
        /// <param name="parent">parent</param>
        /// <returns>a new element</returns>
        public override LuigiElement CopyInto(LuigiElement parent)
        {
            LuigiSet s;
            if (this.IsAutomatic)
            {
                s = new LuigiSet(parent);
            }
            else
            {
                s = new LuigiSet(this.Name, this.IsImmediate, parent);
            }
            foreach (KeyValuePair<string, LuigiElement> kv in this.Parameters.Elements)
            {
                s.AddElement(kv.Value.CopyInto(s));
            }
            return s;
        }

        #endregion
    }
}
