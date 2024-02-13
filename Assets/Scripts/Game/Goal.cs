using System;
using Core;
using Mirror;
using UnityEngine;

namespace Game
{
    public class Goal : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            var ball = other.GetComponent<Ball>();

            if(ball == null)
                return;
            
            ball.CollidedWithGoal(this);
        }
    }
}