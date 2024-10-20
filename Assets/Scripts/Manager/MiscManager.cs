namespace UserData.Controller
{
    using Blueprints;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using UnityEngine;

    public class MiscManager
    {
        private readonly MiscBlueprint miscBlueprint;
        private readonly IGameAssets   gameAssets;
        
        public MiscManager(MiscBlueprint miscBlueprint, IGameAssets gameAssets)
        {
            this.miscBlueprint = miscBlueprint;
            this.gameAssets    = gameAssets;
        }
        
        public async UniTask<Sprite> GetIcon(string id)
        {
            return await gameAssets.LoadAssetAsync<Sprite>(id);
        }
    }
}