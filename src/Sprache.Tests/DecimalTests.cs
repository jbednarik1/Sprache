using System;
using System.Globalization;

using Xunit;

namespace Sprache.Tests
{
    public class DecimalTests : IDisposable
    {
        public DecimalTests()
        {
            _previousCulture = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        }

        public void Dispose()
        {
            CultureInfo.CurrentCulture = _previousCulture;
        }

        private static readonly Parser<string> DecimalParser = Parse.Decimal.End();
        private static readonly Parser<string> DecimalInvariantParser = Parse.DecimalInvariant.End();

        private readonly CultureInfo _previousCulture;

        [Fact]
        public void LeadingDigits()
        {
            const string value = "12.23";
            Assert.Equal(value, DecimalParser.Parse(value));
        }

        [Fact]
        public void LeadingDigitsInvariant()
        {
            const string expected = "12.23";
            Assert.Equal(expected, DecimalInvariantParser.Parse(expected));
        }

        [Fact]
        public void Letters()
        {
            const string input = "1A.5";
            Assert.Throws<ParseException>(() => DecimalParser.Parse(input));
        }

        [Fact]
        public void NoLeadingDigits()
        {
            const string value = ".23";
            Assert.Equal(value, DecimalParser.Parse(value));
        }

        [Fact]
        public void TwoPeriods()
        {
            const string input = "1.2.23";
            Assert.Throws<ParseException>(() => DecimalParser.Parse(input));
        }
    }
}