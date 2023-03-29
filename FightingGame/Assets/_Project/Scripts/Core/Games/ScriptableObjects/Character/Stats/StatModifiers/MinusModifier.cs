namespace Core.SO
{
    public class MinusModifier : IStatModifier
    {
        public float Value { get; set; }

        public MinusModifier(float value)
        {
            Value = value;
        }

        public float ApplyModify(float baseValue)
        {
            return -Value;
        }
    }
}