using Core.Utility;
using UnityEngine;

namespace Core.SO
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "Scriptable Objects/Character Config", order = 1)]
    public class CharacterConfigSO : ScriptableObject
    {
        [Header("Identity")]
        public int Index = 0;

        public string CharacterName = "Name";
        public GameObject Prefab = null;
        public AnimatorOverrideController Controller = null;
        public Sprite Thumbnail = null;
        public Color Color = Color.white;

        [Header("Attacks")]
        public AttackSO Light1 = null;

        public AttackSO Light2 = null;
        public AttackSO Light3 = null;
        public AttackSO Heavy = null;
        public AttackSO Skill1 = null;
        public AttackSO Skill2 = null;

        public AttackSO[] AttackSOs => new AttackSO[] { Light1, Light2, Light3, Heavy, Skill1, Skill2 };

        [Header("Stats")]
        public SerializableDictionary<StatType, int> StatLevels = new()
        {
            { StatType.HP, 1 },
            { StatType.MP, 1 },
            { StatType.MPRegen, 1 },
            { StatType.Arm, 1 },
            { StatType.MSpd, 1 },
            { StatType.JForce, 1 },
            { StatType.ASpd, 1 },
            { StatType.Att, 1 },
        };

        [DebugOnly] public ItemStats CharacterStats = new();
        public LevelStatsConfigSO LevelStatsConfigSO = null;
        public bool CanDoubleJump = true;

        public CharacterConfigSO ApplyStats()
        {
            CharacterStats.ApplyStats(StatLevels, LevelStatsConfigSO.LevelConfigs);
            return this;
        }

        public CharacterConfigSO Clone()
        {
            return (CharacterConfigSO)MemberwiseClone();
        }
    }
}