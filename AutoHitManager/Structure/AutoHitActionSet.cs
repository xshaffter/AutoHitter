using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InControl;

namespace AutoHitManager.Cat
{
    public class AutoHitActionSet : PlayerActionSet
    {
        public PlayerAction prevSplit;
        public PlayerAction nextSplit;
        public PlayerAction SetPB;

        public AutoHitActionSet()
        {
            this.prevSplit = base.CreatePlayerAction("prev_split");
            this.nextSplit = base.CreatePlayerAction("next_split");
            this.SetPB = base.CreatePlayerAction("set_pb");
            this.SetDefaultBinds();
        }

        private void SetDefaultBinds()
        {
            this.prevSplit.AddDefaultBinding(Key.F1);
            this.nextSplit.AddDefaultBinding(Key.F2);
            this.SetPB.AddDefaultBinding(Key.F3);
        }
    }
}