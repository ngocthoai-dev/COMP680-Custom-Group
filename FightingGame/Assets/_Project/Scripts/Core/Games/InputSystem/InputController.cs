using Core.GGPO;
using Core.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using static IA_Player;

namespace Core.View
{
    public class InputController : GameSingleton<InputController>, IPlayerActions
    {
        [System.Serializable]
        public class StackPress
        {
            [DebugOnly] public int InputTime = 0;
            [DebugOnly] public int Count = 0;
            [DebugOnly] public int _maxCount = 0;
            public int LimitInputFrame = 12;

            public StackPress(int maxCount)
            {
                _maxCount = maxCount;
            }

            public StackPress CheckStacked()
            {
                if (Count != 0)
                {
                    int timeSinceLastPress = (int)Mathf.Floor(Time.frameCount - InputTime);
                    if (timeSinceLastPress > LimitInputFrame)
                        Count = 0;
                }

                if (Count == _maxCount) Count = 0;

                Count++;
                InputTime = Time.frameCount;
                return this;
            }
        }

        private SignalBus _signalBus;
        private IA_Player _playerInputAction;
        public PlayerInput PlayerInput => _playerInput;
        private PlayerInput _playerInput;
        private Vector2Int _previousInput;
        public bool IsControllerEnabled { get; set; } = true;

        [SerializeField] private StackPress _dashForwardInput = new(2);
        [SerializeField] private StackPress _dashBackwardInput = new(2);

        [Inject]
        public void Construct(
            SignalBus signalBus)
        {
            _signalBus = signalBus;

            _playerInputAction = new();
            _playerInputAction.Player.Enable();

            _playerInput = GetComponent<PlayerInput>();
            //RegisterInputs();
        }

        private void OnEnable()
        {
            if (_playerInputAction == null) return;
            RegisterInputs();
        }

        private void OnDisable()
        {
            if (_playerInputAction == null) return;
            UnRegisterInputs();
        }

        private void RegisterInputAction(InputAction ia, System.Action<InputAction.CallbackContext> action)
        {
            ia.started += action;
            ia.performed += action;
            ia.canceled += action;
        }

        private void UnregisterInputAction(InputAction ia, System.Action<InputAction.CallbackContext> action)
        {
            ia.started -= action;
            ia.performed -= action;
            ia.canceled -= action;
        }

        private void RegisterInputs()
        {
            RegisterInputAction(_playerInputAction.Player.Move, OnMove);
            RegisterInputAction(_playerInputAction.Player.Jump, OnJump);
            RegisterInputAction(_playerInputAction.Player.DashForward, OnDashForward);
            RegisterInputAction(_playerInputAction.Player.DashBackward, OnDashBackward);
            RegisterInputAction(_playerInputAction.Player.LightAtk, OnLightAtk);
            RegisterInputAction(_playerInputAction.Player.HeavyAtk, OnHeavyAtk);
            RegisterInputAction(_playerInputAction.Player.Skill1, OnSkill1);
            RegisterInputAction(_playerInputAction.Player.Skill2, OnSkill2);
        }

        private void UnRegisterInputs()
        {
            UnregisterInputAction(_playerInputAction.Player.Move, OnMove);
            UnregisterInputAction(_playerInputAction.Player.Jump, OnJump);
            UnregisterInputAction(_playerInputAction.Player.DashForward, OnDashForward);
            UnregisterInputAction(_playerInputAction.Player.DashBackward, OnDashBackward);
            UnregisterInputAction(_playerInputAction.Player.LightAtk, OnLightAtk);
            UnregisterInputAction(_playerInputAction.Player.HeavyAtk, OnHeavyAtk);
            UnregisterInputAction(_playerInputAction.Player.Skill1, OnSkill1);
            UnregisterInputAction(_playerInputAction.Player.Skill2, OnSkill2);
        }

        public void OnDashBackward(InputAction.CallbackContext context)
        {
            if (context.performed && IsControllerEnabled)
                NetworkInput.ONE_DASH_BACKWARD = _dashBackwardInput.CheckStacked().Count == 2;
        }

        public void OnDashForward(InputAction.CallbackContext context)
        {
            if (context.performed && IsControllerEnabled)
                NetworkInput.ONE_DASH_FORWARD = _dashForwardInput.CheckStacked().Count == 2;
        }

        public void OnHeavyAtk(InputAction.CallbackContext context)
        {
            if (context.performed && IsControllerEnabled)
                NetworkInput.ONE_HEAVY = true;
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed && IsControllerEnabled)
                NetworkInput.ONE_UP = true;
        }

        public void OnLightAtk(InputAction.CallbackContext context)
        {
            if (context.performed && IsControllerEnabled)
                NetworkInput.ONE_LIGHT = true;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            var input = new Vector2Int(Mathf.RoundToInt(context.ReadValue<Vector2>().x), Mathf.RoundToInt(context.ReadValue<Vector2>().y));
            if (context.performed && IsControllerEnabled && _previousInput != input)
            {
                if (input.x == 1)
                {
                    _previousInput = input;
                    NetworkInput.ONE_RIGHT = true;
                }
                if (input.x == -1)
                {
                    _previousInput = input;
                    NetworkInput.ONE_LEFT = true;
                }
                if (input.y == -1)
                {
                    NetworkInput.ONE_DOWN = true;
                }
            }
            if (input == Vector2Int.zero)
            {
                _previousInput = Vector2Int.zero;
            }

            if (input.x == 0)
            {
                NetworkInput.ONE_RIGHT = false;
                NetworkInput.ONE_LEFT = false;
            }
            if (input.y == 0)
            {
                NetworkInput.ONE_DOWN = false;
            }
        }

        public void OnSkill1(InputAction.CallbackContext context)
        {
            if (context.performed && IsControllerEnabled)
                NetworkInput.ONE_SKILL1 = true;
        }

        public void OnSkill2(InputAction.CallbackContext context)
        {
            if (context.performed && IsControllerEnabled)
                NetworkInput.ONE_SKILL2 = true;
        }
    }
}