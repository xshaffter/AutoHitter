using MenuApiPlusPlus.Cat;
using Modding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MenuApiPlusPlus.Behaviours
{
    public class KeyListener : MonoBehaviour
    {
        public TextInputConfig config;
        private bool mayus = false;

        private KeyCode[] invalidCodes = new KeyCode[]
        {
            KeyCode.KeypadEnter,
            KeyCode.Return,
            KeyCode.Escape,
            KeyCode.RightShift,
            KeyCode.LeftShift,
            KeyCode.Backspace
        };

        public void Update()
        {
            if (Event.current.isKey)
            {
                var e = Event.current;
                var key = e.keyCode;
                if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
                {
                    config.EnterAction();
                }
                else if (Input.GetKeyDown(KeyCode.Backspace) && config.Text.Length > 0)
                {
                    config.Text = config.Text.Substring(0, config.Text.Length - 1);
                }
                else if (Input.GetKeyDown(KeyCode.Escape))
                {
                    config.UnFocus();
                }
                else if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftShift))
                {
                    mayus = true;
                }
                else if (Event.current.type == EventType.KeyDown)
                {
                    var text = e.character.ToString();
                    try
                    {
                        if (!char.IsControl(key.ToString(), 0) && !invalidCodes.ToList().Contains(key))
                        {
                            config.Text += text;
                        }
                    }
                    catch { }
                }
                else if (Input.GetKeyUp(KeyCode.RightShift) || Input.GetKeyUp(KeyCode.LeftShift))
                {
                    mayus = false;
                }
            }
        }
    }
}
