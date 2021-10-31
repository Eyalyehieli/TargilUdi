using System;

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
            Console.WriteLine("Which algorithm?\n1-FIFO\n2-LIFO\n3-LFU\n4-LRU\n");
            choose=int.Parse(Console.ReadLine());
            Console.WriteLine("Enter the string access");
            strAccess = Console.ReadLine();
            strAccessAfterSplit = strAccess.Split(' ');
            Console.WriteLine("Enter the working set size");
            workingSetSize = int.Parse(Console.ReadLine());
            algorithm = new Algorithm(strAccessAfterSplit, workingSetSize);
            switch(choose)
            {
                case 1 :algorithm.FIFO();break;
                case 2 :algorithm.LIFO();break;
                case 3 :algorithm.LFU();break;
                case 4: algorithm.LRU();break;
            }


            
        }
    }
}
