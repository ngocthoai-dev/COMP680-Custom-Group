using Core.Business;
using Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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
                //_gameStore.GState.RemoveModel<DummyModel>();
            });

            _playBtn.onClick.AddListener(() =>
            {

            });

            _optionsBtn.onClick.AddListener(() =>
            {

            });
        }

        public void Refresh()
        {
        }
    }
}