using System;
using System.Collections.Generic;
using System.Text;

namespace TargilUdi
{
    class Algorithm
    {
        private int numberOfpageFault;
        private double missRatio;
        private int[] swapInCounter;
        private int[] swapOutCounter;
        private string[] strAccess;
        private int  workingSetSize;
        
       public int findMaxByInt (string[] str)
        {
            int max = -1;
            foreach(string s in str)
            {
                if(Int32.Parse(s)>max)
                {
                    max = Int32.Parse(s);
                }
            }
            return max;
        }
        
        public Algorithm(string[] strAccess,int workingSetSize)
        {
            this.numberOfpageFault = 0;
            this.missRatio = 0;
            this.swapInCounter = new int[findMaxByInt(strAccess) +1];
            this.swapOutCounter = new int[findMaxByInt(strAccess) + 1];
            this.strAccess = strAccess;
            this.workingSetSize = workingSetSize;
        }
 
        public void LRU()
        {
            Dictionary<string, Page> dic = new Dictionary<string, Page>();
            Page temp,temp1;
            List<Page> pages = new List<Page>();
            Queue<Page> lastIgnored = new Queue<Page>();
            Queue<Page> pos = new Queue<Page>();
            Page newPage;
            foreach (string c in strAccess)
            {
                if (!isExistList(c, pages))
                {
                    if (pages.Count < workingSetSize)
                    {
                        temp1 = new Page(c);
                        if (!dic.ContainsKey(c)) { dic.Add(c, temp1); }
                        pages.Add(temp1);
                        swapInCounter[Convert.ToInt32(c)]++;
                        Console.WriteLine("swap in " + c);
                        EnterToQueue(lastIgnored, temp1);
                    }
                    else
                    {   
                        temp = lastIgnored.Dequeue();
                        pages.Remove(dic[temp.getValue()]);
                        dic.Remove(c);
                        newPage = new Page(c);
                        if (!dic.ContainsKey(c)) { dic.Add(c, newPage); }
                        pages.Add(newPage);
                        EnterToQueue(lastIgnored,newPage);
                        Console.WriteLine("swap out " + temp.getValue());
                        Console.WriteLine("swap in " + c);
                        swapOutCounter[Convert.ToInt32(temp.getValue())]++;
                        swapInCounter[Convert.ToInt32(c)]++;
                    }
                }
                else
                {
                    temp1 = new Page(c);
                    EnterToQueue(lastIgnored,temp1);
                    Console.WriteLine("ignored " + c);
                }
                printList(pages);
            }
            calc();

        }
        public void LFU()
        {
            Page temp;
            List<Page> pages = new List<Page>();
            foreach(string c in strAccess)
            {
               if(!isExistList(c,pages))
                {
                    if(pages.Count<workingSetSize)
                    {
                        pages.Add(new Page(c));
                        swapInCounter[Convert.ToInt32(c)]++;
                        Console.WriteLine("swap in " + c);
                    }
                    else
                    {
                        temp = FindMinIgnores(pages);
                        pages.Remove(temp);
                        swapOutCounter[Convert.ToInt32(temp.getValue())]++;
                        swapInCounter[Convert.ToInt32(c)]++;
                        pages.Add(new Page(c));
                        Console.WriteLine("swap out " + temp.getValue());
                        temp.setNumberOfIgnores(0);
                        Console.WriteLine("swap in " + c);
                    }
                }
                else
                {
                    Page ignoredPage = FindPageInList(pages, c);
                    ignoredPage.setNumberOfIgnores(ignoredPage.getNumberOfIgnores()+1);
                    Console.WriteLine("ignored " + c);
                }
                printList(pages);
            }
            calc();
        }

      
        public void LIFO()
        {
            Page temp;
            Stack<Page> pages = new Stack<Page>();
            foreach (string c in strAccess)
            {
                if (!isExistStack(c, pages))
                {
                    if (pages.Count < workingSetSize)
                    {
                        pages.Push(new Page(c));
                        swapInCounter[Convert.ToInt32(c)]++;
                        Console.WriteLine("swap in " + c);
                    }
                    else
                    {
                        temp = pages.Pop();
                        swapOutCounter[Convert.ToInt32(temp.getValue())]++;
                        swapInCounter[Convert.ToInt32(c)]++;
                        pages.Push(new Page(c));
                        Console.WriteLine("swap out " + temp.getValue());
                        Console.WriteLine("swap in " + c);
                    }
                }
                else
                {
                    Console.WriteLine("ignored " + c);
                }
                printStack(pages);
            }
            calc();
        }

