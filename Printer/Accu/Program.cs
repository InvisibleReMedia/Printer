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
            Accu root = new Accu("t", "");
            root.AddElement(new Accu("r", "1"));
            Accu r2 = new Accu("r2", "2");
            Accu s2 = new Accu("s2", "22");
            s2.AddElement(new Accu("t1", "221"));
            s2.AddElement(new Accu("t2", "222"));
            r2.AddElement(new Accu("s1", "21"));
            r2.AddElement(s2);
            r2.AddElement(new Accu("s3", "23"));
            root.AddElement(r2);
            root.AddElement(new Accu("r3", "3"));
            root.AddElement(new Accu("r4", "4"));

            Console.WriteLine(AccuWorker.ToString(root));

            Console.WriteLine("Touch your keyboard");
            Console.ReadKey();
        }
    }
}
