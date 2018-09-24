using System;
using System.Collections.Generic;
using Xunit;

namespace AdmInflux.Client.Tests
{
    public class DurationTests
    {
        [Theory]
        [MemberData(nameof(SingleValuesData))]
        public void ParsesSingleValuesCorrectly(TimeSpan expected, string duration)
        {
            Assert.Equal(expected, Durations.TryParse(duration, out var actual) ? actual : default);
        }

        public static IEnumerable<object[]> SingleValuesData()
        {
            yield return new object[] {TimeSpan.FromDays(7), "1w"};
            yield return new object[] {TimeSpan.FromDays(70), "10w"};
            yield return new object[] {TimeSpan.FromDays(1), "1d"};
            yield return new object[] {TimeSpan.FromDays(10), "10d"};
            yield return new object[] {TimeSpan.FromHours(1), "1h"};
            yield return new object[] {TimeSpan.FromHours(10), "10h"};
            yield return new object[] {TimeSpan.FromMinutes(1), "1m"};
            yield return new object[] {TimeSpan.FromMinutes(10), "10m"};
            yield return new object[] {TimeSpan.FromSeconds(1), "1s"};
            yield return new object[] {TimeSpan.FromSeconds(10), "10s"};
            yield return new object[] {TimeSpan.FromMilliseconds(1), "1ms"};
            yield return new object[] {TimeSpan.FromMilliseconds(10), "10ms"};
            yield return new object[] {TimeSpan.FromMilliseconds(0.001), "1u"};
            yield return new object[] {TimeSpan.FromMilliseconds(0.01), "10u"};
            yield return new object[] {TimeSpan.FromMilliseconds(0.000001), "1ns"};
            yield return new object[] {TimeSpan.FromMilliseconds(0.00001), "10ns"};
        }
        
        [Theory]
        [MemberData(nameof(CompoundValuesData))]
        public void ParsesCompoundValuesCorrectly(TimeSpan expected, string duration)
        {
            Assert.Equal(expected, Durations.TryParse(duration, out var actual) ? actual : default);
        }

        public static IEnumerable<object[]> CompoundValuesData()
        {
            yield return new object[] {TimeSpan.FromDays(9), "1w2d"};
            yield return new object[] {TimeSpan.FromDays(1.5), "1d12h"};
            yield return new object[] {TimeSpan.FromHours(1.25), "1h15m"};
            yield return new object[] {TimeSpan.FromMinutes(1.5), "1m30s"};
            yield return new object[] {TimeSpan.FromSeconds(1.5), "1s500ms"};
            yield return new object[] {TimeSpan.FromMilliseconds(1.5), "1ms500u"};
            yield return new object[] {TimeSpan.FromMilliseconds(0.0015), "1u500ns"};
        }

        [Theory]
        [MemberData(nameof(FormatSingleValuesData))]
        public void FormatsSingleValuesCorrectly(string expected, TimeSpan timeSpan)
        {
            Assert.Equal(expected, timeSpan.ToDurationString());
        }

        public static IEnumerable<object[]> FormatSingleValuesData()
        {
            yield return new object[] {"1w", TimeSpan.FromDays(7)};
            yield return new object[] {"6d", TimeSpan.FromDays(6)};
            yield return new object[] {"23h", TimeSpan.FromHours(23)};
            yield return new object[] {"59m", TimeSpan.FromMinutes(59)};
            yield return new object[] {"59s", TimeSpan.FromSeconds(59)};
            yield return new object[] {"999ms", TimeSpan.FromMilliseconds(999)};
            yield return new object[] {"999u", TimeSpan.FromMilliseconds(999).Divide(1000d)};
        }
        
        [Theory]
        [MemberData(nameof(FormatCompoundValuesData))]
        public void FormatsCompoundValuesCorrectly(string expected, TimeSpan timeSpan)
        {
            Assert.Equal(expected, timeSpan.ToDurationString());
        }

        public static IEnumerable<object[]> FormatCompoundValuesData()
        {
            yield return new object[] {"1w1d", TimeSpan.FromDays(8)};
            yield return new object[] {"1d1h", TimeSpan.FromHours(25)};
            yield return new object[] {"1h30m", TimeSpan.FromMinutes(90)};
            yield return new object[] {"1m30s", TimeSpan.FromSeconds(90)};
            yield return new object[] {"1s500ms", TimeSpan.FromMilliseconds(1500)};
            yield return new object[] {"1ms999u", TimeSpan.FromMilliseconds(1999).Divide(1000d)};
            yield return new object[] {"1u100ns", TimeSpan.FromMilliseconds(1100).Divide(1000000d)};
        }
    }
}
