namespace ConfigurationTool.Model.Input
{
    class AxisMap
    {
        readonly int YPositive = 0;
        readonly int YNegative = 0;
        readonly int XPositive = 0;
        readonly int XNegative = 0;

        public override string ToString()
        {
            return $"{YPositive} {YNegative} {XPositive} {XNegative} ";
        }
    }
}
