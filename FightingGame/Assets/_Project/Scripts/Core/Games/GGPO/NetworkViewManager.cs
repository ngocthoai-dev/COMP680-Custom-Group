using Core.Business;
using Core.Gameplay;
using Core.SO;
using Core.Utility;
using Network.UnityGGPO;
using System;
using UnityEngine;
using Zenject;

namespace Core.GGPO
{
    public class NetworkViewManager : MonoBehaviour, IGameView
    {
        private DiContainer _diContainer;
        private IBundleLoader _bundleLoader;

        private NetworkObjectView[] _charViews = Array.Empty<NetworkObjectView>();
        private GameManager gameManager => GameManager.Instance;

        [SerializeField] private bool isDebugBattle = false;
        [SerializeField] private MapConfig _mapConfig;
        [SerializeField][DebugOnly] private CharacterConfigSO[] _charConfigs;

        private async void LoadAssets()
        {
            if (isDebugBattle)
            {
                _charConfigs = new CharacterConfigSO[2];
                _charConfigs[0] = await _bundleLoader.LoadAssetAsync<CharacterConfigSO>("Assets/_Project/Bundles/ScriptableObjects/Shared/Character/CharacterConfig.asset");
                _charConfigs[1] = await _bundleLoader.LoadAssetAsync<CharacterConfigSO>("Assets/_Project/Bundles/ScriptableObjects/Shared/Character/CharacterConfig 1.asset");
                _charConfigs[0].ApplyStats();
                _charConfigs[1].ApplyStats();
                ((NetworkManager)gameManager).PreStartGame(_mapConfig, _charConfigs).StartLocalGame();
            }
        }

        [Inject]
        public void Construct(
            DiContainer diContainer,
            [Inject(Id = BundleLoaderName.Addressable)]
            IBundleLoader bundleLoader)
        {
            _diContainer = diContainer;
            _bundleLoader = bundleLoader;
            LoadAssets();
        }

        private void ResetView(NetworkGame networkGame)
        {
            var characterNetworks = networkGame.Characters;
            _charViews = new NetworkObjectView[characterNetworks.Length];

            for (int idx = 0; idx < characterNetworks.Length; ++idx)
            {
                _charViews[idx] = _diContainer.InstantiatePrefab(_charConfigs[idx].Prefab, transform).GetComponent<NetworkObjectView>();
                Transform tr = _mapConfig.GetRandomSpawnTransform();
                _charViews[idx].transform.position = tr.position;
            }
        }

        public void UpdateGameView(IGameRunner runner)
        {
            var nw = (NetworkGame)runner.Game;
            var gameInfo = runner.GameInfo;

            var characterNetworks = nw.Characters;
            if (_charViews.Length != characterNetworks.Length)
            {
                ResetView(nw);
            }
            if (NetworkGame.Start)
            {
                for (int idx = 0; idx < nw.Characters.Length; idx++)
                {
                    nw.Characters[idx].CharacterController = (CharacterController2D)_charViews[idx];
                    nw.Characters[idx].CharacterConfigSO = _charConfigs[idx];
                    bool left = _mapConfig.OccupiedPositions[idx].rotation.eulerAngles.y != 0;
                    _charViews[idx].GetComponent<CharacterController2D>()
                        .Setup(idx, _charConfigs[idx], left, !left);
                }
                Camera.main.GetComponent<CameraFollow>().Setup(nw.Characters[0].CharacterController, _mapConfig.Bound);
                NetworkGame.Start = false;
            }
            for (int i = 0; i < characterNetworks.Length; ++i)
            {
                _charViews[i].Populate(characterNetworks[i], gameInfo.players[i]);
            }
        }

        private void Update()
        {
            if (gameManager.IsRunning)
            {
                UpdateGameView(gameManager.Runner);
            }
        }
    }
}