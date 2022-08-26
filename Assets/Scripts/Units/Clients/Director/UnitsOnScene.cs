using System.Collections.Generic;
using Units.Behaviours;
using Units.Controllers;
using UnityEngine;

namespace Units.Clients.Director
{
    public class UnitsOnScene : MonoBehaviour
    {
        private List<IUnit> _units;
        private IUnit _playerUnit;

        public List<IUnit> Units => _units;

        private void Awake()
        {
            var units = FindObjectsOfType<UnitBehaviour>();
            _units = new List<IUnit>();
            _units.AddRange(units);
            foreach (var unit in _units)
            {
                if (unit.Affinity == Affinity.Player)
                {
                    _playerUnit = unit;
                    break;
                }
            }

            _units.Remove(_playerUnit);
        }

        private void OnEnable()
        {
            foreach (var unit in _units)
            {
                unit.Damageable.Died += OnUnitDied;
            }
            
            _playerUnit.Damageable.Died += OnPlayerDied; 
        }

        private void OnPlayerDied(IUnit obj)
        {
            // todo game over.
        }

        private void OnDisable()
        {
            if (_units != null && _units.Count != 0)
            {
                foreach (var unit in _units)
                {
                    unit.Damageable.Died -= OnUnitDied;
                }
            }
        }

        private void OnUnitDied(IUnit obj)
        {
            _units.Remove(obj);
            obj.Damageable.Died -= OnUnitDied;
        }
    }
}