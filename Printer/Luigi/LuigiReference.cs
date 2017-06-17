using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luigi
{
    public class LuigiReference : LuigiElement
    {

        #region Constructor

        public LuigiReference(string n, dynamic v) : base(n, "reference", v)
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
