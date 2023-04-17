namespace Core.Business
{
    public interface IModeMenu : IBaseModule
    { }

    public class ModeMenuModel : IModuleContextModel
    {
        public string ViewId { get; set; }

        public IBaseModule Module { get; set; }

        public ModeMenuModel()
        { }

        public ModeMenuModel(string viewId)
        {
            ViewId = viewId;
        }

        public IModuleContextModel Clone()
        {
            return new ModeMenuModel(ViewId);
        }

        public void Refresh()
        {
            Module.Refresh(this);
        }

        public void CustomRefresh(string comparer)
        { }
    }
}