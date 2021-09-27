using AutoHitManager.Cat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoHitManager.Structure
{
    public class Split
    {
        public string ForcedName = null;
        public int splitID = 0;
        public int Hits = 0;
        public int? ForcedPB = null;
        private int? previous = null;
        public int? PB()
        {
            if (ForcedPB != null)
            {
                return ForcedPB;
            }
            return PBSplit()?.Hits ?? 0;
        }

        public RunConfig Run()
        {
            return Global.GlobalSaveData.Runs.Find(run => run.Splits.Any(split => split.Id == this.splitID));
        }

        public SplitConfig Config()
        {
            return Global.GlobalSaveData.Runs.SelectMany(run => run.Splits).ToList().Find(split => split.Id == this.splitID);
        }

        public string Name()
        {
            if (ForcedName != null)
            {
                return this.ForcedName;
            }
            return this.Config()?.Name;
        }
        public Split PBSplit()
        {
            if (this.Run().PB == null) return null;
            return this.Run().PB?.Splits?.Find(split => split.splitID == this.splitID);
        }
        public int Diff()
        {
            return Hits - (PB() ?? 0);
        }
        public int GetIndex()
        {
            return Run()?.Splits?.IndexOf(Config()) ?? 0;
        }
        public int GetPrevious()
        {
            if (previous == null)
            {
                try
                {
                    previous = this.Run().PB.Splits.Where(split => split.GetIndex() < PBSplit().GetIndex()).Sum(split => split.Hits);
                }
                catch
                {
                    previous = 0;
                }
            }
            return previous ?? 0;
        }
        public int GetPreviousHits()
        {
            int previous;
            try
            {
                previous = Global.LocalSaveData.Run.Splits.Where(split => split.GetIndex() <= GetIndex()).Sum(split => split.Hits);
            }
            catch
            {
                previous = 0;
            }
            return previous;
        }

        public int GetPreviousDiff()
        {
            int diff;
            try
            {
                diff = GetPreviousHits() - GetPrevious() - (PBSplit()?.Hits ?? 0);
            }
            catch
            {
                diff = 0;
            }
            return diff;
        }

        public override string ToString()
        {
            return $"{{Name:\"{Name()}\", Hits:\"{Hits} ({GetPreviousHits()})\", PB:\"{PB()?.ToString() ?? "-"} ({GetPrevious()})\", Diff:{Diff()}, PrevDiff: {GetPreviousDiff()}, split: {GetIndex()}}}";
        }
    }
}
