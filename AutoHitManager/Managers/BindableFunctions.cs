using AutoHitManager.Cat;
using AutoHitManager.Structure;
using Modding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

namespace AutoHitManager.Managers
{
    public static class BindableFunctions
    {
        internal const int UP = 0;
        internal const int DOWN = 1;
        internal static int CountUp = 0;
        internal static int CountDown = 0;
        internal static void ToggleIntentHit()
        {
            Global.IntentionalHit = true;
            Global.FuryTimer.Start();
        }
        #region Actions
        internal static void FuryStep(int type)
        {
            bool isFuryAvailable = PlayerData.instance.GetInt("health") > 1 && Global.IsFuryEquipped && !Global.IsProhibitedZone && !Global.IntentionalHit;
            if (type == UP && CountUp == CountDown && isFuryAvailable)
            {
                CountUp++;
                Global.FuryGestureTimer.Stop();
                Global.FuryGestureTimer.Start();
            }
            else if (type == DOWN && CountUp > CountDown && isFuryAvailable)
            {
                CountDown++;
                Global.FuryGestureTimer.Stop();
                Global.FuryGestureTimer.Start();
            }

            if (CountDown == Global.GlobalSaveData.FuryCount)
            {
                Global.FuryGestureTimer.Stop();
                ToggleIntentHit();
            }
        }

        internal static void NextSplit()
        {
            if (Global.LocalSaveData.CurrentSplit < Global.LocalSaveData.Run.Splits.Count - 1)
            {
                Global.LocalSaveData.CurrentSplit++;
                Global.UpdateRunDataFile();
            } 
        }

        internal static void PrevSplit()
        {
            if (Global.LocalSaveData.CurrentSplit > 0)
            {
                Global.LocalSaveData.CurrentSplit--;
                Global.UpdateRunDataFile();
            }
        }

        internal static void SetPB(Run run)
        {
            Global.GlobalSaveData.ActualRun().PB = run;
            Global.UpdateRunDataFile();
        }

        internal static void ForceReloadSplits()
        {
        }
        #endregion Actions

        #region HookDelegates


        private static void CheckProhibitedZone(string name)
        {

            if (Global.IsProhibitedZone = Constants.ProhibitedZones.Any(zone => name.StartsWith(zone) || name == zone))
            {
                Global.IntentionalHit = false;
            }
        }

        private static void CheckCredits(string name)
        {
            if (name == "End_Credits")
            {
                EndRun();
            }
        }

        private static void CheckDreamerZone(string name)
        {
            if (Global.IsDreamerZone = Constants.DreamerZones.Any(zone => name.ToLower() == zone.ToLower() || name.ToLower().StartsWith(zone.ToLower())))
            {
                Global.IntentionalHit = false;
            }
        }

        #endregion HookDelegates

        #region HookActions

        [HookFunction(Hook = "BeforeSceneLoadHook")]
        private static string CheckScene(string name)
        {
            CheckCredits(name);
            CheckProhibitedZone(name);
            CheckDreamerZone(name);
            return name;
        }

        [HookFunction(Hook = "CharmUpdateHook")]
        private static void CheckFuryEquipped(PlayerData data, HeroController _)
        {
            Global.IsFuryEquipped = data.equippedCharm_6;
            if (!Global.IsFuryEquipped)
            {
                Global.IntentionalHit = false;
            }
        }

        [HookFunction(Hook = "AfterPlayerDeadHook")]
        private static void EndRun()
        {
            Global.LocalSaveData.Run.RunConfig().History.Add(Global.LocalSaveData.Run);
            StartRun();
        }

        [HookFunction(Hook = "SavegameLoadHook")]
        private static void LoadRun(int _)
        {
            StartRun();
        }

        [HookFunction(Hook = "NewGameHook")]
        private static void StartRun()
        {
            if (Global.LocalSaveData.NewRun)
            {
                Global.LocalSaveData.Run = new()
                {
                    RunConfigId = Global.GlobalSaveData.ActualRun().Id,
                    number = Global.GlobalSaveData.ActualRun().MaxRun++,
                    Ended = false
                };
                Global.LocalSaveData.NewRun = false;
            }
            Global.UpdateRunDataFile();
        }

