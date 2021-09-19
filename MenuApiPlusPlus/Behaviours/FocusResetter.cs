using MenuApiPlusPlus.Components;
using Modding.Menu;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace MenuApiPlusPlus.Behaviours
{
    internal class FocusResetter : MonoBehaviour
    {
        internal ContentArea content;
        public void OnMouseDown()
        {
            content.ResetFocus();
        }
    }
}
