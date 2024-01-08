using UnityEngine;

namespace Managers
{
    public class ManagerHolder : MonoBehaviour
    {
        public static Transform Holder { get; private set; }

        private void Awake()
        {
            Holder = transform;
            DontDestroyOnLoad(gameObject);
        }

        private void Start() => InitManager.Instance.Init().Forget();
    }
}