using Core.Gameplay;
using Core.SO;
using Network.UnityGGPO;
using System.Collections.Generic;
using UnityGGPO;

namespace Core.GGPO
{
    public class NetworkManager : GameManager
    {
        public MapConfig MapConfig { get; private set; }
        public CharacterConfigSO[] CharConfigs { get; private set; }

        public NetworkManager PreStartGame(MapConfig mapConfig, CharacterConfigSO[] characterConfigSOs)
        {
            MapConfig = mapConfig;
            CharConfigs = characterConfigSOs;
            return this;
        }

        public override void StartLocalGame()
        {
            StartGame(new LocalRunner(new NetworkGame(MapConfig, CharConfigs)));
        }

        public override void StartGGPOGame(IPerfUpdate perfPanel, IList<Connections> connections, int playerIndex)
        {
            var game = new GGPORunner(GetType().Name.ToString(), new NetworkGame(MapConfig, CharConfigs), perfPanel);
            game.Init(connections, playerIndex);
            StartGame(game);
        }
    }
}