namespace Core.SO
{
    public class AddModifier : IStatModifier
    {
        public float Value { get; set; }

        public AddModifier(float value)
        {
            Value = value;
        }

        public float ApplyModify(float baseValue)
        {
            return Value;
        }
    }
}