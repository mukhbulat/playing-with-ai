using Units.Behaviours;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Units.Clients.Player
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private UnitBehaviour _playerCharacter;
        
        private PlayerInput _playerInput;
        private InputAction _movement, _attack;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _movement = _playerInput.actions["Movement"];
            _attack = _playerInput.actions["Attack"];
        }

        private void Update()
        {
            var movementInput = _movement.ReadValue<Vector2>();
            if (movementInput != Vector2.zero)
            {
                if (movementInput.magnitude > 1) movementInput.Normalize();
                _playerCharacter.Movable.Move(movementInput);
            }

            var attackInput = _attack.ReadValue<Vector2>();
            if (attackInput != Vector2.zero)
            {
                _playerCharacter.Attacking.StartAttack(attackInput);
            }
        }
    }
}