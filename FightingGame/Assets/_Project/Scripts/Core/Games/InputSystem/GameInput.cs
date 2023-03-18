namespace Network
{
    public class GameInput
    {
        public const int UP_BYTE = (1 << 1);
        public const int DOWN_BYTE = (1 << 2);
        public const int LEFT_BYTE = (1 << 3);
        public const int RIGHT_BYTE = (1 << 4);
        public const int LIGHT_BYTE = (1 << 5);
        public const int HEAVY_BYTE = (1 << 6);
        public const int SKILL1_BYTE = (1 << 7);
        public const int SKILL2_BYTE = (1 << 8);
        public const int DASH_FORWARD_BYTE = (1 << 9);
        public const int DASH_BACKWARD_BYTE = (1 << 10);

        public static bool ONE_UP { get; set; }
        public static bool ONE_DOWN { get; set; }
        public static bool ONE_LEFT { get; set; }
        public static bool ONE_RIGHT { get; set; }
        public static bool ONE_LIGHT { get; set; }
        public static bool ONE_HEAVY { get; set; }
        public static bool ONE_SKILL1 { get; set; }
        public static bool ONE_SKILL2 { get; set; }
        public static bool ONE_DASH_FORWARD { get; set; }
        public static bool ONE_DASH_BACKWARD { get; set; }

        public static long GetInput()
        {
            long input = 0;
            if (ONE_UP)
            {
                input |= UP_BYTE;
                ONE_UP = false;
            }
            if (ONE_DOWN)
            {
                input |= DOWN_BYTE;
            }
            if (ONE_LEFT)
            {
                input |= LEFT_BYTE;
            }
            if (ONE_RIGHT)
            {
                input |= RIGHT_BYTE;
            }
            if (ONE_LIGHT)
            {
                input |= LIGHT_BYTE;
                ONE_LIGHT = false;
            }
            if (ONE_HEAVY)
            {
                input |= HEAVY_BYTE;
                ONE_HEAVY = false;
            }
            if (ONE_SKILL1)
            {
                input |= SKILL1_BYTE;
                ONE_SKILL1 = false;
            }
            if (ONE_SKILL2)
            {
                input |= SKILL2_BYTE;
                ONE_SKILL2 = false;
            }
            if (ONE_DASH_FORWARD)
            {
                input |= DASH_FORWARD_BYTE;
                ONE_DASH_FORWARD = false;
            }
            if (ONE_DASH_BACKWARD)
            {
                input |= DASH_BACKWARD_BYTE;
                ONE_DASH_BACKWARD = false;
            }
            return input;
        }

        public static void ParseInputs(long inputs,
            out bool up, out bool down, out bool left, out bool right,
            out bool light, out bool heavy, out bool skill1, out bool skill2,
            out bool dashForward, out bool dashBackward)
        {
            up = (inputs & UP_BYTE) != 0;
            down = (inputs & DOWN_BYTE) != 0;
            left = (inputs & LEFT_BYTE) != 0;
            right = (inputs & RIGHT_BYTE) != 0;
            light = (inputs & LIGHT_BYTE) != 0;
            heavy = (inputs & HEAVY_BYTE) != 0;
            skill1 = (inputs & SKILL1_BYTE) != 0;
            skill2 = (inputs & SKILL2_BYTE) != 0;
            dashForward = (inputs & DASH_FORWARD_BYTE) != 0;
            dashBackward = (inputs & DASH_BACKWARD_BYTE) != 0;
        }
    }
}