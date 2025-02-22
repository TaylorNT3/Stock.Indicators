﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skender.Stock.Indicators;

namespace Internal.Tests;

[TestClass]
public class Fcb : TestBase
{
    [TestMethod]
    public void Standard()
    {
        List<FcbResult> results = quotes.GetFcb(2).ToList();

        // assertions

        // proper quantities
        // should always be the same number of results as there is quotes
        Assert.AreEqual(502, results.Count);
        Assert.AreEqual(497, results.Where(x => x.UpperBand != null).Count());
        Assert.AreEqual(493, results.Where(x => x.LowerBand != null).Count());

        // sample values
        FcbResult r1 = results[4];
        Assert.AreEqual(null, r1.UpperBand);
        Assert.AreEqual(null, r1.LowerBand);

        FcbResult r2 = results[10];
        Assert.AreEqual(214.84m, r2.UpperBand);
        Assert.AreEqual(212.53m, r2.LowerBand);

        FcbResult r3 = results[120];
        Assert.AreEqual(233.35m, r3.UpperBand);
        Assert.AreEqual(231.14m, r3.LowerBand);

        FcbResult r4 = results[180];
        Assert.AreEqual(236.78m, r4.UpperBand);
        Assert.AreEqual(233.56m, r4.LowerBand);

        FcbResult r5 = results[250];
        Assert.AreEqual(258.70m, r5.UpperBand);
        Assert.AreEqual(257.04m, r5.LowerBand);

        FcbResult r6 = results[501];
        Assert.AreEqual(262.47m, r6.UpperBand);
        Assert.AreEqual(229.42m, r6.LowerBand);
    }

    [TestMethod]
    public void BadData()
    {
        IEnumerable<FcbResult> r = Indicator.GetFcb(badQuotes);
        Assert.AreEqual(502, r.Count());
    }

    [TestMethod]
    public void Removed()
    {
        List<FcbResult> results = quotes.GetFcb(2)
            .RemoveWarmupPeriods()
            .ToList();

        // assertions
        Assert.AreEqual(502 - 5, results.Count);

        FcbResult last = results.LastOrDefault();
        Assert.AreEqual(262.47m, last.UpperBand);
        Assert.AreEqual(229.42m, last.LowerBand);
    }

    [TestMethod]
    public void Exceptions()
    {
        // bad lookback period
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            Indicator.GetFcb(quotes, 1));

        // insufficient quotes
        Assert.ThrowsException<BadQuotesException>(() =>
            Indicator.GetFcb(TestData.GetDefault(60), 30));
    }
}
