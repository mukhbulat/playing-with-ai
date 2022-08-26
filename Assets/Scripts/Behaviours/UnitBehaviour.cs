﻿using System;
using Clients;
using Controllers;
using UnityEngine;

namespace Behaviours
{
    public class UnitBehaviour : MonoBehaviour, IUnit
    {
        // Props
        public IMovable Movable { get; private set; }
        public IDamageable Damageable { get; private set; }
        public Affinity Affinity => _affinity;
        
        // Fields
        [SerializeField] private Affinity _affinity;
        [SerializeField] private float _maxHealth = 10;
        private void Awake()
        {
            Movable = GetComponent<IMovable>();
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