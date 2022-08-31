using Units.Controllers;
using UnityEngine;

namespace Projectiles.Behaviours
{
    public class ProjectileMovement : MonoBehaviour, IMovable
    {
        [SerializeField] private float _speed = 15;
        
        public void Move(Vector2 direction)
        {
            direction = direction.normalized;

            var velocity = direction * _speed * Time.deltaTime;
            transform.Translate(velocity);
        }
    }
}