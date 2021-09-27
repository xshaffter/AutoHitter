using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoHitManager.Structure;

namespace AutoHitManager.Cat
{
    public class HitManagerSaveData
    {
        public int CurrentSplit = 0;
        public Run Run = new();
        public bool NewRun = true;
        public List<bool> gotCharms = new List<bool>() { true };
        public List<bool> newCharms = new List<bool>() { false };
        public List<bool> equippedCharms = new List<bool>() { false };
        public List<int> charmCosts = new List<int>() { 1 };
    }
}
