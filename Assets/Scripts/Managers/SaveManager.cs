using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Managers.Singleton;
using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoSingleton<SaveManager>
    {
        private const string SaveName = "GameSave";
        private const int SaveWaitTime = 1000;
        private bool isSaving;

        public SaveState SaveState;
        
        public UniTask Initialize()
        {
            // if we want, we can load here all data, except creating new save state instance
            SaveState = new();
            return UniTask.CompletedTask;
        }
        
        public void TrySave()
        {
            if (isSaving)
                return;

            isSaving = true;
            WaitForSave().Forget();
        }

        private async UniTask WaitForSave()
        {
            await Task.Delay(SaveWaitTime, destroyToken);
            Save().Forget();
        }

        private UniTask Save()
        {
            PlayerPrefs.SetString(SaveName, JsonUtility.ToJson(SaveState));
            PlayerPrefs.Save();
            
#if UNITY_EDITOR
            SaveSOManager.Instance.Save(SaveState);
#endif
            isSaving = false;
            
            Debug.Log($"{Tag} Save done.");
            
            return UniTask.CompletedTask;
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
            Debug.Log($"{Tag} Finished loading.");
        }
        
        private string Tag => "[SaveManager] ";
    }
}
