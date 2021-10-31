using System;
using System.Collections.Generic;
using System.Text;

namespace TargilUdi
{
    class Page
    {
        private string value;
        private int numberOfPageFault;
        private int numberOfSwapOuts;
        private int numberOfIgnores;

        public Page(string value)
        {
            this.value = value;
            this.numberOfIgnores = 0;
            this.numberOfPageFault = 0;
            this.numberOfSwapOuts = 0;
        }

        public void setValue(string value) { this.value = value; }
        public void setNumberOfPageFault(int numberOfPageFault) { this.numberOfPageFault = numberOfPageFault; }
        public void setNumberOfSwapOuts(int numberOfSwapOuts) { this.numberOfSwapOuts = numberOfSwapOuts; }
        public void setNumberOfIgnores(int numberOfIgnores) { this.numberOfIgnores = numberOfIgnores; }
        public string getValue() { return this.value; }
        public int getNumberOfPageFault() { return this.numberOfPageFault; }
        public int getNumberOfIgnores() { return this.numberOfIgnores; }
        public int getNumberOfSwapOuts() { return this.numberOfSwapOuts; }

        public string Print()
        {
            return this.value.ToString() ;
        }
    }
}
