using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace Common.Maths.UnitTests
{
    public class CalculateTests
    {
        [Theory]
        [InlineData("2008-02-29", "2012-02-29", 4)]
        [InlineData("2008-02-29", "2009-02-28", 0)]
        [InlineData("2008-02-29", "2009-03-01", 1)]
        [InlineData("1970-01-01", "1970-12-31", 0)]
        [InlineData("1970-01-01", "1971-01-01", 1)]
        [InlineData("1971-01-01", "1970-01-01", -1)]
        [InlineData("2012-02-29", "2008-02-29", -4)]
        public void Age(string birthDateString, string fromDateString, int expected)
        {
            DateTime birthDate = DateTime.ParseExact(birthDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            DateTime fromDate = DateTime.ParseExact(fromDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            Assert.Equal(expected, Calculate.AgeInYears(birthDate, fromDate));
        }

        [Theory]
        [InlineData(0, false)]
        [InlineData(1, false)]
        [InlineData(2, true)]
        [InlineData(3, true)]
        [InlineData(4, false)]
        [InlineData(5, true)]
        [InlineData(6, false)]
        [InlineData(7, true)]
        [InlineData(8, false)]
        [InlineData(9, false)]
        [InlineData(10, false)]
        [InlineData(11, true)]
        [InlineData(19, true)]
        [InlineData(647, true)]
        [InlineData(2477, true)]
        public void IsPrime(uint number, bool isPrime)
        {
            if (isPrime)
            {
                Assert.True(Calculate.IsPrime(number));
            }
            else
            {
                Assert.False(Calculate.IsPrime(number));
            }
        }

        [Fact]
        public void GetPrimes()
        {
            var actuals = Calculate.GetPrimes(0);
            Assert.Empty(actuals);
            actuals = Calculate.GetPrimes(1);
            Assert.Empty(actuals);
            actuals = Calculate.GetPrimes(2);
            Assert.True(new List<ulong>() { 2L }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimes(3);
            Assert.True(new List<ulong>() { 2L, 3L }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimes(4);
            Assert.True(new List<ulong>() { 2L, 3L }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimes(5);
            Assert.True(new List<ulong>() { 2L, 3L, 5L }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimes(6);
            Assert.True(new List<ulong>() { 2L, 3L, 5L }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimes(7);
            Assert.True(new List<ulong>() { 2L, 3L, 5L, 7L }.SequenceEqual(actuals));
        }

        [Fact]
        public void GetPrimeFactorsDictionary()
        {
            var actuals = Calculate.GetPrimeFactorsDictionary(1);
            Assert.Empty(actuals);
            actuals = Calculate.GetPrimeFactorsDictionary(2);
            Assert.True(new Dictionary<ulong, int>() { { 2L, 1 } }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactorsDictionary(3);
            Assert.True(new Dictionary<ulong, int>() { { 3L, 1 } }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactorsDictionary(4);
            Assert.True(new Dictionary<ulong, int>() { { 2L, 2 } }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactorsDictionary(5);
            Assert.True(new Dictionary<ulong, int>() { { 5L, 1 } }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactorsDictionary(6);
            Assert.True(new Dictionary<ulong, int>() { { 2L, 1 }, { 3L, 1 } }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactorsDictionary(8);
            Assert.True(new Dictionary<ulong, int>() { { 2L, 3 } }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactorsDictionary(9);
            Assert.True(new Dictionary<ulong, int>() { { 3L, 2 } }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactorsDictionary(10);
            Assert.True(new Dictionary<ulong, int>() { { 2L, 1 }, { 5L, 1 } }.SequenceEqual(actuals));
        }

        [Fact]
        public void GetPrimeFactors()
        {
            var actuals = Calculate.GetPrimeFactors(1);
            Assert.Empty(actuals);
            actuals = Calculate.GetPrimeFactors(2);
            Assert.True(new List<ulong> { 2L }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactors(3);
            Assert.True(new List<ulong>() { 3L }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactors(4);
            Assert.True(new List<ulong>() { 2L, 2L }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactors(5);
            Assert.True(new List<ulong>() { { 5L } }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactors(6);
            Assert.True(new List<ulong>() { 2L, 3L }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactors(8);
            Assert.True(new List<ulong>() { 2L, 2L, 2L }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactors(9);
            Assert.True(new List<ulong>() { 3L, 3L }.SequenceEqual(actuals));
            actuals = Calculate.GetPrimeFactors(10);
            Assert.True(new List<ulong>() { 2L, 5L }.SequenceEqual(actuals));
        }

        [Fact]
        public void GcdTwoNumbers()
        {
            Assert.Equal<ulong>(5L, Calculate.Gcd(10L, 5L));
            Assert.Equal<ulong>(12L, Calculate.Gcd(12L, 12L));
            Assert.Equal<ulong>(6L, Calculate.Gcd(12L, 18L));
        }

        [Fact]
        public void GcdArrayOfNumbers()
        {
            Assert.Equal<ulong>(5L, Calculate.Gcd(new ulong[] { 10L, 5L, 15L }));
            Assert.Equal<ulong>(12L, Calculate.Gcd(new ulong[] { 12L, 12L, 36L }));
            Assert.Equal<ulong>(3L, Calculate.Gcd(new ulong[] { 3L, 12L, 15L, 9L }));
            Assert.Equal<ulong>(0L, Calculate.Gcd(new ulong[] { 0L, 0L }));
        }

        [Theory]
        [InlineData(2, 4, 4)]
        [InlineData(2, 9, 18)]
        [InlineData(3, 12, 12)]
        [InlineData(18, 24, 72)]
        [InlineData(21, 6, 42)]
        [InlineData(0, 6, 0)]
        [InlineData(6, 0, 0)]
        [InlineData(0, 0, 0)]
        public void LcmTwoNumbers(ulong a, ulong b, ulong expected)
        {
            Assert.Equal(expected, Calculate.Lcm(a, b));
        }

        [Fact]
        public void LcmArrayOfNumbers()
        {
            Assert.Equal<ulong>(8L, Calculate.Lcm(new ulong[] { 2L, 4L, 8L }));
            Assert.Equal<ulong>(27L, Calculate.Lcm(new ulong[] { 3L, 9L, 27L }));
            Assert.Equal<ulong>(260L, Calculate.Lcm(new ulong[] { 4L, 5L, 13L }));
            Assert.Equal<ulong>(4L, Calculate.Lcm(new ulong[] { 2L, 4L }));
            Assert.Equal<ulong>(2L, Calculate.Lcm(new ulong[] { 2L }));
            Assert.Equal<ulong>(0L, Calculate.Lcm(new ulong[] { 0L }));
            Assert.Equal<ulong>(0L, Calculate.Lcm(new ulong[] { 0L, 0L }));
        }

        [Fact]
        public void GetFibonacciNumbers()
        {
            var fibs = Calculate.GetFibonacciNumbers(6);
            var expected = new List<ulong>() { 0L, 1L, 1L, 2L, 3L, 5L };
            Assert.True(expected.SequenceEqual(fibs));

            Assert.Empty(Calculate.GetFibonacciNumbers(0));
        }

        [Fact]
        public void GetFibonacciNumber()
        {
            Assert.Throws<ArgumentException>(() => Calculate.GetFibonacciNumber(0));

            Assert.Equal<ulong>(0, Calculate.GetFibonacciNumber(1));
            Assert.Equal<ulong>(1, Calculate.GetFibonacciNumber(2));
            Assert.Equal<ulong>(1, Calculate.GetFibonacciNumber(3));
            Assert.Equal<ulong>(2, Calculate.GetFibonacciNumber(4));
            Assert.Equal<ulong>(3, Calculate.GetFibonacciNumber(5));
            Assert.Equal<ulong>(5, Calculate.GetFibonacciNumber(6));
            Assert.Equal<ulong>(8, Calculate.GetFibonacciNumber(7));
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(3, 6)]
        [InlineData(4, 24)]
        [InlineData(5, 120)]
        public void Factorial(int number, ulong result)
        {
            Assert.Equal(result, Calculate.Factorial(number));
        }

        [Theory]
        [InlineData(3,2,6)]
        [InlineData(5, 5, 120)]
        public void Permutations(int sizeOfSet, int sizeOfPermutations, ulong result)
        {
            Assert.Equal(result, Calculate.NumberOfPermutations(sizeOfSet, sizeOfPermutations));
        }

        [Theory]
        [InlineData(3, 2, 3)]
        [InlineData(4, 3, 4)]
        public void Combinations(int sizeOfSet, int sizeOfCombinations, ulong result)
        {
            Assert.Equal(result, Calculate.NumberOfCombinations(sizeOfSet, sizeOfCombinations));
        }
    }
}
