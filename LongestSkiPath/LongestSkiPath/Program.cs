using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LongestSkiPath
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = @"4 4 
4 8 7 3 
2 5 9 3 
6 3 2 5 
4 4 1 6";

            //var wc = new WebClient();
            //var stream = wc.OpenRead("http://s3-ap-southeast-1.amazonaws.com/geeks.redmart.com/coding-problems/map.txt");
            //Debug.Assert(stream != null, "stream != null");
            //string input = new StreamReader(stream).ReadToEnd();

            var sr = new StringReader(input);
            var size = sr.ReadLine();
            Debug.Assert(size != null, "size != null");
            var firstLine = size.Split(' ');
            var resolver = new SkiingResolver(Convert.ToInt32(firstLine[0]), Convert.ToInt32(firstLine[1]),
                sr.ReadToEnd());
            resolver.Calculate();
            Console.WriteLine(resolver.ToString());
            Console.WriteLine("The longest path (with a drop of " + resolver.GetLongestSkiingPathSteepness() + ") length is = " + resolver.GetLongestSkiingPathLength());
            Console.ReadKey();
        }
    }
}
