namespace Common.Maths
{
    public static partial class Calculate
    {
        public static ulong NumberOfPermutations(int sizeOfSet, int sizeOfPermutations)
        {
            return Factorial(sizeOfSet) / Factorial(sizeOfSet - sizeOfPermutations);
        }
    }
}
