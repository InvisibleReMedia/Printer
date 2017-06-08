using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printer
{
    /// <summary>
    /// A printer variable
    /// </summary>
    [Serializable]
    public class PrinterVariable
    {

        #region Fields

        /// <summary>
        /// Indent switch
        /// </summary>
        private bool shouldIndent;
        /// <summary>
        /// Name of the variable
        /// </summary>
        private string name;

        /// <summary>
        /// Value of the variable
        /// </summary>
        private string value;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public PrinterVariable()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Execute the variable
        /// </summary>
        /// <param name="sb">string builder</param>
        public void Execute(StringBuilder sb)
        {
            sb.Append(this.value);
        }

        #endregion

    }
}
