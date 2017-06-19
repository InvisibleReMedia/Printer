using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luigi
{
    /// <summary>
    /// Operates functions
    /// </summary>
    [Serializable]
    public class LuigiFunction : LuigiElement
    {

        #region Constructor
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n">name of the function</param>
        /// <param name="v">parameters</param>
        /// <param name="p">parent</param>
        public LuigiFunction(string n, LuigiList v, LuigiElement p) : base(n, v, p)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n">name of the function</param>
        /// <param name="p">parent</param>
        public LuigiFunction(string n, LuigiElement p)
            : base(n, null, p)
        {
            this.Value = new LuigiList("params", this);
        }

        #endregion

        #region Properties

        /// <summary>
        /// List of effective parameters
        /// </summary>
        public LuigiList EffectiveValues
        {
            get
            {
                return this.Value as LuigiList;
            }
        }

        /// <summary>
        /// Implements built-in functions
        /// </summary>
        private Dictionary<string, Func<TextWriter, int, int>> BuiltIn
        {
            get
            {
                Dictionary<string, Func<TextWriter, int, int>> functions = new Dictionary<string, Func<TextWriter, int, int>>();
                functions.Add("concat", (t, i) =>
                {
                    foreach (LuigiElement e in this.EffectiveValues.Elements)
                    {
                        e.Execute(t, ref i);
                    }
                    return i;
                });
                return functions;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add element list
        /// </summary>
        /// <param name="e">element to add</param>
        public void AddParameter(LuigiElement e)
        {
            this.EffectiveValues.AddElement(e);
        }

        /// <summary>
        /// Insert an element
        /// </summary>
        /// <param name="index">index position</param>
        /// <param name="e"></param>
        public void InsertElement(int index, LuigiElement e)
        {
            this.EffectiveValues.InsertElement(index, e);
        }

        /// <summary>
        /// Edit element list
        /// </summary>
        /// <param name="index">index position</param>
        /// <param name="e">element to add</param>
        public void EditElement(int index, LuigiElement e)
        {
            this.EffectiveValues.EditElement(index, e);
        }

        /// <summary>
        /// Remove an element from the list
        /// </summary>
        /// <param name="index">index to remove</param>
        public void RemoveElement(int index)
        {
            this.EffectiveValues.RemoveElement(index);
        }

        /// <summary>
        /// Execute this object
        /// </summary>
        /// <param name="w">writer</param>
        /// <param name="indentValue">indent size</param>
        public override void Execute(TextWriter w, ref int indentValue)
        {
            Dictionary<string, Func<TextWriter, int, int>> list = this.BuiltIn;
            if (list.ContainsKey(this.Name))
            {
                indentValue = list[this.Name](w, indentValue);
            }
            else
            {
                // get functions from Root; set parameters and executes
            }
        }

        /// <summary>
        /// Copy this into a new element
        /// </summary>
        /// <param name="parent">parent</param>
        /// <returns>a new element</returns>
        public override LuigiElement CopyInto(LuigiElement parent)
        {
            LuigiFunction f = new LuigiFunction(this.Name, parent);
            foreach (LuigiElement e in this.EffectiveValues.Elements)
            {
                f.EffectiveValues.AddElement(e.CopyInto(f));
            }
            return f;
        }

        #endregion
    }
}