        public void FIFO()
        {
            Page temp;
            Queue<Page> pages = new Queue<Page>();
            foreach (string c in strAccess)
            {
                if (!isExistQueue(c, pages))
                {
                    if (pages.Count < workingSetSize)
                    {
                        pages.Enqueue(new Page(c));
                        swapInCounter[Convert.ToInt32(c)]++;
                        Console.WriteLine("swap in " + c);
                    }
                    else
                    {
                        temp = pages.Dequeue();
                        swapOutCounter[Convert.ToInt32(temp.getValue())]++;
                        swapInCounter[Convert.ToInt32(c)]++;
                        pages.Enqueue(new Page(c));
                        Console.WriteLine("swap out " + temp.getValue());
                        Console.WriteLine("swap in " + c);
                    }

                }
                else 
                { 
                    Console.WriteLine("ignored " + c); 
                }
                printQueue(pages);
            }
            calc();
        }

        public Page FindPageInList(List<Page> pages, string c)
        {
            foreach (Page p in pages)
            {
                if (c == p.getValue())
                {
                    return p;
                }
            }
            return null;
        }

        public Page FindMinIgnores(List<Page> pages)
        {
            int minIgnore = Int32.MaxValue;
            Page temp = null;
            foreach (Page p in pages)
            {
                if (p.getNumberOfIgnores() <= minIgnore)
                {
                    minIgnore = p.getNumberOfIgnores();
                    temp = p;
                }
            }
            return temp;
        }


        public void printStack(Stack<Page> pages)
        {
            Stack<Page> tempPage = new Stack<Page>();
            Page temp;
            Console.WriteLine("-------------");
            for (int i = 0; i < workingSetSize; i++)
            {
                if (pages.Count != 0)
                {
                    temp = pages.Pop();
                    tempPage.Push(temp);
                    Console.WriteLine(temp.getValue());
                }
                else { Console.WriteLine("---"); }
            }
            foreach (Page p in tempPage)
            {
                pages.Push(p);
            }
            Console.WriteLine("-------------");
            tempPage.Clear();
        }

        public void printList(List<Page> pages)
        {
            Console.WriteLine("--------------");
            for (int i = 0; i < workingSetSize; i++)
            {
                if (pages.Count != 0 && i<pages.Count)
                {
                    Console.WriteLine(pages[i].getValue());
                }
                else { Console.WriteLine("---"); }
            }
            Console.WriteLine("-------------");     
        }

        public void printQueue(Queue<Page> pages)
        {
            Queue<Page> tempPage = new Queue<Page>();
            Page temp;
            Console.WriteLine("-------------");
            for(int i=0;i<workingSetSize;i++)
            {
                if(pages.Count!=0)
                {
                    temp = pages.Dequeue();
                    tempPage.Enqueue(temp);
                    Console.WriteLine(temp.getValue());
                }
                else { Console.WriteLine("---"); }
            }
            foreach(Page p in tempPage)
            {
                pages.Enqueue(p);
            }
            Console.WriteLine("-------------");
            tempPage.Clear();

        }
        public bool isExistQueue(string page,Queue<Page> pages)
        {
            foreach(Page p in pages)
            {
                if (p.getValue()==page) { return true; }
            }
            return false;
        }

        public bool isExistStack(string page,Stack<Page> pages)
        {
            foreach(Page p in pages)
            {
                if (p.getValue() == page) { return true; }
            }
            return false;
        }
        public bool isExistList(string page,List<Page> pages)
        {
            foreach(Page p in pages)
            {
                if (p.getValue() == page) { return true; }
            }
            return false;
        }

        public void EnterToQueue(Queue<Page> q, Page p)
        {
            Queue<Page> temp = new Queue<Page>();
            while (q.Count != 0)
            {
                if (p.getValue() == q.Peek().getValue())
                {
                    q.Dequeue();
                }
                else
                {
                    temp.Enqueue(q.Dequeue());
                }
            }
            temp.Enqueue(p);
            while (temp.Count != 0)
            {
                q.Enqueue(temp.Dequeue());
            }
        }


        public void calc()
        {
            int i = 0;
            for (i = 0; i < swapInCounter.Length; i++)
            {
                this.numberOfpageFault += swapInCounter[i];
            }
            this.missRatio = this.numberOfpageFault / strAccess.Length;
            for (i = 0; i < swapInCounter.Length; i++)
            {
                if (swapInCounter[i] != 0)
                {
                    Console.WriteLine("Page number " + i + " was swapped in " + swapInCounter[i] + " times");
                }
            }

            for (i = 0; i < swapOutCounter.Length; i++)
            {
                if (swapOutCounter[i] != 0)
                {
                    Console.WriteLine("Page number " + i + " was swapped out " + swapOutCounter[i] + " times");
                }
            }
            Console.WriteLine("The miss ratio is " + this.missRatio*100+"%");
        }
    }

}
