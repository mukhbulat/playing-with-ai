using Units.Clients;

namespace Units.Controllers
{
    public interface IUnit
    {
        public IMovable Movable { get; }
        public IDamageable Damageable { get; }
        public Affinity Affinity { get; }
    }
}