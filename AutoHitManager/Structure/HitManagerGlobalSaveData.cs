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
        [JsonConverter(typeof(PlayerActionSetConverter))]
        public AutoHitActionSet binds = new();
        public int MaxVisibleSplits = 10;
        public int NextId = 1;
        public int NextRunId = 1;
        public int ActualRunId = 0;
        public RunConfig ActualRun
        {
            get
            {
                return Runs.Find(run => run.Id == ActualRunId);
            }
        }

        public List<RunConfig> Runs = new();
    }
}
