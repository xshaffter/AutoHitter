using AutoHitManager.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoHitManager.Cat
{
    public class Run
    {
        public List<Split> Splits = new();
        public bool Ended = false;
        public int number = 0;
        public int Hits() => Splits.Sum(s => s.Hits);

        public override string ToString()
        {
            return "";
        }
    }
}
