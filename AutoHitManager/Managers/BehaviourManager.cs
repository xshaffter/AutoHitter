using AutoHitManager.Cat;
using System.Linq;
using UnityEngine;

namespace AutoHitManager.Managers
{
    public class BehaviourManager : MonoBehaviour
    {
        
        public void Update()
        {
            if (HeroController.instance?.gameObject != null && !HeroController.instance.gameObject.GetComponents<CollisionManager>().Any())
            {
                HeroController.instance.gameObject.AddComponent<CollisionManager>();
            }

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