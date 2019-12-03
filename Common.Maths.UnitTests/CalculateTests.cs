using System;
using System.Globalization;
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
            Assert.Equal(expected, Calculate.Age(birthDate, fromDate));
        }
    }
}
