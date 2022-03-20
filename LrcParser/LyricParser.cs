using System;
using System.Collections.Generic;
using System.IO;

namespace LrcParser
{
    public class LyricParser
    {
        private readonly List<string> _timestampTemp = new List<string>();
        private readonly Stack<(int, char)> _timestampMatchTemp = new Stack<(int, char)>();

        public List<Lyric> Parse(string lyrics, char eol = '\n')
        {
            if (string.IsNullOrEmpty(lyrics) || string.IsNullOrWhiteSpace(lyrics))
                return null;

            return Parse(lyrics.Split(eol));
        }

        public List<Lyric> Parse(StreamReader stream, bool isDispose = false)
        {
            if (stream is null)
                return null;

            var result = new List<Lyric>();

            while (!stream.EndOfStream)
                ParseSingleLine(stream.ReadLine(), result, _timestampTemp, _timestampMatchTemp);

            if (isDispose)
                stream.Dispose();

            return result;
        }

        public List<Lyric> Parse(string[] lyrics)
        {
            var result = new List<Lyric>(lyrics.Length);

            for (int i = 0; i < lyrics.Length; i++)
                ParseSingleLine(lyrics[i], result, _timestampTemp, _timestampMatchTemp);

            return result;
        }

        public List<Lyric> ParseSingleLine(string lrc)
        {
            var result = new List<Lyric>();
            ParseSingleLine(lrc, result);
            return result.Count == 0 ? null : result;
        }

        public int ParseSingleLine(string lrc, List<Lyric> lyrics)
        {
            return ParseSingleLine(lrc, lyrics, _timestampTemp, _timestampMatchTemp);
        }

        private static int ParseSingleLine(string lrc, List<Lyric> lyrics, List<string> timestampTemp, Stack<(int, char)> timestampMatchTemp)
        {
            if (lyrics is null || string.IsNullOrEmpty(lrc) || string.IsNullOrWhiteSpace(lrc))
                return 0;

            timestampTemp.Clear();
            timestampMatchTemp.Clear();

            lrc = lrc.TrimStart();

            if (lrc[0] != '[' || lrc.Length < 8)  //min length format: [0:0.0]
                return 0;

            timestampMatchTemp.Push((0, '['));

            int length = lrc.Length;
            bool hasContent = false;

            for (int i = 1; i < length; i++)
            {
                if (lrc[i] == '[' && timestampMatchTemp.Count == 0)
                {
                    timestampMatchTemp.Push((i, '['));
                }
                else if (lrc[i] == ']')
                {
                    var (index, c) = timestampMatchTemp.Pop();
                    if (c != '[')
                        continue;

                    var s = lrc.Substring(index + 1, i - index - 1);
                    if (IsTimestamp(s))
                    {
                        timestampTemp.Add(s);
                    }
                    else
                    {
                        lrc = ExtractLyricContent(lrc, index);
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

        private static TimeSpan ParseTimestamp(string timestamp)
        {
            var (index1, index2) = IndexOfTimestampSeparator(timestamp);

            int min = index1 > 0 ? ExtractNumber(timestamp, 0, index1) : 0;
            int sec = ExtractNumber(timestamp, index1 + 1, (index2 > 0 ? index2 : timestamp.Length) - index1 - 1);
            int ms  = index2 > 0 ? ExtractNumber(timestamp, index2 + 1, timestamp.Length - index2 - 1) : 0;

            return new TimeSpan(0, 0, min, sec, ms);
        }

        private static (int, int) IndexOfTimestampSeparator(string timestamp)
        {
            int index1 = -1;
            int index2 = -1;

            if (!string.IsNullOrEmpty(timestamp))
            {
                for (int i = 0; i < timestamp.Length; i++)
                {
                    if (timestamp[i] == ':')
                    {
                        index1 = i;
                    }
                    else if (timestamp[i] == '.')
                    {
                        index2 = i;
                        break;
                    }
                }
            }

            return (index1, index2);
        }

        private static int ExtractNumber(string str, int start, int length)
        {
            if (start < 0 || length < 1 || start + length > str.Length)
                return 0;

            int result = 0;
            for (int i = start; i < start + length; i++)
            {
                if (char.IsDigit(str[i]))
                    result = result * 10 + (str[i] - '0');
                else
                    return 0;
            }
            return result;
        }

        private static bool IsTimestamp(string str)
        {
            int len = str.Length;
            var (index1, index2) = IndexOfTimestampSeparator(str);
            
            if (index1 == -1)
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