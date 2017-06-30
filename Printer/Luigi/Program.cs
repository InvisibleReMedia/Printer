using System;
using System.IO;
using Luigi.accu;

namespace Luigi
{
    /// <summary>
    /// The principal program
    /// </summary>
    class Program
    {
        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="args">command-line arguments</param>
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    throw new ArgumentException("USAGE : printer file.lgi");
                }

                FileInfo fi = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, args[0]));
                TopLevel top;

                if (fi.Exists)
                {
                    using (FileStream fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        top = TopLevel.Load(fs) as TopLevel;
                        fs.Close();
                    }
                }
                else
                {
                    top = new TopLevel();
                    Literal l = new Literal("r", top);
                    l.Delimiter = "//";
                    l.Text = "test avec délimiteur";
                    top.AddType(l);
                    Mapper m = new Mapper("m", top);
                    m.AddKey("s", ".", "mapper1");
                    m.AddKey("t", ".", "mapper2");
                    top.AddType(m);
                    Set v = new Set("v", top);
                    v.AddParameter("r", new Literal("p", v));
                    v.AddParameter("r2", new Literal("p2", v));
                    v.AddParameter("r3", new Literal("p3", v));
                    top.AddType(v);
                }



                using (FileStream fs = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, args[0]), FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    TopLevel.Save(top, fs);
                    fs.Close();
                }

                Console.WriteLine(top.ToString());

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Touch your keyboard");
                Console.ReadKey();
            }
        }
    }
}
