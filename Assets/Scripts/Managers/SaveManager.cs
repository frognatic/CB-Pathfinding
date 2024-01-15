using System;
using System.IO;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Managers.Singleton;
using UnityEngine;

namespace Managers
{
    public class SaveManager : MonoSingleton<SaveManager>
    {
        public static event Action<bool> SavingAction;

        private const string SaveFileName = "/Save.txt";
        private const string SaveName = "GameSave";
        private const int SaveWaitTime = 2000;

        private bool isSaving;
        private bool isLoading;

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

            SavingAction?.Invoke(true);
            isSaving = true;
            WaitForSave().Forget();
        }

        private async UniTask WaitForSave()
        {
            await Task.Delay(SaveWaitTime, destroyToken);
            Save().Forget();
            SavingAction?.Invoke(false);
        }

        private async UniTask Save()
        {
            string jsonString = JsonUtility.ToJson(SaveState);

            PlayerPrefs.SetString(SaveName, jsonString);
            PlayerPrefs.Save();

#if UNITY_EDITOR
            SaveSOManager.Instance.Save(SaveState);
#endif

            await SaveToFile(jsonString);

            isSaving = false;
            Debug.Log($"{Tag} Save done.");
        }

        private async UniTask SaveToFile(string jsonText) =>
            await File.WriteAllTextAsync(Application.dataPath + "/Save.txt", jsonText);

        public void Load()
        {
            if (HasSave())
            {
                if (IsLoadingFromFile())
                    LoadFromFile();
                else
                    LoadFromPrefs();
            }
            else
                SaveState = new SaveState();

            SaveSOManager.Instance.Load();
            Debug.Log($"{Tag} Finished loading.");
        }
        
        public bool HasSave() => HasSave(IsLoadingFromFile());
        
        private bool IsLoadingFromFile()
        {
            bool isFromFile;

#if LOAD_SAVE_FROM_FILE
            isFromFile = true;
#else
            isFromFile = false;
#endif

            return isFromFile;
        }

        private bool HasSave(bool isFromFile) =>
            isFromFile ? File.Exists(Application.dataPath + "/Save.txt") : PlayerPrefs.HasKey(SaveName);

        private void LoadFromPrefs()
        {
            string json = PlayerPrefs.GetString(SaveName);
            SaveState result = !string.IsNullOrEmpty(json) ? JsonUtility.FromJson<SaveState>(json) : null;
            SaveState = result;
        }

        private void LoadFromFile()
        {
            string json = File.ReadAllText(Application.dataPath + "/Save.txt");
            SaveState result = !string.IsNullOrEmpty(json) ? JsonUtility.FromJson<SaveState>(json) : null;
            SaveState = result;
        }

        private string Tag => "[SaveManager] ";
    }
}