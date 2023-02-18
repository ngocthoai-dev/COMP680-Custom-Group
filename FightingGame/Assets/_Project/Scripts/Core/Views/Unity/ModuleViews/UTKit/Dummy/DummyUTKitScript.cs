using Core.Business;

namespace Core.View
{
    public class DummyUTKitScript : IBaseScript
    {
        public BaseViewConfig GetConfig()
        {
            return new BaseViewConfig(
                bundle: "Assets/_Project/Bundles/Prefabs/Views/UIElements/Dummy/Dummy.prefab",
                uiType: UIType.UIToolkit
            )
            {
                Config = ""
            };
        }
    }
}