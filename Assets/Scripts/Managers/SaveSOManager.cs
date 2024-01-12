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
            scriptable = AssetDatabase.LoadAssetAtPath<SaveSO>(SaveSO.Path);
            if (!scriptable)
            {
                scriptable = ScriptableObject.CreateInstance<SaveSO>();
                AssetDatabase.CreateAsset(scriptable, SaveSO.Path);
                AssetDatabase.SaveAssets();
            }
        }
    
        public void Save(SaveState state)
        {
            scriptable.save = JsonUtility.FromJson<SaveState>(JsonUtility.ToJson(state));
            EditorUtility.SetDirty(scriptable);
            AssetDatabase.SaveAssetIfDirty(scriptable);
        }
    }
}
