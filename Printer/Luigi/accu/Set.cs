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
    /// Statement for a set
    /// </summary>
    [Serializable]
    public class Set
    {

        #region Fields

        /// <summary>
        /// Accumulator
        /// </summary>
        private Accu.Accu accu;

        /// <summary>
        /// Keys
        /// </summary>
        private List<Parameter> pars;

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
        /// <param name="n">name</param>
        /// <param name="p">parent</param>
        public Set(string n, dynamic p)
        {
            this.parent = p;
            this.root = p.Root;
            this.accu = new Accu.Accu(false, false, n, this);
            this.accu.AddElement(new Accu.Accu(false, true, "type", this.GetType().Name));
            this.accu.AddElement(new Accu.Accu(false, true, "count", 0));
            this.accu.AddElement(new Accu.Accu(false, true, "print", "result"));
            this.accu.AddElement(new Accu.Accu(false, true, "add", "result"));
            this.accu.AddElement(new Accu.Accu(false, true, "insert", "result"));
            this.accu.AddElement(new Accu.Accu(false, true, "remove", "result"));
            this.pars = new List<Parameter>();
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
        /// Gets the name
        /// </summary>
        public string Name
        {
            get
            {
                return this.accu.Name;
            }
        }

        /// <summary>
        /// Gets the number of keys
        /// </summary>
        public int Count
        {
            get
            {
                return this.accu.FindByIndex(1).Value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a key
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="v">value</param>
        public void AddParameter(string key, dynamic v)
        {
            int pos = this.pars.FindLastIndex(x => x.Name == key);
            if (pos != -1)
            {
                this.pars[pos].Value = v;
            }
            else
            {
                Parameter p = new Parameter(key, v, this);
                this.pars.Add(p);
                this.accu.AddElement(new Accu.Accu(false, false, key, p));
                int n = this.accu.FindByIndex(1).Value;
                this.accu.FindByIndex(1).Value = n + 1;
            }
        }

        /// <summary>
        /// Edit a key
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="v">value</param>
        public void EditParameter(string key, dynamic v)
        {
            int pos = this.pars.FindLastIndex(x => x.Name == key);
            if (pos != -1)
            {
                this.pars[pos].Value = v;
            }
            else
            {
                Parameter p = new Parameter(key, v, this);
                this.pars.Add(p);
                this.accu.AddElement(new Accu.Accu(false, false, key, p));
                int n = this.accu.FindByIndex(1).Value;
                this.accu.FindByIndex(1).Value = n + 1;
            }
        }

        /// <summary>
        /// Delete a key
        /// </summary>
        /// <param name="key">key name</param>
        public void DeleteParameter(string key)
        {
            int pos = this.pars.FindLastIndex(x => x.Name == key);
            if (pos != -1)
            {
                this.pars.RemoveAt(pos);
                this.accu.DeleteElement(pos + 6);
                int n = this.accu.FindByIndex(1).Value;
                this.accu.FindByIndex(1).Value = n - 1;
            }
        }

        /// <summary>
        /// Converts accumulator to a string representation
        /// </summary>
        /// <param name="index">iterator</param>
        /// <param name="child">child</param>
        /// <param name="subPv">printer variable</param>
        private static void ToString(int index, Accu.Accu child, PrinterVariable subPv)
        {
            subPv.Include = true;
            Accu.Accu e = child.Children.ElementAt(index);
            subPv.Value = "set-child.prt";
            subPv.AddVariable("value", e.Value.ToString());
            if (index + 1 < child.Children.Count())
            {
                PrinterVariable current = new PrinterVariable();
                current.Indent = false;
                current.Name = "next";
                Set.ToString(index + 1, child, current);
                subPv.AddVariable("next", current);
            }
            else
            {
                subPv.AddVariable("next", string.Empty);
            }
        }

        /// <summary>
        /// Converts this object into a string representation (source code)
        /// </summary>
        /// <returns>string representation as the source code</returns>
        public override string ToString()
        {
            PrinterObject po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "set.prt"));

            if (this.pars.Count > 0)
            {
                PrinterVariable pv = new PrinterVariable();
                pv.Include = true;
                pv.Name = "items";
                pv.Value = "node.prt";
                PrinterVariable current = new PrinterVariable();
                current.Name = "node";
                current.Indent = true;
                Set.ToString(6, this.accu, current);
                pv.AddVariable("node", current);

                po.AddVariable("items", pv);
            }
            return po.Execute();
        }

        #endregion

    }
}
