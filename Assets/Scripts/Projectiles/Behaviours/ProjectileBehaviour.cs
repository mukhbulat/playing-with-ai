using System;
using System.Collections;
using Units.Controllers;
using UnityEngine;

namespace Projectiles.Behaviours
{
    public class ProjectileBehaviour : MonoBehaviour
    {
        [SerializeField] private float _lifetime = 10f;
        private IMovable _movable;
        private float _damage;
        private float _colliderRadius = 0.15f;
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
            StartCoroutine(Lifetime());
        }

        private void Update()
        {
            _movable?.Move(_direction);
            
            CollisionCheck();
        }

        private void CollisionCheck()
        {
            var hits = Physics2D.OverlapCircleAll(transform.position, _colliderRadius);
            
            foreach (var hit in hits)
            {
                var layer = hit.gameObject.layer;
                layer = (int) Mathf.Pow(2f, layer);
                if (layer == _objectLayer)
                {
                    Destroy(gameObject);
                }

                if (_isPlayers && layer == _enemyLayer)
                {
                    DealDamage(hit.GetComponent<IUnit>().Damageable, _damage);
                    Destroy(gameObject);
                }

                if (!_isPlayers && layer == _playerLayer)
                {
                    DealDamage(hit.GetComponent<IUnit>().Damageable, _damage);
                    Destroy(gameObject);
                }
            }
        }

        private void DealDamage(IDamageable damageable, float damage)
        {
            damageable?.DealDamage(damage);
        }
        
        private IEnumerator Lifetime()
        {
            yield return new WaitForSeconds(_lifetime);
            Destroy(gameObject);
        }
    }
}