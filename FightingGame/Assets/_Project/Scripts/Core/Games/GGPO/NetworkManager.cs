using Core.Business;
using Core.Gameplay;
using Core.SO;
using Core.Utility;
using Core.View;
using Core.Extension;
using Network.UnityGGPO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityGGPO;
using Zenject;

namespace Core.GGPO
{
    public class NetworkManager : GameManager
    {
        [SerializeField] private bool isDebugBattle = false;
        private IBundleLoader _bundleLoader;
        private SignalBus _signalBus;

        [SerializeField] private Transform _mapContainer;
        [SerializeField][DebugOnly] private CharacterConfigSO[] _charConfigs;

        [SerializeField][DebugOnly] MapConfig _mapConfig;
        public MapConfig MapConfig => _mapConfig;
        public CharacterConfigSO[] CharConfigs => _charConfigs;

        [Inject]
        public void Construct(
            SignalBus signalBus,
            [Inject(Id = BundleLoaderName.Addressable)]
            IBundleLoader bundleLoader)
        {
            _signalBus = signalBus;
            _bundleLoader = bundleLoader;
            LoadAssets();
        }

        private void SelectRandomMap()
        {
            MapConfig[] maps = _mapContainer.GetComponentsInChildren<MapConfig>(true);
            int randomIdx = Random.Range(0, maps.Length);
            _mapConfig = maps[randomIdx];
            _mapConfig.SetActive(true);
        }

        private async void LoadAssets()
        {
            if (isDebugBattle)
            {
                _charConfigs = new CharacterConfigSO[2];
                _charConfigs[0] = await _bundleLoader.LoadAssetAsync<CharacterConfigSO>("Assets/_Project/Bundles/ScriptableObjects/Character/4Tails/CharacterConfig.asset");
                _charConfigs[1] = await _bundleLoader.LoadAssetAsync<CharacterConfigSO>("Assets/_Project/Bundles/ScriptableObjects/Character/3Tails/CharacterConfig 1.asset");
                PreStartGame(_charConfigs).StartLocalGame();
            }
        }

        public NetworkManager PreStartGame(CharacterConfigSO[] characterConfigSOs)
        {
            SelectRandomMap();
            _charConfigs = characterConfigSOs.Select(ele => ele.ApplyStats().Clone()).ToArray();

            if (_charConfigs[0].CharacterName == _charConfigs[1].CharacterName)
                _charConfigs[1].Color = new Color(0x00, 0xFF, 0x23);
            else
                _charConfigs[1].Color = Color.white;
            InputController.Instance.PlayerInput.enabled = true;

            return this;
        }

        public override void StartLocalGame()
        {
            StartGame(new LocalRunner(new NetworkGame(_signalBus, MapConfig, _charConfigs)));
        }

        public override void StartGGPOGame(IPerfUpdate perfPanel, IList<Connections> connections, int playerIndex)
        {
            var game = new GGPORunner(GetType().Name.ToString(), new NetworkGame(_signalBus, MapConfig, _charConfigs), perfPanel);
            game.Init(connections, playerIndex);
            StartGame(game);
        }
    }
}