using Core.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace Core.SO
{
    [CreateAssetMenu(fileName = "LevelStatsConfig", menuName = "Scriptable Objects/Level Stats Config", order = 2)]
    public class LevelStatsConfigSO : ScriptableObject
    {
        public SerializableDictionary<StatType, List<float>> LevelConfigs = new()
        {
            { StatType.HP, new List<float>{
                100,
                120,
                150,
                180,
                200
            } },
            { StatType.MP, new List<float>{
                60,
                80,
                100,
                120,
                140
            } },
            { StatType.MPRegen, new List<float>{
                0.8f,
                1f,
                1.15f,
                1.2f,
                1.25f
            } },
            { StatType.Arm, new List<float>{
                5,
                8,
                12,
                14,
                16
            } },
            { StatType.MSpd, new List<float>{
                2.8f,
                3f,
                3.2f,
                3.4f
            } },
            { StatType.JForce, new List<float>{
                5.6f,
                6f,
                6.2f
            } },
            { StatType.ASpd, new List<float>{
                0.8f,
                1f,
                1.1f,
                1.2f,
                1.3f
            } },
            { StatType.Att, new List<float>{
                12,
                14,
                15,
                16,
                18
            } },
        };
    }
}