using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Managers.Singleton;
using UnityEngine.AddressableAssets;

namespace Managers
{
    public class AddressableManager : MonoSingleton<AddressableManager>
    {
        private const string AddressableLabel = "MainAssets";
        private UnitDetailsSO unitDetails;
        
        public async UniTask Initialize() => await Addressables.LoadAssetsAsync<UnitDetailsSO>(AddressableLabel, FillUnitDetailsAsset).WithCancellation(destroyToken);

        private void FillUnitDetailsAsset(UnitDetailsSO asset)
        {
            asset.Init();
            unitDetails = asset;
        }

        public UnitDetails GetUnitDetails(string id) => unitDetails.GetUnitDetails(id);
        public List<UnitDetails> GetUnitDetailsList() => unitDetails.GetUnitDetailsList();
    }
}
