using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luigi
{
    /// <summary>
    /// Result of a worked item
    /// </summary>
    public class LuigiResult : LuigiElement
    {

        #region Fields

        /// <summary>
        /// String that's anterior
        /// </summary>
        private string stacked;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for an integer
        /// </summary>
        /// <param name="n">aame</param>
        /// <param name="init">value (string)</param>
        /// <param name="p">parent</param>
        public LuigiResult(string n, int init, LuigiElement p)
            : base(n, init, p)
        {
        }

        /// <summary>
        /// Constructor for an integer
        /// </summary>
        /// <param name="n">aame</param>
        /// <param name="init">value (integer)</param>
        /// <param name="p">parent</param>
        public LuigiResult(string n, string init, LuigiElement p)
            : base(n, init, p)
        {
            this.stacked = string.Empty;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current string
        /// </summary>
        public string Current
        {
            get
            {
                return this.Value;
            }
            set
            {
                this.Value = value;
            }
        }

        /// <summary>
        /// Gets the anterior's string
        /// </summary>
        public string Stacked
        {
            get
            {
                return this.stacked;
            }
        }

        #endregion

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
