using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Printer
{
    /// <summary>
    /// A set of versions as a printer file
    /// </summary>
    public class PrinterVersion
    {

        #region Fields

        /// <summary>
        /// Latest major version number
        /// </summary>
        private int currentMajorVersion;

        /// <summary>
        /// Latest minor version number
        /// </summary>
        private int currentMinorVersion;

        /// <summary>
        /// Path file
        /// </summary>
        private string path;

        /// <summary>
        /// File name prefix
        /// </summary>
        private string fileName;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="path">path file</param>
        /// <param name="name">file name</param>
        public PrinterVersion(string path, string name)
        {
            this.path = path;
            this.fileName = name;
            this.currentMajorVersion = 1;
            this.currentMinorVersion = 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the latest version ID
        /// </summary>
        public string LatestVersion
        {
            get
            {
                return String.Format("{0}-{1}", this.currentMajorVersion, this.currentMinorVersion);
            }
        }

        /// <summary>
        /// Gets all relative versions
        /// </summary>
        public IEnumerable<string> Versions
        {
            get
            {
                List<string> list = new List<string>();
                DirectoryInfo di = new DirectoryInfo(this.path);
                FileInfo first = new FileInfo(Path.Combine(di.FullName, this.fileName, ".prt"));
                if (first.Exists)
                {
                    list.Add("1-0");
                }
                foreach (FileInfo fi in di.GetFiles(String.Format("{0}-*.prt", this.fileName)))
                {
                    Regex reg = new Regex(String.Format(@"^{0}-([^.]+)\.prt$", this.fileName));
                    Match m = reg.Match(fi.Name);
                    if (m.Success)
                    {
                        list.Add(m.Groups[1].Value);
                    }
                }
                return list;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Select a specific version
        /// </summary>
        /// <param name="version"></param>
        /// <returns>printer object selected</returns>
        public PrinterObject Select(string version)
        {
            if (!String.IsNullOrEmpty(version))
            {
                return PrinterObject.Load(String.Format("{0}-{1}.prt", Path.Combine(this.path, this.fileName), version));
            }
            else
            {
                return PrinterObject.Load(String.Format("{0}.prt", Path.Combine(this.path, this.fileName)));
            }
        }

        /// <summary>
        /// Add a new version
        /// </summary>
        /// <param name="po">printer object</param>
        public void AddVersion(PrinterObject po)
        {
            if (this.currentMinorVersion == 0 && this.currentMajorVersion == 1)
            {
                PrinterObject.Save(po, Path.Combine(this.path, this.fileName, ".prt"));
            }
            else
            {
                PrinterObject.Save(po, Path.Combine(this.path, this.fileName, this.LatestVersion, ".prt"));
            }
            if (this.currentMinorVersion == 9)
            {
                this.currentMinorVersion = 0;
                ++this.currentMajorVersion;
            }
            else
            {
                ++this.currentMinorVersion;
            }
        }

        #endregion

    }
}
