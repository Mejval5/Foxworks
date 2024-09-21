using System;
using UnityEngine;

namespace Foxworks.Interfaces
{
    /// <summary>
    ///     Interface for entities that can be damaged.
    /// </summary>
    public interface IDamageable
    {
        public float CurrentHealth { get; }
        public void Damage(float amount, ITargetable sender, DamageType damageType = DamageType.Generic, Vector3? hitPosition = null, Vector3? hitNormal = null);
        public event Action<ITargetable, float> GotDamaged;
    }
}