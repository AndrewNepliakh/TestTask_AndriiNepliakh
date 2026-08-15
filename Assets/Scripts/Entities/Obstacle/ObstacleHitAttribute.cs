using System;
using UnityEngine;

namespace Entities
{
    public class ObstacleHitAttribute : MonoBehaviour
    {
        public event Action OnHit;

        public void Hit()
        {
            OnHit?.Invoke();
        }
    }
}