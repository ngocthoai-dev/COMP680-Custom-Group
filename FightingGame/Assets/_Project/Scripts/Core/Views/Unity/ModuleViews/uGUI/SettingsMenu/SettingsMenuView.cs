using Core.Business;
using Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.View
{
    public class SettingsMenuView : UnityView
    {
        private GameStore _gameStore;
        private SignalBus _signalBus;
        private AudioPoolManager _audioPoolManager;

        [SerializeField] private Button _backBtn;

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
                await _gameStore.CreateModule<IOptionsMenu, OptionsMenuModel>("", ViewName.Unity, ModuleName.OptionsMenu);
                _gameStore.GState.RemoveModel<SettingsMenuModel>();
            });
        }

        public void Refresh()
        {
        }
    }
}