using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luigi
{
    public class LuigiPrint : LuigiElement
    {

        #region Methods

        /// <summary>
        /// Execute operation
        /// </summary>
        /// <param name="po">printer output</param>
        /// <param name="indentValue">indent size</param>
        public override void Execute(Printer.PrinterObject po, ref int indentValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Copy this into a new element
        /// </summary>
        /// <param name="parent">parent for the new element</param>
        /// <returns>new element</returns>
        public override LuigiElement CopyInto(LuigiElement parent)
        {
            LuigiResult r = new LuigiResult(this.Name, this.stacked, parent);
            r.Current = this.Current;
            return r;
        }

        #endregion

    }
}
