﻿using Printer;
using Accu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace Luigi
{
    /// <summary>
    /// Abstract class for Luigi language
    /// Takes all needs for each class
    /// </summary>
    [Serializable]
    public abstract class LuigiElement : ICloneable
    {

        #region Fields

        /// <summary>
        /// Root of this program
        /// </summary>
        protected LuigiObject root;
        /// <summary>
        /// Parent
        /// </summary>
        private LuigiElement parent;
        /// <summary>
        /// Data handler
        /// </summary>
        protected Accu.Accu data;
        /// <summary>
        /// Value
        /// </summary>
        private dynamic value;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor for top-level object
        /// </summary>
        /// <param name="n">name</param>
        /// <param name="v">value</param>
        protected LuigiElement(string n, dynamic v)
        {
            this.data = new Accu.Accu(n, this);
            this.value = v;
            this.parent = null;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="n">name</param>
        /// <param name="v">value</param>
        /// <param name="parent">parent</param>
        protected LuigiElement(string n, dynamic v, LuigiElement parent)
        {
            this.data = new Accu.Accu(n, this);
            this.value = v;
            this.parent = parent;
            this.root = parent.Root;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the root of this program
        /// </summary>
        public LuigiObject Root
        {
            get
            {
                return this.root;
            }
        }

        /// <summary>
        /// Gets the parent of this object
        /// </summary>
        public LuigiElement Parent
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
                return this.data.Name;
            }
        }

        /// <summary>
        /// Gets or sets the type name
        /// </summary>
        public string TypeName
        {
            get
            {
                return this.GetType().Name;
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

        #endregion

        #region Methods

        /// <summary>
        /// Write output as interpretation result
        /// </summary>
        /// <param name="pars">parameters</param>
        /// <returns>string value</returns>
        public abstract string Execute(Dictionary<string, string> pars);

        /// <summary>
        /// Write output as interpretation result
        /// </summary>
        /// <returns>output</returns>
        public string Execute()
        {
            return Accu.AccuWorker.Execute(this.data, (e, l) =>
            {
                LuigiElement le = e as LuigiElement;
                Dictionary<string, string> d = new Dictionary<string, string>();
                foreach (Accu.Accu a in l)
                {
                    d.Add(a.Name, a.Result);
                }
                return le.Execute(d);
            });
        }

        /// <summary>
        /// Load a file from disk
        /// </summary>
        /// <param name="fileName">full path of fileName</param>
        /// <returns>object</returns>
        public static LuigiElement Load(string fileName)
        {
            LuigiElement po = null;
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    po = bf.Deserialize(fs) as LuigiElement;
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

            return po;
        }


        /// <summary>
        /// Save a LuigiElement to disk
        /// </summary>
        /// <param name="obj">object to save</param>
        /// <param name="fileName">full path of fileName to save</param>
        public static void Save(LuigiElement obj, string fileName)
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
        public static LuigiElement Load(Stream stream)
        {
            LuigiElement po = null;
            BinaryFormatter bf = new BinaryFormatter();
            try
            {
                po = bf.Deserialize(stream) as LuigiElement;
            }
            catch (Exception)
            {
                throw;
            }

            return po;
        }


        /// <summary>
        /// Save a LuigiElement to memory
        /// You must close the stream after this method
        /// </summary>
        /// <param name="obj">object to save</param>
        /// <param name="stream">stream buffer</param>
        public static void Save(LuigiElement obj, Stream stream)
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
        /// Copy this into a new element
        /// </summary>
        /// <param name="parent">parent</param>
        /// <returns>a new element</returns>
        public abstract LuigiElement CopyInto(LuigiElement parent);

        /// <summary>
        /// Clone one element
        /// </summary>
        /// <returns>cloned object</returns>
        public object Clone()
        {
            return this.CopyInto(this.Parent);
        }

        #endregion
    }
}
