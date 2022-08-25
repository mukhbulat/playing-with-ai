using System;
using Behaviours;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Clients.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private MovementBehaviour _playerCharacterMovement;
        
        private PlayerInput _playerInput;
        private InputAction _movement;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _movement = _playerInput.actions["Movement"];
        }

        private void Update()
        {
            var movementInput = _movement.ReadValue<Vector2>();
            if (movementInput != Vector2.zero)
            {
                _playerCharacterMovement.Move(movementInput);
            }
        }
    }
}