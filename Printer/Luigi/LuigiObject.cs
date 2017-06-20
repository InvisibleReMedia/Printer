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
    /// Top-level class of Luigi language
    /// </summary>
    [Serializable]
    public class LuigiObject : LuigiElement
    {

        #region Fields

        /// <summary>
        /// Types
        /// </summary>
        private LuigiDictionary typeNames;

        /// <summary>
        /// Variables
        /// </summary>
        private LuigiDictionary variables;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n">name of the program</param>
        public LuigiObject(string n)
            : base(n, null)
        {
            this.Value = new LuigiList("datas", this);
            this.typeNames = new LuigiDictionary("types", this);
            this.variables = new LuigiDictionary("vars", "LuigiVariable", this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// New implementation for Root
        /// </summary>
        public new LuigiElement Root
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Datas stored into this program
        /// </summary>
        public LuigiList Datas
        {
            get
            {
                return this.Value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Find any type name
        /// </summary>
        /// <param name="name">type name</param>
        /// <returns>object if exists</returns>
        /// <exception cref="KeyNotFoundException">not exist</exception>
        public LuigiElement Find(string name)
        {
            if (this.variables.Elements.ContainsKey(name))
            {
                return this.variables.Elements[name];
            }
            else if (this.typeNames.Elements.ContainsKey(name))
            {
                return this.typeNames.Elements[name];
            }
            else
            {
                throw new KeyNotFoundException(String.Format("Variable or type name {0} doesn't exist", name));
            }
        }

        /// <summary>
        /// Find literal type name
        /// </summary>
        /// <param name="name">type name</param>
        /// <returns>object if exists</returns>
        /// <exception cref="InvalidCastException">not a literal</exception>
        /// <exception cref="KeyNotFoundException">not exist</exception>
        public LuigiLiteral FindLiteral(string name)
        {
            if (this.typeNames.Elements.ContainsKey(name))
            {
                LuigiElement e = this.typeNames.Elements[name];
                if (e is LuigiLiteral)
                    return e as LuigiLiteral;
                else
                    throw new InvalidCastException(String.Format("Type name {0} is not a literal type", name));
            }
            else
            {
                throw new KeyNotFoundException(String.Format("Type name {0} doesn't exist", name));
            }
        }

        /// <summary>
        /// Find mapper type name
        /// </summary>
        /// <param name="name">type name</param>
        /// <returns>object if exists</returns>
        /// <exception cref="InvalidCastException">not a literal</exception>
        /// <exception cref="KeyNotFoundException">not exist</exception>
        public LuigiMapper FindMapper(string name)
        {
            if (this.typeNames.Elements.ContainsKey(name))
            {
                LuigiElement e = this.typeNames.Elements[name];
                if (e is LuigiMapper)
                    return e as LuigiMapper;
                else
                    throw new InvalidCastException(String.Format("Type name {0} is not a mapper type", name));
            }
            else
            {
                throw new KeyNotFoundException(String.Format("Type name {0} doesn't exist", name));
            }
        }

        /// <summary>
        /// Find set type name
        /// </summary>
        /// <param name="name">type name</param>
        /// <returns>object if exists</returns>
        /// <exception cref="InvalidCastException">not a literal</exception>
        /// <exception cref="KeyNotFoundException">not exist</exception>
        public LuigiSet FindSet(string name)
        {
            if (this.typeNames.Elements.ContainsKey(name))
            {
                LuigiElement e = this.typeNames.Elements[name];
                if (e is LuigiSet)
                    return e as LuigiSet;
                else
                    throw new InvalidCastException(String.Format("Type name {0} is not a set type", name));
            }
            else
            {
                throw new KeyNotFoundException(String.Format("Type name {0} doesn't exist", name));
            }
        }

        /// <summary>
        /// Find variable instance
        /// </summary>
        /// <param name="name">variable name</param>
        /// <returns>object if exists</returns>
        /// <exception cref="KeyNotFoundException">not exist</exception>
        public LuigiElement FindVariable(string name)
        {
            if (this.variables.Elements.ContainsKey(name))
            {
                return this.variables.Elements[name];
            }
            else
            {
                throw new KeyNotFoundException(String.Format("Variable {0} doesn't exist", name));
            }
        }

        /// <summary>
        /// Add element list
        /// </summary>
        /// <param name="e">element to add</param>
        public void AddElement(LuigiElement e)
        {
            this.Datas.AddElement(e);
            switch (e.TypeName)
            {
                case "LuigiLiteral":
                case "LuigiMapper":
                case "LuigiSet":
                    this.typeNames.AddElement(e);
                    break;
                case "LuigiVariable":
                    this.variables.AddElement(e);
                    break;
            }
        }

        /// <summary>
        /// Insert an element
        /// </summary>
        /// <param name="index">index position</param>
        /// <param name="e"></param>
        public void InsertElement(int index, LuigiElement e)
        {
            this.Datas.AddElement(e);
            switch (e.TypeName)
            {
                case "LuigiLiteral":
                case "LuigiMapper":
                case "LuigiSet":
                    this.typeNames.AddElement(e);
                    break;
                case "LuigiVariable":
                    this.variables.AddElement(e);
                    break;
            }
        }

        /// <summary>
        /// Edit element list
        /// </summary>
        /// <param name="index">index position</param>
        /// <param name="e">element to add</param>
        public void EditElement(int index, LuigiElement e)
        {
            this.Datas.EditElement(index, e);
        }

        /// <summary>
        /// Remove an element from the list
        /// </summary>
        /// <param name="index">index to remove</param>
        public void RemoveElement(int index)
        {
            this.Datas.RemoveElement(index);
        }

        /// <summary>
        /// Execute the process of a list
        /// </summary>
        /// <param name="po">printer</param>
        /// <param name="indentValue">indent</param>
        public override void Execute(PrinterObject po, ref int indentValue)
        {
            foreach (LuigiElement e in this.Datas.Elements)
            {
                if (e is LuigiMapper)
                {
                    LuigiMapper m = e as LuigiMapper;
                    if (m.Keys.Elements.Count > 0)
                    {
                        m.ExecuteCall(m.Keys.Elements.Keys.First()).Execute(po, ref indentValue);
                    }
                }
                else
                {
                    e.Execute(po, ref indentValue);
                }
            }
        }

        /// <summary>
        /// Converts this object into a string representation (source code)
        /// </summary>
        /// <returns>string representation as the source code</returns>
        public override string ToString()
        {
            string output = string.Empty;
            foreach (LuigiElement e in this.Datas.Elements)
            {
                output += e.ToString();
            }
            return output;
        }

        /// <summary>
        /// Copy this into a new element
        /// </summary>
        /// <param name="parent">parent</param>
        /// <returns>a new element</returns>
        public override LuigiElement CopyInto(LuigiElement parent)
        {
            LuigiObject o = new LuigiObject(this.Name);
            foreach (LuigiElement e in this.Datas.Elements)
            {
                LuigiElement copied = e.CopyInto(o);
                o.AddElement(copied);
            }
            return o;
        }

        #endregion
    }
}
