using UnityEngine;

namespace Foxworks.Interfaces
{
    /// <summary>
    ///     Interface for entities that can be targeted.
    /// </summary>
    public interface ITargetable : IEntity, IDamageable
    {
        public Transform AttackPoint { get; }
        public EntityType EntityType { get; }
    }
}