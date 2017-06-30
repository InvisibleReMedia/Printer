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
    public class Type
    {

        #region Fields

        /// <summary>
        /// Accumulator
        /// </summary>
        private Accu.Accu accu;

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
        {
            this.parent = p;
            this.root = p.Root;
            this.accu = new Accu.Accu(false, false, v.Name, this);
            this.accu.AddElement(new Accu.Accu(false, true, "type", this.GetType().Name));
            this.accu.AddElement(new Accu.Accu(false, true, "print", "result"));
            this.accu.AddElement(new Accu.Accu(false, true, "innerType", v.GetType().Name));
            this.accu.AddElement(new Accu.Accu(false, true, "value", v));
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
        /// Gets the name
        /// </summary>
        public string Name
        {
            get
            {
                return this.accu.Name;
            }
        }

        /// <summary>
        /// Gets or sets the value (inner object)
        /// </summary>
        public dynamic Value
        {
            get
            {
                return this.accu.FindByName("value").Value;
            }
            set
            {
                this.accu.FindByName("value").Value = value;
            }
        }

        #endregion

        #region Methods

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
