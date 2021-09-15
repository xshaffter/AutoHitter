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
        public int RunConfigId = 0;
        public bool Ended = false;
        public int number = 0;
        public int Hits() => Splits.Sum(s => s.Hits);

        public RunConfig RunConfig()
        {
            return Global.GlobalSaveData.Runs.Find(run => run.Id == this.RunConfigId);
        }

        public override string ToString()
        {
            return "";
        }
    }
}
