﻿using AutoHitManager.Cat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AutoHitManager.Managers
{
    public static class BindableFunctions
    {
        public const int UP = 0;
        public const int DOWN = 1;
        public static int CountUp = 0;
        public static int CountDown = 0;
        public static void ToggleIntentHit()
        {
            if (Global.IsFuryEquipped && !Global.IntentionalHit)
            {
                Global.IntentionalHit = true;
            }
            else
            {
                Global.Log(PlayerData.instance.equippedCharms);
            }
        }
        #region Actions
        public static void FuryStep(int type)
        {
            if (type == UP && CountUp == CountDown)
            {
                CountUp++;
            }
            else if (type == DOWN && CountUp > CountDown)
            {
                CountDown++;
            }

            if (CountDown == 3)
            {
                ToggleIntentHit();
            }
        }

        public static void NextSplit()
        {
            if (Global.LocalSaveData.CurrentSplit < Global.Splits.Count - 1)
            {
                Global.LocalSaveData.CurrentSplit++;
                Global.UpdateRunDataFile();
                Global.Log($"Split moved from {Global.LocalSaveData.CurrentSplit - 1} to {Global.LocalSaveData.CurrentSplit}");
            } 
            else
            {
                Global.Log($"Split can't be greater than {Global.Splits.Count}");
            }
        }

        public static void PrevSplit()
        {
            if (Global.LocalSaveData.CurrentSplit > 0)
            {
                Global.LocalSaveData.CurrentSplit--;
                Global.UpdateRunDataFile();
                Global.Log($"Split moved from {Global.LocalSaveData.CurrentSplit + 1} to {Global.LocalSaveData.CurrentSplit}");
            }
            else
            {
                Global.Log($"Split can't be lesser than 0");
            }
        }

        public static void SetPB()
        {
            Global.Log("Set new PB!!");
            Global.GlobalSaveData.PB = Global.GlobalSaveData.LastRun;
        }

        public static void ForceReloadSplits()
        {
            var settings = Path.Combine(Constants.DirFolder, "settings.json");
            Global.ReadSettings(settings);
        }
        #endregion Actions
    }
}
