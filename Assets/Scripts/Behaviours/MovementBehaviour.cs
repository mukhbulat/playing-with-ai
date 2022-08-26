using Controllers;
using UnityEngine;

namespace Behaviours
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class MovementBehaviour : MonoBehaviour, IMovable
    {
        [SerializeField] private float _speed;

        private CircleCollider2D _collider;

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
        }

        public void Move(Vector2 direction)
        {
            var velocity = Vector2.zero;
            var position = (Vector2) transform.position;
            
            var horVelocity = direction.x * _speed * Time.deltaTime * Vector2.right;
            var newHorPosition = position + horVelocity;
            var verVelocity = direction.y * _speed * Time.deltaTime * Vector2.up;
            var newVerPosition = position + verVelocity;
            
            if (CollisionCheck(newHorPosition)) velocity += horVelocity;
            if (CollisionCheck(newVerPosition)) velocity += verVelocity;
            
            if (velocity == Vector2.zero) return;
            transform.Translate(velocity);
        }

        private bool CollisionCheck(Vector2 newPosition)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(newPosition, _collider.radius);

            var collisionCheck = true;
            foreach (var hit in hits)
            {
                if (hit != _collider) collisionCheck = false;
            }

            return collisionCheck;
        }
    }
}
