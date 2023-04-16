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
        [SerializeField] private Button _rulesBtn;

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

            _rulesBtn.onClick.AddListener(() =>
            {
                //_gameStore.GState.RemoveModel<OptionsMenuModel>();
            });
        }

        public void Refresh()
        {
        }
    }
}