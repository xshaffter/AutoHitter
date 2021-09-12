using System;
using System.Collections.Generic;
using AutoHitManager.Structure;
using Modding;
using Modding.Converters;
using Newtonsoft.Json;
using HitManagerSplits = System.Collections.Generic.List<AutoHitManager.Structure.Split>;

namespace AutoHitManager.Cat
{
    public class HitManagerGlobalSaveData
    {
        public HitManagerSplits HitManagerDict = new();
        public Run PB = null;
        public bool FirstRun = true;
        public int MaxRun = 1;
        [JsonConverter(typeof(PlayerActionSetConverter))]
        public AutoHitActionSet binds = new();
        public List<string> splits = new() {};
        public Run LastRun = null;

    }
}
