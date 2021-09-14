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
using System.Timers;
using Modding.Menu;
using UnityEngine.UI;
using AutoHitManager.UI.Scenes;

namespace AutoHitManager
{
    public class AutoHitMod : Mod, ITogglableMod
    {
        public static AutoHitMod LoadedInstance { get; set; }

        public GameObject Game { get; private set; }

        public bool ToggleButtonInsideMenu => true;

        public MenuScreen screen { get; private set; }


        /// <summary>
        /// Gets or sets the save settings of this Mod
        /// </summary>
        public override ModSettings SaveSettings
        {
            get => Global.LocalSaveData;
            // ReSharper disable once ValueParameterNotUsed overriden by super class
            set { }
        }

        /// <summary>
        /// Gets or sets the global settings of this Mod
        /// </summary>
        public override ModSettings GlobalSettings
        {
            get => Global.GlobalSaveData;
            // ReSharper disable once ValueParameterNotUsed overriden by super class
            set { }
        }

        public override void Initialize()
        {
            if (LoadedInstance != null) return;
            LoadedInstance = this;

            Global.GenerateWidget();

            Game = new GameObject();
            Game.AddComponent<BehaviourManager>();
            GameObject.DontDestroyOnLoad(Game);
            Global.FuryTimer = new Timer
            {
                AutoReset = false,
                Interval = 20_000
            };
            Global.FuryTimer.Elapsed += (sender, e) =>
            {
                Global.IntentionalHit = false;
                Global.FuryTimer.Dispose();
            };
            ModHooks.Instance.AfterTakeDamageHook += ManageHit;
            ModHooks.Instance.SavegameLoadHook += LoadRun;
            ModHooks.Instance.NewGameHook += StartRun;
            ModHooks.Instance.AfterPlayerDeadHook += EndRun;
            ModHooks.Instance.BeforeSceneLoadHook += CheckScene;
            ModHooks.Instance.CharmUpdateHook += CheckFuryEquipped;
            ModHooks.Instance.TakeHealthHook += CheckPlayerHealth;

        }

        // Code that should be run when the mod is disabled.
        public void Unload()
        {
            ModHooks.Instance.AfterTakeDamageHook -= ManageHit;
            ModHooks.Instance.SavegameLoadHook -= LoadRun;
            ModHooks.Instance.AfterPlayerDeadHook -= EndRun;
            ModHooks.Instance.BeforeSceneLoadHook -= CheckScene;
            ModHooks.Instance.CharmUpdateHook -= CheckFuryEquipped;
            ModHooks.Instance.TakeHealthHook -= CheckPlayerHealth;
            LoadedInstance = null;
            GameObject.DestroyImmediate(Game);
        }

        private string CheckScene(string name)
        {
            CheckCredits(name);
            CheckProhibitedZone(name);
            return name;
        }

        private void CheckFuryEquipped(PlayerData data, HeroController controller)
        {
            Global.IsFuryEquipped = data.equippedCharm_6;
            if (!Global.IsFuryEquipped)
            {
                Global.IntentionalHit = false;
            }
        }

        private void CheckProhibitedZone(string name)
        {

            if (Global.IsProhibitedZone = Constants.ProhibitedZones.Any(zone => name.StartsWith(zone) || name == zone))
            {
                Global.IntentionalHit = false;
            }
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
            Global.GlobalSaveData.ActualRun.LastRun = Global.LocalSaveData.Run;
            Global.LocalSaveData.Run.Ended = true;
        }

        private void LoadRun(int slot)
        {
            StartRun();
        }

        private void StartRun()
        {
            if (Global.LocalSaveData.NewRun)
            {
                Global.LocalSaveData.Run = new()
                {
                    number = Global.GlobalSaveData.ActualRun.MaxRun++,
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
                Global.FuryTimer.Stop();
            } 
            else
            {
                Global.FuryTimer.Stop();
                Global.FuryTimer.Start();
            }
            return dmg;
        }
        private int CheckPlayerHealth(int orig)
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
            return orig;
        }

        public void OnLoadLocal(HitManagerSaveData s) => Global.LocalSaveData = s;

        public HitManagerSaveData OnSaveLocal() => Global.LocalSaveData;

        public void OnLoadGlobal(HitManagerGlobalSaveData s)
        {
            Global.GlobalSaveData = s;
            if (s.Runs.Count == 0)
            {

                Global.GlobalSaveData.Runs.Add(new RunConfig
                {
                    Name = "Any %",
                    Splits = new List<SplitConfig> {
                        new SplitConfig("False Knight"),
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

            if (Global.GlobalSaveData.ActualRun == null)
            {
                Global.GlobalSaveData.ActualRun = Global.GlobalSaveData.Runs.First();
            }
        }

        public HitManagerGlobalSaveData OnSaveGlobal() => Global.GlobalSaveData;

        public override string GetVersion() => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
        {
            this.screen = AutoHitMenu.BuildMenu(modListMenu, toggleDelegates);
            try
            {
                Global.RunListMenu = AvailableRunsMenu.BuildMenu(modListMenu, toggleDelegates);
                Global._ModConfigMenu = SettingsMenu.BuildMenu(modListMenu, toggleDelegates);
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
            return this.screen;
        }
    }
}
