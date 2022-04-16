using System.Text;

namespace CleanCode;

public class ComparisonCompactor
{
    private const string ELLIPSIS = "...";
    private const char DELTA_END = ']';
    private const char DELTA_START = '[';
    private readonly int _contextLength;
    private readonly string? _expected;
    private readonly string? _actual;
    private int _suffixLength;
    private int _prefixLength;

    public ComparisonCompactor(int contextLength, string? expected, string? actual)
    {
        _contextLength = contextLength;
        _expected = expected;
        _actual = actual;
    }

    public string FormatCompactedComparison(string? message)
    {
        var compactExpected = _expected;
        var compactActual = _actual;
        if (ShouldBeCompacted())
        {
            FindCommonPrefixAndSuffix();
            compactExpected = Compact(_expected!);
            compactActual = Compact(_actual!);
        }

        return Format(message, compactExpected, compactActual);
    }

    private bool ShouldBeCompacted() => !ShouldNotBeCompacted();
    
    private bool ShouldNotBeCompacted() => _expected is null || _actual is null || _expected.Equals(_actual);


    private void FindCommonPrefixAndSuffix()
    {
        if (_expected is null || _actual is null)
            throw new InvalidOperationException();
        
        FindCommonPrefix();
        _suffixLength = 0;
        for (; !SuffixOverlapsPrefix(); _suffixLength++)
        {
            if (CharFromEnd(_expected, _suffixLength) != CharFromEnd(_actual, _suffixLength))
                break;
        }
    }

    private static char CharFromEnd(string s, int i) => s[s.Length - i - 1];

    private bool SuffixOverlapsPrefix()
    {
        if (_expected is null || _actual is null)
            throw new InvalidOperationException();
        
        return _actual.Length - _suffixLength <= _prefixLength || _expected.Length - _suffixLength <= _prefixLength;
    }

    private void FindCommonPrefix()
    {
        if (_expected is null || _actual is null)
            throw new InvalidOperationException();
        
        _prefixLength = 0;
        var end = Math.Min(_expected.Length, _actual.Length);
        for (; _prefixLength < end; _prefixLength++)
        {
            if (_expected[_prefixLength] != _actual[_prefixLength])
                break;
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
        if (_expected is null)
            throw new InvalidOperationException();
        
        var contextStart = Math.Max(0, _prefixLength - _contextLength);
        var contextEnd = _prefixLength;
        return JavaStyleSubstring(_expected, contextStart, contextEnd);
    }
    
    private string Delta(string s)
    {
        var deltaStart = _prefixLength;
        var deltaEnd = s.Length - _suffixLength;
        return JavaStyleSubstring(s, deltaStart, deltaEnd);
    }

    private string EndingContext()
    {
        if (_expected is null)
            throw new InvalidOperationException();

        var contextStart = _expected.Length - _suffixLength;
        var contextEnd = Math.Min(contextStart + _contextLength, _expected.Length);
        
        return JavaStyleSubstring(_expected, contextStart, contextEnd);
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