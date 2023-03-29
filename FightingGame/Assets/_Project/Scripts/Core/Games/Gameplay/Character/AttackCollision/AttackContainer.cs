using Core.GGPO;
using Core.SO;
using Core.Utility;
using Network.UnityGGPO;
using UnityEngine;

namespace Core.Gameplay
{
    public class AttackContainer : MonoBehaviour
    {
        public CharacterController2D Controller => _controller;
        [SerializeField][DebugOnly] protected CharacterController2D _controller;
        public AttackSO Config => _config;
        [SerializeField][DebugOnly] protected AttackSO _config;
        public AttackTypeIndex AtkIndex => _atkIndex;
        [SerializeField] protected AttackTypeIndex _atkIndex = 0;
        [SerializeField] protected bool _isDestroyOnTrigger = false;

        private NetworkGame GameState => (NetworkGame)GameManager.Instance.Runner.Game;

        protected virtual void GetReferences()
        {
            if (_controller) return;
            _controller = GetComponentInParent<CharacterController2D>();
        }

        public void Setup(AttackSO config)
        {
            _config = config;
            GetReferences();
            if (_config.IsSpawnOnEnemy)
                transform.position = new Vector2(
                    GameState.Characters[_controller.EnemyIndex].CharacterController.transform.position.x,
                    GameState.Characters[_controller.PlayerIndex].CharacterController.transform.position.y);
        }

        private void Awake()
        {
            GetReferences();
        }

        private void OnDoneAnim()
        {
            gameObject.SetActive(false);
            _controller.CharacterState = ECharacterState.IDLE;
        }
    }
}