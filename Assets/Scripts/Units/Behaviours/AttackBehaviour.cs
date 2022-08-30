using Units.Controllers;
using UnityEngine;

namespace Units.Behaviours
{
    public class AttackBehaviour : MonoBehaviour, IAttacking
    {
        public float AttackRange => _attackRange;
        
        [SerializeField] private float _attackRange;
        [SerializeField] private float _damage;
        [SerializeField] private float _attackSpeed;

        public void StartAttack(Vector3 direction)
        {
            // todo: animation and projectile spawn
        }
    }
}