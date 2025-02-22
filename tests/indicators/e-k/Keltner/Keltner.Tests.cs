﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skender.Stock.Indicators;

namespace Internal.Tests;

[TestClass]
public class Keltner : TestBase
{
    [TestMethod]
    public void Standard()
    {
        int emaPeriods = 20;
        int multiplier = 2;
        int atrPeriods = 10;

        List<KeltnerResult> results =
            quotes.GetKeltner(emaPeriods, multiplier, atrPeriods)
            .ToList();

        // assertions

        // proper quantities
        // should always be the same number of results as there is quotes
        Assert.AreEqual(502, results.Count);

        int warmupPeriod = 502 - Math.Max(emaPeriods, atrPeriods) + 1;
        Assert.AreEqual(warmupPeriod, results.Where(x => x.Centerline != null).Count());
        Assert.AreEqual(warmupPeriod, results.Where(x => x.UpperBand != null).Count());
        Assert.AreEqual(warmupPeriod, results.Where(x => x.LowerBand != null).Count());
        Assert.AreEqual(warmupPeriod, results.Where(x => x.Width != null).Count());

        // sample value
        KeltnerResult r1 = results[485];
        Assert.AreEqual(275.4260m, Math.Round((decimal)r1.UpperBand, 4));
        Assert.AreEqual(265.4599m, Math.Round((decimal)r1.Centerline, 4));
        Assert.AreEqual(255.4938m, Math.Round((decimal)r1.LowerBand, 4));
        Assert.AreEqual(0.075085m, Math.Round((decimal)r1.Width, 6));

        KeltnerResult r2 = results[501];
        Assert.AreEqual(262.1873m, Math.Round((decimal)r2.UpperBand, 4));
        Assert.AreEqual(249.3519m, Math.Round((decimal)r2.Centerline, 4));
        Assert.AreEqual(236.5165m, Math.Round((decimal)r2.LowerBand, 4));
        Assert.AreEqual(0.102950m, Math.Round((decimal)r2.Width, 6));
    }

    [TestMethod]
    public void BadData()
    {
        IEnumerable<KeltnerResult> r = Indicator.GetKeltner(badQuotes, 10, 3, 15);
        Assert.AreEqual(502, r.Count());
    }

    [TestMethod]
    public void Removed()
    {
        int emaPeriods = 20;
        int multiplier = 2;
        int atrPeriods = 10;
        int n = Math.Max(emaPeriods, atrPeriods);

        List<KeltnerResult> results =
            quotes.GetKeltner(emaPeriods, multiplier, atrPeriods)
                .RemoveWarmupPeriods()
                .ToList();

        // assertions
        Assert.AreEqual(502 - Math.Max(2 * n, n + 100), results.Count);

        KeltnerResult last = results.LastOrDefault();
        Assert.AreEqual(262.1873m, Math.Round((decimal)last.UpperBand, 4));
        Assert.AreEqual(249.3519m, Math.Round((decimal)last.Centerline, 4));
        Assert.AreEqual(236.5165m, Math.Round((decimal)last.LowerBand, 4));
        Assert.AreEqual(0.102950m, Math.Round((decimal)last.Width, 6));
    }

    [TestMethod]
    public void Exceptions()
    {
        // bad EMA period
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            Indicator.GetKeltner(quotes, 1, 2, 10));

        // bad ATR period
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            Indicator.GetKeltner(quotes, 20, 2, 1));

        // bad multiplier
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            Indicator.GetKeltner(quotes, 20, 0, 10));

        // insufficient quotes for N+100
        Assert.ThrowsException<BadQuotesException>(() =>
            Indicator.GetKeltner(TestData.GetDefault(119), 20, 2, 10));

        // insufficient quotes for 2×N
        Assert.ThrowsException<BadQuotesException>(() =>
            Indicator.GetKeltner(TestData.GetDefault(499), 20, 2, 250));
    }
}
