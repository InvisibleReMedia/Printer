using System;
using System.IO;

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
                LuigiObject lo;

                if (fi.Exists)
                {
                    using (FileStream fs = fi.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        lo = LuigiObject.Load(fs) as LuigiObject;
                        fs.Close();
                    }
                }
                else
                {
                    lo = new LuigiObject("test");
                }
                LuigiLiteral li = new LuigiLiteral("v", false, ".", "test", lo);
                lo.AddElement(li.Clone() as LuigiLiteral);
                LuigiMapper ma = new LuigiMapper("m", false, lo);
                LuigiLiteral li2 = new LuigiLiteral("s", false, ".", "a", ma);
                ma.AddElement(li2.Clone() as LuigiLiteral);
                li2 = new LuigiLiteral("x", false, ".", "b", ma);
                ma.AddElement(li2.Clone() as LuigiLiteral);
                lo.AddElement(ma.Clone() as LuigiMapper);
                LuigiSet s = new LuigiSet("s", false, lo);
                LuigiParameter par = new LuigiParameter("p1", li, s);
                s.AddElement(par.Clone() as LuigiParameter);
                LuigiParameter par2 = new LuigiParameter("p2", li2, s);
                s.AddElement(par2.Clone() as LuigiParameter);
                LuigiParameter par3 = new LuigiParameter("p3", ma, s);
                s.AddElement(par3.Clone() as LuigiParameter);
                lo.AddElement(s.Clone() as LuigiSet);
                LuigiVariable var1 = new LuigiVariable("v", ma, lo);
                lo.AddElement(var1.Clone() as LuigiVariable);

                Console.WriteLine(lo.Execute());

                using (FileStream fs = new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, args[0]), FileMode.Create, FileAccess.Write, FileShare.Write))
                {
                    LuigiObject.Save(lo, fs);
                    fs.Close();
                }

                Console.WriteLine(lo.ToString());

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
