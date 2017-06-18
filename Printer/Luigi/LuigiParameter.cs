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
    /// A parameter
    /// </summary>
    [Serializable]
    public class LuigiParameter : LuigiElement
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n">name of the parameter</param>
        /// <param name="v">value</param>
        /// <param name="p">parent</param>
        public LuigiParameter(string n, LuigiElement v, LuigiElement p)
            : base(n, v, p)
        {
            this.Value.IsImmediate = true;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the parameter value
        /// </summary>
        public LuigiElement ParameterValue
        {
            get
            {
                return this.Value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Execute this object
        /// </summary>
        /// <param name="w">writer</param>
        /// <param name="indentValue">indent size</param>
        public override void Execute(TextWriter w, ref int indentValue)
        {
        }

        /// <summary>
        /// Converts this object into a string representation (source code)
        /// </summary>
        /// <returns>string representation as the source code</returns>
        public override string ToString()
        {
            PrinterObject po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "param-name.prt"));
            po.Configuration.Add("paramName", this.Name);
            string header = "";
            switch (this.ParameterValue.TypeName)
            {
                case "LuigiLiteral":
                    header = "-";
                    break;
                case "LuigiMapper":
                    header = "%";
                    break;
                case "LuigiSet":
                    header = "@";
                    break;
            }
            po.Configuration.Add("paramSwitch", header);
            string value = this.Value.ToString();
            po.Configuration.Add("paramValue", value);
            return po.Execute();
        }

        #endregion
    }
}
