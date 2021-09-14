using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoHitManager.Structure;

using HitManagerSplits = System.Collections.Generic.List<AutoHitManager.Structure.Split>;

namespace AutoHitManager.Cat
{
    public class HitManagerSaveData
    {
        public int CurrentSplit = 0;
        public Run Run = new();
        public bool NewRun = true;

    }
}
