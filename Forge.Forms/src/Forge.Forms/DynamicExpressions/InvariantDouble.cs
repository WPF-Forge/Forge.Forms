using System.Globalization;

namespace Forge.Forms.DynamicExpressions
{
    internal static class InvariantDouble
    {
        private const NumberStyles InvariantNumberStyles =
            NumberStyles.AllowLeadingWhite
            | NumberStyles.AllowTrailingWhite
            | NumberStyles.AllowLeadingSign
            | NumberStyles.AllowDecimalPoint;

        public static bool TryParse(string s, out double result)
        {
            return double.TryParse(s, InvariantNumberStyles, CultureInfo.InvariantCulture, out result);
        }
    }
}
