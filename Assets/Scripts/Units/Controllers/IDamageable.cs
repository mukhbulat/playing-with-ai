using System;

namespace Units.Controllers
{
    public interface IDamageable
    {
        public event Action<IUnit> Died; 
        public float CurrentHealth { get; }
        public float MaxHealth { get; }
    
        public void DealDamage(float damage);
    }
}