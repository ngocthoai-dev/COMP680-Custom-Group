using Core.Business;
using Core.Extension;
using Core.GGPO;
using Core.SO;
using Core.Utility;
using Cysharp.Threading.Tasks;
using Network.UnityGGPO;
using Shared.Extension;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Core.View
{
    public class CharacterSelectionView : UnityView
    {
        private GameStore _gameStore;
        private SignalBus _signalBus;
        private AudioPoolManager _audioPoolManager;
        private ZenjectSceneLoader _sceneLoader;

        [SerializeField] private CharacterConfigSO[] _availableChars;
        [SerializeField][DebugOnly] private CharacterConfigSO[] _copyAvailableChars;
        [SerializeField][DebugOnly] private StatType[] _availableStats;

        [SerializeField][DebugOnly] private Button _backBtn;

        [Header("Char Views")]
        [SerializeField][DebugOnly] private CharView[] _charViews;

        [Header("Char Stats")]
        [SerializeField][DebugOnly] private Transform _charStatsContainer;

        [SerializeField][DebugOnly] private StatsView[] _statsView;

        [Header("List Chars")]
        [SerializeField][DebugOnly] private Transform _charListContainer;

        [SerializeField][DebugOnly] private SelectableCharView[] _selectableCharViews;

        [Header("Dynamic")]
        [SerializeField][DebugOnly] private int[] _selectedCharIdxes;

        [SerializeField][DebugOnly] private bool[] _isReadys;

        public static bool IsPlayer(int playerIdx) => playerIdx == 0;

        [Inject]
        public void Init(
            GameStore gameStore,
            SignalBus signalBus,
            AudioPoolManager audioPoolManager,
            ZenjectSceneLoader sceneLoader)
        {
            _gameStore = gameStore;
            _signalBus = signalBus;
            _audioPoolManager = audioPoolManager;
            _sceneLoader = sceneLoader;
        }

        private void GetReferences()
        {
            _backBtn = transform.Find("Back_Btn").GetComponent<Button>();

            _charViews = new CharView[]
            {
                new CharView(transform.Find("Container/CharView/Char1")),
                new CharView(transform.Find("Container/CharView/Char2"))
            };
            _selectedCharIdxes = new int[2];
            _isReadys = new bool[2];

            _availableStats = Enum.GetValues(typeof(StatType)).Cast<StatType>()
                .Where(val => val > StatType.NONE && val < StatType.Total).ToArray();
            _charStatsContainer = transform.Find("Container/CharStats/Scroll View/Viewport/Content");
            _statsView = new StatsView[_availableStats.Length];
            var template = _charStatsContainer.GetChild(0);
            for (int idx = 0; idx < _availableStats.Length; idx++)
            {
                _statsView[idx] = new StatsView(_charStatsContainer.GetChild(idx));
                if (_charStatsContainer.childCount >= _availableStats.Length) break;
                Instantiate(template, _charStatsContainer);
            }

            _charListContainer = transform.Find("Container/ListChar/List");
            _selectableCharViews = new SelectableCharView[_copyAvailableChars.Length];
            template = _charListContainer.GetChild(0);
            for (int idx = 0; idx < _copyAvailableChars.Length; idx++)
            {
                _selectableCharViews[idx] = new SelectableCharView(_charListContainer.GetChild(idx))
                    .ApplyData(_copyAvailableChars[idx]);
                if (_charListContainer.childCount >= _copyAvailableChars.Length) break;
                Instantiate(template, _charListContainer);
            }
        }

        private void OnSelectChar(int playerIdx, int charIdx)
        {
            _charViews[playerIdx].ApplyData(_copyAvailableChars[charIdx]);
            //if (!IsPlayer(playerIdx)) return;

            var charConfig = _copyAvailableChars[charIdx];
            for (int idx = 0; idx < _statsView.Length; idx++)
                _statsView[idx].ApplyData(_availableStats[idx], charConfig);

            _selectableCharViews[_selectedCharIdxes[playerIdx]].OnSelect(playerIdx, false);
            _selectableCharViews[charIdx].OnSelect(playerIdx, true);
            _selectedCharIdxes[playerIdx] = charIdx;
        }

        private void InitState()
        {
            _charViews[0].SetHost();
            _charViews[1].ReadyTxt.SetActive(true);
            _charViews[1].ReadyBtn.interactable = false;

            OnSelectChar(0, 0);
        }

        private void RegisterEvents()
        {
            _charViews[0].ReadyBtn.onClick.AddListener(async () =>
            {
                if (_isReadys[1])
                {
                    _gameStore.GState.RemoveModel<CharacterSelectionModel>();
                    ((NetworkManager)GameManager.Instance).PreStartGame(new CharacterConfigSO[]
                    {
                        _copyAvailableChars[_selectedCharIdxes[0]],
                        _copyAvailableChars[_selectedCharIdxes[1]]
                    })
                    .StartLocalGame();
                    await _gameStore.CreateModule<IBattleHUD, BattleHUDModel>(
                        "", ViewName.Unity, ModuleName.BattleHUD);
                }
            });
        }

        private IEnumerator AISelectChar()
        {
            int loopTime = UnityEngine.Random.Range(5, 15);
            _isReadys[1] = true;
            while (loopTime > 0)
            {
                loopTime--;
                OnSelectChar(1, UnityEngine.Random.Range(0, _copyAvailableChars.Length));
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        public override void OnReady()
        {
            _copyAvailableChars = _availableChars.Select(ele => ele.Clone()).ToArray();
            GetReferences();
            InitState();

            //_backBtn.onClick.AddListener(() =>
            //{
            //    _gameStore.GState.RemoveModel<CharacterSelectionModel>();
            //});

            RegisterEvents();
            StartCoroutine(AISelectChar());
        }

        public void Refresh()
        {
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
            {
                int newIdx = _selectedCharIdxes[0];
                if (Input.GetKeyUp(KeyCode.LeftArrow))
                    newIdx = Math.Clamp(_selectedCharIdxes[0] - 1, 0, _copyAvailableChars.Length - 1);
                if (Input.GetKeyUp(KeyCode.RightArrow))
                    newIdx = Math.Clamp(_selectedCharIdxes[0] + 1, 0, _copyAvailableChars.Length - 1);
                OnSelectChar(0, newIdx);
            }
        }

        [Serializable]
        public class CharView
        {
            public Animator Animator;
            public Transform ReadyTxt;
            public TextMeshProUGUI NameTxt;
            public Button ReadyBtn;

            public CharView(Transform container)
            {
                Animator = container.Find("Image").GetComponent<Animator>();
                ReadyTxt = container.Find("Image/Ready_Txt");
                NameTxt = container.Find("Info/Name_Txt").GetComponent<TextMeshProUGUI>();
                ReadyBtn = container.Find("Info/Ready_Btn").GetComponent<Button>();

                ReadyTxt.SetActive(false);
            }

            public CharView ApplyData(CharacterConfigSO config)
            {
                Animator.runtimeAnimatorController = config.Controller;
                NameTxt.text = config.CharacterName;
                return this;
            }

            public CharView TriggerReady(bool isReady)
            {
                ReadyTxt.SetActive(isReady);
                return this;
            }

            public CharView SetHost()
            {
                ReadyBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Start";
                return this;
            }
        }

        [Serializable]
        public class StatsView
        {
            public TextMeshProUGUI NameTxt;
            public Image FillImg;

            public StatsView(Transform container)
            {
                NameTxt = container.Find("Text").GetComponent<TextMeshProUGUI>();
                FillImg = container.Find("Process/Fill").GetComponent<Image>();
            }

            public StatsView ApplyData(StatType type, CharacterConfigSO config)
            {
                NameTxt.text = type.ToString();
                FillImg.fillAmount = (float)config.StatLevels[type] / config.LevelStatsConfigSO.LevelConfigs[type].Count;
                var ruler = FillImg.transform.Find("Ruler");
                ruler.GetComponent<HorizontalLayoutGroup>().spacing = 400 / config.LevelStatsConfigSO.LevelConfigs[type].Count - 10;
                ruler.GetComponentsInChildren<Image>()
                    .Where((ele, idx) => (idx + 1) >= config.LevelStatsConfigSO.LevelConfigs[type].Count)
                    .ForEach(ele => ele.SetActive(false));
                return this;
            }
        }

        [Serializable]
        public class SelectableCharView
        {
            public Transform Frame_Selected_P1;
            public Transform Frame_Selected_P2;
            public Transform Frame_Over;
            public Image Image;

            public SelectableCharView(Transform container)
            {
                Frame_Selected_P1 = container.Find("Frame/Frame_Selected_P1");
                Frame_Selected_P2 = container.Find("Frame/Frame_Selected_P2");
                Frame_Over = container.Find("Frame/Frame_Over");
                Image = container.Find("Image").GetComponent<Image>();

                Frame_Selected_P1.SetActive(false);
                Frame_Selected_P2.SetActive(false);
                Frame_Over.SetActive(false);
            }

            public SelectableCharView ApplyData(CharacterConfigSO config)
            {
                Image.sprite = config.Thumbnail;
                return this;
            }

            public SelectableCharView OnSelect(int playerIdx, bool isActive)
            {
                if (IsPlayer(playerIdx)) Frame_Selected_P1.SetActive(isActive);
                if (!IsPlayer(playerIdx)) Frame_Selected_P2.SetActive(isActive);
                return this;
            }
        }
    }
}