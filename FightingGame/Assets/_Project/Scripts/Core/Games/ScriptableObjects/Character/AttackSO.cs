using UnityEngine;

namespace Core.SO
{
    [CreateAssetMenu(fileName = "Attack", menuName = "Scriptable Objects/Attack", order = 2)]
    public class AttackSO : ScriptableObject
    {
        [Header("Identity")]
        public int Index = 0;

        public int MpCost = 0;
        public GameObject HitEffect = null;
        public GameObject SkillEffect = null;

        [Header("Properties")]
        public float BaseAttack = 0;

        public float HeroAttackModifier = 1;
        [Range(0, 1)]
        public float IgnoreArmorPercent = 0;

        public KnockType KnockType = KnockType.Not;
        public bool IsSpawnOnEnemy = false;

        [Header("Sounds")]
        public AudioClip AttackSound = null;

        public CameraShakerSO CameraShaker = null;
    }
}