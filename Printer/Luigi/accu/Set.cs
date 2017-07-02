﻿using Printer;
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
    public class Set : Accu.Accu
    {

        #region Fields

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
            : base(false, false, false, n, null)
        {
            this.parent = p;
            this.root = p.Root;
            this.AddElement(new Accu.Accu(false, true, false, "type", this.GetType().Name));
            this.AddElement(new Accu.Accu(false, true, false, "count", 0));
            this.AddElement(new Accu.Accu(false, true, false, "print", "result"));
            this.AddElement(new Accu.Accu(false, true, false, "add", "result"));
            this.AddElement(new Accu.Accu(false, true, false, "insert", "result"));
            this.AddElement(new Accu.Accu(false, true, false, "remove", "result"));
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
                this.AddElement(new Accu.Accu(false, false, false, key, p));
                int n = this.FindByIndex(1).Value;
                this.FindByIndex(1).Value = n + 1;
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
                this.AddElement(new Accu.Accu(false, false, false, key, p));
                int n = this.FindByIndex(1).Value;
                this.FindByIndex(1).Value = n + 1;
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
                this.DeleteElement(pos + 6);
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
                Set.ToString(6, this, current);
                pv.AddVariable("node", current);

                po.AddVariable("items", pv);
            }
            return po.Execute();
        }

        #endregion

    }
}
