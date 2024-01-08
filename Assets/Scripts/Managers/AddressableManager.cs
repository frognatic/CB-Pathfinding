using Cysharp.Threading.Tasks;
using Initial;
using Managers.Singleton;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Managers
{
    public class AddressableManager : MonoSingleton<AddressableManager>
    {
        private const string AddressableLabel = "MainAssets";

        private SpritesSO sprites;

        public void AddToLoad() => InitManager.AddTaskToPhase(LoadPhase.Addressable, Initialize().ToAsyncLazy());

        private async UniTask Initialize()
        {
            await InitManager.WaitUntilPhaseStarted(LoadPhase.Addressable);
            await Addressables.LoadAssetsAsync<SpritesSO>(AddressableLabel, FillSpriteAsset).WithCancellation(destroyToken);
        }

        private void FillSpriteAsset(SpritesSO asset)
        {
            asset.Init();
            sprites = asset;
        }

        public Sprite GetSprite(string id) => sprites.GetSprite(id);
    }
}
