using AutoHitManager.Cat;
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
            if (Global.IsProhibitedZone)
            {
                return;
            }

            if (Global.IsFuryEquipped && !Global.IntentionalHit)
            {
                Global.IntentionalHit = true;
                Global.FuryTimer.Start();
            }
        }
        #region Actions
        public static void FuryStep(int type)
        {
            bool isFuryAvailable = PlayerData.instance.GetInt("health") > 1;
            if (type == UP && CountUp == CountDown && isFuryAvailable)
            {
                CountUp++;
            }
            else if (type == DOWN && CountUp > CountDown && isFuryAvailable)
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
            } 
        }

        public static void PrevSplit()
        {
            if (Global.LocalSaveData.CurrentSplit > 0)
            {
                Global.LocalSaveData.CurrentSplit--;
                Global.UpdateRunDataFile();
            }
        }

        public static void SetPB(Run run)
        {
            Global.GlobalSaveData.ActualRun().PB = run;
            Global.UpdateRunDataFile();
        }

        public static void ForceReloadSplits()
        {
        }
        #endregion Actions
    }
}
