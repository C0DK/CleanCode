using System.Text;

namespace CleanCode;

public class ComparisonCompactor
{
    private const string ELLIPSIS = "...";
    private const char DELTA_END = ']';
    private const char DELTA_START = '[';
    private readonly int _contextLength;
    private string? Expected => _difference.Expected;
    private string? Actual => _difference.Actual;
    private readonly StringDifference _difference;
    private int _suffixLength;
    private int _prefixLength;

    public ComparisonCompactor(int contextLength, string? expected, string? actual)
    {
        _contextLength = contextLength;
        _difference = new(expected, actual);
    }

    public string FormatCompactedComparison(string? message)
    {
        if (!ShouldBeCompacted()) return Format(message, Expected, Actual);
        
        FindCommonPrefixAndSuffix();
        var compactExpected = Compact(Expected!);
        var compactActual = Compact(Actual!);

        return Format(message, compactExpected, compactActual);
    }

    private bool ShouldBeCompacted() => !ShouldNotBeCompacted();
    
    private bool ShouldNotBeCompacted() => Expected is null || Actual is null || Expected.Equals(Actual);


    private void FindCommonPrefixAndSuffix()
    {
        if (Expected is null || Actual is null)
            throw new InvalidOperationException();
        
        FindCommonPrefix();
        _suffixLength = 0;
        while(!SuffixOverlapsPrefix() && CharFromEnd(Expected, _suffixLength) == CharFromEnd(Actual, _suffixLength))
        {
            _suffixLength++;
        }
    }

    private static char CharFromEnd(string s, int i) => s[s.Length - i - 1];

    private bool SuffixOverlapsPrefix()
    {
        if (Expected is null || Actual is null)
            throw new InvalidOperationException();
        
        return Actual.Length - _suffixLength <= _prefixLength || Expected.Length - _suffixLength <= _prefixLength;
    }

    private void FindCommonPrefix()
    {
        if (Expected is null || Actual is null)
            throw new InvalidOperationException();
        
        _prefixLength = 0;
        var end = Math.Min(Expected.Length, Actual.Length);
        while (_prefixLength < end && Expected[_prefixLength] == Actual[_prefixLength] )
        {
            _prefixLength++;
        }
            
    }

    private string Compact(string s) =>
        new StringBuilder()
            .Append(StartingEllipsis())
            .Append(StartingContext())
            .Append(DELTA_START)
            .Append(Delta(s))
            .Append(DELTA_END)
            .Append(EndingContext())
            .Append(EndingEllipsis())
            .ToString();


    private string StartingEllipsis() => _prefixLength > _contextLength ? ELLIPSIS : "";
    
    private string StartingContext()
    {
        if (Expected is null)
            throw new InvalidOperationException();
        
        var contextStart = Math.Max(0, _prefixLength - _contextLength);
        var contextEnd = _prefixLength;
        return JavaStyleSubstring(Expected, contextStart, contextEnd);
    }
    
    private string Delta(string s)
    {
        var deltaStart = _prefixLength;
        var deltaEnd = s.Length - _suffixLength;
        return JavaStyleSubstring(s, deltaStart, deltaEnd);
    }

    private string EndingContext()
    {
        if (Expected is null)
            throw new InvalidOperationException();

        var contextStart = Expected.Length - _suffixLength;
        var contextEnd = Math.Min(contextStart + _contextLength, Expected.Length);
        
        return JavaStyleSubstring(Expected, contextStart, contextEnd);
    }
    
    
    private string EndingEllipsis() => _suffixLength > _contextLength ? ELLIPSIS : "";
    
    private static string Format(string? message, string? expected, string? actual)
    {
        var prefix = message is not null ? $"{message} " : "";
        return $"{prefix}expected:<{FormatNullable(expected)}> but was:<{FormatNullable(actual)}>";
    }

    private static string FormatNullable(string? s) => s ?? "null";
        

    private static string JavaStyleSubstring(string s, int beginIndex,
      int endIndex)
    {
      var len = endIndex - beginIndex;
      return s.Substring(beginIndex, len);
    }
    
}