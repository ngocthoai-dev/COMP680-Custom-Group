using Core.Business;

namespace Core.View
{
    public class ControlsMenuScript : IBaseScript
    {
        public BaseViewConfig GetConfig()
        {
            return new BaseViewConfig(
                bundle: "Assets/_Project/Bundles/Prefabs/Views/uGUI/ControlsMenu/ControlsMenu.prefab",
                uiType: UIType.uGUI
            )
            {
                Layer = LayerManager.Top
            };
        }
    }
}