#if UNITY_EDITOR
using Core.SO;
using Shared.Extension;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Core.Editor
{
    [CustomEditor(typeof(CharacterConfigSO))]
    public class CharacterConfigSOEditor : UnityEditor.Editor
    {
        private CharacterConfigSO _so;
        private string _rootPath;

        void OnEnable()
        {
            _so = (CharacterConfigSO)target;
            _rootPath = AssetDatabase.GetAssetPath(_so);
            var splits = _rootPath.Split("/");
            _rootPath = splits.SkipLast(1).Join("/");
        }

        public static int TryGetUnityObjectsOfTypeFromPath<T>(string path, out T[] assets) where T : Object
        {
            string[] filePaths = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

            int countFound = 0;

            List<T> assetsFound = new();

            if (filePaths != null && filePaths.Length > 0)
            {
                for (int i = 0; i < filePaths.Length; i++)
                {
                    Object obj = AssetDatabase.LoadAssetAtPath(filePaths[i], typeof(T));
                    if (obj is T asset)
                    {
                        countFound++;
                        if (!assetsFound.Contains(asset))
                            assetsFound.Add(asset);
                    }
                }
            }

            assets = assetsFound.ToArray();
            return countFound;
        }

        private AttackSO TryPickSO(AttackSO[] sos, string prefix)
        {
            return sos.Where(so => so.name.StartsWith(prefix)).FirstOrDefault();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Collect Attack SO"))
            {
                Debug.Log($"Collect Attack SO: {_rootPath}");
                TryGetUnityObjectsOfTypeFromPath($"{_rootPath}/Attack", out AttackSO[] sos);
                _so.Light1 = TryPickSO(sos, nameof(_so.Light1));
                _so.Light2 = TryPickSO(sos, nameof(_so.Light2));
                _so.Light3 = TryPickSO(sos, nameof(_so.Light3));
                _so.Heavy = TryPickSO(sos, nameof(_so.Heavy));
                _so.Skill1 = TryPickSO(sos, nameof(_so.Skill1));
                _so.Skill2 = TryPickSO(sos, nameof(_so.Skill2));
            }

            if (_so.LevelStatsConfigSO != null)
            {
                if (GUILayout.Button("Apply Stats from Level"))
                    _so.CharacterStats.ApplyStats(_so.StatLevels, _so.LevelStatsConfigSO.LevelConfigs);
            }
        }
    }
}
#endif