using System;
using System.Collections;
using Projectiles.Behaviours;
using Units.Controllers;
using UnityEngine;

namespace Units.Behaviours
{
    public class AttackBehaviour : MonoBehaviour, IAttacking
    {
        public float AttackRange => _attackRange;
        public bool CanAttack => _canAttack;
        
        [SerializeField] private float _attackRange;
        [SerializeField] private float _damage;
        [SerializeField] private float _attackSpeed;
        [SerializeField] private ProjectileBehaviour _projectilePrefab;
        [SerializeField] private bool _isPlayers;
        
        private bool _canAttack = true;

        public void StartAttack(Vector3 direction)
        {
            // todo: animation and projectile spawn
            if (!_canAttack) return;
            Debug.Log("AttackBehaviour StartAttack worked");
            direction = direction.normalized;
            var projectile = Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
            projectile.Spawn(_damage, direction, _isPlayers);
            StartCoroutine(WaitForAttack());
        }

        private IEnumerator WaitForAttack()
        {
            _canAttack = false;
            if (_attackSpeed == 0)
            {
                Debug.Log("Attack speed is 0");
                _attackSpeed = 1;
            }
            yield return new WaitForSeconds(1 / _attackSpeed);
            _canAttack = true;
        }
    }
}