using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luigi
{
    public class LuigiObject : LuigiElement
    {

        #region Fields

        #endregion

        #region Constructor

        public LuigiObject(string n) : base(n, "root", new LuigiList("data"))
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
