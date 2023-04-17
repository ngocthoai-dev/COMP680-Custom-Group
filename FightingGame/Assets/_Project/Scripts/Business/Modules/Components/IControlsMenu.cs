namespace Core.Business
{
    public interface IControlsMenu : IBaseModule
    { }

    public class ControlsMenuModel : IModuleContextModel
    {
        public string ViewId { get; set; }

        public IBaseModule Module { get; set; }

        public ControlsMenuModel()
        { }

        public ControlsMenuModel(string viewId)
        {
            ViewId = viewId;
        }

        public IModuleContextModel Clone()
        {
            return new ControlsMenuModel(ViewId);
        }

        public void Refresh()
        {
            Module.Refresh(this);
        }

        public void CustomRefresh(string comparer)
        { }
    }
}