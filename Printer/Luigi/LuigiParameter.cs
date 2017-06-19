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
        /// Execute the process of a list
        /// </summary>
        /// <param name="po">printer</param>
        /// <param name="indentValue">indent</param>
        public override void Execute(PrinterObject po, ref int indentValue)
        {
            this.ParameterValue.Execute(po, ref indentValue);
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
            LuigiElement e;
            if (this.ParameterValue is LuigiReference)
            {
                e = (this.ParameterValue as LuigiReference).ReferencedObject;
            }
            else
            {
                e = this.ParameterValue;
            }
            switch (e.TypeName)
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
                default:
                    throw new InvalidDataException(String.Format("Type name {0} is not allowed as a parameter", e.TypeName));
            }
            po.Configuration.Add("paramSwitch", header);
            string value = e.ToString();
            po.Configuration.Add("paramValue", value);
            return po.Execute();
        }

        /// <summary>
        /// Copy this into a new element
        /// </summary>
        /// <param name="parent">parent</param>
        /// <returns>a new element</returns>
        public override LuigiElement CopyInto(LuigiElement parent)
        {
            return new LuigiParameter(this.Name, this.Value, parent);
        }

        #endregion
    }
}
