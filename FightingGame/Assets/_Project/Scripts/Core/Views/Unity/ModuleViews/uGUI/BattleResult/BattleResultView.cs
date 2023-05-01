using Core.Business;
using Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Core.Utility;
using TMPro;
using Core.Module;
using Core.Extension;

namespace Core.View
{
    public class BattleResultView : UnityView
    {
        private GameStore _gameStore;
        private SignalBus _signalBus;
        private AudioPoolManager _audioPoolManager;

        [SerializeField][DebugOnly] private TextMeshProUGUI _victoryTxt;
        [SerializeField][DebugOnly] private TextMeshProUGUI _defeatTxt;
        [SerializeField][DebugOnly] private Button _retryBtn;
        [SerializeField][DebugOnly] private Button _mmBtn;

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

        private void GetReferences()
        {
            _victoryTxt = transform.Find("VictoryText").GetComponent<TextMeshProUGUI>();
            _defeatTxt = transform.Find("DefeatText").GetComponent<TextMeshProUGUI>();
            _retryBtn = transform.Find("Retry_Btn").GetComponent<Button>();
            _mmBtn = transform.Find("MM_Btn").GetComponent<Button>();
        }

        public override void OnReady()
        {
            GetReferences();

            _retryBtn.onClick.AddListener(async () =>
            {
                await _gameStore.CreateModule<ICharacterSelection, CharacterSelectionModel>("", ViewName.Unity, ModuleName.CharacterSelection);
                _gameStore.GState.RemoveModel<BattleResultModel>();
            });

            _mmBtn.onClick.AddListener(async () =>
            {
                await _gameStore.CreateModule<IMainMenu, MainMenuModel>("", ViewName.Unity, ModuleName.MainMenu);
                _gameStore.GState.RemoveModel<BattleResultModel>();
            });
        }

        public void Refresh()
        {
            bool isWin = ((BattleResult)_module).Model.IsWin;
            _victoryTxt.SetActive(isWin);
            _defeatTxt.SetActive(!isWin);
        }
    }
}