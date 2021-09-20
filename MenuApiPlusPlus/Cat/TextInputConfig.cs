using MenuApiPlusPlus.Behaviours;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using MenuButton = Modding.Patches.MenuButton;

namespace MenuApiPlusPlus.Cat
{
    public class TextInputConfig
    {
        public string Placeholder;
        public KeyListener KeyListener;
        public bool IsFocused;
        internal string _text = "";
        public string Text
        {
            get
            {
                return TextLabel?.text;
            }
            set
            {
                if (TextLabel != null)
                {
                    TextLabel.text = value;
                }
                else
                {
                    _text = value;
                }
            }
        }
        public string OldText;
        internal Text TextLabel;
        /// <summary>
        /// The action to run when the button is pressed.
        /// </summary>
        // public Action<Patch.MenuButton> SubmitAction;
        /// <summary>
        /// Whether the button when activated proceeds to a new menu.
        /// </summary>
        public bool Proceed;
        /// <summary>
        /// The action to run when pressing the menu cancel key while selecting this item.
        /// </summary>
        public Action<MenuSelectable> CancelAction;
        public Action<MenuButton> submitAction;
        public Action<TextInputConfig> PostEnterAction;
        public void EnterAction ()
        {
            GameObject.DestroyImmediate(this.KeyListener);
            this.KeyListener = null;
            PostEnterAction(this);
            this.OldText = Text;
        }

        public void UnFocus()
        {
            GameObject.DestroyImmediate(this.KeyListener);
            this.IsFocused = false;
            this.Text = this.OldText;
        }

        public void Focus()
        {
            this.OldText = Text;
            this.Text = "";
            this.KeyListener = this.TextLabel.gameObject.AddComponent<KeyListener>();
            this.KeyListener.config = this;
            this.IsFocused = true;
        }
    }
}
