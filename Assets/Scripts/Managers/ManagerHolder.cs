using UnityEngine;

namespace Managers
{
    public class ManagerHolder : MonoBehaviour
    {
        private static Transform holder;
        public static Transform Holder => holder;

        private void Awake()
        {
            holder = transform;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            InitManager.Instance.Init();
        }
    }
}