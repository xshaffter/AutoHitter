using AutoHitManager.Cat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoHitManager.Structure
{
    public class Split
    {
        public int _index = -2;
        public string Name;
        public int Hits;
        private int? previous = null;
        public int? PB;
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
                    previous = Global.GlobalSaveData.PB.Splits.Where(split => split.GetIndex() < this.GetIndex()).Sum(split => split.Hits);
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
