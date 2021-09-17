using AutoHitManager.Cat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AutoHitManager.Managers
{
    internal class CollisionManager : MonoBehaviour
    {
        public void OnCollisionEnter2D(Collision2D collision)
        {
            //Check for a match with the specified name on any GameObject that collides with your GameObject
            var rightCollision = collision.gameObject.name.ToLower() == "Bottom".ToLower();
            Global.Log(rightCollision);
            Global.Log(collision.gameObject.name.ToLower());
            if (rightCollision && Global.IsDreamerZone)
            {
                Global.PerformHit();
            }
        }
    }
}
