using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Initial;
using Managers;
using Managers.Singleton;
using UnityEditor;
using UnityEngine;

public class SaveSOManager : MonoSingleton<SaveSOManager>
{
    private SaveSO scriptable;
    public SaveSO Scriptable => scriptable; 
    
    public void AddToLoad() => InitManager.AddTaskToPhase(LoadPhase.Save, Initialize().ToAsyncLazy());

    private async UniTask Initialize()
    {
        await InitManager.WaitUntilPhaseStarted(LoadPhase.Save);
        Load();
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
