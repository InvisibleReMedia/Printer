using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luigi
{
    public class LuigiVariable : LuigiElement
    {

        #region Constructor

        public LuigiVariable(string n, dynamic v) : base(n, "variable", v)
        {
        }

        #endregion

        #region Methods

        public override void Execute(TextWriter w, ref int indentValue)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
