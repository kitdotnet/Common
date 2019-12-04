using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Maths
{
    public static partial class Calculate
    {
        /// <summary>
        /// Get the first <paramref name="numberToReturn"/> numbers in the Fibonacci series.
        /// </summary>
        /// <param name="numberToReturn">The number if items in the series to return.</param>
        /// <returns>The first <paramref name="numberToReturn"/> numbers in the Fibonacci series.</returns>
        public static IEnumerable<ulong> GetFibonacciNumbers(ulong numberToReturn)
        {
            if (numberToReturn > 0)
            {
                yield return 0L;

                if (numberToReturn > 1)
                {
                    yield return 1L;
                }

                ulong[] numbers = new ulong[2] { 0L, 1L };

                for (ulong i = 2; i < numberToReturn; i++)
                {
                    ulong current = numbers[0] + numbers[1];
                    yield return current;

                    numbers[0] = numbers[1];
                    numbers[1] = current;
                }
            }
        }

        /// <summary>
        /// The Fibonacci number in the <paramref name="position"/> position.
        /// The first position is 1, not 0.
        /// </summary>
        /// <param name="position">The position in the Fibonacci series to return.</param>
        /// <returns>The Fibonacci number in the <paramref name="position"/> position.</returns>
        public static ulong GetFibonacciNumber(ulong position)
        {
            if (position < 1) { throw new ArgumentException($"{nameof(position)} must be a positve number"); }
            return GetFibonacciNumbers(position).LastOrDefault();
        }
    }
}
