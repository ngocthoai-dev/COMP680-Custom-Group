using Core.Business;
using Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Core.EventSignal;

namespace Core.View
{
    public class MainMenuView : UnityView
    {
        private GameStore _gameStore;
        private SignalBus _signalBus;
        private AudioPoolManager _audioPoolManager;

        [SerializeField] private Button _quitBtn;
        [SerializeField] private Button _playBtn;
        [SerializeField] private Button _optionsBtn;

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
            _quitBtn.onClick.AddListener(() =>
            {
                Application.Quit();
                UnityEditor.EditorApplication.isPlaying = false;
            });

            _playBtn.onClick.AddListener(async () =>
            {
                await _gameStore.CreateModule<IModeMenu, ModeMenuModel>("", ViewName.Unity, ModuleName.ModeMenu);
                _gameStore.GState.RemoveModel<MainMenuModel>();
            });

            _optionsBtn.onClick.AddListener(async () =>
            {
                await _gameStore.CreateModule<IOptionsMenu, OptionsMenuModel>("", ViewName.Unity, ModuleName.OptionsMenu);
                _gameStore.GState.RemoveModel<MainMenuModel>();
            });
        }

        public void Refresh()
        {
        }
    }
}