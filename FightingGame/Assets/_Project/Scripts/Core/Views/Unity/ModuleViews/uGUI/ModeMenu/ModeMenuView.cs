using Core.Business;
using Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.View
{
    public class ModeMenuView : UnityView
    {
        private GameStore _gameStore;
        private SignalBus _signalBus;
        private AudioPoolManager _audioPoolManager;

        [SerializeField] private Button _backBtn;
        [SerializeField] private Button _singlePlayerBtn;
        [SerializeField] private Button _multiplayerBtn;

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
                _gameStore.GState.RemoveModel<ModeMenuModel>();
            });

            _singlePlayerBtn.onClick.AddListener(async () =>
            {
                await _gameStore.CreateModule<ICharacterSelection, CharacterSelectionModel>("", ViewName.Unity, ModuleName.CharacterSelection);
                _gameStore.GState.RemoveModel<ModeMenuModel>();
            });

            _multiplayerBtn.onClick.AddListener(() =>
            {

                //_gameStore.GState.RemoveModel<ModeMenuModel>();
            });
        }

        public void Refresh()
        {
        }
    }
}