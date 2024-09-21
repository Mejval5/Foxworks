using UnityEngine;

namespace Fox.Interfaces
{
    public interface ITargetable : IEntity, IDamageable
    {
        public Transform AttackPoint { get; }
        public EntityType EntityType { get; }
    }
}