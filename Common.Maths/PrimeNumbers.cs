using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Maths
{
    /// <summary>
    /// Utility class for common calculations.
    /// </summary>
    public static partial class Calculate
    {
        /// <summary>
        /// Determine if a number is prime.
        /// </summary>
        /// <param name="number">The number to evaluate.</param>
        /// <returns>An indicator of whether <paramref name="number"/> is prime.</returns>
        public static bool IsPrime(ulong number)
        {
            if (number == 2) { return true; }
            if (number <= 1 || number % 2 == 0) { return false; }

            ulong boundary = Convert.ToUInt64(Math.Floor(Math.Sqrt(number)));

            for (ulong i = 3; i <= boundary; i += 2)
            {
                if (number % i == 0) { return false; }
            }

            return true;
        }

        /// <summary>
        /// Get all of the prime numbers up to <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="maxValue">The highest possible value returned.</param>
        /// <returns>A collection of primes up to <paramref name="maxValue"/>.</returns>
        public static IEnumerable<ulong> GetPrimes(ulong maxValue)
        {
            if (maxValue >= 2)
            {
                yield return 2;
            }

            for (ulong i = 3; i <= maxValue; i += 2)
            {
                if (IsPrime(i)) { yield return i; }
            }
        }

        /// <summary>
        /// Gets a <see cref="IDictionary{TKey, TValue}"/> of prime factors for the
        /// number provided.
        /// </summary>
        /// <param name="number">The number to evaluate.</param>
        /// <returns>A <see cref="IDictionary{TKey, TValue}"/> wherein the keys are prime
        /// numbers and the values are the counts for those primes.</returns>
        /// <example>
        /// If <paramref name="number"/> is 3, the result would be a dictionary with one
        /// <see cref="KeyValuePair{TKey, TValue}"/> where the key is 3 and the value is 2.
        /// </example>
        public static IDictionary<ulong, int> GetPrimeFactorsDictionary(ulong number)
        {
            IDictionary<ulong, int> results = new SortedDictionary<ulong, int>();

            if (number <= 1L) { return results; }

            ulong[] primes = GetPrimes(number).ToArray();

            if (primes.Length == 0) { return results; }

            ulong quotient = number;
            int primeIndex = 0;

            while (true)
            {
                if (primes[primeIndex] > quotient) { break; }
                while (quotient % primes[primeIndex] == 0)
                {
                    ulong divisor = quotient / primes[primeIndex];
                    if (results.ContainsKey(primes[primeIndex]))
                    {
                        results[primes[primeIndex]]++;
                    }
                    else
                    {
                        results.Add(primes[primeIndex], 1);
                    }
                    quotient = divisor;
                }

                if (primeIndex == primes.Length - 1)
                {
                    break;
                }
                primeIndex++;
            }

            return results;
        }

        /// <summary>
        /// Get prime factors for a number as a collection.
        /// </summary>
        /// <param name="number">The number to evaluate.</param>
        /// <returns></returns>
        public static IEnumerable<ulong> GetPrimeFactors(ulong number)
        {
            IDictionary<ulong, int> pairs = GetPrimeFactorsDictionary(number);
            foreach (var kvp in pairs)
            {
                for (int i = 0; i < kvp.Value; i++)
                {
                    yield return kvp.Key;
                }
            }
        }
    }
}
