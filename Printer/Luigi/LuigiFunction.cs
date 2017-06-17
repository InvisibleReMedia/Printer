using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luigi
{
    public class LuigiFunction : LuigiElement
    {

        #region Constructor

        public LuigiFunction(string n, dynamic v) : base(n, "function", v)
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
