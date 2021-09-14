using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoHitManager.Cat
{
    public class RunConfig
    {
        public string Name;
        public List<Run> History = new();
        public Run PB = null;
        public Run LastRun = null;
        public int MaxRun = 1;
        public List<SplitConfig> Splits;
    }
}
