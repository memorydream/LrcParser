using System;
using System.Collections.Generic;

namespace LrcParser
{
    public struct Lyric
    {
        public static readonly Lyric Empty = new Lyric(TimeSpan.MinValue, string.Empty);
        public readonly TimeSpan Timestamp;
        public readonly string Content;

        public Lyric(TimeSpan timestamp, string text)
        {
            this.Timestamp = timestamp;
            this.Content = text ?? string.Empty;
        }

        public Lyric(int minutes, int seconds, int milliseconds, string text) :
            this(new TimeSpan(0, 0, minutes, seconds, milliseconds), text)
        {
        }

        public override string ToString()
        {
            return $"[{Timestamp.Minutes:00}:{Timestamp.Seconds:00}.{Timestamp.Milliseconds}]{Content}";
        }
    }

    public static class LyricExtensions
    {
        public static bool IsEmpty(this Lyric lyric) => lyric.Timestamp == Lyric.Empty.Timestamp;
    }

    public static class LyricListExtensions
    {
        private class LyricComparer : IComparer<Lyric>
        {
            public int Compare(Lyric x, Lyric y) => x.Timestamp.CompareTo(y.Timestamp);
        }

        private static readonly LyricComparer comparer = new LyricComparer();

        public static List<Lyric> SortByTimestamp(this List<Lyric> lyrics)
        {
            lyrics.Sort(comparer);
            return lyrics;
        }
    }
}