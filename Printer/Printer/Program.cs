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

                    PrinterVariable pv = new PrinterVariable();
                    pv.Name = "condition";
                    pv.Include = true;
                    pv.Indent = true;
                    pv.AddVariable("expression", "true");
                    pv.AddVariable("then", "code");
                    pv.AddVariable("else", "code");
                    pv.Value = "C#.NET/if.prt";

                    po.AddVariable("condition", pv);

                    po.UseVariable("condition");
                    po.AddData(@"
        }
    }
}
");
                }


                Console.WriteLine(po.Execute());

                Console.WriteLine("code:");
                Console.WriteLine(po.ToString());

                PrinterObject.Save(po, args[0]);
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
