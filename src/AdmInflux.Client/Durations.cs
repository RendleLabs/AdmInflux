using System;
using System.Text;

namespace AdmInflux.Client
{
    public static class Durations
    {
        public static bool TryParse(string duration, out TimeSpan timeSpan)
        {
            timeSpan = TimeSpan.Zero;

            var chars = duration.AsSpan();

            int s = 0;
            int l = 0;
            
            for (int i = 0; i < chars.Length; i++)
            {
                if (char.IsDigit(chars[i]))
                {
                    l++;
                    continue;
                }

                if (l == 0)
                {
                    return false;
                }

                var readOnlySpan = chars.Slice(s, l).ToString();
                switch (chars[i])
                {
                    case 'w':
                        if (int.TryParse(readOnlySpan, out int weeks))
                        {
                            timeSpan = timeSpan.Add(TimeSpan.FromDays(weeks * 7));
                        }
                        else
                        {
                            return false;
                        }

                        s = i + 1;
                        l = 0;

                        break;
                    case 'd':
                        if (int.TryParse(readOnlySpan, out int days))
                        {
                            timeSpan = timeSpan.Add(TimeSpan.FromDays(days));
                        }
                        else
                        {
                            return false;
                        }

                        s = i + 1;
                        l = 0;

                        break;
                    case 'h':
                        if (int.TryParse(readOnlySpan, out int hours))
                        {
                            timeSpan = timeSpan.Add(TimeSpan.FromHours(hours));
                        }
                        else
                        {
                            return false;
                        }

                        s = i + 1;
                        l = 0;

                        break;
                    case 'm':
                        if (chars.Length > i + 1 && chars[i + 1] == 's')
                        {
                            if (int.TryParse(readOnlySpan, out int milliseconds))
                            {
                                timeSpan = timeSpan.Add(TimeSpan.FromMilliseconds(milliseconds));
                            }
                            else
                            {
                                return false;
                            }

                            ++i;
                            s = i + 1;
                            l = 0;

                            break;
                        }

                        if (int.TryParse(readOnlySpan, out int minutes))
                        {
                            timeSpan = timeSpan.Add(TimeSpan.FromMinutes(minutes));
                        }
                        else
                        {
                            return false;
                        }

                        s = i + 1;
                        l = 0;

                        break;
                    case 's':
                        if (int.TryParse(readOnlySpan, out int seconds))
                        {
                            timeSpan = timeSpan.Add(TimeSpan.FromSeconds(seconds));
                        }
                        else
                        {
                            return false;
                        }

                        s = i + 1;
                        l = 0;

                        break;
                    case 'u':
                        if (double.TryParse(readOnlySpan, out double microseconds))
                        {
                            timeSpan = timeSpan.Add(TimeSpan.FromMilliseconds(microseconds / 1000));
                        }
                        else
                        {
                            return false;
                        }

                        s = i + 1;
                        l = 0;

                        break;
                    case 'n':
                        if (chars.Length > i + 1 && chars[i + 1] == 's')
                        {
                            if (double.TryParse(readOnlySpan, out double nanoseconds))
                            {
                                timeSpan = timeSpan.Add(TimeSpan.FromMilliseconds(nanoseconds / 1000000));
                            }
                            else
                            {
                                return false;
                            }

                            ++i;
                            s = i + 1;
                            l = 0;
                        }

                        break;
                    default:
                        return false;
                }
            }

            return true;
        }

        public static string ToDurationString(this TimeSpan timeSpan)
        {
            var builder = new StringBuilder(128);
            
            if (timeSpan.Days >= 7)
            {
                var weeks = timeSpan.Days - (timeSpan.Days % 7);
                builder.Append($"{weeks/7}w");
                timeSpan = timeSpan.Subtract(TimeSpan.FromDays(weeks));
            }
            
            AppendIf(builder, timeSpan.Days, "d");
            AppendIf(builder, timeSpan.Hours, "h");
            AppendIf(builder, timeSpan.Minutes, "m");
            AppendIf(builder, timeSpan.Seconds, "s");
            AppendIf(builder, (int)timeSpan.GetMilliseconds(), "ms");
            AppendIf(builder, (int)timeSpan.GetMicroseconds(), "u");
            AppendIf(builder, (int)timeSpan.GetNanoseconds(), "ns");
            return builder.ToString();
        }

        private static void AppendIf(StringBuilder builder, int value, string suffix)
        {
            if (value > 0)
            {
                builder.Append($"{value}{suffix}");
            }
        }
    }

    internal static class TimeSpanExtensions
    {
        public static double GetMilliseconds(this TimeSpan timeSpan)
        {
            return timeSpan.TotalMilliseconds % 1000d;
        }

        public static double GetMicroseconds(this TimeSpan timeSpan)
        {
            return (timeSpan.GetMilliseconds() * 1000d) % 1000d;
        }

        public static double GetNanoseconds(this TimeSpan timeSpan)
        {
            return (timeSpan.GetMicroseconds() * 1000d) % 1000d;
        }
    }
}