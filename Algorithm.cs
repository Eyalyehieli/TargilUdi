using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace TargilUdi
{
    class Algorithm
    {
        private int numberOfpageFault;
        private double missRatio;
        private int[] swapInCounter;
        private int[] swapOutCounter;
        private int[] ignoresCounter;
        private string[] strAccess;
        private int workingSetSize;

        public int findMaxByInt(string[] str)
        {
            int max = -1;
            foreach (string s in str)
            {
                if (Int32.Parse(s) > max)
                {
                    max = Int32.Parse(s);
                }
            }
            return max;
        }

        public Algorithm(string[] strAccess, int workingSetSize)
        {
            this.numberOfpageFault = 0;
            this.missRatio = 0;
            this.swapInCounter = new int[findMaxByInt(strAccess) + 1];
            this.swapOutCounter = new int[findMaxByInt(strAccess) + 1];
            this.ignoresCounter = new int[findMaxByInt(strAccess) + 1];
            this.strAccess = strAccess;
            this.workingSetSize = workingSetSize;
        }
        public bool isExist(string page, string[] pages)
        {
            foreach (string p in pages)
            {
                if (page == p) { return true; }
            }
            return false;
        }

        public void swap(string[] pages,string toSwapOut,string s)
        {
            int i = 0;
            foreach(string str in pages)
            {
                if(str==toSwapOut)
                {
                    pages[i] = s;
                    break;
                }
                i++;
            }
        }

        private void EnterToRecentlyUsedQueue(Queue<string> recentlyUsed, string s)
        {
            Queue<string> temp = new Queue<string>();
            while(recentlyUsed.Count!=0)
            {
                if(recentlyUsed.Peek()==s)
                {
                    recentlyUsed.Dequeue();
                }
                else
                {
                    temp.Enqueue(recentlyUsed.Dequeue());
                }
            }
            while(temp.Count!=0)
            {
                recentlyUsed.Enqueue(temp.Dequeue());
            }
            recentlyUsed.Enqueue(s);
        }

        public void printPages(string[] pages,string s)
        {
            Console.WriteLine("----------------");
            foreach(string str in pages)
            {
                if(str==s)
                {
                    Console.WriteLine("["+str+"]");
                }
                else
                {
                    Console.WriteLine(str);
                }
            }
            Console.WriteLine("----------------");
        }

        public void LRU()
        {
            Queue<string> recentlyUsed = new Queue<string>();
            string[] pages = new string[workingSetSize];
            foreach (string s in strAccess)
            {
                if (!isExist(s, pages))
                {
                    if (pages.Count(s => s != null) < workingSetSize)
                    {
                        int index = Array.FindIndex(pages, i => i == null);//find the first empty element in the array
                        pages[index] = s;
                        swapInCounter[Convert.ToInt32(s)]++;
                        Console.WriteLine("swap in " + s);
                        recentlyUsed.Enqueue(s);
                    }
                    else
                    {
                        string toSwapOut = recentlyUsed.Dequeue();
                        recentlyUsed.Enqueue(s);
                        swap(pages, toSwapOut, s);
                        swapInCounter[Convert.ToInt32(s)]++;
                        swapOutCounter[Convert.ToInt32(toSwapOut)]++;
                        Console.WriteLine("swap in " + s);
                        Console.WriteLine("swap out " + toSwapOut);
                    }
                }
                else
                {
                    Console.WriteLine("ignored " + s);
                    EnterToRecentlyUsedQueue(recentlyUsed, s);
                }
                printPages(pages, s);
            }
            calc();
        }

        private void EnterToFifo(Queue<PageLFU> fifo, string s)
        {
            foreach(PageLFU p in fifo)
            {
                if(p.GetPage_num().Equals(s))
                {
                    p.SetFrequency(p.GetFrequency() + 1);
                    return;
                }
            }
            fifo.Enqueue(new PageLFU(s, 1));
        }

        public string DeleteFromFifo(Queue<PageLFU> fifo,int minFreq)
        {
            Queue<PageLFU> temp = new Queue<PageLFU>();
            string toRet=null;
            while(fifo.Count!=0)
            {
                if (fifo.Peek().GetFrequency() == minFreq)
                {
                    toRet = fifo.Dequeue().GetPage_num();
                    break;
                }
                else
                {
                    temp.Enqueue(fifo.Dequeue());
                }
            }
            while(temp.Count!=0)
            {
                fifo.Enqueue(temp.Dequeue());
            }
            return toRet;
        }

        private string removePageLFU(int[] frequency, Queue<PageLFU> fifo)
        {
            int min = Array.IndexOf(frequency, frequency.Where(x => x > 0).Min());
            int count = 0;
            foreach(int freq in frequency)
            {
                if(frequency[min]==freq)
                {
                    count++;
                }
            }
            if(count>1)
            {
                return DeleteFromFifo(fifo, frequency[min]);
            }
            else
            {
                return min.ToString();
            }
        }



        public void LFU()
        {
            int[] frequncy = new int[findMaxByInt(strAccess) + 1];
            Queue<PageLFU> fifo = new Queue<PageLFU>();
            string[] pages = new string[workingSetSize];

            foreach (string s in strAccess)
            {
                if (!isExist(s, pages))
                {
                    if (pages.Count(s => s != null) < workingSetSize)
                    {
                        int index = Array.FindIndex(pages, i => i == null);//find the first empty element in the array
                        pages[index] = s;
                        swapInCounter[Convert.ToInt32(s)]++;
                        Console.WriteLine("swap in " + s);
                        frequncy[Convert.ToInt32(s)]++;
                        EnterToFifo(fifo,s);
                    }
                    else
                    {
                        string toSwapOut = removePageLFU(frequncy,fifo);
                        frequncy[Convert.ToInt32(s)]++;
                        EnterToFifo(fifo, s);
                        frequncy[Convert.ToInt32(toSwapOut)] = 0;
                        swap(pages, toSwapOut, s);
                        swapInCounter[Convert.ToInt32(s)]++;
                        swapOutCounter[Convert.ToInt32(toSwapOut)]++;
                        Console.WriteLine("swap in " + s);
                        Console.WriteLine("swap out " + toSwapOut);
                    }
                }
                else
                {
                    Console.WriteLine("ignored " + s);
                    frequncy[Convert.ToInt32(s)]++;
                    EnterToFifo(fifo, s);
                }
                printPages(pages, s);
            }
            calc();
        }

        






        //public void LRU()
        //{
        //    Dictionary<string, Page> dic = new Dictionary<string, Page>();
        //    Page temp,temp1;
        //    List<Page> pages = new List<Page>();
        //    Queue<Page> lastIgnored = new Queue<Page>();
        //    Queue<Page> pos = new Queue<Page>();
        //    Page newPage;
        //    foreach (string c in strAccess)
        //    {
        //        if (!isExistList(c, pages))
        //        {
        //            if (pages.Count < workingSetSize)
        //            {
        //                temp1 = new Page(c);
        //                if (!dic.ContainsKey(c)) { dic.Add(c, temp1); }
        //                pages.Add(temp1);
        //                swapInCounter[Convert.ToInt32(c)]++;
        //                Console.WriteLine("swap in " + c);
        //                EnterToQueue(lastIgnored, temp1);
        //            }
        //            else
        //            {   
        //                temp = lastIgnored.Dequeue();
        //                pages.Remove(dic[temp.getValue()]);
        //                dic.Remove(c);
        //                newPage = new Page(c);
        //                if (!dic.ContainsKey(c)) { dic.Add(c, newPage); }
        //                pages.Add(newPage);
        //                EnterToQueue(lastIgnored,newPage);
        //                Console.WriteLine("swap out " + temp.getValue());
        //                Console.WriteLine("swap in " + c);
        //                swapOutCounter[Convert.ToInt32(temp.getValue())]++;
        //                swapInCounter[Convert.ToInt32(c)]++;
        //            }
        //        }
        //        else
        //        {
        //            temp1 = new Page(c);
        //            EnterToQueue(lastIgnored,temp1);
        //            Console.WriteLine("ignored " + c);
        //        }
        //        printList(pages);
        //    }
        //    calc();

        //}
        //public void LFU()
        //{
        //    Page temp;
        //    List<Page> pages = new List<Page>();
        //    foreach(string c in strAccess)
        //    {
        //       if(!isExistList(c,pages))
        //        {
        //            if(pages.Count<workingSetSize)
        //            {
        //                pages.Add(new Page(c));
        //                swapInCounter[Convert.ToInt32(c)]++;
        //                Console.WriteLine("swap in " + c);
        //            }
        //            else
        //            {
        //                temp = FindMinIgnores(pages);
        //                pages.Remove(temp);
        //                swapOutCounter[Convert.ToInt32(temp.getValue())]++;
        //                swapInCounter[Convert.ToInt32(c)]++;
        //                pages.Add(new Page(c));
        //                Console.WriteLine("swap out " + temp.getValue());
        //                temp.setNumberOfIgnores(0);
        //                Console.WriteLine("swap in " + c);
        //            }
        //        }
        //        else
        //        {
        //            Page ignoredPage = FindPageInList(pages, c);
        //            ignoredPage.setNumberOfIgnores(ignoredPage.getNumberOfIgnores()+1);
        //            Console.WriteLine("ignored " + c);
        //        }
        //        printList(pages);
        //    }
        //    calc();
        //}


        //        public Page FindPageInList(List<Page> pages, string c)
        //        {
        //            foreach (Page p in pages)
        //            {
        //                if (c == p.getValue())
        //                {
        //                    return p;
        //                }
        //            }
        //            return null;
        //        }

        //        public Page FindMinIgnores(List<Page> pages)
        //        {
        //            int minIgnore = Int32.MaxValue;
        //            Page temp = null;
        //            foreach (Page p in pages)
        //            {
        //                if (p.getNumberOfIgnores() <= minIgnore)
        //                {
        //                    minIgnore = p.getNumberOfIgnores();
        //                    temp = p;
        //                }
        //            }
        //            return temp;
        //        }


        //        public void printStack(Stack<Page> pages,string c)
        //        {
        //            Stack<Page> tempPage = new Stack<Page>();
        //            Page temp;
        //            Console.WriteLine("-------------");
        //            for (int i = 0; i < workingSetSize; i++)
        //            {
        //                if (pages.Count != 0)
        //                {
        //                    temp = pages.Pop();
        //                    tempPage.Push(temp);
        //                    if (temp.getValue() == c) { Console.WriteLine("[" + temp.getValue() + "]"); }
        //                    else { Console.WriteLine(temp.getValue()); }
        //                }
        //                else { Console.WriteLine("---"); }
        //            }
        //            foreach (Page p in tempPage)
        //            {
        //                pages.Push(p);
        //            }
        //            Console.WriteLine("-------------");
        //            tempPage.Clear();
        //        }

        //        public void printList(List<Page> pages)
        //        {
        //            Console.WriteLine("--------------");
        //            for (int i = 0; i < workingSetSize; i++)
        //            {
        //                if (pages.Count != 0 && i<pages.Count)
        //                {
        //                    Console.WriteLine(pages[i].getValue());
        //                }
        //                else { Console.WriteLine("---"); }
        //            }
        //            Console.WriteLine("-------------");     
        //        }

        //        public void printQueue(Queue<Page> pages,string c)
        //        {
        //            Queue<Page> tempPage = new Queue<Page>();
        //            Page temp;
        //            Console.WriteLine("-------------");
        //            for(int i=0;i<workingSetSize;i++)
        //            {
        //                if(pages.Count!=0)
        //                {
        //                    temp = pages.Dequeue();
        //                    tempPage.Enqueue(temp);
        //                    if (temp.getValue() == c) { Console.WriteLine("[" + temp.getValue() + "]"); }
        //                    else { Console.WriteLine(temp.getValue()); }
        //                }
        //                else { Console.WriteLine("---"); }
        //            }
        //            foreach(Page p in tempPage)
        //            {
        //                pages.Enqueue(p);
        //            }
        //            Console.WriteLine("-------------");
        //            tempPage.Clear();

        //        }


        //        public bool isExistQueue(string page,Queue<Page> pages)
        //        {
        //            foreach(Page p in pages)
        //            {
        //                if (p.getValue()==page) { return true; }
        //            }
        //            return false;
        //        }

        //        public bool isExistStack(string page,Stack<Page> pages)
        //        {
        //            foreach(Page p in pages)
        //            {
        //                if (p.getValue() == page) { return true; }
        //            }
        //            return false;
        //        }
        //        public bool isExistList(string page,List<Page> pages)
        //        {
        //            foreach(Page p in pages)
        //            {
        //                if (p.getValue() == page) { return true; }
        //            }
        //            return false;
        //        }

        //        public void EnterToQueue(Queue<Page> q, Page p)
        //        {
        //            Queue<Page> temp = new Queue<Page>();
        //            while (q.Count != 0)
        //            {
        //                if (p.getValue() == q.Peek().getValue())
        //                {
        //                    q.Dequeue();
        //                }
        //                else
        //                {
        //                    temp.Enqueue(q.Dequeue());
        //                }
        //            }
        //            temp.Enqueue(p);
        //            while (temp.Count != 0)
        //            {
        //                q.Enqueue(temp.Dequeue());
        //            }
        //        }


        public void calc()
        {
            int i = 0;
            for (i = 0; i < swapInCounter.Length; i++)
            {
                this.numberOfpageFault += swapInCounter[i];
            }
            this.missRatio = (double)this.numberOfpageFault / (double)strAccess.Length;
            int sum = 0;
            for (i = 0; i < swapInCounter.Length; i++)
            {
                sum += swapInCounter[i];
            }
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
            Console.WriteLine("sum swapIn: " + sum);
            Console.Write("The miss ratio is ");
            Console.WriteLine(String.Format("{0:0.00}%", this.missRatio * 100));
        }
    }
}
    