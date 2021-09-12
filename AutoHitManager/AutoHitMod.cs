using System;
using Modding;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using System.IO;
using System.Linq;
using UnityEngine;

using AutoHitManager.Structure;
using System.Reflection;
using InControl;
using AutoHitManager.Cat;
using AutoHitManager.Managers;
using Modding.Delegates;

namespace AutoHitManager
{
    public class AutoHitMod : Mod, ITogglableMod, ILocalSettings<HitManagerSaveData>, IGlobalSettings<HitManagerGlobalSaveData>
    {
        public static AutoHitMod LoadedInstance { get; set; }

        public GameObject Game { get; private set; }

        public override void Initialize()
        {
            if (LoadedInstance != null) return;
            LoadedInstance = this;

            if (Global.GlobalSaveData.FirstRun)
            {
                Global.GlobalSaveData.FirstRun = false;
            }

            Global.GenerateWidget();

            Game = new GameObject();
            Game.AddComponent<BehaviourManager>();
            GameObject.DontDestroyOnLoad(Game);
            ModHooks.AfterTakeDamageHook += ManageHit;
            ModHooks.SavegameLoadHook += LoadRun;
            ModHooks.NewGameHook += StartRun;
            ModHooks.AfterPlayerDeadHook += EndRun;
            ModHooks.SceneChanged += CheckCredits;
            ModHooks.SetPlayerIntHook += CheckFury;
            ModHooks.CharmUpdateHook += CheckFuryEquipped;
        }

        private void CheckFuryEquipped(PlayerData data, HeroController controller)
        {
            Global.IsFuryEquipped = data.equippedCharm_6;
            if (!Global.IsFuryEquipped)
            {
                Global.IntentionalHit = false;
            }
        }

        // Code that should be run when the mod is disabled.
        public void Unload()
        {
            ModHooks.AfterTakeDamageHook -= ManageHit;
            ModHooks.SavegameLoadHook -= LoadRun;
            ModHooks.AfterPlayerDeadHook -= EndRun;
            ModHooks.SceneChanged -= CheckCredits;
            ModHooks.SetPlayerIntHook -= CheckFury;
            ModHooks.CharmUpdateHook -= CheckFuryEquipped;
            LoadedInstance = null;
            GameObject.DestroyImmediate(Game);
        }

        private int CheckFury(string name, int orig)
        {
            if (name == "health" && orig == 1 && Global.IntentionalHit)
            {
                Global.IntentionalHit = false;
            }
            return orig;
        }

        private void CheckCredits(string name)
        {
            if (name == "End_Credits")
            {
                EndRun();
            }
        }

        private void EndRun()
        {
            Global.GlobalSaveData.LastRun = Global.LocalSaveData.Run;
            Global.LocalSaveData.Run.Ended = true;
        }

        private void LoadRun(int slot)
        {
            StartRun();
        }

        private void StartRun()
        {
            var settings = Path.Combine(Constants.DirFolder, "settings.json");
            Global.ReadSettings(settings);
            if (Global.LocalSaveData.NewRun)
            {
                Global.LocalSaveData.Run = new()
                {
                    number = Global.GlobalSaveData.MaxRun++,
                    Ended = false,
                    Splits = new()
                };
                Global.LocalSaveData.NewRun = false;
            }
            Global.UpdateRunDataFile();
        }

        private int ManageHit(int hazardType, int dmg)
        {
            if ((!Global.IntentionalHit || (Global.IntentionalHit && hazardType != 2)) && !PlayerData.instance.isInvincible)
            {
                Global.PerformHit();
                Global.IntentionalHit = false;
            }
            return dmg;
        }

        public void OnLoadLocal(HitManagerSaveData s) => Global.LocalSaveData = s;

        public HitManagerSaveData OnSaveLocal() => Global.LocalSaveData;

        public void OnLoadGlobal(HitManagerGlobalSaveData s) => Global.GlobalSaveData = s;

        public HitManagerGlobalSaveData OnSaveGlobal() => Global.GlobalSaveData;

        public override string GetVersion() => Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }
}
