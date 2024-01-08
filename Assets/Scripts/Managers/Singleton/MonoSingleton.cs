using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Managers.Singleton
{
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        protected CancellationToken destroyToken;
        
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
            instance.destroyToken = go.GetCancellationTokenOnDestroy();
        }
    }
}