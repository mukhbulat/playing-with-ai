using System;
using System.Collections.Generic;
using Behaviours;
using Controllers;
using UnityEngine;

namespace Clients.Director
{
    public class UnitsOnScene : MonoBehaviour
    {
        private List<IUnit> _units;

        public List<IUnit> Units => _units;

        private void Awake()
        {
            var units = FindObjectsOfType<UnitBehaviour>();
            _units = new List<IUnit>();
            _units.AddRange(units);
        }

        private void OnEnable()
        {
            foreach (var unit in _units)
            {
                unit.Damageable.Died += OnUnitDied;
            }
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