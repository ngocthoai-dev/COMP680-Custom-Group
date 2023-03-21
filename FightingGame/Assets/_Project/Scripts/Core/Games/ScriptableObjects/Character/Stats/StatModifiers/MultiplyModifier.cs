namespace Core.SO
{
    public class MultiplyModifier : IStatModifier
    {
        public float Value { get; set; }

        public MultiplyModifier(float value)
        {
            Value = value;
        }

        public float ApplyModify(float baseValue)
        {
            return (baseValue * Value);
        }
    }
}