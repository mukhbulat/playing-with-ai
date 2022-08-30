using UnityEngine;

namespace Units.Controllers
{
    public interface IAttacking
    {
        public float AttackRange { get; }

        public void StartAttack(Vector3 direction);
    }
}