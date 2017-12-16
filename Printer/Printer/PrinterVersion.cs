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
            if (name.EndsWith(".prt"))
            {
                name = name.Substring(0, name.Length - 4);
            }
            this.fileName = name;
            this.FindLatestVersion();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the full name of the file
        /// </summary>
        public string FullName
        {
            get
            {
                return Path.Combine(this.path, this.fileName, this.LatestVersion, ".prt");
            }
        }

        /// <summary>
        /// Gets the file name without its extension
        /// </summary>
        public string FileName
        {
            get
            {
                return Path.Combine(this.fileName, this.LatestVersion, ".prt");
            }
        }

        /// <summary>
        /// Gets the latest version ID
        /// </summary>
        public string LatestVersion
        {
            get
            {
                if (currentMajorVersion == 1 && currentMinorVersion == 0)
                    return "";
                else
                    return String.Format("-{0}-{1}", this.currentMajorVersion, this.currentMinorVersion);
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
        /// Find latest version
        /// </summary>
        private void FindLatestVersion()
        {
            int? latestMajorVersion = null;
            int? latestMinorVersion = null;
            foreach (string v in this.Versions)
            {
                Regex r = new Regex("([1-9][0-9]+)-([0-9]*)");
                Match m = r.Match(v);
                if (m.Success)
                {
                    int major = Convert.ToInt32(m.Groups[1].Value);
                    int minor = Convert.ToInt32(m.Groups[2].Value);
                    if (latestMajorVersion.HasValue)
                    {
                        if (latestMajorVersion.Value < major)
                        {
                            latestMajorVersion = major;
                            if (latestMinorVersion.HasValue)
                            {
                                if (latestMinorVersion.Value < minor)
                                    currentMinorVersion = minor;
                            }
                            else
                                latestMinorVersion = minor;

                        }
                    }
                    else
                        latestMajorVersion = major;
                }
            }
            if (latestMajorVersion.HasValue)
            {
                currentMajorVersion = latestMajorVersion.Value;
                currentMinorVersion = latestMinorVersion.Value;
            }
            else
            {
                currentMajorVersion = 1;
                currentMinorVersion = 0;
            }
        }

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
        /// Create a new printer object
        /// </summary>
        /// <returns>printer object</returns>
        public PrinterObject Create()
        {
            return PrinterObject.Create(this);
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
