using Core.Business;
using Core;
using Zenject;

namespace Core.Module
{
    public partial class SettingsMenu : BaseModule, ISettingsMenu
    {
        public enum ViewFunc
        {
            Refresh
        }

        private readonly ILogger _logger;
        private readonly IBundleLoader _bundleLoader;
        private readonly IScreenController _startSessionScreenController;

        public SettingsMenuModel SettingsMenuModel => _settingsMenuModel;
        private SettingsMenuModel _settingsMenuModel;

        public SettingsMenu(
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
            _settingsMenuModel = (SettingsMenuModel)model;
            if (_settingsMenuModel == null) return;

            _viewContext.Call(ViewFunc.Refresh);
        }

        public override void CustomRefresh(IModuleContextModel model, string comparer)
        { }
    }
}