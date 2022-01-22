namespace Skender.Stock.Indicators;

[Serializable]
public class RsiResult : ResultBase
{
    public double? Rsi { get; set; }
    public double? AvgGain { get; set; }
    public double? AvgLoss { get; set; }
}
