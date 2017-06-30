using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accu
{
    /// <summary>
    /// Accumulator
    /// </summary>
    [Serializable]
    public class Accu
    {

        #region Fields

        /// <summary>
        /// Children
        /// </summary>
        private List<Accu> childs;

        /// <summary>
        /// Name
        /// </summary>
        private string name;

        /// <summary>
        /// Value
        /// </summary>
        private dynamic value;

        /// <summary>
        /// Result when operated value
        /// </summary>
        private string res;

        /// <summary>
        /// Method call switch
        /// </summary>
        private bool methodCall;

        /// <summary>
        /// This element is a reference to an another by its name
        /// value contains its name
        /// </summary>
        private bool isRef;

        /// <summary>
        /// True if result has been computed
        /// </summary>
        private bool done;

        #endregion

        #region Constructors

        /// <summary>
        /// Reference constructor
        /// </summary>
        /// <param name="f">make as reference</param>
        /// <param name="m">make as method call</param>
        /// <param name="n">name</param>
        /// <param name="r">reference name</param>
        public Accu(bool f, bool m, string n, string r)
        {
            this.done = false;
            this.isRef = f;
            this.methodCall = m;
            this.name = n;
            this.value = r;
            this.childs = new List<Accu>();
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="f">make as reference</param>
        /// <param name="m">make as method call</param>
        /// <param name="n">name</param>
        /// <param name="v">value</param>
        public Accu(bool f, bool m, string n, dynamic v)
        {
            this.done = false;
            this.isRef = f;
            this.methodCall = m;
            this.name = n;
            this.value = v;
            this.childs = new List<Accu>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public dynamic Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = value;
            }
        }

        /// <summary>
        /// Gets following children
        /// </summary>
        public IEnumerable<Accu> Children
        {
            get
            {
                return this.childs;
            }
        }

        /// <summary>
        /// Gets if its a reference
        /// or not
        /// </summary>
        public bool IsReference
        {
            get
            {
                return this.isRef;
            }
        }

        /// <summary>
        /// Gets if its a method call
        /// or not
        /// </summary>
        public bool IsMethodCall
        {
            get
            {
                return this.methodCall;
            }
        }

        /// <summary>
        /// Gets or sets the result interpretation
        /// </summary>
        public string Result
        {
            get
            {
                return this.res;
            }
            set
            {
                this.res = value;
            }
        }

        /// <summary>
        /// Gets or sets the result
        /// </summary>
        public bool HasResult
        {
            get
            {
                return this.done;
            }
            set
            {
                this.done = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="workingFun">a set of functions that work on value</param>
        /// <returns>list of objects</returns>
        public Dictionary<string, Accu> Execute(Func<dynamic, IEnumerable<Accu>, string> workingFun)
        {
            Dictionary<string, Accu> dict = new Dictionary<string, Accu>();
            foreach (Accu e in this.Children)
            {
                dict.Concat(e.Execute(workingFun));
            }
            if (!this.HasResult)
            {
                if (this.IsReference)
                {
                    if (dict.ContainsKey(this.Value.ToString()))
                    {
                        Accu r = dict[this.Value.ToString()];
                        if (!r.HasResult)
                        {
                            r.Result = workingFun(r.Value, r.Children);
                            r.HasResult = true;
                        }
                    }
                }
                else
                {
                    this.Result = workingFun(this.Value, this.Children);
                    this.HasResult = true;
                }
            }
            return dict;
        }

        /// <summary>
        /// Set a reference (cannot be undo)
        /// </summary>
        /// <param name="referenceName">reference name</param>
        public void SetReference(string referenceName)
        {
            this.value = referenceName;
            this.isRef = true;
        }

        /// <summary>
        /// Set a method call (cannot be undo)
        /// </summary>
        /// <param name="methodName">method name</param>
        public void SetMethodCall(string methodName)
        {
            this.value = methodName;
            this.isRef = false;
            this.methodCall = true;
        }

        /// <summary>
        /// Add an element at the end
        /// of the list
        /// </summary>
        /// <param name="a">element</param>
        public void AddElement(Accu a)
        {
            int pos = this.childs.FindLastIndex(x => x.Name == a.Name);
            if (pos != -1)
            {
                this.childs[pos] = a;
            }
            else
            {
                this.childs.Add(a);
            }
        }

        /// <summary>
        /// Insert an element at the index position
        /// </summary>
        /// <param name="index">index where to inser</param>
        /// <param name="a">element</param>
        public void InsertElement(int index, Accu a)
        {
            this.childs.Insert(index, a);
        }

        /// <summary>
        /// Edit an element
        /// </summary>
        /// <param name="a">element to change</param>
        public void EditElement(Accu a)
        {
            int pos = this.childs.FindLastIndex(x => x.Name == a.Name);
            if (pos != -1)
            {
                this.childs[pos] = a;
            }
            else
            {
                this.childs.Add(a);
            }
        }

        /// <summary>
        /// Delete an element
        /// </summary>
        /// <param name="index">index position to remove</param>
        public void DeleteElement(int index)
        {
            this.childs.RemoveAt(index);
        }

        /// <summary>
        /// Find a specific accu with the same name
        /// that's supplied
        /// </summary>
        /// <param name="name">supplied name</param>
        /// <returns>Accu</returns>
        /// <exception cref="KeyNotFoundException">if the name was not found</exception>
        public Accu FindByName(string name)
        {
            Accu a = this.childs.Find(x => x.Name == name);
            if (a != null)
            {
                return a;
            }
            else
            {
                throw new KeyNotFoundException(String.Format("{0} n'est pas trouvé", name));
            }
        }

        /// <summary>
        /// Find a specific accu with the same index
        /// that's supplied
        /// </summary>
        /// <param name="index">index position</param>
        /// <returns>Accu</returns>
        /// <exception cref="IndexOutOfRangeException">if the name was not found</exception>
        public Accu FindByIndex(int index)
        {
            return this.childs.ElementAt(index);
        }

        /// <summary>
        /// Recursive find by id
        /// </summary>
        /// <param name="root">first accu</param>
        /// <param name="name">name</param>
        /// <returns>last accu</returns>
        public static Accu RecursiveFindByName(Accu root, string name)
        {
            string[] tab = name.Split('.');
            Accu current = root;
            foreach (string s in tab)
            {
                current = current.FindByName(s);
            }
            return current;
        }

        /// <summary>
        /// Recursive find by name
        /// </summary>
        /// <param name="root">first accu</param>
        /// <param name="name">name</param>
        /// <returns>last accu</returns>
        public static Accu RecursiveFindByIndex(Accu root, string name)
        {
            string[] tab = name.Split('.');
            Accu current = root;
            foreach (string s in tab)
            {
                int pos = Convert.ToInt32(s);
                current = current.FindByIndex(pos);
            }
            return current;
        }

        #endregion
    }
}
