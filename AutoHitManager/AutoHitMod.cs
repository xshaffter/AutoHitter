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
using static AutoHitManager.Managers.BindableFunctions;

namespace AutoHitManager
{
    public class AutoHitMod : Mod, ITogglableMod, ICustomMenuMod
    {
        internal static AutoHitMod LoadedInstance { get; set; }

        internal GameObject Game { get; private set; }

        public bool ToggleButtonInsideMenu => true;

        public MenuScreen Screen { get; private set; }


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

            Game = new GameObject();
            Game.AddComponent<BehaviourManager>();
            GameObject.DontDestroyOnLoad(Game);
            StartInit();
            // ModHooks.Res

        }

        // Code that should be run when the mod is disabled.
        public void Unload()
        {
            EndHooks();
            LoadedInstance = null;
            GameObject.DestroyImmediate(Game);
        }

        public void OnLoadLocal(HitManagerSaveData s) => Global.LocalSaveData = s;

        public HitManagerSaveData OnSaveLocal() => Global.LocalSaveData;

        public void OnLoadGlobal(HitManagerGlobalSaveData s)
        {
            Global.GlobalSaveData = s;
        }

        public HitManagerGlobalSaveData OnSaveGlobal() => Global.GlobalSaveData;

        public override string GetVersion() => Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? toggleDelegates)
        {
            return AutoHitMenu.BuildMenu(modListMenu, toggleDelegates);
        }
    }
}
