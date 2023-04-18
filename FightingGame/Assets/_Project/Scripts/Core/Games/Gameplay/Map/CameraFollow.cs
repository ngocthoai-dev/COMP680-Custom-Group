using Core.Utility;
using UnityEngine;

namespace Core.Gameplay
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField][DebugOnly] private CharacterController2D _characterController;
        [SerializeField][DebugOnly] private Bounds _mapBound;
        [SerializeField][DebugOnly] private float _camX, _camY, _camOthSize, _camRatio;
        [SerializeField] private float _smoothSpd = 2f;

        public void Setup(CharacterController2D characterController, Bounds bound)
        {
            _characterController = characterController;
            _mapBound = bound;
            _camOthSize = Camera.main.orthographicSize;
            _camRatio = (Screen.safeArea.width / Screen.safeArea.height) * _camOthSize;
        }

        private void FixedUpdate()
        {
            if (_characterController == null) return;
            _camX = Mathf.Clamp(_characterController.transform.position.x,
                _mapBound.min.x + _camRatio + _mapBound.center.x / 2, _mapBound.max.x - _camRatio - _mapBound.center.x / 2);
            _camY = Mathf.Clamp(_characterController.transform.position.y,
                _mapBound.min.y + _camOthSize + _mapBound.center.y/ 2, _mapBound.max.y - _camOthSize - _mapBound.center.y / 2);
            transform.position = Vector3.Lerp(transform.position, new Vector3(_camX, _camY, transform.position.z), _smoothSpd);
        }
    }
}