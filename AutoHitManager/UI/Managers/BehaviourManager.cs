using AutoHitManager.Cat;
using AutoHitManager.UI.Scenes;
using InControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AutoHitManager.UI.Managers
{
    public class KeyboardManager : MonoBehaviour
    {
        private string Name = "";
        private static GameObject keylistener = null;
        private static SplitConfig Split = null;
        public static KeyCode[] ValidKeys = new KeyCode[]
        {
            KeyCode.A,
            KeyCode.B,
            KeyCode.C,
            KeyCode.D,
            KeyCode.E,
            KeyCode.F,
            KeyCode.G,
            KeyCode.H,
            KeyCode.I,
            KeyCode.J,
            KeyCode.K,
            KeyCode.L,
            KeyCode.M,
            KeyCode.N,
            KeyCode.O,
            KeyCode.P,
            KeyCode.Q,
            KeyCode.R,
            KeyCode.S,
            KeyCode.T,
            KeyCode.U,
            KeyCode.V,
            KeyCode.W,
            KeyCode.X,
            KeyCode.Y,
            KeyCode.Z,
            KeyCode.Space,
            KeyCode.Comma,
            KeyCode.Period,
            KeyCode.Quote,
            KeyCode.Alpha0,
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
            KeyCode.Keypad0,
            KeyCode.Keypad1,
            KeyCode.Keypad2,
            KeyCode.Keypad3,
            KeyCode.Keypad4,
            KeyCode.Keypad5,
            KeyCode.Keypad6,
            KeyCode.Keypad7,
            KeyCode.Keypad8,
            KeyCode.Keypad9
        };

        public void Update()
        {
            foreach (var key in ValidKeys)
            {
                if (Input.GetKeyUp(key))
                {
                    string input = ((char)key).ToString();
                    try
                    {
                        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                        {
                            Name += input.ToUpper();
                        }
                        else
                        {
                            Name += input.ToLower();
                        }
                    }
                    catch
                    {
                        Name += input;
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
            {
                Split.Name = Name;
                Name = "";
                GameObject.DestroyImmediate(keylistener);
                keylistener = null;
                UIManager.instance.UIGoToDynamicMenu(SplitManagerMenu.BuildMenu(SplitManagerMenu.PreviousScreen));
                Global.UpdateRunDataFile();
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                if (name.Length > 0)
                {
                    name = name.Remove(name.Length - 1);
                }
            }
        }

        public static void UsageButton(RunConfig run, SplitConfig split)
        {
            int index;
            switch (SplitManagerMenu.buttonBehaviour)
            {
                case SplitManagerMenu.RENAME:
                    if (keylistener == null)
                    {
                        keylistener = new GameObject();
                        keylistener.AddComponent<KeyboardManager>();
                        DontDestroyOnLoad(keylistener);
                        Split = split;
                    }
                    break;
                case SplitManagerMenu.BEFORE:
                    index = run.Splits.IndexOf(split);
                    run.Splits.Insert(index, new SplitConfig("Dummy"));
                    UIManager.instance.UIGoToDynamicMenu(SplitManagerMenu.BuildMenu(SplitManagerMenu.PreviousScreen));
                    Global.UpdateRunDataFile();
                    break;
                case SplitManagerMenu.AFTER:
                    index = run.Splits.IndexOf(split);
                    run.Splits.Insert(index + 1, new SplitConfig("Dummy"));
                    UIManager.instance.UIGoToDynamicMenu(SplitManagerMenu.BuildMenu(SplitManagerMenu.PreviousScreen));
                    Global.UpdateRunDataFile();
                    break;
                case SplitManagerMenu.DELETE:
                    if (run.Splits.Count > 1)
                    {
                        run.Splits.Remove(split);
                        UIManager.instance.UIGoToDynamicMenu(SplitManagerMenu.BuildMenu(SplitManagerMenu.PreviousScreen));
                        Global.UpdateRunDataFile();
                    }
                    break;
            }
            SplitManagerMenu.buttonBehaviour = SplitManagerMenu.RENAME;
        }

    }
}