        [HookFunction(Hook = "AfterTakeDamageHook")]
        private static int ManageHit(int hazardType, int dmg)
        {

            Global.FuryTimer.Stop();
            if (Global.IntentionalHit && (hazardType == 2 || hazardType == 3))
            {
                Global.FuryTimer.Start();
            }
            else
            {
                Global.PerformHit();
                Global.IntentionalHit = false;
            }
            return dmg;
        }

        [HookFunction(Hook = "SetPlayerIntHook")]
        private static int CheckPlayerHealth(string name, int orig)
        {
            if (name == "health")
            {
                if (orig == 1 && Global.IntentionalHit)
                {
                    Global.IntentionalHit = false;
                    Global.FuryTimer.Stop();
                }
                if (orig < 1 && Global.PracticeMode == "Yes")
                {
                    return 1;
                }
            }
            return orig;
        }

        #endregion HookActions

        #region Utils
        private static void StartHooks()
        {
            var methods = Assembly.GetExecutingAssembly().GetTypes().SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic))
                      .Where(m => m.GetCustomAttributes<HookFunctionAttribute>(false).Any())
                      .ToArray();
            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<HookFunctionAttribute>();
                var eventHandler = typeof(ModHooks).GetEvent(attr.Hook);
                var action = Delegate.CreateDelegate(eventHandler.EventHandlerType, method);
                attr.Action = action;
                eventHandler.AddEventHandler(null, action);
            }
        }

        internal static void EndHooks()
        {
            var methods = Assembly.GetExecutingAssembly().GetTypes().SelectMany(t => t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic))
                      .Where(m => m.GetCustomAttributes<HookFunctionAttribute>(false).Any())
                      .ToArray();
            foreach (var method in methods)
            {
                var attr = method.GetCustomAttribute<HookFunctionAttribute>();
                var eventHandler = typeof(ModHooks).GetEvent(attr.Hook);
                eventHandler.RemoveEventHandler(null, attr.Action);
            }

        }

        private static void StartRunInfo()
        {
            try
            {
                if (Global.GlobalSaveData.Runs.Count == 0)
                {

                    Global.GlobalSaveData.Runs.Add(new RunConfig
                    {
                        Id = Global.GlobalSaveData.NextRunId++,
                        Name = "Any %",
                        Splits = new List<SplitConfig> {
                        new SplitConfig("False Knight"),
                        new SplitConfig("Fireball"),
                        new SplitConfig("Hornet"),
                        new SplitConfig("Mantis Claw"),
                        new SplitConfig("Gruz Mother"),
                        new SplitConfig("Crystal Heart"),
                        new SplitConfig("Shade Soul"),
                        new SplitConfig("Monomon"),
                        new SplitConfig("Herrah"),
                        new SplitConfig("Lurien"),
                        new SplitConfig("Hollow Knight")
                    }
                    });
                }
            }
            catch { }

            try
            {
                if (Global.GlobalSaveData.ActualRun() == null)
                {
                    Global.GlobalSaveData.ActualRunId = Global.GlobalSaveData.Runs.First().Id;
                }
            }
            catch { }
        }

        internal static void StartInit()
        {

            Global.GenerateWidget();
            Global.FuryTimer = new Timer
            {
                AutoReset = false,
                Interval = 20_000
            };
            Global.FuryTimer.Elapsed += (sender, e) =>
            {
                Global.IntentionalHit = false;
            };
            Global.FuryGestureTimer = new Timer
            {
                AutoReset = false,
                Interval = 1_000
            };
            Global.FuryGestureTimer.Elapsed += (sender, e) =>
            {
                CountUp = 0;
                CountDown = 0;
            };

            StartHooks();
            StartRunInfo();

            Global.UpdateRunDataFile();

        }
        #endregion
    }
}
