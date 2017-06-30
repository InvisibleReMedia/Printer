using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luigi.accu
{
    /// <summary>
    /// Print action
    /// </summary>
    [Serializable]
    public class Print
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
        /// <param name="s">sequence of terms</param>
        /// <param name="p">parent</param>
        public Print(IEnumerable<string> s, dynamic p)
        {
            this.parent = p;
            this.root = p.Root;
            string path = String.Join(".", s);
            this.accu = new Accu.Accu(false, false, true,
                                      path,
                                      TopLevel.RecursiveFindByName(p.Root, path));
            this.accu.AddElement(new Accu.Accu(false, true, false, "type", this.GetType().Name));
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
            return this.accu.ToString();
        }

        #endregion

    }
}
