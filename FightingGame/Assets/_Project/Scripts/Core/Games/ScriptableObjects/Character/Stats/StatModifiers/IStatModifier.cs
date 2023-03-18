namespace Core.SO
{
    public enum StatType
    {
        NONE = -1,
        HP,
        MP,
        MPRegen,
        Arm,
        MSpd,
        JForce,
        ASpd,
        Att,

        Total
    }

    public enum KnockType
    {
        Not,
        Barely,
        Absolute
    }

    public enum AttackTypeIndex
    {
        Light1,
        Light2,
        Light3,
        Heavy,
        Skill1,
        Skill2,
    }

    public enum ScaleDirection
    {
        None,
        Horizontal,
        Vertical,
        Both
    }

    public interface IStatModifier
    {
        float Value { get; set; }

        float ApplyModify(float baseValue);
    }
}