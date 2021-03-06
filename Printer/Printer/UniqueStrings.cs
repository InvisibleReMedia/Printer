using System;
using System.Collections.Generic;
using System.Text;

namespace Printer
{
    /// <summary>
    /// Class to stand unique strings
    /// </summary>
    [Serializable]
    public class UniqueStrings
    {
        #region Private Constants

        /// <summary>
        ///  list of admitted chars
        /// </summary>
        private const string list = "0abcdefghijklmnopqrstuvw";
        /// <summary>
        /// maximum size of the string length
        /// threshold of possibilites (length list)^6
        /// </summary>
        private const int maxDepth = 6;

        #endregion

        #region Private Fields
        /// <summary>
        /// Counter
        /// </summary>
        private int counter;
        #endregion

        #region Default Constructor

        /// <summary>
        /// Constructs a new instance
        /// </summary>
        public UniqueStrings()
        {
            this.counter = 1;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public UniqueStrings(int counter)
        {
            this.counter = counter;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the counter position
        /// </summary>
        public int Counter
        {
            get
            {
                return this.counter;
            }
            set
            {
                this.counter = value;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Create a new unique name and increment pointer
        /// </summary>
        /// <returns>new unique name</returns>
        public string ComputeNewString()
        {
            int max = (int)Math.Pow(UniqueStrings.list.Length, UniqueStrings.maxDepth);
            if (this.counter < max)
            {
                int[] seq = new int[UniqueStrings.maxDepth];
                seq[0] = this.counter;
                ++this.counter;
                for (int b = UniqueStrings.maxDepth - 1; b > 0; --b)
                {
                    int q = (int)Math.Pow(UniqueStrings.list.Length, b);
                    int temp = seq[UniqueStrings.maxDepth - b - 1];
                    seq[UniqueStrings.maxDepth - b - 1] = temp / q;
                    seq[UniqueStrings.maxDepth - b] = temp - seq[UniqueStrings.maxDepth - b - 1] * q;
                }
                string output = string.Empty;
                for (int index = maxDepth - 1; index >= 0; --index)
                {
                    output += UniqueStrings.list[seq[index]];
                }
                output = output.PadRight(maxDepth, '0').TrimEnd('0');
                if (output.Length > 0)
                    return output;
                else return "a";
            }
            else
            {
                throw new OverflowException("Nombre maximum de processus anonyme atteint (" + max.ToString() + ")");
            }
        }
        #endregion
    }
}
