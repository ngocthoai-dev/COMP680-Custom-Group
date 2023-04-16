using Core.Business;
using Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.View
{
    public class CharacterToggleMenuView : UnityView
    {
        private GameStore _gameStore;
        private SignalBus _signalBus;
        private AudioPoolManager _audioPoolManager;

        [SerializeField] private Button _backBtn;
        [SerializeField] private Button _startBtn;
        [SerializeField] private ToggleGroup _characterToggle;

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
                await _gameStore.CreateModule<IModeMenu, ModeMenuModel>("", ViewName.Unity, ModuleName.ModeMenu);
                _gameStore.GState.RemoveModel<CharacterToggleMenuModel>();
            });

            _startBtn.onClick.AddListener(() =>
            {

                //_gameStore.GState.RemoveModel<CharacterToggleMenuModel>();
            });
        }

        public void Refresh()
        {
        }
    }
}