using System;
using System.Reflection;
using System.Text.RegularExpressions;

using Xunit;

namespace Sprache.Tests
{
    /// <summary>
    /// These tests exist in order to verify that the modification that is applied to
    /// the regex passed to every call to the <see cref="Parse.Regex(string,string)"/>
    /// or <see cref="Parse.Regex(Regex,string)"/> methods does not change the results
    /// in any way.
    /// </summary>
    public class RegexTests
    {
        private const string StartsWithCarrot = "^([a-z]{3})([0-9]{3})$";
        private const string Alternation = "(this)|(that)|(the other)";

        private static readonly MethodInfo OptimizeRegexMethod = typeof(Parse).GetMethod("OptimizeRegex", BindingFlags.NonPublic | BindingFlags.Static);

        [Fact]
        public void OptimizedRegexIsNotSuccessfulWhenTheMatchIsNotAtTheBeginningOfTheInput()
        {
            var regexOriginal = new Regex("[a-z]+");
            var regexOptimized = OptimizeRegex(regexOriginal);

            const string input = "123abc";

            //Assert.That(regexOriginal.IsMatch(input), Is.True);
            //Assert.That(regexOptimized.IsMatch(input), Is.False);

            Assert.True(regexOriginal.IsMatch(input));
            Assert.False(regexOptimized.IsMatch(input));
        }

        [Fact]
        public void OptimizedRegexIsSuccessfulWhenTheMatchIsAtTheBeginningOfTheInput()
        {
            var regexOriginal = new Regex("[a-z]+");
            var regexOptimized = OptimizeRegex(regexOriginal);

            const string input = "abc123";

            //Assert.That(regexOriginal.IsMatch(input), Is.True);
            //Assert.That(regexOptimized.IsMatch(input), Is.True);

            Assert.True(regexOriginal.IsMatch(input));
            Assert.True(regexOptimized.IsMatch(input));
        }

        //[TestCase(_startsWithCarrot, RegexOptions.None, "abc123", TestName = "Starts with ^, no options, success")]
        //[TestCase(_startsWithCarrot, RegexOptions.ExplicitCapture, "abc123", TestName = "Starts with ^, explicit capture, success")]
        //[TestCase(_startsWithCarrot, RegexOptions.None, "123abc", TestName = "Starts with ^, no options, failure")]
        //[TestCase(_startsWithCarrot, RegexOptions.ExplicitCapture, "123abc", TestName = "Starts with ^, explicit capture, failure")]

        //[TestCase(_alternation, RegexOptions.None, "abc123", TestName = "Alternation, no options, success")]
        //[TestCase(_alternation, RegexOptions.ExplicitCapture, "that", TestName = "Alternation, explicit capture, success")]
        //[TestCase(_alternation, RegexOptions.None, "that", TestName = "Alternation, no options, failure")]
        //[TestCase(_alternation, RegexOptions.ExplicitCapture, "that", TestName = "Alternation, explicit capture, failure")]
        [Theory]
        [InlineData(StartsWithCarrot, RegexOptions.None, "abc123", "Starts with ^, no options, success")]
        [InlineData(StartsWithCarrot, RegexOptions.ExplicitCapture, "abc123", "Starts with ^, explicit capture, success")]
        [InlineData(StartsWithCarrot, RegexOptions.None, "123abc", "Starts with ^, no options, failure")]
        [InlineData(StartsWithCarrot, RegexOptions.ExplicitCapture, "123abc", "Starts with ^, explicit capture, failure")]

        [InlineData(Alternation, RegexOptions.None, "abc123", "Alternation, no options, success")]
        [InlineData(Alternation, RegexOptions.ExplicitCapture, "abc123", "Alternation, explicit capture, success")]
        [InlineData(Alternation, RegexOptions.None, "123abc", "Alternation, no options, failure")]
        [InlineData(Alternation, RegexOptions.ExplicitCapture, "123abc", "Alternation, explicit capture, failure")]
        public void RegexOptimizationDoesNotChangeRegexBehavior(string pattern, RegexOptions options, string input, string testName)
        {
            var regexOriginal = new Regex(pattern, options);
            var regexOptimized = OptimizeRegex(regexOriginal);

            var matchOriginal = regexOriginal.Match(input);
            var matchModified = regexOptimized.Match(input);

            //Assert.That(matchModified.Success, Is.EqualTo(matchOriginal.Success));
            //Assert.That(matchModified.Value, Is.EqualTo(matchOriginal.Value));
            //Assert.That(matchModified.Groups.Count, Is.EqualTo(matchOriginal.Groups.Count));


            Assert.Equal(matchModified.Success, matchOriginal.Success);
            Assert.Equal(matchModified.Value, matchOriginal.Value);
            Assert.Equal(matchModified.Groups.Count, matchOriginal.Groups.Count);


            for (int i = 0; i < matchModified.Groups.Count; i++)
            {
                //Assert.That(matchModified.Groups[i].Success, Is.EqualTo(matchOriginal.Groups[i].Success));
                //Assert.That(matchModified.Groups[i].Value, Is.EqualTo(matchOriginal.Groups[i].Value));


                Assert.Equal(matchModified.Groups[i].Success, matchOriginal.Groups[i].Success);
                Assert.Equal(matchModified.Groups[i].Value, matchOriginal.Groups[i].Value);
            }
        }

        /// <summary>
        /// Calls the <see cref="Parse.OptimizeRegex"/> method via reflection.
        /// </summary>
        private static Regex OptimizeRegex(Regex regex)
        {
            // Reflection isn't the best way of verifying behavior,
            // but cluttering the public api sucks worse.

            if (OptimizeRegexMethod == null)
            {
                throw new Exception("Unable to locate a private static method named " +
                                    "\"OptimizeRegex\" in the Parse class. Has it been renamed?");
            }

            var optimizedRegex = (Regex)OptimizeRegexMethod.Invoke(null, new object[] { regex });
            return optimizedRegex;
        }
    }
}
