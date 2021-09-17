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
using System.Timers;
using Modding.Menu;
using Modding.Menu.Config;
using UnityEngine.UI;
using AutoHitManager.UI.Scenes;
using static AutoHitManager.Managers.BindableFunctions;

namespace AutoHitManager
{
    public class AutoHitMod : Mod, ITogglableMod, ILocalSettings<HitManagerSaveData>, IGlobalSettings<HitManagerGlobalSaveData>, ICustomMenuMod
    {
        internal static AutoHitMod LoadedInstance { get; set; }

        internal GameObject Game { get; private set; }

        public bool ToggleButtonInsideMenu => true;

        public MenuScreen Screen { get; private set; }

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
            this.Screen = AutoHitMenu.BuildMenu(modListMenu, toggleDelegates);
            return this.Screen;
        }
    }
}
