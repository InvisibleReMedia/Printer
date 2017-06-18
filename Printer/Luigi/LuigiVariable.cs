﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luigi
{
    /// <summary>
    /// A variable into Luigi language
    /// </summary>
    [Serializable]
    public class LuigiVariable : LuigiElement
    {

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="n">name of variable</param>
        /// <param name="v">luigi element</param>
        /// <param name="p">parent</param>
        public LuigiVariable(string n, LuigiElement v, LuigiElement p) : base(n, v, p)
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
