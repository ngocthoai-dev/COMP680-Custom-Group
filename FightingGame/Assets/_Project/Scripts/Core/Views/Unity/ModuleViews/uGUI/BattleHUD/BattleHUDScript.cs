using Core.Business;

namespace Core.View
{
    public class BattleHUDScript : IBaseScript
    {
        public BaseViewConfig GetConfig()
        {
            return new BaseViewConfig(
                bundle: "Assets/_Project/Bundles/Prefabs/Views/uGUI/BattleHUD/View.prefab",
                uiType: UIType.uGUI
            )
            {
                Layer = LayerManager.Main
            };
        }
    }
}