using Core.Business;
using Core;
using Zenject;

namespace Core.Module
{
    public partial class BattleHUD : BaseModule, IBattleHUD
    {
        public enum ViewFunc
        {
            Refresh
        }

        private readonly ILogger _logger;
        private readonly IBundleLoader _bundleLoader;
        private readonly IScreenController _startSessionScreenController;

        public BattleHUDModel Model => _model;
        private BattleHUDModel _model;

        public BattleHUD(
            [Inject(Id = BundleLoaderName.Addressable)]
            IBundleLoader bundleLoader,
            [Inject(Id = ScreenName.SessionStart)]
            IScreenController startSessionScreenController)
        {
            _bundleLoader = bundleLoader;
            _startSessionScreenController = (StartSessionScreenController)startSessionScreenController;
        }

        protected override void OnViewReady()
        { }

        protected override void OnDisposed()
        { }

        public override void Refresh(IModuleContextModel model)
        {
            _model = (BattleHUDModel)model;
            if (_model == null) return;

            _viewContext.Call(ViewFunc.Refresh);
        }

        public override void CustomRefresh(IModuleContextModel model, string comparer)
        { }
    }
}