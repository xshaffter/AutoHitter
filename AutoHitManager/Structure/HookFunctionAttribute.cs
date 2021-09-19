using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoHitManager.Structure
{
    internal class HookFunctionAttribute : Attribute
    {
        public string Hook { get; set; }
        public Delegate Action { get; set; }
    }
}
