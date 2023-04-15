using Core.Business;

namespace Core.View
{
    public class ModeMenuScript : IBaseScript
    {
        public BaseViewConfig GetConfig()
        {
            return new BaseViewConfig(
                bundle: "Assets/_Project/Bundles/Prefabs/Views/uGUI/ModeMenu/ModeMenu.prefab",
                uiType: UIType.uGUI
            )
            {
                Layer = LayerManager.Top
            };
        }
    }
}