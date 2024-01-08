using Cysharp.Threading.Tasks;
using Initial;
using Managers.Singleton;
using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoSingleton<SaveManager>
    {
        private const string SaveName = "GameSave";

        public SaveState SaveState;
        
        public void AddToLoad() => InitManager.AddTaskToPhase(LoadPhase.Save, Initialize().ToAsyncLazy());

        private async UniTask Initialize()
        {
            await InitManager.WaitUntilPhaseStarted(LoadPhase.Save);
            // If need load save
        }
        
        public void Save()
        {
            PlayerPrefs.SetString(SaveName, JsonUtility.ToJson(SaveState));
            PlayerPrefs.Save();
            
#if UNITY_EDITOR
            SaveSOManager.Instance.Save(SaveState);
#endif
        }
        
        public void Load()
        {
            if (PlayerPrefs.HasKey(SaveName))
            {
                string json   = PlayerPrefs.GetString(SaveName);
                SaveState result = !string.IsNullOrEmpty(json) ? JsonUtility.FromJson<SaveState>(json) : null;
                SaveState = result;
            }
            else
                SaveState = new SaveState();

            SaveSOManager.Instance.Load();
        }
    }
}
