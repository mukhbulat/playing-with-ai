using UnityEngine;

namespace Navigation.Controllers
{
    public interface ITakesSpace
    {
        public Vector3 Position { get; }
        public float Size { get; }
    }
}