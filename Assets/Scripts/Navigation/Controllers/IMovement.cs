using UnityEngine;

namespace Navigation.Controllers
{
    public interface IMovement
    {
        public float Speed { get; }
        
        public void Move(Vector3 point);
        public void Stop();
        public void ChangeSpeed(float newSpeed);
    }
}