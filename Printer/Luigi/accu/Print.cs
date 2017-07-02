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
    public class Print : Accu.Accu
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
        /// <param name="s">sequence of terms</param>
        /// <param name="p">parent</param>
        public Print(IEnumerable<string> s, dynamic p)
            : base(false, false, true, "", null)
        {
            this.parent = p;
            this.root = p.Root;
            this.name = String.Join(".", s);
            this.AddElement(new Accu.Accu(false, true, false, "type", this.GetType().Name));
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
            return this.Name;
        }

        #endregion

    }
}
