using System;

namespace Controllers
{
    public class Damageable : IDamageable
    {
        public Damageable(IUnit unit, float maxHealth)
        {
            MaxHealth = maxHealth;
            _currentHealth = maxHealth;
            _unit = unit;
        }

        public event Action<IUnit> Died;  

        public float CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                if (value <= 0)
                {
                    Died?.Invoke(_unit);
                    _currentHealth = 0;
                    return;
                }

                if (value >= MaxHealth)
                {
                    _currentHealth = MaxHealth;
                    return;
                }

                _currentHealth = MaxHealth;
            }
        }
        public float MaxHealth { get; }

        private float _currentHealth;
        private IUnit _unit;
        
        public void DealDamage(float damage)
        {
            CurrentHealth -= damage;
        }
    }
}