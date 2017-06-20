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
    /// A reference type into Luigi language
    /// </summary>
    [Serializable]
    public class LuigiReference : LuigiElement
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n">object name</param>
        /// <param name="v">reference type name</param>
        /// <param name="p">parent</param>
        public LuigiReference(string n, string v, LuigiElement p)
            : base(n, v, p)
        {
            this.Value = this.Root.Find(v);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Object that is referenced
        /// </summary>
        public LuigiElement ReferencedObject
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
            this.ReferencedObject.Execute(po, ref indentValue);
        }


        /// <summary>
        /// Copy this into a new element
        /// </summary>
        /// <param name="parent">parent</param>
        /// <returns>a new element</returns>
        public override LuigiElement CopyInto(LuigiElement parent)
        {
            return new LuigiReference(this.Name, this.Value, parent);
        }

        #endregion
    }
}
