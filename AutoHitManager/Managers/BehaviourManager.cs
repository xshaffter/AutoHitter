using AutoHitManager.Cat;
using UnityEngine;

namespace AutoHitManager.Managers
{
    public class BehaviourManager : MonoBehaviour
    {
        
        public void Update()
        {
            if (Global.GlobalSaveData.binds.nextSplit.WasPressed)
            {
                BindableFunctions.NextSplit();
            }
            else if (Global.GlobalSaveData.binds.prevSplit.WasPressed)
            {
                BindableFunctions.PrevSplit();
            }
            else if (InputHandler.Instance.inputActions.rs_up.WasPressed)
            {
                BindableFunctions.FuryStep(BindableFunctions.UP);
            }
            else if (InputHandler.Instance.inputActions.rs_down.WasPressed)
            {
                BindableFunctions.FuryStep(BindableFunctions.DOWN);
            }
        }
    }
}