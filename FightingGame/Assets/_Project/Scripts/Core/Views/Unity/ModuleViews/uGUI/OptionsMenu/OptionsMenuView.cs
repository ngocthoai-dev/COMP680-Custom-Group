using Core.Business;
using Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.View
{
    public class OptionsMenuView : UnityView
    {
        private GameStore _gameStore;
        private SignalBus _signalBus;
        private AudioPoolManager _audioPoolManager;

        [SerializeField] private Button _backBtn;
        [SerializeField] private Button _settingsBtn;
        [SerializeField] private Button _aboutBtn;
        [SerializeField] private Button _controlsBtn;

        [Inject]
        public void Init(
            GameStore gameStore,
            SignalBus signalBus,
            AudioPoolManager audioPoolManager)
        {
            _gameStore = gameStore;
            _signalBus = signalBus;
            _audioPoolManager = audioPoolManager;
        }

        public override void OnReady()
        {
            _backBtn.onClick.AddListener(async () =>
            {
                await _gameStore.CreateModule<IMainMenu, MainMenuModel>("", ViewName.Unity, ModuleName.MainMenu);
                _gameStore.GState.RemoveModel<OptionsMenuModel>();
            });

            _settingsBtn.onClick.AddListener(async () =>
            {
                await _gameStore.CreateModule<ISettingsMenu, SettingsMenuModel>("", ViewName.Unity, ModuleName.SettingsMenu);
                _gameStore.GState.RemoveModel<OptionsMenuModel>();
            });

            _controlsBtn.onClick.AddListener(async () =>
            {
               await _gameStore.CreateModule<IControlsMenu, ControlsMenuModel>("", ViewName.Unity, ModuleName.ControlsMenu);
                _gameStore.GState.RemoveModel<OptionsMenuModel>();
            });

            _aboutBtn.onClick.AddListener(async () =>
            {
                await _gameStore.CreateModule<IAboutMenu, AboutMenuModel>("", ViewName.Unity, ModuleName.AboutMenu);
                _gameStore.GState.RemoveModel<OptionsMenuModel>();
            });
        }

        public void Refresh()
        {
        }
    }
}