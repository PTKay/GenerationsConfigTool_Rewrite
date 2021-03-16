namespace ConfigurationTool.Model.Input
{
    class ButtonConfiguration
    {
        public int A = 31;
        public int X = 30;
        public int Y = 17;
        public int B = 32;

        public int Start = 28;
        public int Back = 14;

        public int LB = 16;
        public int RB = 18;

        public int LT = 2;
        public int RT = 4;

        public int Up = 200;
        public int Down = 208;
        public int Right = 205;
        public int Left = 203;

        public int Unknown0 = 0;
        public int Unknown1 = 0;
        public int Unknown2 = 0;
        public int Unknown3 = 0;
        public int Unknown4 = 2;

        public static ButtonConfiguration DeSerialize(string buttons)
        {
            ButtonConfiguration toReturn = new ButtonConfiguration();

            string[] split = buttons.Split(' ');
            toReturn.A = int.Parse(split[0]);
            toReturn.X = int.Parse(split[1]);
            toReturn.Y = int.Parse(split[2]);
            toReturn.B = int.Parse(split[3]);

            toReturn.Start = int.Parse(split[4]);
            toReturn.Back = int.Parse(split[5]);

            toReturn.LB = int.Parse(split[6]);
            toReturn.RB = int.Parse(split[7]);

            toReturn.LT = int.Parse(split[8]);
            toReturn.RT = int.Parse(split[9]);

            toReturn.Up = int.Parse(split[10]);
            toReturn.Down = int.Parse(split[11]);
            toReturn.Right = int.Parse(split[12]);
            toReturn.Left = int.Parse(split[13]);

            return toReturn;
        }

        public override string ToString()
        {
            return $"{A} {X} {Y} {B} {Start} {Back} {LB} {RB} {LT} {RT} {Up} {Down} {Right} {Left} {Unknown0} {Unknown1} {Unknown2} {Unknown3} {Unknown4}";
        }
    }
}
