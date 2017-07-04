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
                    Luigi.LuigiVariable lv1 = new Luigi.LuigiVariable();
                    lv1.Name = "t";
                    lv1.LeftValueType = Luigi.LuigiVariable.LuigiVariableType.VAR;
                    lv1.RightValueType = Luigi.LuigiVariable.LuigiVariableType.REF;
                    lv1.Value = "expr-bool";
                    po.AddVariable("t", lv1);
                    po.UseVariable("t");
                    po.AddData(") {" + Environment.NewLine);
                    Luigi.LuigiVariable lv2 = new Luigi.LuigiVariable();
                    lv2.Name = "c";
                    lv2.LeftValueType = Luigi.LuigiVariable.LuigiVariableType.VAR;
                    lv2.RightValueType = Luigi.LuigiVariable.LuigiVariableType.REF;
                    lv2.Value = "code";
                    po.AddVariable("c", lv2);
                    po.UseVariable("c");
                    po.AddData(@"}
else {" + Environment.NewLine);
                    Luigi.LuigiVariable lv3 = new Luigi.LuigiVariable();
                    lv3.Name = "d";
                    lv3.LeftValueType = Luigi.LuigiVariable.LuigiVariableType.VAR;
                    lv3.RightValueType = Luigi.LuigiVariable.LuigiVariableType.REF;
                    lv3.Value = "code";
                    po.AddVariable("d", lv3);
                    po.UseVariable("d");
                    po.AddData(Environment.NewLine + "}" + Environment.NewLine);

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
