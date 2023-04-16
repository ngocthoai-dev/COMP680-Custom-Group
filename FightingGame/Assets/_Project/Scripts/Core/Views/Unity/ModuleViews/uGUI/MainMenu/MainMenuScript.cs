using Core.Business;

namespace Core.View
{
    public class MainMenuScript : IBaseScript
    {
        public BaseViewConfig GetConfig()
        {
            return new BaseViewConfig(
                bundle: "Assets/_Project/Bundles/Prefabs/Views/uGUI/MainMenu/MainMenu.prefab",
                uiType: UIType.uGUI
            )
            {
                Layer = LayerManager.Top
            };
        }
    }
}