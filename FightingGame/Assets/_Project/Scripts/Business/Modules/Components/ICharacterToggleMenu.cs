namespace Core.Business
{
    public interface ICharacterToggleMenu : IBaseModule
    { }

    public class CharacterToggleMenuModel : IModuleContextModel
    {
        public string ViewId { get; set; }

        public IBaseModule Module { get; set; }

        public CharacterToggleMenuModel()
        { }

        public CharacterToggleMenuModel(string viewId)
        {
            ViewId = viewId;
        }

        public IModuleContextModel Clone()
        {
            return new CharacterToggleMenuModel(ViewId);
        }

        public void Refresh()
        {
            Module.Refresh(this);
        }

        public void CustomRefresh(string comparer)
        { }
    }
}