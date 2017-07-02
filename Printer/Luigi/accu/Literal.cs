using Printer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luigi.accu
{
    /// <summary>
    /// State a literal
    /// </summary>
    [Serializable]
    public class Literal : Accu.Accu
    {

        #region Fields

        /// <summary>
        /// Top level class (use for a get reference object)
        /// </summary>
        private TopLevel root;

        /// <summary>
        /// Parent object
        /// </summary>
        private dynamic parent;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="n"></param>
        /// <param name="p">parent</param>
        public Literal(string n, dynamic p)
            : base(false, false, false, n, null)
        {
            this.parent = p;
            this.root = p.Root;
            this.AddElement(new Accu.Accu(true, false, false, "type", this.GetType().Name));
            this.AddElement(new Accu.Accu(false, true, false, "delimiter", "."));
            this.AddElement(new Accu.Accu(false, true, false, "text", "text"));
            this.AddElement(new Accu.Accu(false, true, false, "print", "result"));
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the root parent
        /// </summary>
        public TopLevel Root
        {
            get
            {
                return this.root;
            }
        }

        /// <summary>
        /// Gets the parent
        /// </summary>
        public dynamic Parent
        {
            get
            {
                return this.parent;
            }
        }

        /// <summary>
        /// Gets the type name
        /// </summary>
        public string TypeName
        {
            get
            {
                return this.FindByName("type").Value;
            }
        }

        /// <summary>
        /// Gets or sets the delimiter
        /// </summary>
        public string Delimiter
        {
            get
            {
                return this.FindByName("delimiter").Value;
            }
            set
            {
                this.FindByName("delimiter").Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the content
        /// </summary>
        public string Text
        {
            get
            {
                return this.FindByName("text").Value;
            }
            set
            {
                this.FindByName("text").Value = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Apply
        /// </summary>
        /// <param name="pars">parameters</param>
        /// <returns>string result</returns>
        public string Apply(Dictionary<string, string> pars)
        {
            string output = string.Empty;
            switch (pars.ElementAt(0).Key)
            {
                case "delimiter":
                    output = this.Delimiter;
                    break;
                case "text":
                    output = this.Text;
                    break;
                case "print":
                    // TODO : purpose parameters
                    output = this.Text;
                    break;
            }
            return output;
        }

        /// <summary>
        /// Converts this object into a string representation (source code)
        /// </summary>
        /// <returns>string representation as the source code</returns>
        public override string ToString()
        {
            PrinterObject po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "literal.prt"));
            po.Configuration.Add("typeName", this.Name);
            po.Configuration.Add("delimiter", this.Delimiter);
            po.Configuration.Add("value", this.Text);
            return po.Execute();
        }

        #endregion

    }
}
