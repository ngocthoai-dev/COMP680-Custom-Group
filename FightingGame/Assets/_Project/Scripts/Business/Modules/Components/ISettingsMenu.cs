namespace Core.Business
{
    public interface ISettingsMenu : IBaseModule
    { }

    public class SettingsMenuModel : IModuleContextModel
    {
        public string ViewId { get; set; }

        public IBaseModule Module { get; set; }

        public SettingsMenuModel()
        { }

        public SettingsMenuModel(string viewId)
        {
            ViewId = viewId;
        }

        public IModuleContextModel Clone()
        {
            return new MainMenuModel(ViewId);
        }

        public void Refresh()
        {
            Module.Refresh(this);
        }

        public void CustomRefresh(string comparer)
        { }
    }
}