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
    /// State a mapper
    /// </summary>
    [Serializable]
    public class Mapper : Accu.Accu
    {

        #region Fields

        /// <summary>
        /// Keys
        /// </summary>
        private List<Literal> keys;

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
        public Mapper(string n, dynamic p)
            : base(false, false, false, n, null)
        {
            this.parent = p;
            this.root = p.Root;
            this.AddElement(new Accu.Accu(false, true, false, "type", this.GetType().Name));
            this.AddElement(new Accu.Accu(false, true, false, "count", 0));
            this.AddElement(new Accu.Accu(false, true, false, "print", "result"));
            this.keys = new List<Literal>();
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
        /// Gets the number of keys
        /// </summary>
        public int Count
        {
            get
            {
                return this.FindByIndex(1).Value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add a key
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="delimiter">delimiter</param>
        /// <param name="text">text</param>
        public void AddKey(string key, string delimiter, string text)
        {
            int pos = this.keys.FindLastIndex(x => x.Name == key);
            if (pos != -1)
            {
                this.keys[pos].Delimiter = delimiter;
                this.keys[pos].Text = text;
            }
            else
            {
                Literal lit = new Literal(key, this);
                lit.Delimiter = delimiter;
                lit.Text = text;
                this.keys.Add(lit);
                this.AddElement(new Accu.Accu(false, false, false, key, lit));
                int n = this.FindByIndex(1).Value;
                this.FindByIndex(1).Value = n + 1;
            }
        }

        /// <summary>
        /// Edit a key
        /// </summary>
        /// <param name="key">key name</param>
        /// <param name="delimiter">delimiter</param>
        /// <param name="text">text</param>
        public void EditKey(string key, string delimiter, string text)
        {
            int pos = this.keys.FindLastIndex(x => x.Name == key);
            if (pos != -1)
            {
                this.keys[pos].Delimiter = delimiter;
                this.keys[pos].Text = text;
            }
            else
            {
                Literal lit = new Literal(key, this);
                lit.Delimiter = delimiter;
                lit.Text = text;
                this.keys.Add(lit);
                this.AddElement(new Accu.Accu(false, false, false, key, lit));
                int n = this.FindByIndex(1).Value;
                this.FindByIndex(1).Value = n + 1;
            }
        }

        /// <summary>
        /// Delete a key
        /// </summary>
        /// <param name="key">key name</param>
        public void DeleteKey(string key)
        {
            int pos = this.keys.FindLastIndex(x => x.Name == key);
            if (pos != -1)
            {
                this.keys.RemoveAt(pos);
                this.DeleteElement(pos + 3);
                int n = this.FindByIndex(1).Value;
                this.FindByIndex(1).Value = n - 1;
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
            subPv.Value = "mapper-child.prt";
            subPv.AddVariable("name", e.Name);
            subPv.AddVariable("value", e.Value.ToString());
            if (index + 1 < child.Children.Count())
            {
                PrinterVariable current = new PrinterVariable();
                current.Indent = false;
                current.Name = "next";
                Mapper.ToString(index + 1, child, current);
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
            PrinterObject po = PrinterObject.Load(Path.Combine(PrinterObject.PrinterDirectory, "languages", "Luigi", "mapper.prt"));

            if (this.keys.Count > 0)
            {
                PrinterVariable pv = new PrinterVariable();
                pv.Include = true;
                pv.Name = "items";
                pv.Value = "node.prt";
                PrinterVariable current = new PrinterVariable();
                current.Name = "node";
                current.Indent = true;
                Mapper.ToString(3, this, current);
                pv.AddVariable("node", current);

                po.AddVariable("items", pv);
            }
            return po.Execute();
        }

        #endregion

    }
}
