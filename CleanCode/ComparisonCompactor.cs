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

    public string FormatCompactedComparison(string? message, string? expected, string? actual) {
        if(expected is null || actual is null)
            return Format(message, expected, actual);
        
        return FormatCompactedComparison(message, new(expected, actual));
    }
    
    private string FormatCompactedComparison(string? message, StringDifference difference)
    {
        // TODO this should probably be an error. or return an empty string. why not compact equal strings??
        if(difference.AreEqual)
            return Format(message, difference.Expected, difference.Actual);
            
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


    private string StartingEllipsis(StringDifference difference) => difference.CommonPrefix.Length > _contextLength ? ELLIPSIS : "";

    private string StartingContext(StringDifference difference)
    {
        var start = Math.Max(0, difference.CommonPrefix.Length - _contextLength);
        
        return difference.CommonPrefix[start..];
    }
    
    private static string Delta(string s, StringDifference difference)
    {
        var deltaStart = difference.CommonPrefix.Length;
        var deltaEnd = s.Length - difference.CommonSuffix.Length;
        return JavaStyleSubstring(s, deltaStart, deltaEnd);
    }

    private string EndingContext(StringDifference difference) =>
        difference.CommonSuffix[..Math.Min(_contextLength, difference.CommonSuffix.Length)];


    private string EndingEllipsis(StringDifference difference) => difference.CommonSuffix.Length > _contextLength ? ELLIPSIS : "";
    
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