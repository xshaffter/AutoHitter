using MenuApiPlusPlus.Behaviours;
using MenuApiPlusPlus.Components;
using Modding.Menu;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MenuApiPlusPlus
{
    public class Menu : MenuBuilder
    {
        public Menu(string name) : base(name)
        {
        }

        public Menu(GameObject canvas, string name) : base(canvas, name)
        {
            
        }
    }
}
