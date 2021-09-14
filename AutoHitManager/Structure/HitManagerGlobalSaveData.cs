using System;
using System.Collections.Generic;
using AutoHitManager.Structure;
using Modding;
using Modding.Converters;
using Newtonsoft.Json;
using HitManagerSplits = System.Collections.Generic.List<AutoHitManager.Structure.Split>;

namespace AutoHitManager.Cat
{
    public class HitManagerGlobalSaveData : ModSettings
    {
        public HitManagerSplits HitManagerDict = new();
        [JsonConverter(typeof(PlayerActionSetConverter))]
        public AutoHitActionSet binds = new();
        public RunConfig ActualRun = null;
        public int MaxVisibleSplits = 10;
        public int NextId = 1;

        public List<RunConfig> Runs = new();
    }
}
