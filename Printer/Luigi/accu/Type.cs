using Printer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luigi.accu
{
    /// <summary>
    /// Is a type name representing an object
    /// </summary>
    [Serializable]
    public class Type : Accu.Accu
    {

        #region Fields

        /// <summary>
        /// Top level class (use for a get reference object)
        /// </summary>
        private TopLevel root;

        /// <summary>
        /// Parent object
        /// </summary>
        private dynamic parent;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="v">value</param>
        /// <param name="p">parent</param>
        public Type(dynamic v, dynamic p)
            : base(false, false, false, "", null)
        {
            this.parent = p;
            this.root = p.Root;
            this.AddElement(new Accu.Accu(false, true, false, "type", this.GetType().Name));
            this.AddElement(new Accu.Accu(false, true, false, "print", "result"));
            this.AddElement(new Accu.Accu(false, true, false, "innerType", v.GetType().Name));
            this.AddElement(new Accu.Accu(false, true, false, "value", v));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the root parent
        /// </summary>
        public TopLevel Root
        {
            get
            {
                return this.root;
            }
        }

        /// <summary>
        /// Gets the parent
        /// </summary>
        public dynamic Parent
        {
            get
            {
                return this.parent;
            }
        }

        /// <summary>
        /// Gets the type name
        /// </summary>
        public string TypeName
        {
            get
            {
                return this.FindByName("type").Value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Apply
        /// </summary>
        /// <param name="pars">parameters</param>
        /// <returns>string result</returns>
        public string Apply(Dictionary<string, string> pars)
        {
            return "";
        }

        /// <summary>
        /// Converts this object into a string representation (source code)
        /// </summary>
        /// <returns>string representation as the source code</returns>
        public override string ToString()
        {
            return this.Value.ToString();
        }

        #endregion

    }
}
