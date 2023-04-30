using Core.Business;
using Core;
using Zenject;

namespace Core.Module
{
    public partial class AboutMenu : BaseModule, IAboutMenu
    {
        public enum ViewFunc
        {
            Refresh
        }

        private readonly ILogger _logger;
        private readonly IBundleLoader _bundleLoader;
        private readonly IScreenController _startSessionScreenController;

        public AboutMenuModel AboutMenuModel => _aboutMenuModel;
        private AboutMenuModel _aboutMenuModel;

        public AboutMenu(
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
            _aboutMenuModel = (AboutMenuModel)model;
            if (_aboutMenuModel == null) return;

            _viewContext.Call(ViewFunc.Refresh);
        }

        public override void CustomRefresh(IModuleContextModel model, string comparer)
        { }
    }
}