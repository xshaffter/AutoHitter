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

namespace AutoHitManager.Cat
{
    public static class Global
    {
        private static int MaxVisibleSplits { get; set; }
        public static double FuryTime { get; internal set; }
        private static bool fury = false;
        public static bool IsFuryEquipped;
        public static HitManagerSaveData LocalSaveData { get; set; } = new();
        public static HitManagerGlobalSaveData GlobalSaveData { get; set; } = new();
        private static List<string> splits;
        public static Timer FuryTimer;
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
        public static List<Split> Splits
        {
            get
            {
                if (LocalSaveData.Run.Splits == null || LocalSaveData.Run.Splits.Count <= 0)
                {
                    LocalSaveData.Run.Splits = new List<Split>();
                    int index = 0;
                    foreach (var split in Global.splits)
                    {
                        var PB_split = Global.GlobalSaveData.PB?.Splits?.Find(s => s._index == index);
                        LocalSaveData.Run.Splits.Add(new Split {
                            Name = split,
                            Hits = 0,
                            _index = index++
                        });
                    }
                }
                return LocalSaveData.Run.Splits;
            }
        }

        public static void GenerateWidget()
        {
            CopyFileTo("Design.html", Constants.DirFolder);
            CopyFileTo("javascript.js", Constants.DirFolder);
            CopyFileTo("styles.css", Constants.DirFolder);
            CopyFileTo("fotf.png", Constants.DirFolder);
            var settings = Path.Combine(Constants.DirFolder, "settings.json");
            if (!File.Exists(settings))
            {
                CopyFileTo("settings.json", Constants.DirFolder);
            }
            else
            {
                ReadSettings(settings);
            }
        }

        public static void ReadSettings(string file)
        {
            string text = File.ReadAllText(file);
            var json = JsonConvert.DeserializeObject<SettingsJson>(text);
            splits = json.Splits;
            FuryTime = json.FuryTime;
            MaxVisibleSplits = json.MaxVisibleSplits;
        }

        private static void CopyFileTo(string origFile, string dirFolder)
        {
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"AutoHitManager.Resources.{origFile}");
            var path_result = Path.Combine(dirFolder, origFile);
            var file = File.Create(path_result);
            stream.CopyTo(file);
        }

        public static List<T> PaginateList<T>(List<T> list, int actualIndex, int pageSize)
        {
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
            var data = PaginateList(LocalSaveData.Run.Splits, LocalSaveData.CurrentSplit, MaxVisibleSplits).Select(item => item.ToString()).ToList();
            int total_hits = LocalSaveData.Run.Hits();
            int total_pb = GlobalSaveData.PB?.Hits() ?? 0;

            string splits_data = $"[{string.Join(",", data)}]";
            string run_data = $"{{split:{LocalSaveData.CurrentSplit}, split_count:{LocalSaveData.Run.Splits.Count()}, run: {LocalSaveData.Run.number}, fury: {IntentionalHit.ToString().ToLower()}}}";
            string total_data = new Split
            {
                ForcedPB = total_pb,
                Hits = total_hits,
                Name = "Total:"
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
                return Splits[LocalSaveData.CurrentSplit];
            }
        }
    }
}
