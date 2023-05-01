using Core.Business;
using Core.EventSignal;
using Core.Gameplay;
using Core.Module;
using Core.View;
using System.Diagnostics;
using UnityEngine;
using Zenject;

namespace Core
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Application.targetFrameRate = 60;

            Container.BindInterfacesAndSelfTo<GameStore>().AsSingle();

            GameRootInstaller.Install(Container);
            BussinessInstaller.Install(Container);
            NetworkInstaller.Install(Container);

            UnityEngine.Debug.LogFormat("GameInstaller took {0:0.00} seconds", stopwatch.Elapsed.TotalSeconds);
            stopwatch.Stop();
        }

        private void OnApplicationQuit()
        {
            Container.Resolve<SignalBus>().Fire(new OnApplicationQuitSignal());
        }
    }

    public class GameRootInstaller : Installer<GameRootInstaller>
    {
        public override void InstallBindings()
        {
            InstallGameModuleState();
            InstallModules();
            InstallServices();
            InstallShareLogger();
            InstallFactories();
            InstallGameSignal();
            InstallUtilities();
#if UNITY_EDITOR
            Container.BindInterfacesTo<PoolCleanupChecker>().AsSingle();
#endif
        }

        private void InstallGameModuleState()
        {
            Container.Bind<IScreenController>().WithId(ScreenName.SessionStart).To<StartSessionScreenController>().AsSingle();
        }

        private void InstallModules()
        {
            Container.BindFactory<IBaseModule, BaseModule.Factory>()
                .WithId(ModuleName.Dummy).To<IDummy>()
                .FromSubContainerResolve()
                .ByInstaller<Dummy.Installer>();

            Container.BindFactory<IBaseModule, BaseModule.Factory>()
                .WithId(ModuleName.DummyUTKit).To<IDummyUTKit>()
                .FromSubContainerResolve()
                .ByInstaller<DummyUTKit.Installer>();

            Container.BindFactory<IBaseModule, BaseModule.Factory>()
                .WithId(ModuleName.MainMenu).To<IMainMenu>()
                .FromSubContainerResolve()
                .ByInstaller<MainMenu.Installer>();

            Container.BindFactory<IBaseModule, BaseModule.Factory>()
                .WithId(ModuleName.ModeMenu).To<IModeMenu>()
                .FromSubContainerResolve()
                .ByInstaller<ModeMenu.Installer>();

            Container.BindFactory<IBaseModule, BaseModule.Factory>()
                .WithId(ModuleName.CharacterToggleMenu).To<ICharacterToggleMenu>()
                .FromSubContainerResolve()
                .ByInstaller<CharacterToggleMenu.Installer>();

            Container.BindFactory<IBaseModule, BaseModule.Factory>()
                .WithId(ModuleName.OptionsMenu).To<IOptionsMenu>()
                .FromSubContainerResolve()
                .ByInstaller<OptionsMenu.Installer>();

            Container.BindFactory<IBaseModule, BaseModule.Factory>()
                .WithId(ModuleName.SettingsMenu).To<ISettingsMenu>()
                .FromSubContainerResolve()
                .ByInstaller<SettingsMenu.Installer>();

            Container.BindFactory<IBaseModule, BaseModule.Factory>()
                .WithId(ModuleName.CharacterSelection).To<ICharacterSelection>()
                .FromSubContainerResolve()
                .ByInstaller<CharacterSelection.Installer>();

            Container.BindFactory<IBaseModule, BaseModule.Factory>()
                .WithId(ModuleName.AboutMenu).To<IAboutMenu>()
                .FromSubContainerResolve()
                .ByInstaller<AboutMenu.Installer>();

            Container.BindFactory<IBaseModule, BaseModule.Factory>()
                .WithId(ModuleName.ControlsMenu).To<IControlsMenu>()
                .FromSubContainerResolve()
                .ByInstaller<ControlsMenu.Installer>();

            Container.BindFactory<IBaseModule, BaseModule.Factory>()
                .WithId(ModuleName.BattleHUD).To<IBattleHUD>()
                .FromSubContainerResolve()
                .ByInstaller<BattleHUD.Installer>();

            Container.BindFactory<IBaseModule, BaseModule.Factory>()
                .WithId(ModuleName.BattleResult).To<IBattleResult>()
                .FromSubContainerResolve()
                .ByInstaller<BattleResult.Installer>();
        }

        private void InstallServices()
        {
            Container.Bind<IBundleLoader>().WithId(BundleLoaderName.Resource).To<ResourceLoader>().AsSingle();
            Container.Bind<IBundleLoader>().WithId(BundleLoaderName.Addressable).To<AddressableLoader>().AsSingle();
        }

        private void InstallShareLogger()
        {
            Container.BindInterfacesTo<UnityDebugLogger>().AsSingle();
            Container.Bind<ErrorHandler>().AsSingle().NonLazy();
        }

        private void InstallFactories()
        {
            Container.BindInterfacesAndSelfTo<ViewCustomFactory>().AsSingle();

            Container.Bind<UnityViewScriptManager<UnityViewScript>>().AsSingle();
            Container.BindFactory<string, IBaseScript, UnityViewScript, UnityBaseScript.Factory<UnityViewScript>>();
            Container.BindFactory<string, IViewContext, BaseViewContext.Factory>().WithId(ViewName.Unity).To<UnityViewContext>();
            Container.BindFactory<UnityViewScript, GameObject, string, UnityView, UnityView.Factory>().FromFactory<UnityView.CustomFactory>();
        }

        private void InstallGameSignal()
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignalWithInterfaces<GameActionSignal<IModuleContextModel>>().OptionalSubscriber();

            Container.DeclareSignal<GameScreenChangeSignal>().OptionalSubscriber();
            Container.DeclareSignal<GameScreenForceChangeSignal>().OptionalSubscriber();

            Container.DeclareSignal<CheckDownloadSizeStatusSignal>().OptionalSubscriber();
            Container.DeclareSignal<UpdateLoadingProgressSignal>().OptionalSubscriber();
            Container.DeclareSignal<AddressableErrorSignal>().OptionalSubscriber();

            Container.DeclareSignal<GameAudioSignal>().OptionalSubscriber();
            Container.DeclareSignal<PlayOneShotAudioSignal>().OptionalSubscriber();
            Container.DeclareSignal<OnSyncBattleHUD>().OptionalSubscriber();
            Container.DeclareSignal<OnEndBattle>().OptionalSubscriber();

            Container.DeclareSignal<OnApplicationQuitSignal>().OptionalSubscriber();
        }

        private void InstallUtilities()
        {
            Container.BindInterfacesTo<JsonFileReader>().AsSingle();
            Container.Bind<AtlasManager>().AsSingle();
        }
    }

    public class BussinessInstaller : Installer<BussinessInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IPoolManager>().WithId(PoolName.Object).To<PoolManager>().AsSingle();
            Container.Bind<AudioPoolManager>().AsSingle();
            Container.Bind<PlayerPrefManager>().AsSingle();
            Container.Bind<GamePresenter>().AsSingle();
            Container.Bind<SoundDataLoader>().AsSingle();
            Container.BindInterfacesTo<DefinitionManager>().AsSingle();

#if UNITY_EDITOR
            Container.BindInterfacesTo<DefinitionLoader>().AsSingle();
            Container.Bind<IDefinitionLoader>().WithId(DefinitionLocation.Remote).To<RemoteDefinitionLoader>().AsSingle();
#else
            Container.BindInterfacesTo<RemoteDefinitionLoader>().AsSingle();
#endif
        }
    }

    public class NetworkInstaller : Installer<NetworkInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<DefinitionDataController>().AsSingle();
        }
    }
}