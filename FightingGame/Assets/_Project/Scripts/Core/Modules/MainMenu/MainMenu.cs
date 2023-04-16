using Core.Business;
using Core;
using Zenject;

namespace Core.Module
{
    public partial class MainMenu : BaseModule, IMainMenu
    {
        public enum ViewFunc
        {
            Refresh
        }

        private readonly ILogger _logger;
        private readonly IBundleLoader _bundleLoader;
        private readonly IScreenController _startSessionScreenController;

        public MainMenuModel MainMenuModel => _mainMenuModel;
        private MainMenuModel _mainMenuModel;

        public MainMenu(
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
            _mainMenuModel = (MainMenuModel)model;
            if (_mainMenuModel == null) return;

            _viewContext.Call(ViewFunc.Refresh);
        }

        public override void CustomRefresh(IModuleContextModel model, string comparer)
        { }
    }
}