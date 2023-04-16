using Core.Business;

namespace Core.View
{
    public class CharacterSelectionScript : IBaseScript
    {
        public BaseViewConfig GetConfig()
        {
            return new BaseViewConfig(
                bundle: "Assets/_Project/Bundles/Prefabs/Views/uGUI/CharacterSelection/View.prefab",
                uiType: UIType.uGUI
            )
            {
                Layer = LayerManager.Main
            };
        }
    }
}