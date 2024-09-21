using System;
using UnityEngine;
using UnityEngine.Events;

namespace Fox.Interfaces
{
    public interface IDamageable
    {
        public float CurrentHealth { get; }
        public void Damage(float amount, ITargetable sender, DamageType damageType = DamageType.Generic, Vector3? hitPosition = null, Vector3? hitNormal = null);
        public event Action<ITargetable, float> GotDamaged;
    }
}