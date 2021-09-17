using AutoHitManager.Structure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

using KeyValuePair = System.Collections.Generic.Dictionary<string, int>;
using AutoHitManager.Managers;
using System.Timers;
using AutoHitManager.UI.Scenes;

namespace AutoHitManager.Cat
{
    public static class Global
    {
        internal static int MaxVisibleSplits
        {
            get
            {
                return GlobalSaveData.MaxVisibleSplits;
            }
        }
        internal static double FuryTime { get; set; }
        private static bool fury = false;
        internal static bool IsFuryEquipped;
        internal static HitManagerSaveData LocalSaveData { get; set; } = new();
        internal static HitManagerGlobalSaveData GlobalSaveData { get; set; } = new();
        internal static Timer FuryTimer;
        internal static bool IsProhibitedZone = false;

        public static bool IntentionalHit
        {
            get
            {
                return fury;
            }
            set
            {
                fury = value;
                BindableFunctions.CountDown = 0;
                BindableFunctions.CountUp = 0;
                UpdateRunDataFile();
            }
        }

        public static void GenerateWidget()
        {
            CopyFileTo("Design.html", Constants.DirFolder);
            CopyFileTo("javascript.js", Constants.DirFolder, true);
            CopyFileTo("styles.css", Constants.DirFolder);
            CopyFileTo("fotf.png", Constants.DirFolder);
        }

        private static void CopyFileTo(string origFile, string dirFolder, bool alwaysCopy = false)
        {
            var path_result = Path.Combine(dirFolder, origFile);
            if (alwaysCopy || !File.Exists(path_result))
            {
                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"AutoHitManager.Resources.{origFile}");
                var file = File.Create(path_result);
                stream.CopyTo(file);
            }
        }

        public static List<T> PaginateList<T>(List<T> list, int actualIndex, int pageSize)
        {
            if (pageSize >= list.Count)
            {
                return list;
            }
            int otherSplits = pageSize - 1;
            int countSideDown = (int)(otherSplits / 2);
            int countSideUp = (int)(otherSplits - countSideDown);
            int first = actualIndex - countSideUp;

            if (actualIndex <= countSideUp)
            {
                first = 0;
            }
            else if (first >= list.Count - pageSize)
            {
                first = list.Count - pageSize;
            }
            if (first + pageSize >= list.Count)
            {
                pageSize = list.Count - first;
            }
            list = list.GetRange(first, pageSize);
            return list;
        }

        public static void UpdateRunDataFile()
        {
            List<string> data;
            int total_hits;
            int total_pb;
            string run_data;
            if (LocalSaveData.Run.Splits.Count > 0)
            {
                data = PaginateList(LocalSaveData.Run.SplitInfo(), LocalSaveData.CurrentSplit, MaxVisibleSplits).Select(item => item.ToString()).ToList();
                total_hits = LocalSaveData.Run.Hits();
                total_pb = LocalSaveData.Run?.RunConfig()?.PB?.Hits() ?? GlobalSaveData.ActualRun()?.PB?.Hits() ?? 0;
                run_data = $"{{practice: {(PracticeMode == "Yes").ToString().ToLower()},split:{LocalSaveData.CurrentSplit}, split_count:{LocalSaveData.Run.SplitInfo().Count()}, run: {LocalSaveData.Run.number}, fury: {IntentionalHit.ToString().ToLower()}}}";
            }
            else
            {
                var index = 0;
                var fake_splits = GlobalSaveData.ActualRun().Splits.Select(config => new Split
                {
                    Hits = 0,
                    splitID = config.Id,
                    _index = index++
                }).ToList();
                LocalSaveData.Run.Splits = fake_splits;
                data = PaginateList(fake_splits, 0, MaxVisibleSplits).Select(item => item.ToString()).ToList();
                total_hits = 0;
                total_pb = GlobalSaveData.ActualRun()?.PB?.Hits() ?? 0;
                run_data = $"{{practice: {(PracticeMode == "Yes").ToString().ToLower()},split:-1, split_count:{fake_splits.Count()}, run: {LocalSaveData.Run.number}, fury: {IntentionalHit.ToString().ToLower()}}}";
            }
            string splits_data = $"[{string.Join(",", data)}]";
            string total_data = new Split
            {
                ForcedPB = total_pb,
                Hits = total_hits,
                ForcedName = "Total:"
            }.ToString();
            string html_text = string.Format(Constants.HTML, run_data, splits_data, total_data);
            File.WriteAllText(Path.Combine(Constants.DirFolder, "run_data.html"), html_text);
        }

        public static void PerformHit()
        {
            ActualSplit.Hits++;
            UpdateRunDataFile();
        }

        public static void Log(string text)
        {
            AutoHitMod.LoadedInstance.Log(text);
        }

        public static void Log(object text)
        {
            AutoHitMod.LoadedInstance.Log(text);
        }

        public static Split ActualSplit
        {
            get
            {
                return LocalSaveData.Run.Splits[LocalSaveData.CurrentSplit];
            }
        }

        public static int RunDetail = 0;
        internal static string PracticeMode = "No";
        internal static int HistoryId;
        internal static bool IsDreamerZone = false;
    }
}
