using Core.Business;

namespace Core.View
{
    public class DummyScript : IBaseScript
    {
        public BaseViewConfig GetConfig()
        {
            return new BaseViewConfig(
                bundle: "Assets/_Project/Bundles/Prefabs/Views/uGUI/Dummy/Dummy.prefab",
                uiType: UIType.uGUI
            )
            {
                Layer = LayerManager.Top
            };
        }
    }
}