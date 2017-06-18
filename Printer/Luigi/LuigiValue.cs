using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luigi
{
    /// <summary>
    /// A value for effective parameters
    /// </summary>
    [Serializable]
    public class LuigiValue : LuigiElement
    {
        #region Constructor
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n">name of the function</param>
        /// <param name="v">parameters</param>
        /// <param name="p">parent</param>
        public LuigiValue(string n, LuigiElement v, LuigiElement p) : base(n, v, p)
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
