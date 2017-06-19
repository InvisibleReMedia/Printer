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
    public class LuigiList : LuigiElement
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
        public LuigiList(string n, IEnumerable<LuigiElement> v, LuigiElement p) : base(n, null, p)
        {
            this.mixedContent = true;
            this.Value = new List<LuigiElement>(v);
        }

        /// <summary>
        /// Constructor with input list
        /// </summary>
        /// <param name="n">name of the list</param>
        /// <param name="inType">type name of the content</param>
        /// <param name="v">input list</param>
        /// <param name="p">parent</param>
        public LuigiList(string n, string inType, IEnumerable<LuigiElement> v, LuigiElement p) : base(n, null, p)
        {
            this.mixedContent = false;
            this.contentTypeName = inType;
            this.Value = new List<LuigiElement>(v);
        }

        /// <summary>
        /// Constructor with an empty list mixed content
        /// </summary>
        /// <param name="n">name of the list</param>
        /// <param name="p">parent</param>
        public LuigiList(string n, LuigiElement p) : base(n, new List<LuigiElement>(), p)
        {
            this.mixedContent = true;
        }

        /// <summary>
        /// Constructor with an empty list
        /// </summary>
        /// <param name="n">name of the list</param>
        /// <param name="inType">type name of the content</param>
        /// <param name="p">parent</param>
        public LuigiList(string n, string inType, LuigiElement p) : base(n, new List<LuigiElement>(), p)
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
        public List<LuigiElement> Elements
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

            this.Elements.Add(e);
        }

        /// <summary>
        /// Insert an element
        /// </summary>
        /// <param name="index">index position</param>
        /// <param name="e"></param>
        public void InsertElement(int index, LuigiElement e)
        {
            if (!mixedContent && e.TypeName != this.ContentTypeName)
                throw new InvalidCastException(String.Format("{0} type name doesn't match {1} as content type name", e.TypeName, this.ContentTypeName));

            if (index < this.Elements.Count)
            {
                this.Elements.Insert(index, e);
            }
        }

        /// <summary>
        /// Edit element list
        /// </summary>
        /// <param name="index">index position</param>
        /// <param name="e">element to add</param>
        public void EditElement(int index, LuigiElement e)
        {
            if (!mixedContent && e.TypeName != this.ContentTypeName)
                throw new InvalidCastException(String.Format("{0} type name doesn't match {1} as content type name", e.TypeName, this.ContentTypeName));

            if (index < this.Elements.Count)
            {
                this.Elements[index] = e;
            }
        }

        /// <summary>
        /// Remove an element from the list
        /// </summary>
        /// <param name="index">index to remove</param>
        public void RemoveElement(int index)
        {
            if (index < this.Elements.Count)
            {
                this.Elements.RemoveAt(index);
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
            LuigiList l;
            if (this.CanHaveMixedContent)
            {
                l = new LuigiList(this.Name, parent);
                foreach (LuigiElement e in this.Elements)
                {
                    l.Elements.Add(e.CopyInto(l));
                }
            }
            else
            {
                l = new LuigiList(this.Name, this.ContentTypeName, parent);
                foreach (LuigiElement e in this.Elements)
                {
                    l.Elements.Add(e.CopyInto(l));
                }
            }
            return l;
        }

        #endregion
    }
}
