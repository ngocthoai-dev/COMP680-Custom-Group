using Core.Business;

namespace Core.View
{
    public class SettingsMenuScript : IBaseScript
    {
        public BaseViewConfig GetConfig()
        {
            return new BaseViewConfig(
                bundle: "Assets/_Project/Bundles/Prefabs/Views/uGUI/SettingsMenu/SettingsMenu.prefab",
                uiType: UIType.uGUI
            )
            {
                Layer = LayerManager.Top
            };
        }
    }
}