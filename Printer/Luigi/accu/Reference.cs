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
    /// Is an object referencing an another object by its names
    /// </summary>
    [Serializable]
    public class Reference
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
        /// <param name="n">name</param>
        /// <param name="v">value</param>
        /// <param name="p">parent</param>
        public Reference(string n, dynamic v, dynamic p)
        {
            this.parent = p;
            this.root = p.Root;
            this.accu = new Accu.Accu(true, false, false, n, v.Name);
            this.accu.AddElement(new Accu.Accu(false, true, false, "type", this.GetType().Name));
            this.accu.AddElement(new Accu.Accu(false, true, false, "print", "result"));
            this.accu.AddElement(new Accu.Accu(false, true, false, "innerType", v.GetType().Name));
            this.accu.AddElement(new Accu.Accu(false, true, false, "value", v));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the accumulator
        /// </summary>
        public Accu.Accu Accumulator
        {
            get
            {
                return this.accu;
            }
        }

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
        /// Gets the name
        /// </summary>
        public dynamic ReferencedObject
        {
            get
            {
                return this.accu.FindByName("value").Value;
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
            return this.ReferencedObject.Apply(pars);
        }

        /// <summary>
        /// Converts this object into a string representation (source code)
        /// </summary>
        /// <returns>string representation as the source code</returns>
        public override string ToString()
        {
            return "";
        }

        #endregion

    }
}
