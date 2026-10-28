using FunctionLab.Models;
using FunctionLab.Services;
using Xunit;

namespace FunctionLab.Tests;

// Oodatavad väärtused on võetud juhendi tabelist 3.
public class FunktsiooniArvutusTestid
{
    private static readonly AnalysisService Teenus = new();
    [Theory]
    [InlineData("x^3-3*x", 2, 9)]
    [InlineData("x^2-4*x+3", 2, 0)]
    [InlineData("x^3-12*x", 1, -9)]
    public void Tuletis_KontrollFunktsioonid_TagastabOigeVaartuse(
        string valem, double x, double oodatav)
    {
        var tuletis = new ExpressionParser().Parse(valem)
            .Derivative();
        Assert.Equal(oodatav, tuletis.Eval(x), 3);
    }
    [Theory]
    [InlineData("x^2-4*x+3", new double[] { 1, 3 },
        new double[] { 2 })]
    [InlineData("x^3-3*x", new double[] { -1.7320508, 0, 1.7320508 },
        new double[] { -1, 1 })]
    [InlineData("x^3-12*x", new double[] { -3.4641016, 0, 3.4641016 },
        new double[] { -2, 2 })]
    public void Analyze_LeiabNullkohadJaKriitilisedPunktid(
        string valem, double[] nullkohad, double[] kriitilised)
    {
        var t = Teenus.Analyze(new UurimiseSisend(valem, null));
        Assert.Equal(nullkohad.Length, t.Roots.Count);
        for (int i = 0; i < nullkohad.Length; i++)
            Assert.Equal(nullkohad[i], t.Roots[i], 5);
        Assert.Equal(kriitilised.Length, t.CriticalPoints.Count);
        for (int i = 0; i < kriitilised.Length; i++)
            Assert.Equal(kriitilised[i], t.CriticalPoints[i], 5);
    }
    [Theory]
    [InlineData("x^3-3*x", -1, 2, 1, -2)]
    [InlineData("x^3-12*x", -2, 16, 2, -16)]
    public void Analyze_KuupFunktsioon_LeiabMaksimumiJaMiinimumi(
        string valem, double maxX, double maxY,
        double minX, double minY)
    {
        var t = Teenus.Analyze(new UurimiseSisend(valem, null));
        var max = Assert.Single(t.Extrema, e => e.Type == "Максимум");
        var min = Assert.Single(t.Extrema, e => e.Type == "Минимум");
        Assert.Equal(maxX, max.X, 5); Assert.Equal(maxY, max.Y, 5);
        Assert.Equal(minX, min.X, 5); Assert.Equal(minY, min.Y, 5);
    }
}
