namespace Common.Maths
{
    public static partial class Calculate
    {
        /// <summary>
        /// Determines the number of combinations of a certain size within a set.
        /// </summary>
        /// <param name="sizeOfSet">The size of the set.</param>
        /// <param name="sizeOfCombinations">The size of each combination.</param>
        /// <returns></returns>
        public static ulong NumberOfCombinations(int sizeOfSet, int sizeOfCombinations)
        {
            return Factorial(sizeOfSet) / (Factorial(sizeOfCombinations) * Factorial(sizeOfSet - sizeOfCombinations));
        }
    }
}
