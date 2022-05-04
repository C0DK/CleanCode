//using CleanCode;
using NUnit.Framework;

namespace CleanCode.Tests;

public class ComparisonCompactorTests
{
    [Test]
    public void TestMessage()
    {
        var failure = new ComparisonCompactor(0, "b", "c").FormatCompactedComparison("a");
        Assert.That(failure, Is.EqualTo("a expected:<[b]> but was:<[c]>"));
    }
    
    [Test]
    public void TestStartSame()
    {
        var failure = new ComparisonCompactor(1, "ba", "bc").FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<b[a]> but was:<b[c]>"));
    }
    
    [Test]
    public void TestEndSame()
    {
        var failure = new ComparisonCompactor(1, "ab", "cb").FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<[a]b> but was:<[c]b>"));
    }
    
    [Test]
    public void TestSame()
    {
        var failure = new ComparisonCompactor(1, "ab", "ab").FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<ab> but was:<ab>"));
    }


    [Test]
    public void TestSame_NoContext()
    {
        var failure = new ComparisonCompactor(0, "abcdef", "abcdef").FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<abcdef> but was:<abcdef>"));
    }

    [Test]
    public void TestDifferent_NoContext()
    {
        var failure = new ComparisonCompactor(0, "abcDef", "abcdef").FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<...[D]...> but was:<...[d]...>"));
    }

    [Test]
    public void TestNoContextStartAndEndSame()
    {
        var failure = new ComparisonCompactor(0, "abc", "adc").FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<...[b]...> but was:<...[d]...>"));
    }
    
    [Test]
    public void TestStartAndEndContext()
    {
        var failure = new ComparisonCompactor(1, "abc", "adc").FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<a[b]c> but was:<a[d]c>"));
    }
    
    [Test]
    public void TestStartAndEndContextWithEllipses()
    {
        var failure = new ComparisonCompactor(1, "abcde", "abfde").FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<...b[c]d...> but was:<...b[f]d...>"));
    }
    
    [Test]
    public void TestComparisionErrorStartSameComplete()
    {
        var failure = new ComparisonCompactor(2, "ab", "abc").FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<ab[]> but was:<ab[c]>"));
    }
    
    [Test]
    public void TestComparisionErrorEndSameComplete()
    {
        var failure = new ComparisonCompactor(2, "bc", "abc").FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<[]bc> but was:<[a]bc>"));
    }
    
    [Test]
    public void TestComparisionErrorOverlappingMatches()
    {
        var failure = new ComparisonCompactor(0, "abc", "abbc").FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<...[]...> but was:<...[b]...>"));
    }
    
    [Test]
    public void TestComparisionErrorOverlappingMatchesContext()
    {
        var failure = new ComparisonCompactor(2, "abc", "abbc").FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<ab[]c> but was:<ab[b]c>"));
    }
    
    [Test]
    public void TestComparisionErrorOverlappingMatches2()
    {
        var failure = new ComparisonCompactor(0, "abcdde", "abcde").FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<...[d]...> but was:<...[]...>"));
    }
    
    [Test]
    public void TestComparisionErrorOverlappingMatches2Context()
    {
        var failure = new ComparisonCompactor(2, "abcdde", "abcde").FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<...cd[d]e> but was:<...cd[]e>"));
    }
    
    [Test]
    public void TestComparisionErrorWithActualNull()
    {
        var failure = new ComparisonCompactor(0, "a", null).FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<a> but was:<null>"));
    }
    
    [Test]
    public void TestComparisionErrorWithActualNullContext()
    {
        var failure = new ComparisonCompactor(2, "a", null).FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<a> but was:<null>"));
    }
    
    [Test]
    public void TestComparisionErrorWithExpectedNull()
    {
        var failure = new ComparisonCompactor(0, null, "a").FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<null> but was:<a>"));
    }
    
    [Test]
    public void TestComparisionErrorWithExpectedNullContext()
    {
        var failure = new ComparisonCompactor(2, null, "a").FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<null> but was:<a>"));
    }



    [Test, Ignore("This might make sense but isn't supported")]
    public void TestBigCase()
    {
        var failure = new ComparisonCompactor(1, "1_2345678_0", "1234567890").FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<1[_]2...8[_]0> but was:<1[2]3...8[9]0>"));
    }
    
    // Bad name. versionning system stuff. What is case
    // T6 Exhaustively test near this bug too, sir.
    [Test]
    public void TestBug609972()
    {
        var failure = new ComparisonCompactor(10, "S&P500", "0").FormatCompactedComparison(null);
        Assert.That(failure, Is.EqualTo("expected:<[S&P50]0> but was:<[]0>"));
    }
}