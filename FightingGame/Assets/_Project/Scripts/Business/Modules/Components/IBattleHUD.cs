namespace Core.Business
{
    public interface IBattleHUD : IBaseModule
    { }

    public class BattleHUDModel : IModuleContextModel
    {
        public string ViewId { get; set; }

        public IBaseModule Module { get; set; }

        public BattleHUDModel()
        { }

        public BattleHUDModel(string viewId)
        {
            ViewId = viewId;
        }

        public IModuleContextModel Clone()
        {
            return new BattleHUDModel(ViewId);
        }

        public void Refresh()
        {
            Module.Refresh(this);
        }

        public void CustomRefresh(string comparer)
        { }
    }
}