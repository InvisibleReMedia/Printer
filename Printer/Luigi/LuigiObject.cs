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
        public LuigiObject(string n) : base(n, null)
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
        /// Execute this object
        /// </summary>
        /// <param name="w">writer</param>
        /// <param name="indentValue">indent size</param>
        public override void Execute(TextWriter w, ref int indentValue)
        {
            foreach (LuigiElement e in this.Datas.Elements)
            {
                if (e is LuigiMapper)
                {
                    LuigiMapper m = e as LuigiMapper;
                    if (m.Keys.Elements.Count > 0)
                        m.ExecuteCall(m.Keys.Elements.Keys.First());
                }
                else
                {
                    e.Execute(w, ref indentValue);
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

        #endregion
    }
}
