using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Printer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    throw new ArgumentException("USAGE : printer file.prt");
                }
                PrinterObject po = null;
                FileInfo fi = new FileInfo(args[0]);
                if (fi.Exists)
                {
                    po = PrinterObject.Load(args[0]);
                }
                else
                {
                    po = new PrinterObject();
                    po.AddData(@"using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ");
                    po.AddData("[Printer]");
                    po.AddData(@"
{
    class ");
                    po.AddData("[Program]");
                    po.AddData(@"
    {
        static void Main(string[] args)
        {
            //");
                    po.AddData("[code]");
                    po.AddData(@"
        }
    }
}
");
                }

                StringBuilder sb = new StringBuilder();
                po.Execute(sb);
                Console.WriteLine(sb.ToString());

                PrinterObject.Save(po, args[0]);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
