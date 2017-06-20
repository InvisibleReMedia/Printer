using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luigi
{
    /// <summary>
    /// Class to implement an iron path
    /// </summary>
    public class LuigiDepthList : LuigiElement
    {

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="n">name</param>
        /// <param name="parent">parent</param>
        public LuigiDepthList(string n, LuigiElement parent)
            : base(n, null, parent)
        {
            if (parent is LuigiObject)
            {
                this.Value = parent.Root.Find(n);
            }
            else if (parent is LuigiVariable)
            {
                this.Value = parent.Value;
            }
            else if (parent is LuigiSet)
            {
                LuigiSet s = parent as LuigiSet;
                if (string.Equals(n, "print"))
                {
                    // TODO : print method
                }
                else if (string.Equals(n, "insert"))
                {
                    // TODO : insert method
                }
                else if (string.Equals(n, "remove"))
                {
                    // TODO : remove method
                }
                else if (string.Equals(n, "select"))
                {
                    // TODO : select method
                }
                else if (s.Parameters.Elements.ContainsKey(n))
                {
                    this.Value = s.Parameters.Elements[n];
                }
            }
            else if (parent is LuigiLiteral)
            {
                LuigiLiteral r = parent as LuigiLiteral;
                if (string.Equals(n, "print"))
                {
                    // TODO : print method
                }
                else
                {
                    throw new MissingMethodException("LuigiLiteral", n);
                }
            }
            else if (parent is LuigiMapper)
            {
                LuigiMapper m = parent as LuigiMapper;
                if (m.Keys.Elements.ContainsKey(n))
                {
                    this.Value = m.Keys.Elements[n].Value;
                }
            }
            else if (parent is LuigiParameter)
            {
                this.Value = parent.Value;
            }
            else
            {
                throw new KeyNotFoundException(String.Format("{0} is not a valid accessor", n));
            }
        }

        #endregion

        /// <summary>
        /// Write output as interpretation result
        /// </summary>
        /// <param name="po">printer</param>
        /// <param name="indentValue">space size</param>
        public override void Execute(Printer.PrinterObject po, ref int indentValue)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Copy this into a new element
        /// </summary>
        /// <param name="parent">parent</param>
        /// <returns>a new element</returns>
        public override LuigiElement CopyInto(LuigiElement parent)
        {
            throw new NotImplementedException();
        }
    }
}
