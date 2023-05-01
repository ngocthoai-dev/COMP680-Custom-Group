
using System.Linq;
using Core.GGPO;
using Core.SO;
using Core.Utility;
using Network.UnityGGPO;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

namespace Core.Gameplay
{
    [RequireComponent(typeof(CharacterController2D))]
    public class AICharacter : MonoBehaviour
    {
        private NetworkGame GameState => (NetworkGame)GameManager.Instance.Runner.Game;
        private int EnemyIndex => 0;

        private ItemStats CharacterStats() => GetComponent<CharacterController2D>().CharacterStats;
        public float GetStatsValue(StatType type) => CharacterStats().GetStats(type).Value;

        private CharacterController2D _otherPlayer => GameState.Characters[EnemyIndex].CharacterController;

        private int _movementInputX;
        private float _distance;
        private float _movementTimer;

        private float _skillTimer;
        private float _attackTimer;

        private ItemStats? _previousStats = null;

        private void Movement()
        {
            _distance = _otherPlayer.transform.position.x - transform.position.x;
            _movementTimer -= Time.deltaTime;
            if (_movementTimer < 0)
            {
                NetworkInput.TWO_LEFT = _distance < 0;
                NetworkInput.TWO_RIGHT = !NetworkInput.TWO_LEFT;

                int jumpRandom = Random.Range(0, 2);
                if (jumpRandom <= 1 && _otherPlayer.transform.position.y > transform.position.y + 0.5f)
                    NetworkInput.TWO_UP = true;
                _movementTimer = Random.Range(0.2f, 0.35f);
            }
        }

        private void Attack()
        {
            _attackTimer -= Time.deltaTime;
            if (_attackTimer < 0)
            {
                int attackRandom = Random.Range(0, 5);
                if (attackRandom <= 2)
                {
                    NetworkInput.TWO_HEAVY = true;
                }
                else
                {
                    NetworkInput.TWO_LIGHT = true;
                }
                _attackTimer = Random.Range(0.5f, 1.5f);
            }
        }

        private void Specials()
        {
            if (_skillTimer < 0)
            {
                int skillRandom = Random.Range(0, 4);
                if (skillRandom <= 1)
                {
                    NetworkInput.TWO_SKILL1 = true;
                }
                else if (skillRandom <= 2)
                {
                    NetworkInput.TWO_SKILL2 = true;
                    _attackTimer = Random.Range(0.15f, 0.35f);
                    _skillTimer = Random.Range(0.4f, 0.85f);
                }
            }
        }

        private void Parry()
        {
            if (_previousStats == null) return;

            if (_previousStats?.GetStats(StatType.HP).Value > GetStatsValue(StatType.HP))
            {
                int parryRandom = Random.Range(0, 20);
                if (parryRandom <= 1)
                {
                    NetworkInput.TWO_DOWN = true;
                }
            }
        }

        void FixedUpdate()
        {
            if (GameManager.Instance.IsRunning && enabled)
            {
                Movement();
                Parry();
                if (Mathf.Abs(_distance) <= 5)
                    Attack();
                Specials();
                _previousStats = CharacterStats().Duplicate();
            }
        }
    }
}