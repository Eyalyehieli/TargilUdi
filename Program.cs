using System;
using System.Linq;

namespace TargilUdi
{
    class Program
    {
        static void Main(string[] args)
        {
            int choose = 0, workingSetSize = 0;
            Algorithm algorithm;
            string strAccess = " ";
            string[] strAccessAfterSplit;
            Console.WriteLine("Which algorithm?\n3-LFU\n4-LRU\n");
            choose=int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the string access");
            //strAccess = Console.ReadLine();
            strAccess = "7 0 1 2 0 3 0 4 2 3 0 3 2 1 2";
            strAccessAfterSplit = strAccess.Split(' ');
            Console.WriteLine("Enter the working set size");
            workingSetSize = int.Parse(Console.ReadLine());
            algorithm = new Algorithm(strAccessAfterSplit, workingSetSize);
            switch(choose)
            {
                
                case 3 :algorithm.LFU();break;
                case 4: algorithm.LRU();break;
            }


            
        }
    }
}
