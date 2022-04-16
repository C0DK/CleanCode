namespace CleanCode;

public class StringDifference
{
    public string? Expected { get; }
    public string? Actual { get; }
    public int CommonPrefixLength { get; }
    public int CommonSuffixLength { get; }
    
    public StringDifference(string? expected, string? actual)
    {
        Expected = expected;
        Actual = actual;
        CommonPrefixLength = GetCommonPrefix();
        CommonSuffixLength = GetCommonSuffix(CommonPrefixLength);

    }

    public bool AreComparable() => !(Expected is null || Actual is null || Expected.Equals(Actual));

    private int GetCommonPrefix()
    {

        if (Expected is null || Actual is null)
            return 0;
        
        var prefixLength = 0;
        var end = Math.Min(Expected.Length, Actual.Length);
        while (prefixLength < end && Expected[prefixLength] == Actual[prefixLength] )
        {
            prefixLength++;
        }

        return prefixLength;
    }
    
    private int GetCommonSuffix(int commonPrefixLength)
    {
        if (Expected is null || Actual is null)
            return 0;
        
        var suffixLength = 0;
        while(!SuffixOverlapsPrefix(commonPrefixLength, suffixLength) && CharFromEnd(Expected, suffixLength) == CharFromEnd(Actual, suffixLength))
        {
            suffixLength++;
        }

        return suffixLength;
    }
    
    private bool SuffixOverlapsPrefix(int prefixLength, int suffixLength)
    {
        if (Expected is null || Actual is null)
            throw new InvalidOperationException();
        
        return Actual.Length - suffixLength <= prefixLength || Expected.Length - suffixLength <= prefixLength;
    }
    
    private static char CharFromEnd(string s, int i) => s[s.Length - i - 1];
    
}