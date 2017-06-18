using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luigi
{
    /// <summary>
    /// A reference type into Luigi language
    /// </summary>
    [Serializable]
    public class LuigiReference : LuigiElement
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n">type name</param>
        /// <param name="v">reference type</param>
        /// <param name="p">parent</param>
        public LuigiReference(string n, string v, LuigiElement p) : base(n, v, p)
        {
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
            throw new NotImplementedException();
        }

        #endregion
    }
}
