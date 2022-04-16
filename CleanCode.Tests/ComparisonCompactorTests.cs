using CleanCode;
using NUnit.Framework;

namespace CleanCode.Tests;

public class ComparisonCompactorTests
{
    [Test]
    public void TestMessage()
    {
        var failure = new ComparisonCompactor(0).FormatCompactedComparison("a", "b", "c");
        Assert.That(failure, Is.EqualTo("a expected:<[b]> but was:<[c]>"));
    }
    
    [Test]
    public void TestStartSame()
    {
        var failure = new ComparisonCompactor(1).FormatCompactedComparison(null, "ba", "bc");
        Assert.That(failure, Is.EqualTo("expected:<b[a]> but was:<b[c]>"));
    }
    
    [Test]
    public void TestEndSame()
    {
        var failure = new ComparisonCompactor(1).FormatCompactedComparison(null, "ab", "cb");
        Assert.That(failure, Is.EqualTo("expected:<[a]b> but was:<[c]b>"));
    }
    
    [Test]
    public void TestSame()
    {
        var failure = new ComparisonCompactor(1).FormatCompactedComparison(null, "ab", "ab");
        Assert.That(failure, Is.EqualTo("expected:<ab> but was:<ab>"));
    }
    
    [Test]
    public void TestNoContextStartAndEndSame()
    {
        var failure = new ComparisonCompactor(0).FormatCompactedComparison(null, "abc", "adc");
        Assert.That(failure, Is.EqualTo("expected:<...[b]...> but was:<...[d]...>"));
    }
    
    [Test]
    public void TestStartAndEndContext()
    {
        var failure = new ComparisonCompactor(1).FormatCompactedComparison(null, "abc", "adc");
        Assert.That(failure, Is.EqualTo("expected:<a[b]c> but was:<a[d]c>"));
    }
    
    [Test]
    public void TestStartAndEndContextWithEllipses()
    {
        var failure = new ComparisonCompactor(1).FormatCompactedComparison(null, "abcde", "abfde");
        Assert.That(failure, Is.EqualTo("expected:<...b[c]d...> but was:<...b[f]d...>"));
    }
    
    [Test]
    public void TestComparisionErrorStartSameComplete()
    {
        var failure = new ComparisonCompactor(2).FormatCompactedComparison(null, "ab", "abc");
        Assert.That(failure, Is.EqualTo("expected:<ab[]> but was:<ab[c]>"));
    }
    
    [Test]
    public void TestComparisionErrorEndSameComplete()
    {
        var failure = new ComparisonCompactor(2).FormatCompactedComparison(null, "bc", "abc");
        Assert.That(failure, Is.EqualTo("expected:<[]bc> but was:<[a]bc>"));
    }
    
    [Test]
    public void TestComparisionErrorOverlappingMatches()
    {
        var failure = new ComparisonCompactor(0).FormatCompactedComparison(null, "abc", "abbc");
        Assert.That(failure, Is.EqualTo("expected:<...[]...> but was:<...[b]...>"));
    }
    
    [Test]
    public void TestComparisionErrorOverlappingMatchesContext()
    {
        var failure = new ComparisonCompactor(2).FormatCompactedComparison(null, "abc", "abbc");
        Assert.That(failure, Is.EqualTo("expected:<ab[]c> but was:<ab[b]c>"));
    }
    
    [Test]
    public void TestComparisionErrorOverlappingMatches2()
    {
        var failure = new ComparisonCompactor(0).FormatCompactedComparison(null, "abcdde", "abcde");
        Assert.That(failure, Is.EqualTo("expected:<...[d]...> but was:<...[]...>"));
    }
    
    [Test]
    public void TestComparisionErrorOverlappingMatches2Context()
    {
        var failure = new ComparisonCompactor(2).FormatCompactedComparison(null, "abcdde", "abcde");
        Assert.That(failure, Is.EqualTo("expected:<...cd[d]e> but was:<...cd[]e>"));
    }
    
    [Test]
    public void TestComparisionErrorWithActualNull()
    {
        var failure = new ComparisonCompactor(0).FormatCompactedComparison(null, "a", null);
        Assert.That(failure, Is.EqualTo("expected:<a> but was:<null>"));
    }
    
    [Test]
    public void TestComparisionErrorWithActualNullContext()
    {
        var failure = new ComparisonCompactor(2).FormatCompactedComparison(null, "a", null);
        Assert.That(failure, Is.EqualTo("expected:<a> but was:<null>"));
    }
    
    [Test]
    public void TestComparisionErrorWithExpectedNull()
    {
        var failure = new ComparisonCompactor(0).FormatCompactedComparison(null, null, "a");
        Assert.That(failure, Is.EqualTo("expected:<null> but was:<a>"));
    }
    
    [Test]
    public void TestComparisionErrorWithExpectedNullContext()
    {
        var failure = new ComparisonCompactor(2).FormatCompactedComparison(null, null, "a");
        Assert.That(failure, Is.EqualTo("expected:<null> but was:<a>"));
    }
    
    [Test]
    public void TestBug609972()
    {
        var failure = new ComparisonCompactor(10).FormatCompactedComparison(null, "S&P500", "0");
        Assert.That(failure, Is.EqualTo("expected:<[S&P50]0> but was:<[]0>"));
    }
}