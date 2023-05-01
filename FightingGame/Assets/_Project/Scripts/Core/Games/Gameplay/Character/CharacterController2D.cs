using Core.Business;
using Core.EventSignal;
using Core.Extension;
using Core.GGPO;
using Core.SO;
using Core.Utility;
using Cysharp.Threading.Tasks;
using Network.UnityGGPO;
using PlasticPipe.PlasticProtocol.Server.Stubs;
using Shared.Extension;
using System;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Core.Gameplay
{
    public enum ECharacterState
    {
        IDLE,
        RUN,
        JUMP,
        DASH,
        PARRY,
        ATTACK,
        KNOCK,
        DEAD
    }

    [Serializable]
    public class CharacterAnchor
    {
        [DebugOnly] public Transform Hip;

        public void FindPosition(Transform anchor)
        {
            Hip = anchor.Find("Hip");
        }
    }

    [RequireComponent(typeof(Rigidbody2D))]
    public partial class CharacterController2D : NetworkObjectView
    {
        private SignalBus _signalBus;
        private GamePresenter _gamePresenter;

        [SerializeField][DebugOnly] private Rigidbody2D _rb2D;
        [SerializeField][DebugOnly] private CharacterRenderer _characterRenderer;
        [SerializeField][DebugOnly] private CharacterAnchor _characterAnchor = new();

        [SerializeField] private int _maxLightAtkCombo = 3;

        [Header("Jump Config")]
        [SerializeField] private float _windFric = 1.5f;

        [SerializeField] private int _maxJumpTimes = 2;
        [SerializeField][DebugOnly] private bool _isGrounded = false;
        [SerializeField][DebugOnly] private int _jumpCnt = 0;
        [SerializeField][DebugOnly] private Transform _groundChecker;
        [SerializeField][DebugOnly] private LayerMask _isGroundLayer;

        [SerializeField] protected ECharacterState _characterState = ECharacterState.IDLE;

        private bool IsStopMoveState(ECharacterState state) =>
            new ECharacterState[]
            {
                ECharacterState.ATTACK, ECharacterState.PARRY, ECharacterState.KNOCK, ECharacterState.DASH
            }.Contains(state);
        public ECharacterState CharacterState
        {
            get { return _characterState; }
            set
            {
                if (IsStopMoveState(value))
                    _rb2D.velocity = new Vector2();
                CheckIdle(value);
            }
        }

        [Header("Attack Config")]
        [SerializeField] private float _defaultComboCooldown = 0.4f;

        [SerializeField][DebugOnly] private bool _isActivateComboTimer = false;
        [SerializeField][DebugOnly] private int _lightAtkCombo = 0;
        [SerializeField][DebugOnly] private float _comboTimer = 0.4f;

        [Header("Stats")]
        [SerializeField][DebugOnly] private CharacterConfigSO _characterConfigSO;

        public CharacterConfigSO CharacterConfigSO => _characterConfigSO;
        public ItemStats CharacterStats => _characterStats;
        [SerializeField][DebugOnly] protected ItemStats _characterStats = new();

        public float GetStatsValue(StatType type) => _characterStats.GetStats(type).Value;

        public int PlayerIndex { get; private set; }
        private NetworkGame GameState => (NetworkGame)GameManager.Instance.Runner.Game;

        public int EnemyIndex => GameState.Characters.Select((_, idx) => idx)
            .Where(ele => ele != PlayerIndex).First();

        protected override void Awake()
        {
            base.Awake();

            _rb2D = GetComponent<Rigidbody2D>();
            _characterRenderer = GetComponentInChildren<CharacterRenderer>();
            _characterAnchor.FindPosition(_characterRenderer.transform.Find("Anchor"));

            _isGroundLayer = LayerMask.GetMask("Ground", "CharacterCollisionBlocker");
            _groundChecker = _characterRenderer.transform.Find("GroundChecker");
            if (_groundChecker == null) throw new Exception("Require GroundChecker gameObject inside this");
        }

        [Inject]
        public void Construct(
            SignalBus signalBus,
            GamePresenter gamePresenter)
        {
            _signalBus = signalBus;
            _gamePresenter = gamePresenter;
        }

        public void Setup(int playerIdx, CharacterConfigSO characterConfigSO, bool left, bool right)
        {
            PlayerIndex = playerIdx;
            _characterConfigSO = characterConfigSO;
            _characterRenderer.SetColor(_characterConfigSO.Color);
            _characterStats = _characterConfigSO.CharacterStats.Duplicate();
            gameObject.GetComponent<AICharacter>().enabled = playerIdx != 0;
            CheckFlip(left, right);
        }

        private async void CheckIdle(ECharacterState value)
        {
            if (value == ECharacterState.IDLE && _characterState == ECharacterState.ATTACK)
            {
                _ = _characterRenderer.Attack(0);
                _ = _characterRenderer.UseSkill(0);

                await UniTask.WaitUntil(() => _characterRenderer.GetAnimAttackIndex() == 0);
                await UniTask.WaitUntil(() => _characterRenderer.GetAnimSkillIndex() == 0);
            }
            _characterState = value;
        }

        #region Action

        private void CheckFlip(bool left, bool right)
        {
            _characterRenderer.Flip(left, right);
        }

        #region Movement

        private bool IsRunnable =>
            new ECharacterState[] {
                ECharacterState.IDLE, ECharacterState.RUN
            }.Contains(CharacterState);

        public void Move(bool left, bool right)
        {
            bool isJumping = CharacterState == ECharacterState.JUMP;
            if (!IsRunnable & !isJumping) return;

            CheckFlip(left, right);

            bool isRun = left | right;
            CharacterState = isJumping ? CharacterState :
                (isRun ? ECharacterState.RUN : ECharacterState.IDLE);

            var mSpd = GetStatsValue(StatType.MSpd);
            int direction = !isRun ? 0 : (right ? 1 : -1);
            _rb2D.velocity = new Vector2(direction * mSpd /
                (isJumping ? _windFric : 1), _rb2D.velocity.y);
            _characterRenderer.Run(isRun && !isJumping, mSpd / 2);
        }

        private bool IsJumpable =>
            IsRunnable ||
            new ECharacterState[] {
                ECharacterState.JUMP
            }.Contains(CharacterState);

        private void CheckGround()
        {
            Collider2D collider = Physics2D.OverlapPoint(_groundChecker.position, _isGroundLayer);
            _isGrounded = collider != null && !ReferenceEquals(collider.transform.parent.gameObject, gameObject);
            if (!_isGrounded)
                CharacterState = ECharacterState.JUMP;
            else if (CharacterState == ECharacterState.JUMP ||
                _jumpCnt != 0)
            {
                CharacterState = ECharacterState.IDLE;
                _jumpCnt = 0;
            }
        }

        private void Jump(bool up)
        {
            CheckGround();
            if (!IsJumpable) return;

            if (!up || _jumpCnt >= _maxJumpTimes) return;

            CharacterState = ECharacterState.JUMP;
            _jumpCnt++;
            _rb2D.velocity = new Vector2(_rb2D.velocity.x, GetStatsValue(StatType.JForce));
        }

        #endregion Movement

        #region Attack

        private async UniTask LightAttack(bool light)
        {
            if (!light) return;

            if (_lightAtkCombo == _maxLightAtkCombo) return;

            _isActivateComboTimer = true;
            _lightAtkCombo++;
            _comboTimer = _defaultComboCooldown;

            CharacterState = ECharacterState.ATTACK;
            await _characterRenderer.Attack(_lightAtkCombo % (_maxLightAtkCombo + 1), GetStatsValue(StatType.ASpd));
        }

        private async UniTask HeavyAttack(bool heavy)
        {
            if (!heavy) return;

            CharacterState = ECharacterState.ATTACK;
            await _characterRenderer.Attack(4, GetStatsValue(StatType.ASpd));
        }

        private async UniTask Skill1Attack(bool skill1)
        {
            if (!skill1) return;
            if (_characterStats.GetStats(StatType.MP).Value < _characterConfigSO.Skill1.MpCost) return;

            CharacterState = ECharacterState.ATTACK;
            _characterStats.GetStats(StatType.MP).BaseValue -= _characterConfigSO.Skill1.MpCost;
            await _characterRenderer.UseSkill(1, GetStatsValue(StatType.ASpd));
        }

        private async UniTask Skill2Attack(bool skill2)
        {
            if (!skill2) return;
            if (_characterStats.GetStats(StatType.MP).Value < _characterConfigSO.Skill2.MpCost) return;

            CharacterState = ECharacterState.ATTACK;
            _characterStats.GetStats(StatType.MP).BaseValue -= _characterConfigSO.Skill2.MpCost;
            await _characterRenderer.UseSkill(2, GetStatsValue(StatType.ASpd));
        }

        private bool IsAttackable =>
            IsRunnable || new ECharacterState[] {
            }.Contains(CharacterState);

        public void Attack(bool light, bool heavy, bool skill1, bool skill2)
        {
            if (!IsAttackable) return;

            bool isAttack = light | heavy;
            bool isSkill = skill1 | skill2;
            if (!isAttack) _ = _characterRenderer.Attack(0, GetStatsValue(StatType.ASpd));
            if (!isSkill) _ = _characterRenderer.UseSkill(0, GetStatsValue(StatType.ASpd));

            if (skill2) _ = Skill2Attack(skill2);
            else if (skill1) _ = Skill1Attack(skill1);
            else if (heavy) _ = HeavyAttack(heavy);
            else if (light) _ = LightAttack(light);
        }

        #endregion Attack

        public async void Dash(bool dashForward, bool dashBackward)
        {
            if (!IsRunnable) return;
            CheckFlip(dashBackward, dashForward);

            if (dashForward | dashBackward)
            {
                _characterRenderer.Sprint(true);
                CharacterState = ECharacterState.DASH;
            }

            if (CharacterState != ECharacterState.DASH) return;

            var mSpd = GetStatsValue(StatType.MSpd);
            int direction = dashForward ? 1 : -1;
            _rb2D.velocity = new Vector2(direction * mSpd * 3f, _rb2D.velocity.y);

            await UniTask.Delay(500);
            _characterRenderer.Sprint(false);
            CharacterState = ECharacterState.IDLE;
        }

        private bool IsParriable =>
            IsRunnable ||
            new ECharacterState[] {
                ECharacterState.PARRY
            }.Contains(CharacterState);

        private void ExitParry()
        {
            if (CharacterState != ECharacterState.PARRY) return;
            _characterRenderer.Parry(false);
            CharacterState = ECharacterState.IDLE;
        }

        public async void Parry(bool left, bool right, bool down)
        {
            if (!IsParriable) return;
            CheckFlip(left, right);

            if (down)
            {
                CharacterState = ECharacterState.PARRY;
            }

            if (CharacterState != ECharacterState.PARRY) return;

            _characterRenderer.Parry(true);

            await UniTask.Delay(500);
            ExitParry();
        }

        private void ResetAttackCombo()
        {
            if (!_isActivateComboTimer) return;

            _comboTimer -= Time.deltaTime;
            if (_comboTimer <= 0)
            {
                _lightAtkCombo = 0;
                _isActivateComboTimer = false;
                _comboTimer = _defaultComboCooldown;
            }
        }

        private void RegenMP()
        {
            if (_characterStats.GetStats(StatType.MP).BaseValue >=
                _characterConfigSO.CharacterStats.GetStats(StatType.MP).BaseValue) return;
            _characterStats.GetStats(StatType.MP).BaseValue += _characterStats.GetStats(StatType.MPRegen).Value;
        }

        public void Logic(int index, bool up, bool down, bool left, bool right, bool light, bool heavy,
            bool skill1, bool skill2, bool dashForward, bool dashBackward)
        {
            if (CharacterState == ECharacterState.DEAD) return;
            Attack(light, heavy, skill1, skill2);
            Dash(dashForward, dashBackward);
            Parry(left, right, down);
            Jump(up);
            Move(left, right);
            ResetAttackCombo();
            RegenMP();
        }

        #endregion Action

        #region Stats

        public float CalculateDmg(int atkSOIdx, float arm)
        {
            AttackSO atkConfig = _characterConfigSO.AttackSOs[atkSOIdx];
            float dmg = (atkConfig.BaseAttack > 0 ?
                atkConfig.BaseAttack : GetStatsValue(StatType.Att)) * atkConfig.HeroAttackModifier;
            dmg -= arm * (1 - atkConfig.IgnoreArmorPercent);
            return Mathf.Max(1, dmg);
        }

        private void CheckDead()
        {
            if (GetStatsValue(StatType.HP) > 0) return;
            CharacterState = ECharacterState.DEAD;
            _characterRenderer.Dead();
            _rb2D.bodyType = RigidbodyType2D.Static;
            GetComponent<Collider2D>().enabled = false;
            enabled = false;
            GetComponentInChildren<CollisionBlocker>().SetActive(false);
            // Signal on end
            _signalBus.Fire(new OnEndBattle(PlayerIndex != 0));
            GameManager.Instance.Shutdown();
        }

        private void Knock()
        {
            CharacterState = ECharacterState.KNOCK;
            _ = _characterRenderer.BeHit();
        }

        private float CheckKnockAndReturnDmg(KnockType isKnock, float dmg)
        {
            if (isKnock == KnockType.Not) return dmg;
            if (isKnock == KnockType.Absolute || UnityEngine.Random.Range(0, 100) > 50)
            {
                if (_characterState == ECharacterState.PARRY)
                    ExitParry();
                Knock();
            }
            else if (_characterState == ECharacterState.PARRY)
                dmg /= 8f;
            return dmg;
        }

        private void SpawnLostHPFX(float dmg)
        {
            _ = _gamePresenter.SpawnFloatingDamageTex((dmg > 0 ? "-" : "+") + $"{dmg}",
                _characterAnchor.Hip.position, 0.5f);
        }

        public void OnHit(float dmg, KnockType isKnock = 0)
        {
            if (CharacterState == ECharacterState.DEAD) return;
            dmg = CheckKnockAndReturnDmg(isKnock, dmg);
            dmg = Mathf.Floor(dmg);
            _characterStats.GetStats(StatType.HP).BaseValue -= dmg;
            CheckDead();
            SpawnLostHPFX(dmg);
        }

        #endregion Stats
    }
}