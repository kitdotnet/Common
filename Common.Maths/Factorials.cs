namespace Common.Maths
{
    public static partial class Calculate
    {
        /// <summary>
        /// Gets the factorial value of <paramref name="number"/>.
        /// </summary>
        /// <param name="number">The number to evaluate.</param>
        /// <returns>The factorial value of <paramref name="number"/>.</returns>
        public static ulong Factorial(int number)
        {
            ulong result = (ulong)number;
            if (number == 0) { return 1L; }

            for (int i = number - 1; i >= 1; i--)
            {
                result *= (ulong)i;
            }
            
            return result;
        }
    }
}
