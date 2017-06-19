using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luigi
{
    /// <summary>
    /// A list of luigi instance elements
    /// </summary>
    [Serializable]
    public class LuigiDictionary : LuigiElement
    {

        #region Fields

        /// <summary>
        /// Indicates the type of the content
        /// </summary>
        private string contentTypeName;
        /// <summary>
        /// Indicates if this list can have mixed content
        /// </summary>
        private bool mixedContent;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor with input list mixed content
        /// </summary>
        /// <param name="n">name of the list</param>
        /// <param name="v">input list</param>
        /// <param name="p">parent</param>
        public LuigiDictionary(string n, IEnumerable<LuigiElement> v, LuigiElement p) : base(n, null, p)
        {
            this.mixedContent = true;
            this.Value = new Dictionary<string, LuigiElement>(v.ToDictionary(x => x.Name));
        }

        /// <summary>
        /// Constructor with input list
        /// </summary>
        /// <param name="n">name of the list</param>
        /// <param name="inType">type name of the content</param>
        /// <param name="v">input list</param>
        /// <param name="p">parent</param>
        public LuigiDictionary(string n, string inType, IEnumerable<LuigiElement> v, LuigiElement p) : base(n, null, p)
        {
            this.mixedContent = false;
            this.contentTypeName = inType;
            this.Value = new Dictionary<string, LuigiElement>(v.ToDictionary(x => x.Name));
        }

        /// <summary>
        /// Constructor with an empty list mixed content
        /// </summary>
        /// <param name="n">name of the list</param>
        /// <param name="p">parent</param>
        public LuigiDictionary(string n, LuigiElement p) : base(n, new Dictionary<string, LuigiElement>(), p)
        {
            this.mixedContent = true;
        }

        /// <summary>
        /// Constructor with an empty list
        /// </summary>
        /// <param name="n">name of the list</param>
        /// <param name="inType">type name of the content</param>
        /// <param name="p">parent</param>
        public LuigiDictionary(string n, string inType, LuigiElement p)
            : base(n, new Dictionary<string, LuigiElement>(), p)
        {
            this.contentTypeName = inType;
            this.mixedContent = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the mixed content switch
        /// </summary>
        public bool CanHaveMixedContent
        {
            get
            {
                return this.mixedContent;
            }
        }

        /// <summary>
        /// Gets the content type name
        /// </summary>
        public string ContentTypeName
        {
            get
            {
                if (this.mixedContent) return "";
                else return this.contentTypeName;
            }
        }

        /// <summary>
        /// Gets the elements object
        /// </summary>
        public Dictionary<string, LuigiElement> Elements
        {
            get
            {
                return this.Value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Add element list
        /// </summary>
        /// <param name="e">element to add</param>
        public void AddElement(LuigiElement e)
        {
            if (!mixedContent && e.TypeName != this.ContentTypeName)
                throw new InvalidCastException(String.Format("{0} type name doesn't match {1} as content type name", e.TypeName, this.ContentTypeName));

            if (this.Elements.ContainsKey(e.Name))
            {
                this.Elements[e.Name] = e;
            }
            else
            {
                this.Elements.Add(e.Name, e);
            }
        }

        /// <summary>
        /// Change the name of an item
        /// </summary>
        /// <param name="oldName">an existing item name</param>
        /// <param name="newName">the new name of the same item</param>
        public void ChangeName(string oldName, string newName)
        {
            if (this.Elements.ContainsKey(oldName))
            {
                LuigiElement e = this.Elements[oldName];
                e.Name = newName;
                this.Elements.Remove(oldName);
                this.AddElement(e);
            }
        }

        /// <summary>
        /// Edit element list
        /// </summary>
        /// <param name="e">element to add</param>
        public void EditElement(LuigiElement e)
        {
            if (!mixedContent && e.TypeName != this.ContentTypeName)
                throw new InvalidCastException(String.Format("{0} type name doesn't match {1} as content type name", e.TypeName, this.ContentTypeName));

            if (this.Elements.ContainsKey(e.Name))
            {
                this.Elements[e.Name] = e;
            }
            else
            {
                this.Elements.Add(e.Name, e);
            }
        }

        /// <summary>
        /// Remove an element from the list
        /// </summary>
        /// <param name="nameToRemove">name to remove</param>
        public void RemoveElement(string nameToRemove)
        {
            if (this.Elements.ContainsKey(nameToRemove))
            {
                this.Elements.Remove(nameToRemove);
            }
        }

        /// <summary>
        /// Execute the process of a list
        /// </summary>
        /// <param name="w">writer</param>
        /// <param name="indentValue">indent</param>
        public override void Execute(TextWriter w, ref int indentValue)
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
            LuigiDictionary d;
            if (this.CanHaveMixedContent)
            {
                d = new LuigiDictionary(this.Name, parent);
            }
            else
            {
                d = new LuigiDictionary(this.Name, this.ContentTypeName, parent);
            }
            foreach (KeyValuePair<string, LuigiElement> kv in this.Elements)
            {
                d.AddElement(kv.Value.CopyInto(d));
            }
            return d;
        }

        #endregion
    }
}
