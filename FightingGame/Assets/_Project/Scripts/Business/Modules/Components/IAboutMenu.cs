namespace Core.Business
{
    public interface IAboutMenu : IBaseModule
    { }

    public class AboutMenuModel : IModuleContextModel
    {
        public string ViewId { get; set; }

        public IBaseModule Module { get; set; }

        public AboutMenuModel()
        { }

        public AboutMenuModel(string viewId)
        {
            ViewId = viewId;
        }

        public IModuleContextModel Clone()
        {
            return new AboutMenuModel(ViewId);
        }

        public void Refresh()
        {
            Module.Refresh(this);
        }

        public void CustomRefresh(string comparer)
        { }
    }
}