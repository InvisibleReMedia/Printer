using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accu
{
    class Program
    {
        static void Main(string[] args)
        {
            Accu root = new Accu(false, false, "t", 0);
            root.AddElement(new Accu(false, false, "r", 1));
            Accu r2 = new Accu(false, false, "r2", 2);
            Accu s2 = new Accu(false, false, "s2", 22);
            s2.AddElement(new Accu(true, false, "t1", "s1"));
            s2.AddElement(new Accu(true, false, "t2", "s2"));
            r2.AddElement(new Accu(false, false, "s1", 21));
            r2.AddElement(s2);
            r2.AddElement(new Accu(false, false, "s3", 23));
            root.AddElement(r2);
            root.AddElement(new Accu(false, false, "r3", 3));
            root.AddElement(new Accu(false, false, "r4", 4));

            Console.WriteLine(AccuWorker.ToString(root));

            Console.WriteLine(AccuWorker.Execute(root, (d, l) =>
            {
                return d.ToString() + Environment.NewLine;
            }));

            Console.WriteLine("Touch your keyboard");
            Console.ReadKey();
        }
    }
}
