using Core.SO;
using Core.Utility;
using UnityEngine;

namespace Core.Gameplay
{
    public class ScalableAttackCollision : TriggerAttackCollision
    {
        [SerializeField] private ScaleDirection _direction = ScaleDirection.Horizontal;
        [SerializeField] private float _speed = 2;
        [SerializeField] private float _disableAfter = 0;

        [SerializeField][DebugOnly] private Vector2 _currentScale;
        [SerializeField][DebugOnly] private float _start;

        private void OnEnable()
        {
            _currentScale = Vector2.zero;
            _start = Time.time;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            transform.localScale = Vector3.zero;
        }

        private Vector2 GetScaleDirection()
        {
            switch (_direction)
            {
                case ScaleDirection.Horizontal:
                    _currentScale.y = 1;
                    break;

                case ScaleDirection.Vertical:
                    _currentScale.x = 1;
                    break;

                case ScaleDirection.Both:
                    break;

                default: return Vector2.one;
            };
            return _currentScale;
        }

        private void Update()
        {
            Vector2 scaleDir = GetScaleDirection();
            if (!gameObject.activeInHierarchy || scaleDir.x + scaleDir.y >= 2) return;
            if (_disableAfter > 0 && Time.time - _start >= _disableAfter)
            {
                gameObject.SetActive(false);
                _attackContainer.Controller.CharacterState = ECharacterState.IDLE;
                return;
            }

            _currentScale.x = Mathf.Lerp(_currentScale.x, 1, Time.deltaTime * _speed);
            _currentScale.y = Mathf.Lerp(_currentScale.y, 1, Time.deltaTime * _speed);
            transform.localScale = new Vector3(_currentScale.x, _currentScale.y, 1);
        }
    }
}