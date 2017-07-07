using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accumulate
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    throw new ArgumentException("USAGE : accumulate file.acc");
                }
                Luigi.LuigiObject po = null;
                FileInfo fi = new FileInfo(args[0]);
                if (fi.Exists)
                {
                    po = Luigi.LuigiObject.Load(args[0]);
                }
                else
                {
                    po = new Luigi.LuigiObject();
                    po.AddType("expr-bool", new Accu("expr-bool", "C/bool.prt"));
                    po.AddType("var", new Accu("var", "C/var.prt"));
                    po.AddType("code", new Accu("code", "C/add.prt"));

                    po.AddData("if (");
                    po.AddVariable("expr", po.CreateInstanceFromType("expr", "expr-bool"));
                    po.UseVariable("expr");
                    po.AddData(")" + Environment.NewLine);
                    po.AddVariable("then", po.CreateInstanceFromType("then", "code"));
                    po.UseVariable("then");
                    po.AddData(" else " + Environment.NewLine);
                    po.AddVariable("else", po.CreateInstanceFromType("else", "code"));
                    po.UseVariable("else");

                }


                Console.WriteLine(po.Execute());

                Console.WriteLine("code:");
                Console.WriteLine(po.ToString());

                Luigi.LuigiObject.Save(po, args[0]);
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
