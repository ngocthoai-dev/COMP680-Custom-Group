using UnityEngine;

namespace Core.Utility
{
    public class GameSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;

                var objs = FindObjectsOfType<T>();
                if (objs.Length > 1)
                {
                    Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
                }

                if (objs.Length > 0)
                    _instance = objs[0];
                else
                {
                    Debug.LogError("There is no " + typeof(T).Name + " in the scene.");
                }

                return _instance;
            }
        }
    }
}