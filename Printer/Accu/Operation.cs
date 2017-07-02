using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accu
{
    public class Operation
    {

        #region Fields

        /// <summary>
        /// True if function call
        /// </summary>
        private OperationType type;

        /// <summary>
        /// Name of the operation
        /// </summary>
        private string name;

        /// <summary>
        /// Sequence name of accu
        /// </summary>
        private List<string> seq;

        #endregion

        #region Inner Class

        public enum OperationType
        {
            /// <summary>
            /// Affectation
            /// </summary>
            AFFECT,
            /// <summary>
            /// Function
            /// </summary>
            FUNCTION,
            /// <summary>
            /// Play an accumulator element
            /// </summary>
            PLAYER
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor of a couple of name/sequence
        /// </summary>
        /// <param name="name">name</param>
        /// <param name="val">sequence of names</param>
        public Operation(string name, string[] val)
        {
            this.type = OperationType.AFFECT;
            this.name = name;
            this.seq = val.ToList();
        }

        /// <summary>
        /// Constructor for a print function
        /// </summary>
        /// <param name="val"></param>
        public Operation(string[] val)
        {
            this.type = OperationType.FUNCTION;
            this.name = "print";
            this.seq = val.ToList();
        }

        #endregion

    }
}
