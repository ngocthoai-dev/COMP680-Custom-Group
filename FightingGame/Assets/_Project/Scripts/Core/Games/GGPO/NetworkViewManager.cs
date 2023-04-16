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

        private NetworkObjectView[] _charViews = Array.Empty<NetworkObjectView>();
        private NetworkManager gameManager => (NetworkManager)GameManager.Instance;

        [Inject]
        public void Construct(
            DiContainer diContainer)
        {
            _diContainer = diContainer;
        }

        private void ResetView(NetworkGame networkGame)
        {
            var characterNetworks = networkGame.Characters;
            _charViews = new NetworkObjectView[characterNetworks.Length];

            for (int idx = 0; idx < characterNetworks.Length; ++idx)
            {
                _charViews[idx] = _diContainer.InstantiatePrefab(gameManager.CharConfigs[idx].Prefab, transform).GetComponent<NetworkObjectView>();
                Transform tr = gameManager.MapConfig.GetRandomSpawnTransform();
                _charViews[idx].transform.position = tr.position;
            }
        }

        public void UpdateGameView(IGameRunner runner)
        {
            var networkGame = (NetworkGame)runner.Game;
            var gameInfo = runner.GameInfo;

            var characterNetworks = networkGame.Characters;
            if (_charViews.Length != characterNetworks.Length)
            {
                ResetView(networkGame);
            }
            if (NetworkGame.Start)
            {
                for (int idx = 0; idx < networkGame.Characters.Length; idx++)
                {
                    networkGame.Characters[idx].CharacterController = (CharacterController2D)_charViews[idx];
                    networkGame.Characters[idx].CharacterConfigSO = gameManager.CharConfigs[idx];
                    bool left = gameManager.MapConfig.OccupiedPositions[idx].rotation.eulerAngles.y != 0;
                    _charViews[idx].GetComponent<CharacterController2D>()
                        .Setup(idx, gameManager.CharConfigs[idx], left, !left);
                }
                Camera.main.GetComponent<CameraFollow>().Setup(networkGame.Characters[0].CharacterController, gameManager.MapConfig.Bound);
                NetworkGame.Start = false;
            }
            for (int i = 0; i < characterNetworks.Length; ++i)
            {
                _charViews[i].Populate(characterNetworks[i], gameInfo.players[i]);
            }
        }

        private void Update()
        {
            if (_diContainer != null && gameManager.IsRunning)
            {
                UpdateGameView(gameManager.Runner);
            }
        }
    }
}