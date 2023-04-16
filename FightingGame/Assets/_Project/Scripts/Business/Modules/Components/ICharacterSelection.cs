namespace Core.Business
{
    public interface ICharacterSelection : IBaseModule
    { }

    public class CharacterSelectionModel : IModuleContextModel
    {
        public string ViewId { get; set; }

        public IBaseModule Module { get; set; }

        public CharacterSelectionModel()
        { }

        public CharacterSelectionModel(string viewId)
        {
            ViewId = viewId;
        }

        public IModuleContextModel Clone()
        {
            return new CharacterSelectionModel(ViewId);
        }

        public void Refresh()
        {
            Module.Refresh(this);
        }

        public void CustomRefresh(string comparer)
        { }
    }
}