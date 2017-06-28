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
    /// A value for effective parameters
    /// </summary>
    [Serializable]
    public class LuigiValue : LuigiElement
    {

        #region Fields

        /// <summary>
        /// resulted string representation
        /// </summary>
        private string result;
        /// <summary>
        /// Execute just one time
        /// </summary>
        private bool done;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n">name of the function</param>
        /// <param name="v">parameters</param>
        /// <param name="p">parent</param>
        public LuigiValue(string n, LuigiElement v, LuigiElement p)
            : base(n, v, p)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the content of this value
        /// </summary>
        public LuigiElement Content
        {
            get
            {
                return this.Value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Execute the process of a list
        /// </summary>
        /// <param name="po">printer</param>
        /// <param name="indentValue">indent</param>
        public override void Execute(PrinterObject po, ref int indentValue)
        {
            if (!this.done)
            {
                LuigiPrint p = new LuigiPrint("temp", this, this);
                this.result = p.Execute();
                this.done = true;
            }
        }

        /// <summary>
        /// Copy this into a new element
        /// </summary>
        /// <param name="parent">parent</param>
        /// <returns>a new element</returns>
        public override LuigiElement CopyInto(LuigiElement parent)
        {
            return new LuigiValue(this.Name, this.Value, parent);
        }

        #endregion
    }
}
