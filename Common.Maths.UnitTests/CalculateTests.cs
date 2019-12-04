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
    }
}
