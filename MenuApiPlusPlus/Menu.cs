using MenuApiPlusPlus.Behaviours;
using MenuApiPlusPlus.Components;
using Modding.Menu;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace MenuApiPlusPlus
{
    public class MenuBuildernt : MenuBuilder
    {
        public MenuBuildernt(string name) : base(name)
        {
        }

        public MenuBuildernt(GameObject canvas, string name) : base(canvas, name)
        {
            
        }
    }
}
