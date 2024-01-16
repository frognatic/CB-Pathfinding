using Cysharp.Threading.Tasks;
using Managers.Singleton;
using UnityEditor;
using UnityEngine;

namespace Managers
{
    public class SaveSOManager : MonoSingleton<SaveSOManager>
    {
        private SaveSO scriptable;

        public UniTask Initialize()
        {
            Load();
            return UniTask.CompletedTask;
        }

        public void Load()
        {
#if UNITY_EDITOR
            scriptable = AssetDatabase.LoadAssetAtPath<SaveSO>(SaveSO.Path);
            if (!scriptable)
            {
                scriptable = ScriptableObject.CreateInstance<SaveSO>();
                AssetDatabase.CreateAsset(scriptable, SaveSO.Path);
                AssetDatabase.SaveAssets();
            }
#endif
        }
    
        public void Save(SaveState state)
        {
#if UNITY_EDITOR  
            scriptable.save = JsonUtility.FromJson<SaveState>(JsonUtility.ToJson(state));
            EditorUtility.SetDirty(scriptable);
            AssetDatabase.SaveAssetIfDirty(scriptable);
#endif
        }
    }
}
