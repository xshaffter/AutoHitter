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
        public int _index = -2;
        public int splitID = 0;
        public int Hits;
        public int? ForcedPB = null;
        private int? previous = null;
        public int? PB
        {
            get
            {
                if (ForcedPB != null)
                {
                    return ForcedPB;
                }
                return PBSplit?.Hits ?? 0;
            }
        }

        public SplitConfig Config
        {
            get
            {
                return Global.GlobalSaveData.ActualRun.Splits.Find(config => config.Id == this.splitID);
            }
        }

        public string Name 
        {
            get
            {
                if (ForcedName != null)
                {
                    return this.ForcedName;
                }
                return this.Config.Name;
            }
        }
        public Split PBSplit
        {
            get
            {
                if (Global.GlobalSaveData.ActualRun.PB == null) return null;
                return Global.GlobalSaveData.ActualRun.PB?.Splits?.Find(split => split.splitID == this.splitID);
            }
        }
        public int Diff 
        {
            get
            {
                return (PB ?? 0) - Hits;
            }
        }
        public int GetIndex()
        {
            if (_index == -2)
            {
                _index = Global.Splits.IndexOf(this);
            }
            return _index;
        }
        public int GetPrevious()
        {
            if (previous == null)
            {
                try
                {
                    previous = Global.GlobalSaveData.ActualRun.PB.Splits.Where(split => split.GetIndex() < PBSplit.GetIndex()).Sum(split => split.Hits);
                }
                catch
                {
                    previous = 0;
                }
            }
            return previous ?? 0;
        }

        public override string ToString()
        {
            return $"{{Name:\"{Name}\", Hits:{Hits}, PB:\"{PB?.ToString() ?? "-"} ({GetPrevious()})\", Diff:{Diff}, split: {_index}}}";
        }
    }
}
