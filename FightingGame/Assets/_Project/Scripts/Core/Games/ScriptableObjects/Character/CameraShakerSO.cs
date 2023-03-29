using UnityEngine;

namespace Core.SO
{
    [CreateAssetMenu(fileName = "CameraShaker", menuName = "Scriptable Objects/CameraShaker", order = 2)]
    public class CameraShakerSO : ScriptableObject
    {
        public float Intensity = 35;
        public float Timer = 0.15f;
    }
}