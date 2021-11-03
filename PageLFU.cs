using System;
using System.Collections.Generic;
using System.Text;

namespace TargilUdi
{
    class PageLFU
    {
        private string page_num;
        private int frequency;

        public PageLFU(string page_num,int frequency)
        {
            this.page_num = page_num;
            this.frequency = frequency;
        }
        public string GetPage_num() { return this.page_num; }
        public int GetFrequency() { return this.frequency; }
        public void SetPage_num(string page_num) { this.page_num = page_num; }
        public void SetFrequency(int frequency) { this.frequency = frequency; }
    }
}
