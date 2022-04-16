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
    private int SuffixLength => _difference.CommonSuffixLength;
    private int PrefixLength => _difference.CommonPrefixLength;

    public ComparisonCompactor(int contextLength, string? expected, string? actual)
    {
        _contextLength = contextLength;
        _difference = new(expected, actual);
    }

    public string FormatCompactedComparison(string? message)
    {
        if (!_difference.AreComparable()) return Format(message, Expected, Actual);
        
        var compactExpected = Compact(Expected!);
        var compactActual = Compact(Actual!);

        return Format(message, compactExpected, compactActual);
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


    private string StartingEllipsis() => PrefixLength > _contextLength ? ELLIPSIS : "";
    
    private string StartingContext()
    {
        if (Expected is null)
            throw new InvalidOperationException();
        
        var contextStart = Math.Max(0, PrefixLength - _contextLength);
        var contextEnd = PrefixLength;
        return JavaStyleSubstring(Expected, contextStart, contextEnd);
    }
    
    private string Delta(string s)
    {
        var deltaStart = PrefixLength;
        var deltaEnd = s.Length - SuffixLength;
        return JavaStyleSubstring(s, deltaStart, deltaEnd);
    }

    private string EndingContext()
    {
        if (Expected is null)
            throw new InvalidOperationException();

        var contextStart = Expected.Length - SuffixLength;
        var contextEnd = Math.Min(contextStart + _contextLength, Expected.Length);
        
        return JavaStyleSubstring(Expected, contextStart, contextEnd);
    }
    
    
    private string EndingEllipsis() => SuffixLength > _contextLength ? ELLIPSIS : "";
    
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