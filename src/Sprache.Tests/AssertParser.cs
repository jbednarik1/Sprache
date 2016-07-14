using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;
using Xunit.Sdk;

namespace Sprache.Tests
{
    internal static class AssertParser
    {
        public static void SucceedsWithOne<T>(Parser<IEnumerable<T>> parser, string input, T expectedResult)
        {
            SucceedsWith(parser, input, t =>
            {
                IEnumerable<T> ts = t as T[] ?? t.ToArray();
                Assert.Equal(1, ts.Count());
                Assert.Equal(expectedResult, ts.Single());
            });
        }

        public static void SucceedsWithMany<T>(Parser<IEnumerable<T>> parser, string input,
            IEnumerable<T> expectedResult)
        {
            SucceedsWith(parser, input, t => Assert.True(t.SequenceEqual(expectedResult)));
        }

        public static void SucceedsWithAll(Parser<IEnumerable<char>> parser, string input)
        {
            SucceedsWithMany(parser, input, input.ToCharArray());
        }

        public static void SucceedsWith<T>(Parser<T> parser, string input, Action<T> resultAssertion)
        {
            parser.TryParse(input)
                .IfFailure(f => { throw new XunitException($"Parsing of \"{input}\" failed unexpectedly. {f}"); })
                .IfSuccess(s =>
                {
                    resultAssertion(s.Value);
                    return s;
                });
        }

        public static void Fails<T>(Parser<T> parser, string input)
        {
            FailsWith(parser, input, f => { });
        }

        public static void FailsAt<T>(Parser<T> parser, string input, int position)
        {
            FailsWith(parser, input, f => Assert.Equal(position, f.Remainder.Position));
        }

        public static void FailsWith<T>(Parser<T> parser, string input, Action<IResult<T>> resultAssertion)
        {
            parser.TryParse(input)
                .IfSuccess<T, T>(s => { throw new XunitException($"Expected failure but succeeded with {s.Value}."); })
                .IfFailure(f =>
                {
                    resultAssertion(f);
                    return f;
                });
        }
    }
}