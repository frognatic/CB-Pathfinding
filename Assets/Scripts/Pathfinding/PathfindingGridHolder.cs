using UnityEngine;

namespace Pathfinding
{
    public class PathfindingGridHolder : MonoBehaviour
    {
        public static Transform Holder { get; private set; }
        
        private void Awake() => Holder = transform;
    }
}
