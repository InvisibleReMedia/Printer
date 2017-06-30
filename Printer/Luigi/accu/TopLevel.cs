using Printer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Luigi.accu
{
    /// <summary>
    /// Top level of the structure
    /// </summary>
    [Serializable]
    public class TopLevel
    {

        #region Fields

        /// <summary>
        /// Accumulator
        /// </summary>
        private Accu.Accu accu;

        /// <summary>
        /// Keys
        /// </summary>
        private List<Type> types;

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
        public TopLevel()
        {
            this.parent = null;
            this.root = this;
            this.accu = new Accu.Accu(false, false, false, "root", this);
            this.accu.AddElement(new Accu.Accu(false, true, false, "type", this.GetType().Name));
            this.accu.AddElement(new Accu.Accu(false, true, false, "count", 0));
            this.accu.AddElement(new Accu.Accu(false, true, false, "print", "result"));
            this.types = new List<Type>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the accumulator
        /// </summary>
        public Accu.Accu Accumulator
        {
            get
            {
                return this.accu;
            }
        }

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
        /// <param name="v">value</param>
        public void AddType(dynamic v)
        {
            int pos = this.types.FindLastIndex(x => x.Name == v.Name);
            if (pos != -1)
            {
                this.types[pos].Value = v;
            }
            else
            {
                Type t = new Type(v, this);
                this.types.Add(t);
                this.accu.AddElement(new Accu.Accu(false, false, false, v.Name, t));
                int n = this.accu.FindByIndex(1).Value;
                this.accu.FindByIndex(1).Value = n + 1;
            }
        }

        /// <summary>
        /// Edit a key
        /// </summary>
        /// <param name="v">value</param>
        public void EditType(dynamic v)
        {
            int pos = this.types.FindLastIndex(x => x.Name == v.Name);
            if (pos != -1)
            {
                this.types[pos].Value = v;
            }
            else
            {
                Type t = new Type(v, this);
                this.types.Add(t);
                this.accu.AddElement(new Accu.Accu(false, false, false, v.Name, t));
                int n = this.accu.FindByIndex(1).Value;
                this.accu.FindByIndex(1).Value = n + 1;
            }
        }

        /// <summary>
        /// Delete a key
        /// </summary>
        /// <param name="key">key name</param>
        public void DeleteType(string key)
        {
            int pos = this.types.FindLastIndex(x => x.Name == key);
            if (pos != -1)
            {
                this.types.RemoveAt(pos);
                this.accu.DeleteElement(pos + 3);
                int n = this.accu.FindByIndex(1).Value;
                this.accu.FindByIndex(1).Value = n - 1;
            }
        }

        /// <summary>
        /// Load a file from disk
        /// </summary>
        /// <param name="fileName">full path of fileName</param>
        /// <returns>object</returns>
        public static TopLevel Load(string fileName)
        {
            TopLevel top = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    top = bf.Deserialize(fs) as TopLevel;
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

            return top;
        }


        /// <summary>
        /// Save a LuigiElement to disk
        /// </summary>
        /// <param name="obj">object to save</param>
        /// <param name="fileName">full path of fileName to save</param>
        public static void Save(TopLevel obj, string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    bf.Serialize(fs, obj);
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
        public static TopLevel Load(Stream stream)
        {
            TopLevel top = null;
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                top = bf.Deserialize(stream) as TopLevel;
            }
            catch (Exception)
            {
                throw;
            }

            return top;
        }


        /// <summary>
        /// Save a LuigiElement to memory
        /// You must close the stream after this method
        /// </summary>
        /// <param name="obj">object to save</param>
        /// <param name="stream">stream buffer</param>
        public static void Save(TopLevel obj, Stream stream)
        {
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                bf.Serialize(stream, obj);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Recursive find by id
        /// </summary>
        /// <param name="root">first accu</param>
        /// <param name="name">name</param>
        /// <returns>last accu</returns>
        public static dynamic RecursiveFindByName(TopLevel root, string name)
        {
            string[] tab = name.Split('.');
            Accu.Accu current = root.Accumulator;
            foreach (string s in tab)
            {
                Accu.Accu a = current.FindByName(s);
                if (!a.IsMethodCall)
                {
                    current = a.Value.Value.Accumulator;
                }
            }
            return current;
        }

        /// <summary>
        /// Converts this object into a string representation (source code)
        /// </summary>
        /// <returns>string representation as the source code</returns>
        public override string ToString()
        {
            return Accu.AccuWorker.ToString(this.accu);
        }

        #endregion

    }
}
