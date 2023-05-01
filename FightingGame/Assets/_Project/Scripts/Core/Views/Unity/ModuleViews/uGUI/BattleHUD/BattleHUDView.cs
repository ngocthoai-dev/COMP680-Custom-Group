using Core.Business;
using Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;
using Core.Module;
using Core.EventSignal;
using Core.GGPO;
using System;

namespace Core.View
{
    [Serializable]
    public class CharacterView
    {
        public Image avatarImg;
        public Image hpFill;
        public TextMeshProUGUI hpTxt;
        public Image mpFill;
        public TextMeshProUGUI mpTxt;

        public CharacterView GetReferences(Transform transform)
        {
            avatarImg = transform.Find("Avatar/BG/Image").GetComponent<Image>();
            hpFill = transform.Find("HP/Process/Fill").GetComponent<Image>();
            hpTxt = transform.Find("HP/Text").GetComponent<TextMeshProUGUI>();
            mpFill = transform.Find("MP/Process/Fill").GetComponent<Image>();
            mpTxt = transform.Find("MP/Text").GetComponent<TextMeshProUGUI>();
            return this;
        }

        public void Refresh(NetworkCharacter data)
        {
            avatarImg.sprite = data.CharacterConfigSO.Thumbnail;

            var hp = data.CharacterController.CharacterStats.GetStats(SO.StatType.HP).Value;
            var hpMax = data.CharacterController.CharacterConfigSO.CharacterStats.GetStats(SO.StatType.HP).Value;
            hp = Mathf.Clamp(hp, 0, hpMax);
            hpFill.fillAmount = hp / hpMax;
            hpTxt.text = $"{Mathf.Ceil(hp)}/{hpMax}";

            var mp = data.CharacterController.CharacterStats.GetStats(SO.StatType.MP).Value;
            var mpMax = data.CharacterController.CharacterConfigSO.CharacterStats.GetStats(SO.StatType.MP).Value;
            mp = Mathf.Clamp(mp, 0, mpMax);
            mpFill.fillAmount = mp / mpMax;
            mpTxt.text = $"{Mathf.Ceil(mp)}/{mpMax}";
        }
    }

    public class BattleHUDView : UnityView
    {
        private GameStore _gameStore;
        private SignalBus _signalBus;
        private AudioPoolManager _audioPoolManager;

        [SerializeField] private CharacterView[] _characterViews = new CharacterView[2];

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
            _characterViews[0] = new CharacterView().GetReferences(transform.Find("Char1"));
            _characterViews[1] = new CharacterView().GetReferences(transform.Find("Char2"));

            _signalBus.Subscribe<OnSyncBattleHUD>(SyncBattle);
            _signalBus.Subscribe<OnEndBattle>(OnEndBattle);
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<OnSyncBattleHUD>(SyncBattle);
            _signalBus.Unsubscribe<OnEndBattle>(OnEndBattle);
        }

        private void SyncBattle(OnSyncBattleHUD signal)
        {
            _characterViews[0].Refresh(signal.NetworkCharacters[0]);
            _characterViews[1].Refresh(signal.NetworkCharacters[1]);
        }

        private async void OnEndBattle(OnEndBattle signal)
        {
            _gameStore.GState.RemoveModel<BattleHUDModel>();
            await _gameStore.CreateModule<IBattleResult, BattleResultModel>(
                "", ViewName.Unity, ModuleName.BattleResult);
            _gameStore.GState.GetModel<BattleResultModel>().SetResult(signal.IsWin).Refresh();
        }

        public void Refresh()
        {
        }
    }
}