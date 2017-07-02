using Printer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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
        protected string name;

        /// <summary>
        /// This is a file name of a .prt file
        /// </summary>
        private string fileName;

        /// <summary>
        /// Result when operated value
        /// </summary>
        private string res;

        /// <summary>
        /// Type of accu object
        /// </summary>
        private AccuType type;

        /// <summary>
        /// True if result has been computed
        /// </summary>
        private bool done;

        #endregion

        #region Inner Class

        /// <summary>
        /// Accu type
        /// </summary>
        public enum AccuType
        {
            /// <summary>
            /// Normal
            /// </summary>
            NORMAL,
            /// <summary>
            /// As reference
            /// </summary>
            REFERENCE,
            /// <summary>
            /// As method
            /// </summary>
            METHOD,
            /// <summary>
            /// Runnable
            /// </summary>
            RUN,
            /// <summary>
            /// Constant
            /// </summary>
            CONST,
            /// <summary>
            /// Printer with no file
            /// </summary>
            INLINE
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constant or filename constructor
        /// </summary>
        /// <param name="t">type</param>
        /// <param name="n">name</param>
        /// <param name="s">file name or value</param>
        public Accu(AccuType t, string n, string s)
        {
            this.type = t;
            this.name = n;
            this.childs = new List<Accu>();
            if (t == AccuType.CONST)
            {
                this.res = s;
                this.done = true;
            }
            else
            {
                this.done = false;
                if (t != AccuType.INLINE)
                {
                    this.fileName = s;
                    PrinterObject po = PrinterObject.Load(s);
                    ImportParameters(po, this);
                }
                else
                {
                    throw new InvalidOperationException("Cannot have a file name caused by an inline state");
                }
            }
        }

        /// <summary>
        /// Constructor with a printer object
        /// </summary>
        /// <param name="t">type</param>
        /// <param name="n">name</param>
        /// <param name="po">printer object</param>
        public Accu(AccuType t, string n, PrinterObject po)
        {
            this.type = t;
            this.name = n;
            this.childs = new List<Accu>();
            if (t == AccuType.CONST)
            {
                this.res = po.Execute();
                this.done = true;
            }
            else
            {
                this.done = false;
                if (t == AccuType.INLINE)
                {
                    ImportParameters(po, this);
                }
                else
                {
                    throw new InvalidOperationException("Cannot make an import without an inline state");
                }
            }
        }

        /// <summary>
        /// Constructor with an included printer variable
        /// </summary>
        /// <param name="t">type</param>
        /// <param name="n">name</param>
        /// <param name="pv">printer variable</param>
        /// <param name="src">source printer object</param>
        public Accu(AccuType t, string n, PrinterVariable pv, PrinterObject src)
        {
            this.type = t;
            this.name = n;
            this.childs = new List<Accu>();
            if (t == AccuType.CONST)
            {
                this.res = pv.Execute(src.Configuration, src.CurrentDirectory);
                this.done = true;
            }
            else
            {
                this.done = false;
                if (t == AccuType.INLINE)
                {
                    ImportParameters(pv, this, src);
                }
                else
                {
                    throw new InvalidOperationException("Cannot make an import without an inline state");
                }
            }
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
        /// Gets or sets the file name
        /// </summary>
        public dynamic FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                this.fileName = value;
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
        public bool IsStandardUse
        {
            get
            {
                return this.type == AccuType.NORMAL;
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
                return this.type == AccuType.REFERENCE;
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
                return this.type == AccuType.METHOD;
            }
        }

        /// <summary>
        /// Gets if a sequence of terms
        /// or not
        /// </summary>
        public bool IsRunnable
        {
            get
            {
                return this.type == AccuType.RUN;
            }
        }

        /// <summary>
        /// Gets if a constant string
        /// or not
        /// </summary>
        public bool IsConstant
        {
            get
            {
                return this.type == AccuType.CONST;
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
                else if (!this.IsMethodCall)
                {
                    this.Result = workingFun(this.Value, this.Children);
                    this.HasResult = true;
                }
                else
                {
                    this.Result = string.Empty;
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
            this.name = referenceName;
            this.type = AccuType.REFERENCE;
        }

        /// <summary>
        /// Set a method call (cannot be undo)
        /// </summary>
        /// <param name="methodName">method name</param>
        public void SetMethodCall(string methodName)
        {
            this.name = methodName;
            this.type = AccuType.METHOD;
        }

        /// <summary>
        /// Set a sequence of terms (cannot be undo)
        /// </summary>
        /// <param name="root">root accu</param>
        /// <param name="name">sequence of terms</param>
        public void SetRunnable(Accu root, params string[] name)
        {
            string path = String.Join(".", name);
            this.SetRunnable(root, path);
        }

        /// <summary>
        /// Set a sequence of terms (cannot be undo)
        /// </summary>
        /// <param name="path">path string</param>
        public void SetRunnable(string path)
        {
            this.name = path;
            this.type = AccuType.RUN;
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
            Accu a = this.childs.FindLast(x => x.Name == name);
            if (a != null)
            {
                return a;
            }
            else
            {
                throw new KeyNotFoundException(String.Format("{0} not found", name));
            }
        }

        /// <summary>
        /// Find a specific accu with the same name
        /// that's supplied
        /// </summary>
        /// <param name="name">supplied name</param>
        /// <returns>true of false</returns>
        public bool ExistByName(string name)
        {
            return this.childs.FindLast(x => x.Name == name) != null;
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

        /// <summary>
        /// Load a file from disk
        /// </summary>
        /// <param name="fileName">full path of fileName</param>
        /// <returns>object</returns>
        public static Accu Load(string fileName)
        {
            Accu a = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    a = bf.Deserialize(fs) as Accu;
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }

            return a;
        }


        /// <summary>
        /// Save a PrinterObject to disk
        /// </summary>
        /// <param name="a">object to save</param>
        /// <param name="fileName">full path of fileName to save</param>
        public static void Save(Accu a, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    bf.Serialize(fs, a);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// Load a file from memory
        /// You must close the stream after this method
        /// </summary>
        /// <param name="stream">stream buffer</param>
        /// <returns>object</returns>
        public static Accu Load(Stream stream)
        {
            Accu a = null;
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                a = bf.Deserialize(stream) as Accu;
            }
            catch (Exception)
            {
                throw;
            }

            return a;
        }


        /// <summary>
        /// Save a PrinterObject to memory
        /// You must close the stream after this method
        /// </summary>
        /// <param name="a">object to save</param>
        /// <param name="stream">stream buffer</param>
        public static void Save(Accu a, Stream stream)
        {
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                bf.Serialize(stream, a);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Copy parameters from Accu object
        /// into a printer object
        /// </summary>
        /// <param name="a">accu</param>
        /// <param name="pv">Accu variable</param>
        /// <param name="src">source printer object</param>
        public static void CopyParameters(Accu a, PrinterVariable pv, PrinterObject src)
        {

            foreach (Accu child in a.Children)
            {
                if (child.type == AccuType.CONST)
                {
                    pv.AddVariable(child.Name, child.Result);
                }
                else if (child.type == AccuType.INLINE)
                {
                    PrinterVariable pv = 
                    CopyParameters(child, pv, src);
                }
                pv.AddVariable(child.Name, child.FileName);
                if (!pv.)
                {

                    if (sub.Include)
                    {
                        Accu child = new Accu(AccuType.INLINE, sub.Name, sub, src);
                        ImportParameters(sub, child, src);
                        a.AddElement(child);
                    }
                    else
                    {
                        a.AddElement(new Accu(AccuType.CONST, sub.Name, sub.Value));
                    }
                }
                else
                {
                    if (sub.Include)
                    {
                        Accu child = a.FindByName(sub.Name);
                        ImportParameters(sub, child, src);
                        a.EditElement(child);
                    }
                    else
                    {
                        Accu child = a.FindByName(sub.Name);
                        ImportParameters(sub, child, src);
                        a.EditElement(child);
                    }
                }
            }
        }

        /// <summary>
        /// Import parameters from printer object
        /// and creates them into accu
        /// </summary>
        /// <param name="pv">printer variable</param>
        /// <param name="a">accu</param>
        /// <param name="src">source printer object</param>
        public static void ImportParameters(PrinterVariable pv, Accu a, PrinterObject src) {

            foreach(PrinterVariable sub in pv.Values)
            {
                if (!a.ExistByName(sub.Name))
                {

                    if (sub.Include)
                    {
                        Accu child = new Accu(AccuType.INLINE, sub.Name, sub, src);
                        ImportParameters(sub, child, src);
                        a.AddElement(child);
                    }
                    else
                    {
                        a.AddElement(new Accu(AccuType.CONST, sub.Name, sub.Value));
                    }
                }
                else
                {
                    if (sub.Include)
                    {
                        Accu child = a.FindByName(sub.Name);
                        ImportParameters(sub, child, src);
                        a.EditElement(child);
                    }
                    else
                    {
                        Accu child = a.FindByName(sub.Name);
                        ImportParameters(sub, child, src);
                        a.EditElement(child);
                    }
                }
            }
        }

        /// <summary>
        /// Import parameters from printer object
        /// and creates them into accu
        /// </summary>
        /// <param name="po">printer object</param>
        /// <param name="a">accu</param>
        public static void ImportParameters(PrinterObject po, Accu a) {

            foreach(PrinterVariable pv in po.Values)
            {
                if (!a.ExistByName(pv.Name)) {

                    if (pv.Include)
                    {
                        Accu child = new Accu(AccuType.INLINE, pv.Name, pv, po);
                        ImportParameters(pv, child, po);
                        a.AddElement(child);
                    }
                    else
                    {
                        a.AddElement(new Accu(AccuType.CONST, pv.Name, pv.Value));
                    }
                }
                else
                {
                    if (pv.Include)
                    {
                        Accu child = a.FindByName(pv.Name);
                        ImportParameters(pv, child, po);
                        a.EditElement(child);
                    }
                    else
                    {
                        Accu child = a.FindByName(pv.Name);
                        ImportParameters(pv, child, po);
                        a.EditElement(child);
                    }
                }
            }
        }

        /// <summary>
        /// Converts Accumulator to a string
        /// </summary>
        /// <param name="pv">variable for printing</param>
        /// <returns>string result</returns>
        public virtual void ToString(PrinterVariable pv)
        {
            if (this.IsReference)
            {
                pv.Value = Path.Combine("Accu", "ref-child.prt");
                pv.AddVariable("name", this.Name);
                pv.AddVariable("value", this.Value);
            }
            else if (this.IsRunnable)
            {
                pv.Value = Path.Combine("Accu", "run-child.prt");
                pv.AddVariable("name", this.Name);
                pv.AddVariable("value", this.Value.ToString());
            }
            else
            {
                pv.Value = Path.Combine("Accu", "child.prt");
                pv.AddVariable("name", this.Name);
                pv.AddVariable("value", this.Value.ToString());
            }
            pv.AddVariable("next", string.Empty);
        }

        #endregion
    }
}
