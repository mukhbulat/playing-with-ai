using Units.Clients;
using UnityEngine;

namespace Units.Controllers
{
    public interface IUnit
    {
        public Transform Transform { get; } 
        public IMovable Movable { get; }
        public IAttacking Attacking { get; }
        public IDamageable Damageable { get; }
        public Affinity Affinity { get; }
    }
}