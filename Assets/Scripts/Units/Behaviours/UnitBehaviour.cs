using Units.Clients;
using Units.Controllers;
using UnityEngine;

namespace Units.Behaviours
{
    public class UnitBehaviour : MonoBehaviour, IUnit
    {
        // Props
        public Transform Transform => transform;
        public IMovable Movable { get; private set; }
        public IAttacking Attacking { get; private set; }
        public IDamageable Damageable { get; private set; }
        public Affinity Affinity => _affinity;
        
        // Fields
        [SerializeField] private Affinity _affinity;
        [SerializeField] private float _maxHealth = 10;
        private void Awake()
        {
            Movable = GetComponent<IMovable>();
            Attacking = GetComponent<IAttacking>();
            Damageable = new Damageable(this, _maxHealth);
        }

        private void OnEnable()
        {
            Damageable.Died += OnDied;
        }

        private void OnDisable()
        {
            Damageable.Died -= OnDied;
        }

        private void OnDied(IUnit obj)
        {
            // todo play Death Animation
        }
    }
}