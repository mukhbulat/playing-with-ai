using System;
using Units.Controllers;
using UnityEngine;

namespace Units.Behaviours
{
    public class ProjectileBehaviour : MonoBehaviour
    {
        private IMovable _movable;
        private float _damage;
        private float _currentDistance;
        private Vector3 _direction;
        private bool _isPlayers;
        private int _playerLayer = 1 << 7;
        private int _enemyLayer = 1 << 8;
        private int _objectLayer = 1 << 6;
        

        public void Spawn(float damage, Vector3 direction, bool isPlayers = false)
        {
            _movable = GetComponent<IMovable>();
            _damage = damage;
            _direction = direction;
            _isPlayers = isPlayers;
        }

        private void Update()
        {
            _movable?.Move(_direction);
        }

        private void DealDamage(IDamageable damageable, float damage)
        {
            damageable?.DealDamage(damage);
        }

        private void OnTriggerEnter(Collider other)
        {
            var layer = other.gameObject.layer;
            if (layer == _objectLayer)
            {
                Destroy(gameObject);
            }

            if (_isPlayers && layer == _enemyLayer)
            {
                DealDamage(other.GetComponent<IUnit>().Damageable, _damage);
                Destroy(gameObject);
            }

            if (!_isPlayers && layer == _playerLayer)
            {
                DealDamage(other.GetComponent<IUnit>().Damageable, _damage);
                Destroy(gameObject);
            }
        }
    }
}