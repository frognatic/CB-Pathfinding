using UnityEngine;

namespace Managers.Singleton
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        public static T Instance
        {
            get
            {
                if (!instance)
                    Create();
                return instance;
            }
        }

        private static T instance;

        private static void Create()
        {
            if (instance)
                return;

            var go = new GameObject(typeof(T).Name);
            go.transform.SetParent(ManagerHolder.Holder);

            instance = go.AddComponent<T>();
        }
    }
}