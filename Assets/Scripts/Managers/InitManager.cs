using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Initial;
using Managers.Singleton;
using UnityEngine;

namespace Managers
{
    public class InitManager : MonoSingleton<InitManager>
    {
        public static LoadPhase CurrentPhase { get; private set; }
        public static LoadPhase LastPhase    { get; private set; }
        
        private static readonly Dictionary<LoadPhase, List<AsyncLazy>> initTaskDictionary       = new();
        private static readonly Dictionary<LoadPhase, AsyncLazy> StartTaskDictionary = new();   
        
        public async UniTaskVoid Init()
        {
            AddressablesManager.Instance.Init();
            SaveManager.Instance.Init();
            WindowManager.Instance.Init();
            PathfindingManager.Instance.Init();
            MovingUnitsManager.Instance.Init();

            LoadPhase[] allPhases = (LoadPhase[])Enum.GetValues(typeof(LoadPhase));
            foreach (var phase in allPhases)
            {
                CurrentPhase = phase;
                if (initTaskDictionary.TryGetValue(phase, out var tasks))
                {
                    if (tasks == null)
                        continue;
                    foreach (var task in tasks)
                        await task;
                }

                LastPhase = phase;
            }
        }

        public static AsyncLazy WaitUntilPhaseStarted(LoadPhase phase)
        {
            if (StartTaskDictionary.TryGetValue(phase, out var task))
                return task;
            return StartTaskDictionary[phase] =
                UniTask.WaitUntil(() => CurrentPhase >= phase, cancellationToken: Instance.destroyCancellationToken).ToAsyncLazy();
        }
        
        public static void AddTaskToPhase(LoadPhase loadPhase, AsyncLazy task)
        {
            initTaskDictionary.TryAdd(loadPhase, new());
            initTaskDictionary[loadPhase].Add(task);
        }
    }
}
