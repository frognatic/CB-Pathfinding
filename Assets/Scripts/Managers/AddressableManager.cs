using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Initial;
using Managers.Singleton;
using UnityEngine.AddressableAssets;

namespace Managers
{
    public class AddressableManager : MonoSingleton<AddressableManager>
    {
        private const string AddressableLabel = "MainAssets";

        private UnitDetailsSO unitDetails;

        public void AddToLoad() => InitManager.AddTaskToPhase(LoadPhase.Addressable, Initialize().ToAsyncLazy());

        private async UniTask Initialize()
        {
            await InitManager.WaitUntilPhaseStarted(LoadPhase.Addressable);
            await Addressables.LoadAssetsAsync<UnitDetailsSO>(AddressableLabel, FillUnitDetailsAsset).WithCancellation(destroyToken);
        }

        private void FillUnitDetailsAsset(UnitDetailsSO asset)
        {
            asset.Init();
            unitDetails = asset;
        }

        public UnitDetails GetUnitDetails(string id) => unitDetails.GetUnitDetails(id);
        public List<UnitDetails> GetUnitDetailsList() => unitDetails.GetUnitDetailsList();
    }
}
