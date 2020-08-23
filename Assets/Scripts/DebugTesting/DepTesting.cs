using Common.Dependencies;
using SingletonsPreloaders;
using UnityEngine;
using UnityEngine.Assertions;

namespace NunclearGame.DebugTesting
{
    public class DepTesting : MonoBehaviour
    {
        void Start()
        {
            var equipHolder = DepResolver.Resolve<EquipmentHolder>();
            var commonGui = DepResolver.Resolve<CommonGui>();
            var globalPlayer = DepResolver.Resolve<GlobalPlayer>();
            var itemCraftSystem = DepResolver.Resolve<ItemCraftSystem>();
            var itemHolder = DepResolver.Resolve<ItemHolder>();
            var itemUserManager = DepResolver.Resolve<ItemUserManager>();
            var itemVisualHolder = DepResolver.Resolve<ItemVisualHolder>();
            var loadingUI = DepResolver.Resolve<LoadingUI>();
            var metroHolder = DepResolver.Resolve<MetroHolder>();
            var sceneLoader = DepResolver.Resolve<SceneLoader>();
            var spritesHolder = DepResolver.Resolve<SpritesHolder>();
            var textLocalizer = DepResolver.Resolve<TextLocalizer>();
            
            Assert.IsNotNull(equipHolder, "equipHolder != null");
            Assert.IsNotNull(commonGui, "commonGui != null");
            Assert.IsNotNull(globalPlayer, "globalPlayer != null");
            Assert.IsNotNull(itemCraftSystem, "itemCraftSystem != null");
            Assert.IsNotNull(itemHolder, "itemHolder != null");
            Assert.IsNotNull(itemUserManager, "itemUserManager != null");
            Assert.IsNotNull(itemVisualHolder, "itemVisualHolder != null");
            Assert.IsNotNull(loadingUI, "loadingUI != null");
            Assert.IsNotNull(metroHolder, "metroHolder != null");
            Assert.IsNotNull(sceneLoader, "sceneLoader != null");
            Assert.IsNotNull(spritesHolder, "spritesHolder != null");
            Assert.IsNotNull(textLocalizer, "textLocalizer != null");
        }
    }
   
}
