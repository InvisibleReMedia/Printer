using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Printer;

namespace Luigi
{
    /// <summary>
    /// Polymorph class
    /// helps to work with literal, mapper, set and functions
    /// </summary>
    [Serializable]
    public class LuigiPolymorph : LuigiElement
    {

        #region Fields

        /// <summary>
        /// Parameters
        /// </summary>
        private LuigiDictionary pars;

        /// <summary>
        /// Content
        /// </summary>
        private string innerType;

        /// <summary>
        /// File name to call
        /// </summary>
        private string fileName;

        /// <summary>
        /// Selected key
        /// </summary>
        private string selectedKey;

        /// <summary>
        /// Luigi function
        /// </summary>
        private LuigiFunction f;

        /// <summary>
        /// Content
        /// </summary>
        private string content;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n">type name</param>
        /// <param name="v">value</param>
        /// <param name="p">parent</param>
        public LuigiPolymorph(string n, LuigiElement v, LuigiElement p)
            : base(n, v, p)
        {
            this.innerType = v.TypeName;
            this.fileName = v.Name;
            switch (this.innerType)
            {
                case "LuigiSet":
                    LuigiSet s = v as LuigiSet;
                    this.pars = new LuigiDictionary("pars", this);
                    foreach(KeyValuePair<string, LuigiElement> kv in s.Parameters.Elements)
                    {
                        LuigiParameter pItem = kv.Value as LuigiParameter;
                        this.pars.Elements.Add(kv.Key, pItem.ParameterValue);
                    }
                    this.f = s.Function;
                    break;
                case "LuigiMapper":
                    LuigiMapper m = v as LuigiMapper;
                    this.pars = new LuigiDictionary("pars", this);
                    foreach (KeyValuePair<string, LuigiElement> kv in m.Keys.Elements)
                    {
                        LuigiLiteral pItem = kv.Value as LuigiLiteral;
                        this.pars.Elements.Add(kv.Key, pItem);
                    }
                    break;
                case "LuigiLiteral":
                    this.content = v.Value;
                    break;
                case "LuigiVariable":
                    this.Value = new LuigiPolymorph(n, v.Value, this);
                    break;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the parameters of polymorph
        /// </summary>
        public LuigiDictionary Parameters
        {
            get
            {
                return this.pars;
            }
        }

        /// <summary>
        /// Gets or sets the selected key
        /// </summary>
        public string SelectedKey
        {
            get
            {
                switch(this.innerType)
                {
                    case "LuigiVariable":
                        return this.Value.SelectedKey;
                    default:
                        return this.selectedKey;
                }
            }
            set
            {
                switch (this.innerType)
                {
                    case "LuigiVariable":
                        this.Value.SelectedKey = value;
                        break;
                    default:
                        this.selectedKey = value;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets the content
        /// </summary>
        public string Content
        {
            get
            {
                return this.content;
            }
            set
            {
                this.content = value;
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
        /// Execute operation
        /// </summary>
        /// <param name="po">printer output</param>
        /// <param name="indentValue">indent size</param>
        public override void Execute(Printer.PrinterObject po, ref int indentValue)
        {
            switch (this.innerType)
            {
                case "LuigiLiteral":
                    {
                        po.AddData(this.content);
                    }
                    break;
                case "LuigiMapper":
                    {
                        PrinterVariable pv = new PrinterVariable();
                        pv.Include = true;
                        pv.Name = po.ComputeNewString();
                        pv.Value = this.fileName + ".prt";
                        LuigiElement current = this.Parameters.Elements[this.selectedKey];
                        LuigiPrint p = new LuigiPrint(this.selectedKey, current, this);
                        string output = p.Execute();
                        pv.AddVariable("select", output);
                        po.AddVariable(pv.Name, pv);
                        po.UseVariable(pv.Name);
                        break;
                    }
                case "LuigiSet":
                    {
                        PrinterVariable pv = new PrinterVariable();
                        pv.Include = true;
                        pv.Name = po.ComputeNewString();
                        pv.Value = this.fileName + ".prt";
                        foreach (KeyValuePair<string, LuigiElement> kv in this.Parameters.Elements)
                        {
                            LuigiPrint p = new LuigiPrint(kv.Key, kv.Value, this);
                            string output = p.Execute();
                            pv.AddVariable(kv.Key, output);
                        }
                        po.AddVariable(pv.Name, pv);
                        po.UseVariable(pv.Name);
                        break;
                    }
                case "LuigiVariable":
                    {
                        this.Value.Execute(po, ref indentValue);
                        break;
                    }
            }
        }

        /// <summary>
        /// Copy this into a new element
        /// </summary>
        /// <param name="parent">parent for the new element</param>
        /// <returns>new element</returns>
        public override LuigiElement CopyInto(LuigiElement parent)
        {
            return new LuigiPolymorph(this.Name, this.Value, parent);
        }

        /// <summary>
        /// Converts this object in a string representation
        /// </summary>
        /// <returns>string</returns>
        public override string ToString()
        {
            string output = string.Empty;
            switch (this.innerType)
            {
                case "LuigiMapper":
                case "LuigiSet":
                    output += this.selectedKey;
                    break;
                case "LuigiVariable":
                    output += this.Value.SelectedKey;
                    break;
            }
            return output;
        }

        #endregion

    }
}
