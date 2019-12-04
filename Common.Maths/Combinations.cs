using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Maths
{
    public static partial class Calculate
    {
        public static ulong NumberOfCombinations(int sizeOfSet, int sizeOfCombinations)
        {
            return Factorial(sizeOfSet) / (Factorial(sizeOfCombinations) * Factorial(sizeOfSet - sizeOfCombinations));
        }
    }
}
