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
        public List<Split> SplitInfo()
        {
            var index = 0;
            return RunConfig().Splits.Select(split => {
                var usedSplit = Splits.Find(info => info.splitID == split.Id);
                if (usedSplit == null)
                {
                    usedSplit = new Split
                    {
                        Hits = 0,
                        splitID = split.Id
                    };
                    Splits.Add(usedSplit);
                }
                return new Split
                {
                    Hits = usedSplit.Hits,
                    splitID = split.Id,
                    _index = index++
                };
            }).ToList();
        }

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
