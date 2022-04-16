namespace CleanCode;

public class StringDifference
{
    public string? Expected { get; }
    public string? Actual { get; }
    public string CommonSuffix { get; }
    public string CommonPrefix { get; }

    public StringDifference(string? expected, string? actual)
    {
        Expected = expected;
        Actual = actual;
        var commonPrefixLength = GetCommonPrefixLength(expected, actual);
        CommonPrefix = GetCommonPrefix(commonPrefixLength);
        CommonSuffix = GetCommonSuffix(GetCommonSuffixLength(commonPrefixLength, expected, actual));
    }

    public bool AreComparable() => !(Expected is null || Actual is null || Expected.Equals(Actual));

    private string GetCommonPrefix(int length) => !AreComparable() ? "" : Expected![..length];

    private string GetCommonSuffix(int length) => !AreComparable() ? "" : Expected!.Substring(Expected!.Length - length, length);

    private static int GetCommonPrefixLength(string? expected, string? actual)
    {
        if (expected is null || actual is null) return 0;

        var prefixLength = 0;
        var end = Math.Min(expected.Length, actual.Length);
        while (prefixLength < end && expected[prefixLength] == actual[prefixLength])
        {
            prefixLength++;
        }

        return prefixLength;
    }

    private static int GetCommonSuffixLength(int commonPrefixLength, string? expected, string? actual)
    {
        if (expected is null || actual is null) return 0;

        var suffixLength = 0;
        while (
            actual.Length - suffixLength > commonPrefixLength
            && expected.Length - suffixLength > commonPrefixLength
            && CharFromEnd(expected, suffixLength) == CharFromEnd(actual, suffixLength))
        {
            suffixLength++;
        }

        return suffixLength;
    }

    private static char CharFromEnd(string s, int i) => s[s.Length - i - 1];
}