using System;
using System.Collections.Generic;
using AutoHitManager.Structure;
using Modding;
using static Modding.Converters.PlayerActionSetConverter;
using Newtonsoft.Json;

namespace AutoHitManager.Cat
{
    public class HitManagerGlobalSaveData : ModSettings
    {
        [JsonConverter(typeof(Modding.Converters.PlayerActionSetConverter))]
        public AutoHitActionSet binds = new();
        public int MaxVisibleSplits = 10;
        public int NextId = 1;
        public int NextRunId = 1;
        public int ActualRunId = 0;
        public RunConfig ActualRun()
        {
            return Runs.Find(run => run.Id == ActualRunId);
        }

        public List<RunConfig> Runs = new();

        public int FuryCount = 3;
    }
}
