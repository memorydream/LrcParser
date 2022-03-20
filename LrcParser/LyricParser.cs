using System;
using System.Collections.Generic;
using System.IO;

namespace LrcParser
{
    public static class LyricParser
    {
        private struct LrcTimestampMatchItem
        {
            public int Index;
            public char Char;

            public LrcTimestampMatchItem(int index, char c)
            {
                this.Index = index;
                this.Char = c;
            }
        }

        public static List<Lyric> Parse(string lyrics)
        {
            if (string.IsNullOrEmpty(lyrics) || string.IsNullOrWhiteSpace(lyrics))
                return null;

            return Parse(lyrics.Split('\n'));
        }

        public static List<Lyric> Parse(StreamReader stream, bool isDispose = false)
        {
            if (stream is null)
                return null;

            var result = new List<Lyric>();
            var timestampTemp = new List<string>();
            var timestampMatchTemp = new Stack<LrcTimestampMatchItem>();

            while (!stream.EndOfStream)
                ParseSingleLine(stream.ReadLine(), result, timestampTemp, timestampMatchTemp);

            if (isDispose)
                stream.Dispose();

            return result;
        }

        public static List<Lyric> Parse(string[] lyrics)
        {
            var result = new List<Lyric>(lyrics.Length);
            var timestampTemp = new List<string>();
            var timestampMatchTemp = new Stack<LrcTimestampMatchItem>();

            for (int i = 0; i < lyrics.Length; i++)
                ParseSingleLine(lyrics[i], result, timestampTemp, timestampMatchTemp);

            return result;
        }

        public static List<Lyric> ParseSingleLine(string lrc)
        {
            var result = new List<Lyric>();
            ParseSingleLine(lrc, result);
            return result.Count == 0 ? null : result;
        }

        public static int ParseSingleLine(string lrc, List<Lyric> lyrics)
        {
            return ParseSingleLine(lrc, lyrics, new List<string>(), new Stack<LrcTimestampMatchItem>());
        }

        private static int ParseSingleLine(string lrc, List<Lyric> lyrics, List<string> timestampTemp, Stack<LrcTimestampMatchItem> timestampMatchTemp)
        {
            if (lyrics is null || string.IsNullOrEmpty(lrc) || string.IsNullOrWhiteSpace(lrc))
                return 0;

            timestampTemp.Clear();
            timestampMatchTemp.Clear();

            lrc = lrc.TrimStart();

            if (lrc[0] != '[' || lrc.Length < 8)  //min length format: [0:0.0]
                return 0;

            timestampMatchTemp.Push(new LrcTimestampMatchItem(0, '['));

            int length = lrc.Length;
            bool hasContent = false;

            for (int i = 1; i < length; i++)
            {
                if (lrc[i] == '[' && timestampMatchTemp.Count == 0)
                {
                    timestampMatchTemp.Push(new LrcTimestampMatchItem(i, '['));
                }
                else if (lrc[i] == ']')
                {
                    var item = timestampMatchTemp.Pop();
                    if (item.Char != '[')
                        continue;

                    var s = lrc.Substring(item.Index + 1, i - item.Index - 1);
                    if (IsTimestamp(s))
                    {
                        timestampTemp.Add(s);
                    }
                    else
                    {
                        lrc = ExtractLyricContent(lrc, item.Index);
                        hasContent = true;
                        break;
                    }
                }
                else if (timestampMatchTemp.Count == 0)
                {
                    lrc = ExtractLyricContent(lrc, i);
                    hasContent = true;
                    break;
                }
            }

            if (timestampTemp.Count == 0)
                return 0;

            if (!hasContent)
                lrc = string.Empty;

            for (int i = 0; i < timestampTemp.Count; i++)
            {
                lyrics.Add(new Lyric(ParseTimestamp(timestampTemp[i]), lrc));
            }

            return timestampTemp.Count;

        }

        private static TimeSpan ParseTimestamp(string timestampStr)
        {
            int index1 = timestampStr.IndexOf(':');
            int index2 = timestampStr.IndexOf('.');

            int min = index1 > 0 && int.TryParse(timestampStr.Substring(0, index1), out int m) ? m : 0;
            int sec = index2 > 0 && index2 > index1
                ? int.TryParse(timestampStr.Substring(index1 + 1, index2 - index1 - 1), out int s) ? s : 0
                : int.TryParse(timestampStr.Substring(index1 + 1, timestampStr.Length - index1 - 1), out s) ? s : 0;
            int ms  = index2 > 0 && int.TryParse(timestampStr.Substring(index2 + 1, timestampStr.Length - index2 - 1), out int l) ? l : 0;

            return new TimeSpan(0, 0, min, sec, ms);
        }

        private static bool IsTimestamp(string str)
        {
            int len = str.Length;
            int index1 = str.IndexOf(':');
            int index2 = str.IndexOf('.');
            
            if (index1 == -1 || (index2 >= 0 && index2 <= index1))
                return false;

            for (int i = 0; i < index1; i++)
            {
                if (!char.IsDigit(str[i]))
                    return false;
            }

            if (index2 == -1)
            {
                for (int i = index1 + 1; i < len; i++)
                {
                    if (!char.IsDigit(str[i]))
                        return false;
                }
            }
            else
            {
                for (int i = index1 + 1; i < index2; i++)
                {
                    if (!char.IsDigit(str[i]))
                        return false;
                }

                for (int i = index2 + 1; i < len; i++)
                {
                    if (!char.IsDigit(str[i]))
                        return false;
                }
            }

            return true;
        }
    
        private static string ExtractLyricContent(string lrc, int startIndex)
        {
            return lrc.Substring(startIndex, lrc.Length - startIndex);
        }
    }
}