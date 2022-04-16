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
        var commonPrefixLength = GetCommonPrefixLength();
        CommonPrefix = GetCommonPrefix(commonPrefixLength);
        CommonSuffix = GetCommonSuffix(GetCommonSuffixLength(commonPrefixLength));
    }

    public bool AreComparable() => !(Expected is null || Actual is null || Expected.Equals(Actual));

    private string GetCommonPrefix(int length) => !AreComparable() ? "" : Expected![..length];

    private string GetCommonSuffix(int length) => !AreComparable() ? "" : Expected!.Substring(Expected!.Length - length, length);

    private int GetCommonPrefixLength()
    {
        if (Expected is null || Actual is null) return 0;

        var prefixLength = 0;
        var end = Math.Min(Expected.Length, Actual.Length);
        while (prefixLength < end && Expected[prefixLength] == Actual[prefixLength])
        {
            prefixLength++;
        }

        return prefixLength;
    }

    private int GetCommonSuffixLength(int commonPrefixLength)
    {
        if (Expected is null || Actual is null) return 0;

        var suffixLength = 0;
        while (
            Actual.Length - suffixLength > commonPrefixLength
            && Expected.Length - suffixLength > commonPrefixLength
            && CharFromEnd(Expected, suffixLength) == CharFromEnd(Actual, suffixLength))
        {
            suffixLength++;
        }

        return suffixLength;
    }

    private static char CharFromEnd(string s, int i) => s[s.Length - i - 1];
}