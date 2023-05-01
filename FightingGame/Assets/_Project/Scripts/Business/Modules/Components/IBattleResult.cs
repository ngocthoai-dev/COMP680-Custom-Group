namespace Core.Business
{
    public interface IBattleResult : IBaseModule
    { }

    public class BattleResultModel : IModuleContextModel
    {
        public string ViewId { get; set; }

        public IBaseModule Module { get; set; }

        public bool IsWin { get; private set; }

        public BattleResultModel()
        { }

        public BattleResultModel(string viewId)
        {
            ViewId = viewId;
        }

        public IModuleContextModel Clone()
        {
            return new BattleResultModel(ViewId);
        }

        public void Refresh()
        {
            Module.Refresh(this);
        }

        public BattleResultModel SetResult(bool isWin)
        {
            IsWin = isWin;
            return this;
        }

        public void CustomRefresh(string comparer)
        { }
    }
}