using System;
using Controllers;
using UnityEngine;

namespace Behaviours
{
    public class UnitBehaviour : MonoBehaviour
    {
        public IMovable Movable { get; private set; }

        private void Awake()
        {
            Movable = GetComponent<IMovable>();
        }
    }
}