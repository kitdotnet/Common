namespace Common.Maths
{
    /// <summary>
    /// Utility class for common calculations.
    /// </summary>
    public static partial class Calculate
    {
        /// <summary>
        /// Determine the greatest common divisor between two numbers.
        /// </summary>
        /// <param name="a">The first number.</param>
        /// <param name="b">The second number.</param>
        /// <returns>The greatest common divisor between <paramref name="a"/> and <paramref name="b"/>.</returns>
        public static ulong Gcd(ulong a, ulong b)
        {
            if (b == 0) { return a; }
            return Gcd(b, a % b);
        }

        /// <summary>
        /// Determine the greatest common divisor among an array of numbers.
        /// </summary>
        /// <param name="numbers">The array of numbers to evaluate.</param>
        /// <returns>The greatest common divisor from the collection of numbers.</returns>
        public static ulong Gcd(ulong[] numbers)
        {
            ulong result = numbers[0];
            for (int i = 1; i < numbers.Length; i++)
            {
                result = Gcd(numbers[i], result);

                if (result == 1)
                {
                    return 1;
                }
            }

            return result;
        }
    }
}
