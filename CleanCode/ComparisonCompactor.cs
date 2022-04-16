using System.Text;

namespace CleanCode;

public class ComparisonCompactor
{
    private const string ELLIPSIS = "...";
    private const char DELTA_END = ']';
    private const char DELTA_START = '[';
    private readonly int _contextLength;

    public ComparisonCompactor(int contextLength)
    {
        _contextLength = contextLength;
    }

    public string FormatCompactedComparison(string? message, string? expected, string? actual) =>
        FormatCompactedComparison(message, new(expected, actual));
    
    private string FormatCompactedComparison(string? message, StringDifference difference)
    {
        if (!difference.AreComparable()) return Format(message, difference.Expected, difference.Actual);
        
        var compactExpected = Compact(difference.Expected!, difference);
        var compactActual = Compact(difference.Actual!, difference);

        return Format(message, compactExpected, compactActual);
    }

    private string Compact(string s, StringDifference difference) =>
        new StringBuilder()
            .Append(StartingEllipsis(difference))
            .Append(StartingContext(difference))
            .Append(DELTA_START)
            .Append(Delta(s, difference))
            .Append(DELTA_END)
            .Append(EndingContext(difference))
            .Append(EndingEllipsis(difference))
            .ToString();


    private string StartingEllipsis(StringDifference difference) => difference.CommonPrefixLength > _contextLength ? ELLIPSIS : "";
    
    private string StartingContext(StringDifference difference)
    {
        if (difference.Expected is null)
            throw new InvalidOperationException();
        
        var contextStart = Math.Max(0, difference.CommonPrefixLength - _contextLength);
        var contextEnd = difference.CommonPrefixLength;
        return JavaStyleSubstring(difference.Expected, contextStart, contextEnd);
    }
    
    private static string Delta(string s, StringDifference difference)
    {
        var deltaStart = difference.CommonPrefixLength;
        var deltaEnd = s.Length - difference.CommonSuffixLength;
        return JavaStyleSubstring(s, deltaStart, deltaEnd);
    }

    private string EndingContext(StringDifference difference)
    {
        if (difference.Expected is null)
            throw new InvalidOperationException();

        var contextStart = difference.Expected.Length - difference.CommonSuffixLength;
        var contextEnd = Math.Min(contextStart + _contextLength, difference.Expected.Length);
        
        return JavaStyleSubstring(difference.Expected, contextStart, contextEnd);
    }
    
    
    private string EndingEllipsis(StringDifference difference) => difference.CommonSuffixLength > _contextLength ? ELLIPSIS : "";
    
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